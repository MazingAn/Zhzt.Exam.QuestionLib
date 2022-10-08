<template>
    <vue-pdf-embed :source="source"  class="vue-pdf-embed" :page="pageNum" />
</template>

<script>
import { onMounted, reactive, toRefs } from 'vue';
import { useRouter } from 'vue-router';
import cache from '../utils/cache';
import VuePdfEmbed from "vue-pdf-embed";
export default {
    name: 'CourseContent',
    components: { VuePdfEmbed, createLoadingTask },
    setup() {

        const router = useRouter()

        const state = reactive({
            source: cache.docBaseUrl + router.query.pdfUrl,
            pageNum: 1,
            scale: 1,
            numPages: 0,
        })

        onMounted(()=>{
            console.log(state.source)
            loadPdf()
        })

        const loadPdf = ()=>{
            const loadingTask = createLoadingTask(state.source);
            loadingTask.promise.then( pdf => {
                state.numPages = pdf.numPages;
            });
        }

        return {
            ...toRefs(state),
            cache
        }
    }

}
</script>

<style scoped>

</style>