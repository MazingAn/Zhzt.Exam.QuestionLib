<!-- 题库列表组件 -->
<!-- 使用示范
    <QuesList :pageSize="10" :questionClass="1"  :questionTypeId="questionTypeId" />
-->
<template>
    <div class="table-header" v-if="props.hasHeader">
        <div class="left">
            <h3>{{ props.tableTitle }}</h3>
        </div>
        <div class="right">
            <el-button type="success" @click="handleImport()" v-if="props.importable">
                批量导入题库
            </el-button>

            <el-button type="primary" @click="handleCreate()" v-if="props.createable">
                新建题目
            </el-button>

            <el-button type="danger" @click="handleDeleteMany()" v-if="props.multiDeleteable">
                删除选中项
            </el-button>
        </div>
    </div>
    <el-table :data="tableData" style="width: 100%" :border="true" @selection-change="handleSelectionChange">
        <el-table-column type="selection" width="55" v-if="props.multiDeleteable" />
        <el-table-column label="所属科目" width="180">
            <template #default="scope">
                <el-tag>{{ scope.row.questionType?.name ?? '未分类' }}</el-tag>
            </template>
        </el-table-column>
        <el-table-column label="题型" width="180">
            <template #default="scope">
                <el-tag :type="questionClassTypeName(scope.row)">{{
                        cache.queryQuestionClassName(scope.row.questionClass)
                }}</el-tag>
            </template>
        </el-table-column>
        <el-table-column label="题目">
            <template #default="scope">
                <p style="line-height: 1;" v-html="scope.row.questionBody"></p>
            </template>
        </el-table-column>
        <el-table-column label="操作" width="260" v-if="props.editable">
            <template #default="scope">
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
    <QuesImptDlg ref="quesImportorRef" :reload="filterQuestions" :allQuestionTypes="props.allQuestionTypes" />
</template>

<script>
import { ElMessage, ElMessageBox } from 'element-plus'
import { onMounted, reactive, toRefs, ref } from 'vue'
import axios from '../utils/axios'
import cache from '../utils/cache'
import QuesImptDlg from './QuesImptDlg.vue'

export default {
    name: "QuesList",
    components: { QuesImptDlg },
    props: {
        pageSize: { type: Number, default: () => 15 },
        questionClass: { type: Number, default: () => 0 },
        questionTypeId: { type: Number, default: () => 0 },
        hasHeader: { type: Boolean, default: () => false },
        editable: { type: Boolean, default: () => false },
        doEdit: Function,
        createable: { type: Boolean, default: () => false },
        doCreate: Function,
        multiDeleteable : {type : Boolean, default : ()=>false},
        importable: { type: Boolean, default: () => false },
        tableTitle: { type: String, default: () => "题目列表" },
        allQuestionTypes: { type: Array, default: () => [] }
    },
    setup(props) {
        const state = reactive({
            tableData: [],
            pageIndex: 1,
            pageSize: props.pageSize ?? 10,
            questionClass: props.questionClass ?? 0,
            questionType: props.questionType ?? 0,
            totalCount: 0,
            allQuestionTypes: [],
            selectedRows: [],
        });
        const quesImportorRef = ref(null)

        // 挂载方法
        onMounted(() => {
            filterQuestions()
        })

        // 分页加载问题列表 支持筛选
        const filterQuestions = (filter) => {
            let url = `/questionlib/question/filterpage`;
            let filterData = {};
            if (filter) {
                filterData = filter;
            }
            else {
                filterData = {
                    questionClass: state.questionClass,
                    questionTypeId: state.questionType,
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
            filterQuestions();
        };
        // 根据题型赋予每一行不同的样式
        const tableRowClassName = ({ row, rowIndex }) => {
            switch (row.questionClass) {
                case 1:
                    return "signlechoice-row";
                    break;
                case 2:
                    return "multichoice-row";
                    break;
                case 3:
                    return "judge-row";
                    break;
                case 4:
                    return "blankfill-row";
                    break;
                case 5:
                    return "answerquestion-row";
                    break;
                default:
                    break;
            }
        };
        // 根据题型赋予每一行标签不同的类型
        const questionClassTypeName = (row) => {
            switch (row.questionClass) {
                case 1:
                    return "success";
                    break;
                case 2:
                    return "danger";
                    break;
                case 3:
                    return "info";
                    break;
                case 4:
                    return "warning";
                    break;
                case 5:
                    return "";
                    break;
                default:
                    break;
            }
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
        // 执行导入 和题型无关可以直接在这里统一实现
        const handleImport = () => {
            quesImportorRef.value.open()
        };
        // 执行删除 和题型无关可以直接在这里统一写一个删除方法
        const handleDeleteOne = (id) => {
            axios.delete(`/questionlib/question/delete?id=${id}`)
                .then(res => {
                    ElMessage.success("删除数据成功");
                    filterQuestions();
                }).catch(err => {
                    console.log(err);
                    ElMessage.error("删除数据失败");
                });
        }

        // 转换科目列表支持cascader
        const convertQuestionType = (questionList) => {
            if (questionList) {
                return questionList.map((val) => {
                    var rObj = {}
                    rObj['key'] = val.id
                    rObj['value'] = val.id
                    rObj['label'] = val.name
                    rObj['children'] = convertQuestionType(val.child)
                    rObj['disabled'] = false
                    return rObj
                })
            }
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
                axios.delete('/questionlib/question/deletemany',{data: deleteIds})
                .then(res=>{
                    ElMessage.success('删除数据成功')
                    filterQuestions()
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
            tableRowClassName,
            questionClassTypeName,
            handlePageChange,
            handleCreate,
            handleDeleteOne,
            handleEdit,
            handleImport,
            cache,
            props,
            filterQuestions,
            quesImportorRef,
            convertQuestionType,
            handleSelectionChange,
            handleDeleteMany,
        };
    }
}
</script>