<template>
    <el-card>
        <QuesList ref="quesListRef" :hasHeader="true" :tableTitle="'论述题题目列表'" :pageSize="10" :questionClass="8"
            :editable="true" :importable="true" :createable="true" :doEdit="handleEdit" :doCreate="handleCreate"
            :allQuestionTypes="allQuestionTypes" :multiDeleteable="true"/>
    </el-card>
    <QuesEssayQuesAddDlg ref="quesEditRef" :reload="reload" :allQuestionTypes="allQuestionTypes" :type="editType"/>
</template>

<script>
import { onMounted, reactive, ref, toRefs } from "vue";
import { ElMessage } from "element-plus";
import QuesList from "../components/QuesList.vue";
import axios from "../utils/axios";
import QuesEssayQuesAddDlg from "../components/QuesEssayQuesAddDlg.vue";

export default {
    name: 'QuesAnswer',
    components: { QuesList, QuesEssayQuesAddDlg },
    setup() {
        const quesListRef = ref(null)
        const quesEditRef = ref(null)

        const state = reactive({
            allQuestionTypes : [],
            editType: 'add'
        })

        onMounted(()=>{
            loadQuestionTypeTree()
        })

        // 加载选项列表并转换成为uimodel
        const loadQuestionTypeTree = ()=>{
            let url = '/questionlib/questiontype/all/tree'
            axios.get(url)
            .then(res => {
               state.allQuestionTypes = quesListRef.value.convertQuestionType(res.data)
            })
            .catch(err => {
                console.log(err)
                ElMessage.error(err)
            })
        }

        // 重新加载列表
        const reload = ()=>{
            quesListRef.value.filterQuestions()
        }

        // 创建
        const handleCreate = ()=>{
            state.editType = 'add'
            quesEditRef.value.open()
        }
        
        // 修改
        const handleEdit = (data)=>{
            state.editType = 'edit'
            quesEditRef.value.open(data)
        }

        return {
            ...toRefs(state),
            quesListRef,
            quesEditRef,
            handleEdit,
            handleCreate,
            reload
        }
    }
}

</script>