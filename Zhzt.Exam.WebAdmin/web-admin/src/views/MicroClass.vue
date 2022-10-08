<template>
    <el-card>
        <MicroClassVideoList ref="microClassVideoListRef" :hasHeader="true" :tableTitle="'微课列表'" :pageSize="15" :questionClass="1"
            :editable="true" :importable="true" :createable="true" :doEdit="handleEdit" :doCreate="handleCreate" 
            :allVideoCategories="allVideoCategories" :multiDeleteable="true"/>
    </el-card>
    <MicroClassVideoAddDlg ref="quesEditRef" :reload="reload" :allVideoCategories="allVideoCategories" :type="editType"/>
</template>

<script>
import { onMounted, reactive, ref, toRefs } from "vue";
import { ElMessage } from "element-plus";
import MicroClassVideoList from "../components/MicroClassVideoList.vue";
import MicroClassVideoAddDlg from "../components/MicroClassVideoAddDlg.vue";
import axios from "../utils/axios";

export default {
    name: 'MicroClass',
    components: { MicroClassVideoList, MicroClassVideoAddDlg },
    setup() {
        const microClassVideoListRef = ref(null)
        const quesEditRef = ref(null)

        const state = reactive({
            allVideoCategories : [],
            editType: 'add'
        })

        onMounted(()=>{
            loadQuestionTypeTree()
        })

        // 加载选项列表并转换成为uimodel
        const loadQuestionTypeTree = ()=>{
            let url = '/microclasslib/videocategory/all/tree'
            axios.get(url)
            .then(res => {
               state.allVideoCategories = microClassVideoListRef.value.convertVideoCategoty(res.data)
            })
            .catch(err => {
                console.log(err)
                ElMessage.error(err)
            })
        }

        // 重新加载列表
        const reload = ()=>{
            microClassVideoListRef.value.filterMicroClassVideos()
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
            microClassVideoListRef,
            quesEditRef,
            handleEdit,
            handleCreate,
            reload
        }
    }
}

</script>