import { createRouter, createWebHashHistory } from "vue-router";

import Index from '../views/Index.vue'
import QuesType from '../views/QuesType.vue'
import SingleChoice from '../views/SingleChoice.vue'
import MultiChoice from '../views/MultiChoice.vue'
import Judge from '../views/Judge.vue'
import BlankFill from '../views/BlankFill.vue'
import QuesAnswer from '../views/quesAnswer.vue'

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
        }
    ]
})

export default router