<template>
    <el-card class="function-card">
        <h2>数据统计</h2>
        <StaNum/>
    </el-card>

    <el-card class="function-card">
        <h2>快速查看</h2>
        <QuesList ref="quesListRef" :pageSize="10" :questionClass="0" :hasHeader="true" :importable="true" :allQuestionTypes="allQuestionTypes" :multiDeleteable="true"/>
    </el-card>
</template>

<script>
import StaNum from '../components/StaNum.vue'
import QuesList from '../components/QuesList.vue'
import { onMounted, reactive, ref, toRefs } from 'vue'
import axios from '../utils/axios'
import { ElMessage } from 'element-plus'
export default {
    name: "Index",
    components: {
        StaNum,
        QuesList,
    },
    setup(){
        const state = reactive({
            allQuestionTypes : []
        })

        const quesListRef = ref(null)

        onMounted(()=>{
            loadQuestionTypeTree()
        })
        
        // 加载选项列表并转换成为uimodel
        const loadQuestionTypeTree = ()=>{
            let url = '/questionType/all/tree'
            axios.get(url)
            .then(res => {
               state.allQuestionTypes = quesListRef.value.convertQuestionType(res.data)
            })
            .catch(err => {
                console.log(err)
                ElMessage.error(err)
            })
        }

        return{
            ...toRefs(state),
            quesListRef
        } 
    }
}
</script>

<style scoped>
.function-card{
    margin-bottom: 20px;
}
</style>
