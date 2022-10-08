<template>
    <div class="video-content">

        <div class="video-player">
            <videoPlay  v-bind="options" :poster='cache.videoBaseUrl + course.thumb' />
        </div>
        <div class="video-list">
            <span class="header">相关视频：</span>
            <ul>
                <li v-for="item in videos" :key="item.id" @click="handleClick(item)">
                    {{item.title}}
                </li>
            </ul>
        </div>
    </div>
</template>

<script>
import 'vue3-video-play/dist/style.css'
import { videoPlay } from 'vue3-video-play'
import { onMounted, reactive, toRefs, ref } from 'vue';
import axios from '../utils/axios';
import { useRoute, useRouter } from 'vue-router';
import cache from '../utils/cache';
export default {
    name: 'CourseContent',
    components: { videoPlay },
    setup() {
        
        const route = useRoute()
        const router = useRouter()


        const state = reactive({
            course: route.query,
            options: {
                width: '890px', //播放器高度
                height: '570px', //播放器高度
                color: "#409eff", //主题色
                title: '测试视频001', //视频名称
                src: cache.videoBaseUrl + route.query.videoUrl, //视频源
                muted: false, //静音
                webFullScreen: false,
                speedRate: ["0.75", "1.0", "1.25", "1.5", "2.0"], //播放倍速
                autoPlay: false, //自动播放
                loop: false, //循环播放
                mirror: false, //镜像画面
                ligthOff: false,  //关灯模式
                volume: 0.3, //默认音量大小
                control: true, //是否显示控制器
            },
            videos : [],
        })



        onMounted(()=>{
            loadRelaVideos(route.query.videoCategoryId)
        })

        const handleClick = (item)=>{
            router.push({
                path:'/courseContent',
                query: item
            })
            let video = document.getElementsByTagName("video")[0]
            video.pause()
            video.src = cache.videoBaseUrl + item.videoUrl
            video.poster = cache.videoBaseUrl + item.thumb
            video.load()
        }

        const loadRelaVideos = (id)=>{
            axios.post('microclasslib/MicroClassVideo/filterpage', {
                pageIndex: 1,
                pageSize: 20,
                categoryId: id
            })
                .then(res => {
                    console.log(res.data.pageData)
                    state.videos = res.data.pageData
                })
        }

        return {
            ...toRefs(state),
            cache,
            handleClick
        }
    }

}
</script>

<style scoped>
.video-content {
    width: 1200px;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
}

.video-content .video-player {
    flex: 1;
    height: 570px;
}

.video-content .video-list {
    width: 300px;
    height: 570px;
    overflow: hidden;
}

.video-content .video-list ul{
    margin: 0;
    padding: 0;
    list-style: none;
    height: 535px;
    margin-top: 10px;
    overflow-y: scroll;
}

.video-list header{
    color: #333;
    font-weight: 400;
}

.video-content .video-list li{
    width: 100%;
    height: 35px;
    color: #999;
    border-bottom: 1px  #ccc dashed;
    line-height: 35px;
    cursor: pointer;
}

</style>