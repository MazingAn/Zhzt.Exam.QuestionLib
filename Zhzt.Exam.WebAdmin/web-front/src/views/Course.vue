<template>
    <div class="common-filter">
        <div class="header">
            <img src="/video.png" width="60" height="60" />
            <h1>所有微课视频</h1>
        </div>
        <div class="filter">
            <div class="filter-title">
                方向
            </div>
            <div class="filter-choices">
                <ul>
                    <li v-for="item in mainVideoCategories" :key="item.id" :class="item.active?'active':''"
                        @click="handleMainCatClick(item)">
                        {{item.name}}
                    </li>
                </ul>
            </div>
        </div>
        <div class="filter">
            <div class="filter-title">
                类别
            </div>
            <div class="filter-choices">
                <ul>
                    <li v-for="item in subVideoCategories" :key="item.id" :class="item.active?'active':''"
                        @click="handleSubCatClick(item)">
                        {{item.name}}
                    </li>
                </ul>
            </div>
        </div>
    </div>

    <div class="img-text-grid-column-left-2-right">
        <div v-for="item in videos" :key="item.id">
            <course-card :course="item" :width="280" :height="260" />
        </div>
    </div>
    <el-pagination :page-size="pageSize" layout="prev, pager, next" :total="totalCount" :current-page="page"
        @current-change="handlePageChange" />

</template>

<script>
import axios from '../utils/axios';
import { onMounted, reactive, toRefs } from 'vue';
import CourseCard from '../components/CourseCard.vue';

export default {
    name: "Course",
    components: { CourseCard },
    setup() {
        const state = reactive({
            allCategories: [],
            mainVideoCategories: [],
            subVideoCategories: [],
            videos: [],
            selectedCatId: 0,
            page: 1,
            pageSize: 16,
            totalCount: 0,
        })

        onMounted(() => {
            loadVideoCategories()
            loadVideos();
        })

        const loadVideos = () => {
            axios.post('microclasslib/MicroClassVideo/filterpage', {
                pageIndex: state.page,
                pageSize: state.pageSize,
                categoryId: state.selectedCatId
            })
                .then(res => {
                    state.videos = res.data.pageData
                    state.totalCount = res.data.totalCount
                    state.page = res.data.pageIndex
                })
        }

        const loadVideoCategories = () => {
            axios.post("/microclasslib/VideoCategory/filter", { parentId: -1 })
                .then(res => {
                    state.allCategories = res.data
                    state.allCategories.forEach(item => {
                        item.active = false
                    })
                    state.mainVideoCategories = [{ name: "全部", active: true, id: "0" }]
                    state.mainVideoCategories = state.mainVideoCategories.concat(state.allCategories.filter(x => x.parentId == 0))
                    state.subVideoCategories = [{ name: "全部", active: true, id: "0" }]
                    state.subVideoCategories = state.subVideoCategories.concat(state.allCategories.filter(x => !(x.parentId == 0)))
                })
        }

        const handleMainCatClick = (item) => {
            state.mainVideoCategories.filter(x => x.active).forEach(x => x.active = false)
            item.active = true
            if (item.id == '0') {
                state.subVideoCategories = [{ name: "全部", active: true, id: "0" }]
                state.allCategories.filter(x => !(x.parentId == 0)).forEach(x => x.active = false)
                state.subVideoCategories = state.subVideoCategories.concat(state.allCategories.filter(x => !(x.parentId == 0)))
            } else {
                state.subVideoCategories = [{ name: "全部", active: true, id: "0" }]
                state.allCategories.filter(x => x.parentId == item.id).forEach(x => x.active = false)
                state.subVideoCategories = state.subVideoCategories.concat(state.allCategories.filter(x => x.parentId == item.id))
            }
            state.selectedCatId = item.id
            state.page = 1
            loadVideos()
        }

        const handleSubCatClick = (item) => {
            state.subVideoCategories.filter(x => x.active).forEach(x => x.active = false)
            item.active = true
            state.selectedCatId = item.id
            state.page = 1
            loadVideos()
        }

        const handlePageChange = (pageNumber) => {
            state.page = pageNumber
            loadVideos()
        }

        return {
            ...toRefs(state),
            handleMainCatClick,
            handleSubCatClick,
            handlePageChange
        }
    }
}
</script>

<style scoped>
.common-filter {
    display: flex;
    flex-direction: column;
    padding-bottom: 25px;
    border-bottom: 2px solid #ccc;
}

.common-filter .header {
    display: flex;
    flex-direction: row;
    justify-content: flex-start;
    align-items: center;
}

.common-filter .header h1 {
    font-size: 2em;
    font-weight: 400;
    padding: 5px;
}

.common-filter .filter {
    display: flex;
    flex-direction: row;
    height: auto;
    margin: 10px 0;
}

.common-filter .filter .filter-title {
    font-size: 1.3em;
    font-weight: 600;
    height: auto;
    display: flex;
    justify-items: flex-start;
    align-items: center;
}

.common-filter .filter .filter-choices {
    flex: 1;
    margin: 0 0 0 20px;
    height: auto;
}

.common-filter .filter .filter-choices ul {
    margin: 0;
    padding: 0;
    display: block;
    list-style: none;
    background: white;
    border-radius: 5px;
    overflow: hidden;
}

.common-filter .filter .filter-choices ul li {
    margin: 5px 6px;
    width: auto;
    float: left;
    height: 30px;
    padding: 0px 10px;
    line-height: 30px;
    border-radius: 2px;
    cursor: pointer;
    color: #666;
}

.common-filter .filter .filter-choices ul li.active {
    background: #e2f1ff;
    color: #1a8ef4;
    font-weight: 600;
}
</style>