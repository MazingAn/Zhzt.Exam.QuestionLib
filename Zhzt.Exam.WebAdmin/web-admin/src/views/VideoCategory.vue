<template>
    <el-card>
        <VideoCatList :pageSize="12" />
    </el-card>
</template>
    
<script>
import VideoCatList from '../components/VideoCatList.vue'
export default {
    name: 'VideoCategory',
    components: { VideoCatList }
}
</script>