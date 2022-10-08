import { createRouter, createWebHashHistory } from "vue-router";

import Index from '../views/Index.vue'
import Login from '../views/Login.vue'
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
import VideoCategory from '../views/VideoCategory.vue' 
import MicroClass from '../views/MicroClass.vue'
import Document from "../views/Document.vue"
import DocumentCategory from '../views/DocumentCategory.vue'

const router = createRouter({
    history: createWebHashHistory(),
    routes:[
        {
            path: '/',
            name: 'index',
            component: Index
        },
        {
            path : '/login',
            name: 'login',
            component: Login
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
        },
        {
            path: '/videoCategory',
            name: 'videoCategory',
            component: VideoCategory
        },
        {
            path: '/microclass',
            name: 'microclass',
            component: MicroClass
        },
        {
            path: '/document',
            name: 'document',
            component: Document
        },
        {
            path: '/documentCategory',
            name: 'documentCategory',
            component: DocumentCategory
        }
    ]
})

export default router