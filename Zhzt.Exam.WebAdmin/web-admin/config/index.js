// config/index.js
export default {
    development: {
        baseUrl: '/api' // 开发代理地址
    },
    beta: {
        baseUrl: '/api' // 测试接口域名
    },
    release: {
        baseUrl: '//localhost:8080' // 正式接口域名
    }
}