import { createRouter, createWebHashHistory } from "vue-router";

import Index from '../views/Index.vue'
import Course from '../views/Course.vue'
import CourseContent from '../views/CourseContent.vue'
import Document from '../views/Document.vue'
import DocumentContent from '../views/DocumentContent.vue'

const router = createRouter({
    history: createWebHashHistory(),
    routes:[
        {
            path: '/',
            name: 'index',
            component: Index
        },
        {
            path: '/course',
            name: 'course',
            component: Course
        },
        {
            path: '/courseContent',
            name: 'courseContent',
            component: CourseContent
        },
        {
            path: '/document',
            name: 'document',
            component: Document
        },
        {
            path: '/documentContent',
            name: 'documentContent',
            component: DocumentContent
        },
    ]
})

export default router