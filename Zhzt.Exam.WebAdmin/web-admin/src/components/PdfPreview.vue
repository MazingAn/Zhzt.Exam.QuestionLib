<template>
    <el-dialog v-model="visible">
        <div class="pdf-preview">
            <div class="pdf-wrap">
                <vue-pdf-embed :source="source"  class="vue-pdf-embed" :page="pageNum" />
            </div>
            <div class="page-tool">
                <div class="page-tool-item" @click="lastPage">上一页</div>
                <div class="page-tool-item" @click="nextPage">下一页</div>
                <div class="page-tool-item">{{pageNum}}/{{numPages}}</div>
                <div class="page-tool-item" @click="pageZoomIn">放大</div>
                <div class="page-tool-item" @click="pageZoomOut">缩小</div>
            </div>
        </div>
    </el-dialog>
</template>
<script>
import VuePdfEmbed from "vue-pdf-embed";
import { createLoadingTask } from "vue3-pdfjs/esm"; // 获得总页数
import { reactive, toRefs } from "vue";

export default {
    name: 'PdfPreview',
    components:{VuePdfEmbed},
    setup() {

        const state = reactive({
            source: "",
            pageNum: 1,
            scale: 1,
            numPages: 0,
            visible: false
        })

        const open = (source)=>{
            state.source = source
            state.visible = true
            reload()
        }

        const close = ()=>{
            state.visible = false
        }

        const reload = ()=>{
            const loadingTask = createLoadingTask(state.source);
            loadingTask.promise.then( pdf => {
                state.numPages = pdf.numPages;
            });
        }

        const lastPage = ()=> {
            if (state.pageNum > 1) {
                state.pageNum -= 1;
            }
        }
        const nextPage = ()=> {
            if (state.pageNum < state.numPages) {
                state.pageNum += 1;
            }
        }

        const pageZoomOut = ()=> {
            if (state.scale < 2) {
                state.scale += 0.1;
            }
        }

        const pageZoomIn = ()=> {
            if (state.scale > 1) {
                state.scale -= 0.1;
            }
        }


        return {
            ...toRefs(state),
            pageZoomIn,
            pageZoomOut,
            lastPage,
            nextPage,
            open,
            close,
        }
    }
}

</script>
<style lang="css" scoped>
.pdf-preview {
    position: relative;
    height: 700px;
    padding: 20px 0;
    box-sizing: border-box;
    background: rgb(66, 66, 66);
}

.vue-pdf-embed {
    text-align: center;
    width: 480px;
    border: 1px solid #e5e5e5;
    margin: 0 auto;
    box-sizing: border-box;
}

.pdf-wrap{
    overflow-y:auto ;
}
.page-tool {
    position: absolute;
    bottom: 35px;
    padding-left: 15px;
    padding-right: 15px;
    display: flex;
    align-items: center;
    background: rgb(66, 66, 66);
    color: white;
    border-radius: 19px;
    z-index: 100;
    cursor: pointer;
    margin-left: 50%;
    transform: translateX(-50%);
}
.page-tool-item {
    padding: 8px 15px;
    padding-left: 10px;
    cursor: pointer;
}
</style>