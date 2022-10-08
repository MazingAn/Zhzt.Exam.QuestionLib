<template>
    <div style="margin-bottom:60px">
        <h1>热门课程</h1>
        <div class="more">
            <router-link to="/course">更多></router-link>
        </div>
        <div class="img-text-grid-column">
            <div v-for="item in courseList" :key="item.id">
                <course-card :course="item" :width="280" :height="260" />
            </div>
        </div>

        <h1>参考资料</h1>
        <div class="more">
            <router-link to="/document">更多></router-link>
        </div>
        <div class="img-text-grid-column">
            <div v-for="item in docList" :key="item.id">
                <document-card :document="item" :width="220" :height="400" />
            </div>
        </div>

        <h1>理论题库</h1>
        <div class="more">
            <router-link to="/course">更多></router-link>
        </div>
        <div class="img-text-grid-column">
            <div v-for="(item, idx) in quesCountList" :key="item.name" class="ques-card">
                <a href="#">
                    <el-card :class="getQuesCardClass(idx)"
                        :style="{width: '285px', height: '190px', margin: '10px 0'}">
                        <div class="ques-counter-header">
                            <div style="width: 80px; height: 80px;">
                                <component :is="getQuesCardIcon(item.name)"></component>
                            </div>
                            <div style="font-size: 1.6em;">
                                {{item.name}}
                            </div>
                        </div>
                        <div class="ques-counter-footer">
                            <p>收录题目<span>{{item.count}}</span>道</p>
                        </div>
                    </el-card>
                </a>
            </div>
        </div>

        <h1>新闻资讯</h1>
        <div class="more">
            <a href="#">更多></a>
        </div>
        <div class="img-text-grid-column-left-2-right">
            <div class="left-news">
                <div v-for="item in newsList" :key="item.id" class="news-col">
                    <div class="news-line">
                        <a href="#">{{ subStrWithComment(item.title, 24)}}</a>
                        <span>{{item.pubDate}}</span>
                    </div>
                </div>
            </div>
            <div class="right-news">
                <div v-for="item in newsList" :key="item.id" class="news-col">
                    <div class="news-line">
                        <a href="#">{{ subStrWithComment(item.title, 24)}}</a>
                        <span>{{item.pubDate}}</span>
                    </div>
                </div>
            </div>
        </div>

    </div>
</template>

<script>
import { onMounted, reactive, toRefs } from 'vue';
import { CircleCheck, Open, SuccessFilled, Comment, Promotion, EditPen, QuestionFilled, Ship, Watch } from '@element-plus/icons-vue';
import CourseCard from '../components/CourseCard.vue';
import DocumentCard from '../components/DocumentCard.vue';
import axios from '../utils/axios';
import cache from '../utils/cache';
export default {
    name: "Index",
    components: { CourseCard, DocumentCard },
    setup() {
        const state = reactive({
            courseList: [],
            docList: [],
            quesCountList: [],
            newsList: [
                { title: "南部战区空军航空兵某旅开展飞行训练", pubDate: "2022-9-19", id: 0 },
                { title: "空中最高礼仪！祖国用“双20”接迎志愿军烈士回家", pubDate: "2022-9-19", id: 1 },
                { title: "空降兵某部红色网课提升教育质效", pubDate: "2022-9-19", id: 2 },
                { title: "空军航空兵某旅：战鹰翱翔，搏击长空守安宁", pubDate: "2022-9-19", id: 3 },
                { title: "空降兵某旅“模范空降兵连”排长程强：英雄传人 续写新篇", pubDate: "2022-9-19", id: 4 },
                { title: "南部战区空军航空兵某旅组织飞行训练", pubDate: "2022-9-19", id: 5 },
                { title: "“模范空降兵连”发扬光荣传统，英雄的火炬永不熄", pubDate: "2022-9-19", id: 6 }
            ]
        })

        onMounted(()=>{
            loadVideos()
            loadDocuments()
            for(let i = 1; i < 9; i++){
                getQuestionCount(i)
            }
        })

        // 加载最新视频
        const loadVideos = ()=>{
            axios.get('/microclasslib/MicroClassVideo/page?pageIndex=1&pageSize=8')
            .then(res=>{
                console.log(res.data)
                state.courseList = res.data.pageData
            })
        }

        // 加载最新资料
        const loadDocuments = ()=>{
            axios.get('/documentlib/document/page?pageIndex=1&pageSize=10')
            .then(res=>{
                state.docList = res.data.pageData
            })
        }

        // 加载题目数量
        const getQuestionCount = (questionClassId) => {
            axios.post('/questionlib/question/count/filter',
            {
                "questionClass": questionClassId
            })
            .then(res=>{
                let quesClassName = cache.queryQuestionClassName(questionClassId)
                state.quesCountList.push({name:quesClassName, count: res.data})
            })
        }


        const getQuesCardClass = (idx) => {
            switch (idx) {
                case 0:
                    return 'card-blue'
                case 1:
                    return 'card-pink'
                case 2:
                    return 'card-purple'
                case 3:
                    return 'card-cyan'
                case 4:
                    return 'card-green'
                case 5:
                    return 'card-orange'
                case 6:
                    return 'card-blue'
                case 7:
                    return 'card-pink'
                default:
                    return 'card-blue'
            }
        }

        const getQuesCardIcon = (name) => {
            switch (name) {
                case "单项选择题":
                    return CircleCheck
                case "多项选择题":
                    return Open
                case "判断题":
                    return SuccessFilled
                case "填空题":
                    return EditPen
                case "问答题":
                    return QuestionFilled
                case "名词解释题":
                    return Promotion
                case "计算题":
                    return Watch
                case "论述题":
                    return Comment
                default:
                    return Ship
            }
        }

        const subStrWithComment = (value, count)=>{
            return value.length > count ? value.slice(0,count-1) + '...' : value;
        }

        return {
            ...toRefs(state),
            getQuesCardClass,
            getQuesCardIcon,
            subStrWithComment
        }
    }
}
</script>

<style scoped>
h1 {
    font-size: 2em;
    font-weight: 500;
    width: 150px;
    display: block;
    text-align: center;
    margin: 30px auto;
    padding: 10px 0;
    letter-spacing: 0.15em;
    border-bottom: 8px solid #72beff;
}

.more {
    display: flex;
    justify-content: flex-end;
    margin-top: -30px;
}

.more a {
    text-decoration: none;
    color: #ccc;
}

.ques-card a {
    text-decoration: none;
}

.ques-card .ques-counter-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.ques-card .ques-counter-footer {
    display: flex;
    justify-content: flex-end;
    align-items: center;
    font-size: 1.3em;
}

.ques-card .ques-counter-footer span {
    font-weight: 700;
    font-size: 1.5em;
}

.left-news {
    border-right: 2px dashed #ccc;
}

.news-col{
    width: 580px;
    flex: 1;
}

.news-line{
    margin: 15px ;
    display: flex;
    justify-content: space-between;
    color: #999;
    font-size: 1.1em;
}

.news-line a{
    color: #333;
    text-decoration: none;
}
</style>