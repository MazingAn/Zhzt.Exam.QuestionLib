const questionClassMap = {
    '1' : '单项选择题',
    '2' : '多项选择题',
    '3' : '判断题',
    '4' : '填空题',
    '5' : '问答题',
    '6' : '名词解释题',
    '7' : '计算题',
    '8' : '论述题',
}

const cache = {
    queryQuestionClassName : (id)=>{
        return questionClassMap[id] ?? '未知'
    },
    videoBaseUrl : 'http://127.0.0.1:9999/',
    docBaseUrl : 'http://127.0.0.1:9998/',
}

export default cache