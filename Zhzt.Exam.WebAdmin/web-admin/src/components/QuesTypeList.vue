<!-- 题库列表组件 -->
<!-- 使用示范
    <QuesTypeList :pageSize="10" :questionClass="1"  :questionTypeId="questionTypeId" />
-->
<template>
    <div class="table-header">
        <div class="left">
            <h3>科目列表</h3>
        </div>
        <div class="right">
            <el-button type="primary" @click="handleCreate">
                新建科目
            </el-button>
        </div>
    </div>
    <el-table :data="tableData" style="width: 100%" :border="true" :tree-props="treeTbaleProps" row-key="id">
        <el-table-column prop="name" label="科目名称">
        </el-table-column>
        <el-table-column label="包含下级">
            <template #default="scope">
                <el-tag :type="scope.row.child.length > 0 ? 'success' : 'error'">
                    {{scope.row.child.length > 0  ? '是' : '否'}}
                </el-tag>
            </template>
        </el-table-column>
        <el-table-column prop="createTime" label="添加时间" >
        </el-table-column>
        <el-table-column prop="updateTime" label="最后修改时间" >
        </el-table-column>
        <el-table-column label="操作" width="260">
            <template #default="scope">
                <a style="cursor: pointer; margin-right: 10px" @click="handleEdit(scope.row)">修改</a>
                <el-popconfirm title="将连带删除所有子科目，确定删除吗？" @confirm="handleDeleteOne(scope.row.id)">
                    <template #reference>
                        <a style="cursor: pointer">删除</a>
                    </template>
                </el-popconfirm>
            </template>
        </el-table-column>
    </el-table>
    <el-pagination background layout="prev, pager, next" class="center-pagination" v-model:currentPage="pageIndex"
        v-model:page-size="pageSize" :total="totalCount" :pageSize="pageSize" @current-change="handlePageChange" />
    <QuestTypeAddDlg ref="quesTypeAddDlgRef" :type="editType" :reload="reload" :allQuestionTypes="allQuestionTypes" />
</template>

<script>
import { ElMessage } from 'element-plus'
import { onMounted, reactive, toRefs, ref } from 'vue'
import axios from '../utils/axios'
import QuestTypeAddDlg from './QuestTypeAddDlg.vue'

const treeTbaleProps = {
    children: 'child'
}

export default {
    name: "QuesTypeList",
    props: {
        pageSize: Number
    },
    components: { QuestTypeAddDlg },
    setup(props) {
        const quesTypeAddDlgRef = ref(null)
        const state = reactive({
            tableData: [],
            pageIndex: 1,
            pageSize: props.pageSize ?? 10,
            questionClass: props.questionClass ?? 0,
            questionType: props.questionType ?? 0,
            totalCount: 0,
            editType: 'add',
            allQuestionTypes: []
        })

        // 挂载方法
        onMounted(() => {
            loadQuestionTypes()
            loadAllQuestionTypes()
        })

        const reload = () => {
            loadQuestionTypes()
            loadAllQuestionTypes()
        }

        // 分页加载问题分类列表 支持筛选
        const loadQuestionTypes = () => {
            let url = `/questionType/page/tree?pageIndex=${state.pageIndex}&pageSize=${state.pageSize}`
            axios.get(url)
                .then(res => {
                    state.tableData = res.data.pageData
                    state.pageIndex = res.data.pageIndex
                    state.pageSize = res.data.pageSize
                    state.totalCount = res.data.totalCount
                })
                .catch(err => {
                    ElMessage.warning(err)
                })
        }

        // 加载所有问题分类列表
        const loadAllQuestionTypes = () => {
            let url = '/questionType/all/tree'
            axios.get(url)
                .then(res => {
                    state.allQuestionTypes = quesTypeAddDlgRef.value.convertQuestionType(res.data)
                    state.allQuestionTypes.push({ value: '0', label: "不设置" })
                })
                .catch(err => {
                    ElMessage.error(err)
                })
        }

        // 切换页码
        const handlePageChange = (newPageIndex) => {
            state.pageIndex = newPageIndex
            loadQuestionTypes()
        }

        // 删除单个
        const handleDeleteOne = (id) => {
            axios.delete(`/questiontype/delete?id=${id}`)
                .then(res => {
                    ElMessage.success("删除成功！")
                    reload()
                }).catch(err => {
                    ElMessage.warning("删除失败");
                })
        }

        // 编辑
        const handleEdit = (row) => {
            state.editType = 'edit'
            state.allQuestionTypes = quesTypeAddDlgRef.value.disableSelf(state.allQuestionTypes, row.id)
            quesTypeAddDlgRef.value.open(row)
        }

        // 创建
        const handleCreate = () => {
            state.editType = 'add'
            state.allQuestionTypes = quesTypeAddDlgRef.value.unDisableAll(state.allQuestionTypes)
            quesTypeAddDlgRef.value.open()
        }

        return {
            ...toRefs(state),
            handlePageChange,
            handleDeleteOne,
            handleEdit,
            handleCreate,
            loadQuestionTypes,
            quesTypeAddDlgRef,
            reload,
            treeTbaleProps,
        }

    }
}
</script>