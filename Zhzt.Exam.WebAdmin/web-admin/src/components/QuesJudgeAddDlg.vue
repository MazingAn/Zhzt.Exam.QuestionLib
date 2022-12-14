<template>
    <el-dialog :title="type == 'add' ? '添加题目' : '修改题目'" v-model="visible" width="600px">
        <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="100px">
            <el-form-item label="题干内容" prop="questionBody">
                <el-input type="textarea" v-model="ruleForm.questionBody"></el-input>
            </el-form-item>
            <el-form-item label="所属科目" prop="questionTypeId">
                <el-cascader v-model="ruleForm.questionTypeId" :options="allQuestionTypes" :props="cascaderProps"
                    clearable />
            </el-form-item>
            <el-form-item label="正确答案" prop="rightAnswer">
                <el-select v-model="ruleForm.rightAnswer">
                    <el-option v-for="item in choiceOptions" :key="item.value" :label="item.label"
                        :value="item.value" />
                </el-select>
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="visible = false">取 消</el-button>
                <el-button type="primary"  @click="submitForm">确 定</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script>
import { ElMessage } from 'element-plus'
import { reactive, ref, toRefs, watch } from 'vue'
import axios from '../utils/axios'

const cascaderProps = {
    checkStrictly: true,
    emitPath: false
}

const choiceOptions = [
    { value: "true", label: '正确' },
    { value: "false", label: '错误' },
]

export default {
    name: 'QuesJudgeAddDlg',
    props: {
        type: String,
        reload: Function,
        allQuestionTypes: Array,
    },
    setup(props) {
        const formRef = ref(null)
        const state = reactive({
            ruleForm: {
                questionBody: "",
                questionTypeId: "",
                questionClass: 3,
                rightAnswer: "",
            },
            rules: {
                questionBody: [{ required: true, message: '题干内容不能为空', trigger: 'change' }],
                questionTypeId: [{ required: true, message: '请选择一个科目类别', trigger: 'blur' }],
                rightAnswer: [{ required: true, message: '请设置正确答案', trigger: 'blur' }]
            },
            id: '',
            visible: false,
            allQuestionTypes: null,
            selectedRightAnswers: []
        })

        const open = (currentData) => {
            state.visible = true
            if (currentData) {
                state.ruleForm.questionBody = currentData.questionBody
                state.ruleForm.questionTypeId = currentData.questionTypeId
                state.ruleForm.rightAnswer = currentData.rightAnswer
                state.id = currentData.id
            } else {
                state.ruleForm = {
                    questionBody: "",
                    questionTypeId: "",
                    questionClass: 3,
                    rightAnswer: "",
                }
                state.selectedRightAnswers = []
            }
        }

        const close = () => {
            state.visible = false
        }

        const submitForm = () => {
            formRef.value.validate((valid, fields) => {
                if (valid) {
                    if (props.type == 'add') {
                        // 添加方法
                        axios.post('/questionlib/question/create', state.ruleForm)
                            .then(() => {
                                ElMessage.success('添加成功')
                                state.visible = false
                                // 接口回调之后，运行重新获取列表方法 reload
                                if (props.reload) props.reload()
                            })
                    } else {
                        // 修改方法
                        axios.put('/questionlib/question/update', {
                            id: state.id,
                            questionBody: state.ruleForm.questionBody,
                            questionTypeId: state.ruleForm.questionTypeId,
                            questionClass: state.ruleForm.questionClass,
                            rightAnswer: state.ruleForm.rightAnswer
                        }).then(() => {
                            ElMessage.success('修改成功')
                            state.visible = false
                            // 接口回调之后，运行重新获取列表方法 reload
                            if (props.reload) props.reload()
                        })
                    }
                } else {
                    ElMessage.error("请确保内容填写正确")
                }
            })
        }

        const handleRightAnswerChange = (val)=>{
            state.ruleForm.rightAnswer = val.join('$')
        }

        watch(
            () => props.allQuestionTypes,
            (newVal, oldVal) => {
                state.allQuestionTypes = newVal
            }
        )

        return {
            ...toRefs(state),
            formRef,
            open,
            close,
            cascaderProps,
            submitForm,
            choiceOptions,
            handleRightAnswerChange,
        }
    }
}
</script>