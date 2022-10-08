<template>
  <Header/>
  <Banner :banner-info="bannerInfo" />
  <div class="main">
    <div class="content">
      <router-view />
    </div>
  </div>
  <Footer/>
</template>

<script>
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'
import Banner from './components/Banner.vue'
import { reactive, toRefs } from 'vue'

export default {
    name: "App",
    setup() {
      const state = reactive({
        bannerInfo: {
          title: "飞行原理在线学习",
          subtitle: "理论基础 / 飞行技能 / 迈向战场的第一步",
          description: "这是一门能够帮助你查漏补缺，快速寻找理论支撑的网站",
          img:"/banner.png"
        }
      })
      return{
        ...toRefs(state)
      }
    },
    components: { Header, Footer, Banner }
}
</script>

<style scoped>
.main{
  width: 100%;
  display: flex;
  justify-content: center;
}
.content{
  width: 1200px;
  margin: 20px 0;
}
</style>