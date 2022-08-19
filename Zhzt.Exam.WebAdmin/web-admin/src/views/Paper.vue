<template>
    <el-card>
        <PaperList ref="plistRef" :hasHeader="true" :PageSize="12" 
        :editable="true" :createable="true"
        :doEdit="editPaper" :doCreate="createPaper" />
    </el-card>
    <PaperAddDlg ref="dlgRef" :reload="reload" :type="editType" :allQuestionTypes="allQuestionTypes"/>
</template>

<script>
import PaperList from '../components/PaperList.vue';
import PaperAddDlg from '../components/PaperAddDlg.vue';
import { toRefs, reactive, ref, onMounted } from 'vue';
import axios from '../utils/axios';
import { ElMessage } from 'element-plus';
export default {
    name: "Paper",
    components: { PaperList, PaperAddDlg},
    setup(){
        
        onMounted(()=>{
            loadQuestionTypeTree()
        })

        const state = reactive({
            editType : 'open',
            allQuestionTypes: []
        })

        const dlgRef = ref(null)
        const plistRef = ref(null)

        const editPaper = (row)=>{
            state.editType = 'edit'
            dlgRef.value.open(row)
        }

        const createPaper = ()=>{
            state.editType = 'add'
            dlgRef.value.open()
        }

        const reload = ()=>{
            plistRef.value.filterPapers()
        }

        // 加载选项列表并转换成为uimodel
        const loadQuestionTypeTree = ()=>{
            let url = '/questionlib/questiontype/all/tree'
            axios.get(url)
            .then(res => {
                state.allQuestionTypes = convertQuestionType(res.data)
            })
            .catch(err => {
                console.log(err)
                ElMessage.error(err)
            })
        }

        // 转换科目列表支持cascader
        const convertQuestionType = (questionList) => {
            if (questionList) {
                return questionList.map((val) => {
                    var rObj = {}
                    rObj['key'] = val.id
                    rObj['value'] = val.id
                    rObj['label'] = val.name
                    rObj['parentId'] = val.parentId
                    rObj['children'] = convertQuestionType(val.child)
                    rObj['disabled'] = false
                    return rObj
                })
            }
        }

        return{
            ...toRefs(state),
            editPaper,
            createPaper,
            reload,
            dlgRef,
            plistRef,
        }
    }
}
</script>