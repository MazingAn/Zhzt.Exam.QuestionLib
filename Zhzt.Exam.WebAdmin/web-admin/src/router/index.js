import { createRouter, createWebHashHistory } from "vue-router";

import Index from '../views/Index.vue'
import QuesType from '../views/QuesType.vue'
import SingleChoice from '../views/SingleChoice.vue'
import MultiChoice from '../views/MultiChoice.vue'
import Judge from '../views/Judge.vue'
import BlankFill from '../views/BlankFill.vue'
import QuesAnswer from '../views/QuesAnswer.vue'
import Paper from '../views/Paper.vue'
import NounParsing from '../views/NounParsing.vue'
import ComputeQues from '../views/ComputeQues.vue'
import EssayQues from '../views/EssayQues.vue'

const router = createRouter({
    history: createWebHashHistory(),
    routes:[
        {
            path: '/',
            name: 'index',
            component: Index
        },
        {
            path: '/quesType',
            name: 'quesType',
            component: QuesType
        },
        {
            path: '/singleChoice',
            name: 'singleChoice',
            component: SingleChoice
        },
        {
            path: '/multiChoice',
            name: 'multiChoice',
            component: MultiChoice
        },
        {
            path: '/judge',
            name: 'judge',
            component: Judge
        },
        {
            path: '/blankFill',
            name: 'blankFill',
            component: BlankFill
        },
        {
            path: '/quesAnswer',
            name: 'quesAnswer',
            component: QuesAnswer
        },
        {
            path: '/computeQues',
            name: 'conputeQues',
            component: ComputeQues
        },
        {
            path: '/essayQues',
            name: 'essayQues',
            component: EssayQues
        },
        {
            path: '/nounParsing',
            name: 'nounParsing',
            component: NounParsing
        },
        {
            path: '/paper',
            name: 'paper',
            component: Paper
        }
    ]
})

export default router