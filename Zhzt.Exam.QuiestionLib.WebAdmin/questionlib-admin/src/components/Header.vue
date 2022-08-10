<!--Header.vue-->
<template>
  <div class="header">
    <div class="left">
       <span style="font-size: 20px">{{ name }}</span>
    </div>
    <div class="right">--</div>
  </div>
</template>

<script>
import { reactive, toRefs } from 'vue';
import { useRouter } from 'vue-router';

export default {
  name: 'Header',
  setup(){
    const router = useRouter();
    const pathMap = {
      index : '控制台',
      quesType:'科目分类',
      singleChoice : '单选题',
      multiChoice : '多选题',
      judge: '判断题',
      blankFill: '填空题',
      quesAnswer: '问答题'
    }
    const states = reactive({
      name : '控制台'
    })

    router.afterEach((to)=>{
      const {id} = to.query
      states.name = pathMap[to.name]
    })

    return {
      ...toRefs(states)
    }
  }

}
</script>

<style scoped>
  .header {
    height: 50px;
    border-bottom: 1px solid #e9e9e9;
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 20px;
  }
</style>