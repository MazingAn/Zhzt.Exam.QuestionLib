<template>
    <div class="course-card" :style="{width: width, height: height, cursor:'pointer'}" @click="handleClick">
        <div class="course-img">
            <img :src="cache.videoBaseUrl + course.thumb" :style="{width:width}" />
        </div>
        <div class="course-title">
            {{ course.title }}
        </div>
        <div class="course-footer">
            <div>{{course.category?.name}}</div>
            <div>{{course.pubDate}}</div>
        </div>
    </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import cache from '../utils/cache';

export default {
    name: 'CourseCard',
    props: {
        course: Object,
        width: Number,
        height: Number,
    },
    setup(props) {
        const course = ref(props.course)
        const width = ref(props.width + 'px')
        const height = ref(props.height + 'px')

        const router = useRouter()

        const handleClick = ()=>{
            router.push({
                path:'/courseContent',
                query: props.course
            })
        }

        return {
            course,
            width,
            height,
            cache,
            handleClick
        }
    }
}
</script>

<style scoped>
.course-card {
    display: flex;
    flex-direction: column;
    box-shadow: 1px 1px 9px 0px #ccc;
    border-radius: 5px;
    overflow: hidden;
    margin: 20px 10px;
}

.course-img {
    flex: 1;
}

.course-title {
    padding: 5px 10px;
    height: 45px;
    font-size: 1.2em;
    color: #666;
    word-spacing: 1em;
}

.course-footer {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    padding: 10px;
    color: #ccc;
}
</style>