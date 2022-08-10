<template>
<el-card>
    <QuesTypeList :pageSize="15"/>
</el-card>
</template>

<script>
import QuesTypeList from '../components/QuesTypeList.vue'
export default{
    name : 'QuesType',
    components: {QuesTypeList}
}
</script>