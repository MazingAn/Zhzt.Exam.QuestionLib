<template>
    <el-card>
        <DocumentList ref="documentListRef" :hasHeader="true" :tableTitle="'微课列表'" :pageSize="15" :questionClass="1"
            :editable="true" :importable="true" :createable="true" :doEdit="handleEdit" :doCreate="handleCreate" 
            :allDocumentCategories="allDocumentCategories" :multiDeleteable="true"/>
    </el-card>
    <DocumentAddDlg ref="docEditRef" :reload="reload" :allDocumentCategories="allDocumentCategories" :type="editType"/>
</template>

<script>
import { onMounted, reactive, ref, toRefs } from "vue";
import { ElMessage } from "element-plus";
import DocumentList from "../components/DocumentList.vue";
import DocumentAddDlg from "../components/DocumentAddDlg.vue";
import axios from "../utils/axios";

export default {
    name: 'Document',
    components: { DocumentList, DocumentAddDlg },
    setup() {
        const documentListRef = ref(null)
        const docEditRef = ref(null)

        const state = reactive({
            allDocumentCategories : [],
            editType: 'add'
        })

        onMounted(()=>{
            loadQuestionTypeTree()
        })

        // 加载选项列表并转换成为uimodel
        const loadQuestionTypeTree = ()=>{
            let url = '/documentlib/documentcategory/all/tree'
            axios.get(url)
            .then(res => {
               state.allDocumentCategories = documentListRef.value.convertCategory(res.data)
            })
            .catch(err => {
                console.log(err)
                ElMessage.error(err)
            })
        }

        // 重新加载列表
        const reload = ()=>{
            documentListRef.value.filterDocuments()
        }

        // 创建
        const handleCreate = ()=>{
            state.editType = 'add'
            docEditRef.value.open()
        }
        
        // 修改
        const handleEdit = (data)=>{
            state.editType = 'edit'
            docEditRef.value.open(data)
        }

        return {
            ...toRefs(state),
            documentListRef,
            docEditRef,
            handleEdit,
            handleCreate,
            reload
        }
    }
}

</script>