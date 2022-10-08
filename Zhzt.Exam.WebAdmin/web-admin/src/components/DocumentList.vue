<template>
    <div class="table-header" v-if="props.hasHeader">
        <div class="left">
            <h3>{{ props.tableTitle }}</h3>
        </div>
        <div class="right">

            <el-button type="primary" @click="handleCreate()" v-if="props.createable">
                新建文档
            </el-button>

            <el-button type="danger" @click="handleDeleteMany()" v-if="props.multiDeleteable">
                删除选中项
            </el-button>
        </div>
    </div>
    <el-table :data="tableData" style="width: 100%" :border="true" @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55" v-if="props.multiDeleteable" />
        
        <el-table-column label="标题">
            <template #default="scope">
                {{scope.row.name}}
            </template>
        </el-table-column>

        <el-table-column label="所属类别" >
            <template #default="scope">
                <el-tag>{{ scope.row.docCategory?.name ?? '未分类' }}</el-tag>
            </template>
        </el-table-column>

        <el-table-column label="发布时间">
            <template #default="scope">
                {{ scope.row.createTime }}
            </template>
        </el-table-column>

        <el-table-column label="操作"  v-if="props.editable">
            <template #default="scope">
                <a style="cursor: pointer; margin-right: 10px" @click="handlePreview(scope.row)">预览</a>
                <a style="cursor: pointer; margin-right: 10px" @click="handleEdit(scope.row)">修改</a>
                <el-popconfirm title="确定删除吗？" @confirm="handleDeleteOne(scope.row.id)">
                    <template #reference>
                        <a style="cursor: pointer">删除</a>
                    </template>
                </el-popconfirm>
            </template>
        </el-table-column>
    </el-table>
    <el-pagination background layout="prev, pager, next" class="center-pagination" v-model:currentPage="pageIndex"
     :total="totalCount" :pageSize="pageSize" @current-change="handlePageChange" />
     <PdfPreview ref="pdfPreDlgRef"/>
</template>

<script>
import { ElMessage, ElMessageBox } from 'element-plus'
import { onMounted, reactive, toRefs, ref } from 'vue'
import axios from '../utils/axios'
import PdfPreview from './PdfPreview.vue'
import cache from '../utils/cache'

export default {
    name: "DocumentList",
    components:{PdfPreview},
    props: {
        pageSize: { type: Number, default: () => 15 },
        categoryId: { type: Number, default: () => 0 },
        hasHeader: { type: Boolean, default: () => false },
        editable: { type: Boolean, default: () => false },
        doEdit: Function,
        createable: { type: Boolean, default: () => false },
        doCreate: Function,
        multiDeleteable : {type : Boolean, default : ()=>false},
        tableTitle: { type: String, default: () => "资源列表" },
        allDocumentCategories: { type: Array, default: () => [] }
    },
    setup(props) {
        const state = reactive({
            tableData: [],
            pageIndex: 1,
            pageSize: props.pageSize ?? 10,
            categoryId: props.categoryId ?? 0,
            totalCount: 0,
            allDocumentCategories: [],
            selectedRows: [],
        });
        const pdfPreDlgRef = ref(null)

        // 挂载方法
        onMounted(() => {
            filterDocuments()
        })

        // 分页加载问题列表 支持筛选
        const filterDocuments = (filter) => {
            let url = `/documentlib/document/filterpage`;
            let filterData = {};
            if (filter) {
                filterData = filter;
            }
            else {
                filterData = {
                    categoryId: state.categoryId,
                    pageIndex: state.pageIndex,
                    pageSize: state.pageSize
                };
            }
            axios.post(url, filterData)
                .then(res => {
                    state.tableData = res.data.pageData;
                    state.pageIndex = res.data.pageIndex;
                    state.pageSize = res.data.pageSize;
                    state.totalCount = res.data.totalCount;
                })
                .catch(err => {
                    ElMessage.warning(err);
                });
        };

        // 切换页码
        const handlePageChange = (newPageIndex) => {
            state.pageIndex = newPageIndex;
            filterDocuments();
        };
       
        // 执行创建
        const handleEdit = (row) => {
            if (props.doEdit) {
                props.doEdit(row);
            }
        };

        // 执行修改
        const handleCreate = () => {
            if (props.doCreate) {
                props.doCreate();
            }
        };
        
        // 执行删除 和题型无关可以直接在这里统一写一个删除方法
        const handleDeleteOne = (id) => {
            axios.delete(`/documentlib/document/delete?id=${id}`)
                .then(res => {
                    ElMessage.success("删除数据成功");
                    filterDocuments();
                }).catch(err => {
                    console.log(err);
                    ElMessage.error("删除数据失败");
                });
        }

        // 转换科目列表支持cascader
        const convertCategory = (questionList) => {
            if (questionList) {
                return questionList.map((val) => {
                    var rObj = {}
                    rObj['key'] = val.id
                    rObj['value'] = val.id
                    rObj['label'] = val.name
                    rObj['children'] = convertCategory(val.child)
                    rObj['disabled'] = false
                    return rObj
                })
            }
        }

        // 选择行
        const handleSelectionChange = (val)=>{
            state.selectedRows = val
        }

        // 预览视频
        const handlePreview = (val)=>{
            console.log(val)
            let pdfUrl = cache.docBaseUrl + val.pdfUrl
            console.log(pdfUrl)
            pdfPreDlgRef.value.open(pdfUrl)
        }

        // 删除多个
        const handleDeleteMany = ()=>{
            ElMessageBox.confirm(
                '批量删除前请确认选择内容，您确定要删除当前选中的数据吗?',
                '危险操作告警',
                {
                    confirmButtonText: '确认',
                    cancelButtonText: '取消',
                    type: 'warning',
                }
            )
            .then(() => {
                let deleteIds = {
                    ids : state.selectedRows.map((v)=>{return v.id})
                }
                axios.delete('/questionlib/question/deletemany',{data: deleteIds})
                .then(res=>{
                    ElMessage.success('删除数据成功')
                    filterDocuments()
                })
                .catch(err=>{
                    ElMessage.error("删除数据失败")
                })
            })
            .catch(() => {
                ElMessage({
                    type: 'info',
                    message: '删除操作取消！',
                })
            })
        }

        return {
            ...toRefs(state),
            handlePageChange,
            handleCreate,
            handleDeleteOne,
            handleEdit,
            props,
            filterDocuments,
            convertCategory,
            handleSelectionChange,
            handleDeleteMany,
            pdfPreDlgRef,
            handlePreview
        };
    }
}
</script>