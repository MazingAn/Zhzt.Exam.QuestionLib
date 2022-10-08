<template>
    <div :class="randomColor()" @click="handleClick">
        <div class="title">{{document.name}}</div>
        <div class="subtitle">{{document.docCategory?.name}}</div>
        <div class="author"></div>
        <div class="publish">{{document.createTime}}</div>
    </div>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import cache from '../utils/cache';

export default {
    name: 'DocumentCard',
    props: {
        document: Object,
        width: Number,
        height: Number,
    },
    setup(props) {
        const document = ref(props.document)
        const width = ref(props.width + 'px')
        const height = ref(props.height + 'px')

        const colorArray = ['card-blue', 'card-pink', 'card-purple', 'card-orange', 'card-cyan'] 

        const router = useRouter()

        const handleClick = () => {
            router.push({
                path: '/documentContent',
                query: props.document
            })
        }

        const randomColor = ()=>{
            return 'book ' + colorArray[Math.floor(Math.random() * colorArray.length)];
        }

        return {
            document,
            width,
            height,
            cache,
            handleClick,
            randomColor
        }
    }
}
</script>

<style scoped>
.book {
    margin: 10px;
    padding: 10px;
    color: white;
    width: 200px;
    height: 280px;
    border-radius: 5px 10px 10px 5px;
    float: left;
    cursor: pointer;
}


.title {
    padding-top: 10px;
    font-size: 24px;
    font-weight: bold;
    text-align: center;
}

.subtitle {
    padding: 10px 0px 0px 70px;
    font-size: 14px;
    font-weight: bold;
}

.author {
    font-size: 9px;
    padding: 5px;
}

.publish {
    font-size: 9px;
    padding-top: 150px;
    padding-left: 50px;
}
</style>