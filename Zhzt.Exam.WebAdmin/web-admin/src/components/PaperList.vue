<!-- 题库列表组件 -->
<!-- 使用示范
    <PaperList :pageSize="10" :questionClass="1"  />
-->
<template>
    <div class="table-header" v-if="props.hasHeader">
        <div class="left">
            <h3>{{ props.tableTitle }}</h3>
        </div>
        <div class="right">
            <el-button type="primary" @click="handleCreate()" v-if="props.createable">
                新建试卷
            </el-button>

            <el-button type="danger" @click="handleDeleteMany()" v-if="props.multiDeleteable">
                删除选中项
            </el-button>
        </div>
    </div>
    <el-table :data="tableData" style="width: 100%" :border="true" @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55" v-if="props.multiDeleteable" />
        <el-table-column label="试卷名称">
            <template #default="scope">
                <span class="title">{{ scope.row?.name }}</span>
            </template>
        </el-table-column>
        <el-table-column label="所属科目" width="200px">
            <template #default="scope">
                <el-tag>{{ scope.row.subject?.name ?? '未知科目' }}</el-tag>
            </template>
        </el-table-column>
        <el-table-column label="单选题数量" width="150px">
            <template #default="scope">
                <el-tag>
                    {{scope.row.pagerConfig.singleChoiceCount}}
                </el-tag>
            </template>
        </el-table-column>
        <el-table-column label="多选题数量" width="150px">
            <template #default="scope">
                <el-tag>
                {{scope.row.pagerConfig.multiChoiceCount}}
                </el-tag>
            </template>
        </el-table-column>
        <el-table-column label="判断题数量" width="100px">
            <template #default="scope">
                <el-tag>
                {{scope.row.pagerConfig.judgeCount}}
                </el-tag>
            </template>
        </el-table-column>
        <el-table-column label="填空题数量" width="150px">
            <template #default="scope">
                <el-tag>
                {{scope.row.pagerConfig.blankFillCount}}
                </el-tag>
            </template>
        </el-table-column>
        <el-table-column label="问答题数量" width="150px">
            <template #default="scope">
                <el-tag>
                {{scope.row.pagerConfig.quesAnswerCount}}
                </el-tag>
            </template>
        </el-table-column>
        <el-table-column label="操作" width="200px" v-if="props.editable">
            <template #default="scope">
                <a style="cursor: pointer; margin-right: 10px" @click="handleEdit(scope.row)">修改</a>
                <a style="cursor: pointer; margin-right: 10px" :href="'/api/paperlib/paper/download/'+scope.row.id">下载</a>
                <el-popconfirm title="确定删除吗？" @confirm="handleDeleteOne(scope.row.id)">
                    <template #reference>
                        <a style="cursor: pointer">删除</a>
                    </template>
                </el-popconfirm>
            </template>
        </el-table-column>
    </el-table>
    <el-pagination background layout="prev, pager, next" class="center-pagination" v-model:currentPage="pageIndex"
        v-model:page-size="pageSize" :total="totalCount" :pageSize="pageSize" @current-change="handlePageChange" />
</template>

<script>
import { ElMessage, ElMessageBox } from 'element-plus'
import { onMounted, reactive, toRefs, ref } from 'vue'
import axios from '../utils/axios'

export default {
    name: "PaperList",
    props: {
        pageSize: { type: Number, default: () => 15 },
        hasHeader: { type: Boolean, default: () => false },
        editable: { type: Boolean, default: () => false },
        doEdit: Function,
        createable: { type: Boolean, default: () => false },
        doCreate: Function,
        multiDeleteable : {type : Boolean, default : ()=>false},
        tableTitle: { type: String, default: () => "试卷列表" },
        allQuestionTypes: { type: Array, default: () => [] }
    },
    setup(props) {
        const state = reactive({
            tableData: [],
            pageIndex: 1,
            pageSize: props.pageSize ?? 10,
            questionType: props.questionType ?? 0,
            totalCount: 0,
            allQuestionTypes: [],
            selectedRows: [],
        });

        // 挂载方法
        onMounted(() => {
            filterPapers()
        })

        // 分页加载试卷列表 支持筛选
        const filterPapers = (filter) => {
            let url = `/paperlib/paper/filterpage`;
            let filterData = {};
            if (filter) {
                filterData = filter;
            }
            else {
                filterData = {
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
            filterPapers();
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
            axios.delete(`/paperlib/paper/delete?id=${id}`)
                .then(res => {
                    ElMessage.success("删除数据成功");
                    filterPapers();
                }).catch(err => {
                    console.log(err);
                    ElMessage.error("删除数据失败");
                });
        }

        // 选择行
        const handleSelectionChange = (val)=>{
            state.selectedRows = val
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
                axios.delete('/paperlib/paper/deletemany',{data: deleteIds})
                .then(res=>{
                    ElMessage.success('删除数据成功')
                    filterPapers()
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
            filterPapers,
            handleSelectionChange,
            handleDeleteMany,
        };
    }
}
</script>

<style scoped>
.title{
    font-size: 1.2em;
    font-weight: 200;
}
</style>