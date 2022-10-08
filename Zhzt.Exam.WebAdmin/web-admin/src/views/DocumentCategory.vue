<template>
    <el-card>
        <DocumentCatList :pageSize="12" />
    </el-card>
</template>
    
<script>
import DocumentCatList from '../components/DocumentCatList.vue'
export default {
    name: 'DocumentCategory',
    components: { DocumentCatList }
}
</script>