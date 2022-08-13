<template>
    <div class="question-number">
        <el-card class="order-item card-cyan">
            <template #header>
                <div class="card-header">
                    <span>收录题目数量</span>
                </div>
            </template>
            <div class="item">
                <el-icon class="icon">
                    <DataAnalysis />
                </el-icon>
                <span class="number">
                    {{ numbers.total }}
                </span>
            </div>
        </el-card>
        <el-card class="order-item card-orange">
            <template #header>
                <div class="card-header">
                    <span>单选题</span>
                </div>
            </template>
            <div class="item">
                <el-icon class="icon">
                    <Check />
                </el-icon>
                <span class="number">
                    {{ numbers.singleChoice }}
                </span>
            </div>
        </el-card>
        <el-card class="order-item card-green">
            <template #header>
                <div class="card-header">
                    <span>多选题</span>
                </div>
            </template>
            <div class="item">
                <el-icon class="icon">
                    <FolderChecked />
                </el-icon>
                <span class="number">
                    {{ numbers.multiChoice }}
                </span>
            </div>
        </el-card>
        <el-card class="order-item card-blue">
            <template #header>
                <div class="card-header">
                    <span>判断题</span>
                </div>
            </template>
            <div class="item">
                <el-icon class="icon">
                    <Finished />
                </el-icon>
                <span class="number">
                    {{ numbers.judge }}
                </span>
            </div>
        </el-card>
        <el-card class="order-item card-pink">
            <template #header>
                <div class="card-header">
                    <span>填空题</span>
                </div>
            </template>
            <div class="item">
                <el-icon class="icon">
                    <Edit />
                </el-icon>
                <span class="number">
                    {{ numbers.blankFill }}
                </span>
            </div>
        </el-card>
        <el-card class="order-item card-purple">
            <template #header>
                <div class="card-header">
                    <span>问答题</span>
                </div>
            </template>
            <div class="item">
                <el-icon class="icon">
                    <Tickets />
                </el-icon>
                <span class="number">
                    {{ numbers.answerQuestion }}
                </span>
            </div>
        </el-card>
    </div>
</template>

<script>
import axios from 'axios'
import { onMounted, reactive, toRefs } from 'vue'
import cache from '../utils/cache'

export default {
    name: 'StaNum',
    setup() {
        const states = reactive({
            numbers: {
                total: 78998,
                singleChoice: 5678,
                multiChoice: 8761,
                blankFill: 7612,
                judge: 17622,
                answerQuestion: 26151
            }
        })

        onMounted(() => {
            for(var i = 0; i <=5; i++){
                getQuestionCount(i)
            }
        })

        const getQuestionCount = (questionClassId) => {
            axios.post('/questionlib/question/count/filter',
            {
                "questionClass": questionClassId
            })
            .then(res=>{
                switch(questionClassId){
                    case 0:
                        states.numbers.total = res.data;
                        break;
                    case 1:
                        states.numbers.singleChoice = res.data;
                        break;
                    case 2:
                        states.numbers.multiChoice = res.data;
                        break;
                    case 3:
                        states.numbers.judge = res.data;
                        break;
                    case 4:
                        states.numbers.blankFill = res.data;
                        break;
                    case 5:
                        states.numbers.answerQuestion = res.data;
                        break;
                }
            })
        }

        return {
            ...toRefs(states)
        }
    }
}
</script>


<style scoped>
.question-number {
    display: flex;
    margin-bottom: 10px;
}

.question-number .order-item {
    flex: 1;
    margin-right: 20px;
}

.question-number .order-item:last-child {
    margin-right: 0;
}

.item {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
}

.item .icon {
    font-size: 48px;
}

.item .number {
    font-size: 28px;
    line-height: 48px;
}
</style>
