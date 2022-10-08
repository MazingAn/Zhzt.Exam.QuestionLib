<template>
    <el-dialog :title="video?.title" v-model="visible" width="500px" @close="close">
        <video ref="myVideo" width="450" height="280" autoplay controls>
            <source type="video/mp4" :src="video?.videoUrl">
        </video>
    </el-dialog>
</template>

<script>
import { clone } from 'lodash'
import { reactive, toRefs, ref, onMounted } from 'vue'
import cache from '../utils/cache'


export default {
    name: 'VideoPreviewDlg',
    setup() {
        const state = reactive({
            visible: false,
            video: null
        })

        const myVideo = ref(null)

        const open = (video) => {
            console.log(video)
            console.log(state.video)
            state.visible = true
            state.video = clone(video)
            state.video.videoUrl = cache.videoBaseUrl + state.video.videoUrl
            myVideo.value.src = state.video.videoUrl
        }

        const close = () => {
            state.visible = false
            myVideo.value.currentTime = 0
            myVideo.value.pause()
            state.video = null
        }

        return {
            ...toRefs(state),
            open,
            close,
            cache,
            myVideo
        }
    }
}
</script>