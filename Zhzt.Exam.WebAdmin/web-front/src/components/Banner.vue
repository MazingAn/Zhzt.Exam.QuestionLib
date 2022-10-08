<template>
    <div class="banner">
        <div class="banner-content">
            <div class="banner-left">
                <h2>{{info.title}}</h2>
                <h3>{{info.subtitle}}</h3>
                <p>{{info.description}}</p>
                <button>立即学习</button>
            </div>
            <div class="banner-right">
                <img :src="info.img" :alt="info.title" />
            </div>
        </div>
    </div>
</template>

<script>
import { ref } from 'vue';

export default {
    name: "Banner",
    props: {
        bannerInfo: Object
    },
    setup(props) {
        const info = ref(props.bannerInfo)
        return {
            info
        }
    }
}
</script>

<style scoped>
.banner {
    width: 100%;
    height: 400px;
    margin: 0;
    padding: 0;
    background: linear-gradient(65deg, #6fb3ff 0%, #6fbbff 50%, #8fdbff 100%);
    display: flex;
    justify-content: center;
    align-items: center;
}

.banner .banner-content{
    width: 1200px;
    display: flex;
    margin: 0;
    padding: 0;
    margin: 0;
    padding: 0;
    align-items: center;
    justify-content: space-between;
}

h2{
    font-size: 2em;
    color: white ;
    font-weight: 100 ;
}


h3{
    font-size: 1.6em;
    color: white ;
    font-weight: 100 ;
}

p{
    font-size: 1em;
    color: white;
    font-weight: 100;
    margin: 30px 0;
}

button{
    border: white 1px solid;
    background: transparent;
    padding: 10px 50px;
    color: white;
    cursor: pointer;
}

button:hover{
    background: #cccccc33;
}

.banner-right img{
    width: auto;
    height: 350px;
}
</style>
