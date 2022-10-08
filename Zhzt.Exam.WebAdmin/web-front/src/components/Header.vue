<!--Header.vue-->
<template>
  <div class="header">
    <div class="navbar">
      <div class="logo">
        <img src="/logo.webp" />
      </div>
      <div class="menu">
        <el-menu :default-active="route.path" mode="horizontal" :router="true">
          <el-menu-item index="/">首页</el-menu-item>
          <el-menu-item index="/course">微课视频</el-menu-item>
          <el-menu-item index="/document">学习资料</el-menu-item>
          <el-menu-item index="4">考试题库</el-menu-item>
          <el-menu-item index="5">新闻资讯</el-menu-item>
        </el-menu>
      </div>
      <div class="search-bar">
        <el-input class="w-50 m-2" size="large" placeholder="搜索" :suffix-icon="Search" />
      </div>
      <div class="user-info">
        <a href="#">登录</a>
      </div>
    </div>
  </div>
</template>

<script>
import { Search } from '@element-plus/icons-vue';
import { useRoute } from 'vue-router';

export default {
  name: 'Header',
  components : {Search},
  setup(){
    const route = useRoute()
    return{
      route
    }
  }
}
</script>

<style scoped>
.header {
  height: 80px;
  background: #ffffff;
  border-bottom: 1px solid #e9e9e9;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 0;
  padding: 0;
}

.header .navbar {
  width: 1200px !important;
  height: 80px;
  display: flex;
  margin: 0;
  padding: 0;
}

.header .navbar .logo {
  width: 80px;
  height: 80px;
  padding: 0;
  margin: 0;
  overflow: hidden;
}

.header .navbar .logo img {
  width: 80px;
  height: 80px;
}

.menu {
  height: 80px;
  flex: 1;
  justify-items: flex-end;
}

.el-menu--horizontal {
  height: 80px !important;
  border: 0;
  margin: 0;
  padding: 0;    
  flex: 1;
  justify-content: flex-end;
}

.search-bar{
  width: 260px;
  height: 80px;
  display: flex;
  justify-content: center;
  align-items: center;
  margin: 0 20px;
}
.user-info {
  height: 80px;
  margin: 0;
  padding: 0;
  display: flex;
  align-items: center;
  justify-content: flex-end;
}

a{
  text-decoration: none;
}
</style>

<style>
.search-bar .el-input--large .el-input__wrapper {
    border-radius: 20px !important;
}
</style>