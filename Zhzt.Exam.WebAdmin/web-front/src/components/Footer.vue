<!--Footer.vue-->
<template>
  <div class="footer">
    <div class="footer-middle">
      <div class="footer-middle-content">
        <div class="footer-col">
          <h1>我是学员</h1>
          <div class="under-line"></div>
          <ul>
            <li><a href="#">如何学习</a></li>
            <li><a href="#">如何查找资料</a></li>
          </ul>
        </div>
        <div class="footer-col">
          <h1>我是教员</h1>
          <div class="under-line"></div>
          <ul>
            <li><a href="#">如何发布课程</a></li>
          </ul>
        </div>
        <div class="footer-col">
          <h1>友情链接</h1>
          <div class="under-line"></div>
          <ul>
            <li><a href="#">学院官网</a></li>
          </ul>
        </div>
        <div class="footer-col">
          <h1>新手指南</h1>
          <div class="under-line"></div>
          <ul>
            <li><a href="#">常见问题</a></li>
            <li><a href="#">联系我们</a></li>
            <li><a href="#">账号分配和申请</a></li>
            <li><a href="#">考试系统使用指南</a></li>
          </ul>
        </div>
        <div class="footer-col">
          <h1>系统管理</h1>
          <div class="under-line"></div>
          <ul>
            <li><a href="#">进入后台</a></li>
          </ul>
        </div>
      </div>
    </div>
    <div class="footer-end">
      <span>Copyright © 2019-2021 ZHZT Col All rights reserved.</span>
    </div>
  </div>
</template>

<script>
export default {
  name: 'Footer'
}
</script>

<style scoped>
.footer {
  width: 100%;
}

.footer-middle {
  width: 100%;
  height: 260px;
  background: #2a2a2a;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  padding: 30px 0;
}

.footer-middle .footer-middle-content {
  width: 1200px;
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
}

.footer-col h1{
  font-size: 1.5em;
  color: #EEEEEE;
  font-weight: 400;
}

.footer-col .under-line{
  width: 30px;
  border-bottom: 5px solid #EEEEEE;
}

.footer-col ul{
  list-style: none;
  padding: 0;
  margin: 20px 0;
}

.footer-col ul li{
  height: 40px;
}

.footer-col ul li a{
  color: #aaaaaa;
  text-decoration: none;
}

.footer-end {
  width: 100%;
  height: 50px;
  background: #000000;
  display: flex;
  justify-content: center;
  align-items: center;
}

.footer-end span {
  color: #999999;
}
</style>