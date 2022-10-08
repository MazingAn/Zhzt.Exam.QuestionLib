<template>
  <div class="layout">
    <el-container class="container">
      <el-aside class="aside">
        <!--系统名称+logo-->
        <div class="head">
          <div>
            <!-- <el-icon :size="22" color="#FFFFFF">
              <SetUp />
            </el-icon> -->
            <span>后台管理系统</span>
          </div>
        </div>
        <!--一条为了美观的线条-->
        <div class="line" />
        <!--一个菜单-->
        <el-menu default-active="1" background-color="#222832" text-color="#fff" :router="true">
          <el-menu-item index="/">
            <template #title>
              <el-icon>
                <Trophy />
              </el-icon>
              <span>控制台</span>
            </template>
          </el-menu-item>
          <el-sub-menu index="1">
            <template #title>
              <el-icon>
                <Reading />
              </el-icon>
              <span>题目管理</span>
            </template>
            <el-menu-item-group>
              <template #title><span>分类信息</span></template>
              <el-menu-item index="/quesType">科目配置</el-menu-item>
            </el-menu-item-group>
            <el-menu-item-group title="问题内容">
              <el-menu-item index="/singleChoice">单项选择题</el-menu-item>
              <el-menu-item index="/multiChoice">多项选择题</el-menu-item>
              <el-menu-item index="/judge">判断题</el-menu-item>
              <el-menu-item index="/blankFill">填空题</el-menu-item>
              <el-menu-item index="/computeQues">计算题</el-menu-item>
              <el-menu-item index="/nounParsing">名词解释题</el-menu-item>
              <el-menu-item index="/essayQues">论述题</el-menu-item>
              <el-menu-item index="/quesAnswer">问答题</el-menu-item>
            </el-menu-item-group>
          </el-sub-menu>
          <el-menu-item index="/paper">
            <el-icon>
              <Promotion />
            </el-icon>
            <template #title>试卷管理</template>
          </el-menu-item>
          <el-sub-menu index="2">
            <template #title>
              <el-icon>
                <Reading />
              </el-icon>
              <span>微课管理</span>
            </template>
            <el-menu-item-group>
              <template #title><span>微课详情</span></template>
              <el-menu-item index="/videoCategory">分类配置</el-menu-item>
              <el-menu-item index="/microClass">微课视频</el-menu-item>
            </el-menu-item-group>
          </el-sub-menu>
          <el-sub-menu index="3">
            <template #title>
              <el-icon>
                <Document />
              </el-icon>
              <span>资料管理</span>
            </template>
            <el-menu-item-group>
              <template #title><span>资料详情</span></template>
              <el-menu-item index="/documentCategory">资料分类</el-menu-item>
              <el-menu-item index="/document">资料内容</el-menu-item>
            </el-menu-item-group>
          </el-sub-menu>
        </el-menu>
      </el-aside>
      <el-container class="content">
        <Header/>
        <div class="main">
          <router-view />
        </div>
        <Footer/>
      </el-container>
    </el-container>
  </div>
</template>

<script>
import { ref } from 'vue'
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'

const isCollapse = ref(false)

export default {
    name: "App",
    setup() {
    },
    components: { Header, Footer }
}
</script>

<style scoped>
.layout {
  min-height: 100vh;
  background-color: #ffffff;
}

.container {
  height: 100vh;
}

.aside {
  width: 200px !important;
  background-color: #222832;
}

.head {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 50px;
}

.head>div {
  display: flex;
  align-items: center;
}


.head span {
  font-size: 20px;
  color: #ffffff;
}

.line {
  border-top: 1px solid hsla(0, 0%, 100%, .05);
  border-bottom: 1px solid rgba(0, 0, 0, .2);
}

.content {
  display: flex;
  flex-direction: column;
  max-height: 100vh;
  overflow: hidden;
}

.main {
  height: calc(100vh - 100px);
  overflow: auto;
  padding: 10px;
}
</style>

<style>

body{
  margin: 0;
  padding: 0;
}

.el-menu-vertical-demo:not(.el-menu--collapse) {
  width: 200px;
  min-height: 400px;
}

.el-menu {
  border-right: none !important;
}

.el-submenu {
  border-top: 1px solid hsla(0, 0%, 100%, .05);
  border-bottom: 1px solid rgba(0, 0, 0, .2);
}

.el-submenu:first-child {
  border-top: none;
}

.el-submenu [class^="el-icon-"] {
  vertical-align: -1px !important;
}

a {
  color: #409eff;
  text-decoration: none;
}

.el-pagination {
  text-align: center;
  margin-top: 20px;
}

.el-popper__arrow {
  display: none;
}

</style>