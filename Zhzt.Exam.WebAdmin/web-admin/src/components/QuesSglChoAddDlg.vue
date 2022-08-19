<template>
    <el-dialog :title="type == 'add' ? '添加题目' : '修改题目'" v-model="visible" width="600px">
        <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="100px">
            <el-form-item label="题干内容" prop="questionBody">
                <el-input type="textarea" v-model="ruleForm.questionBody"></el-input>
            </el-form-item>
            <el-form-item label="所属科目" prop="questionTypeId">
                <el-cascader  placeholder="选择科目" v-model="ruleForm.questionTypeId" :options="allQuestionTypes" :props="cascaderProps"
                    clearable />
            </el-form-item>
            <el-form-item label="A选项" prop="answer1">
                <el-input type="text" v-model="ruleForm.answer1"></el-input>
            </el-form-item>
            <el-form-item label="B选项" prop="answer2">
                <el-input type="text" v-model="ruleForm.answer2"></el-input>
            </el-form-item>
            <el-form-item label="C选项" prop="answer3">
                <el-input type="text" v-model="ruleForm.answer3"></el-input>
            </el-form-item>
            <el-form-item label="D选项" prop="answer4">
                <el-input type="text" v-model="ruleForm.answer4"></el-input>
            </el-form-item>
            <el-form-item label="E选项" prop="answer5">
                <el-input type="text" v-model="ruleForm.answer5"></el-input>
            </el-form-item>
            <el-form-item label="F选项" prop="answer6">
                <el-input type="text" v-model="ruleForm.answer6"></el-input>
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
    { value: "A", label: 'A选项' },
    { value: "B", label: 'B选项' },
    { value: "C", label: 'C选项' },
    { value: "D", label: 'D选项' },
    { value: "E", label: 'E选项' },
    { value: "F", label: 'F选项' },
]

export default {
    name: 'QuesSglChoAddDlg',
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
                questionClass: 1,
                answer1: "",
                answer2: "",
                answer3: "",
                answer4: "",
                answer5: "",
                answer6: "",
                rightAnswer: ""
            },
            rules: {
                questionBody: [{ required: true, message: '题干内容不能为空', trigger: 'change' }],
                questionTypeId: [{ required: true, message: '请选择一个科目类别', trigger: 'blur' }],
                answer1: [{ required: true, message: '选项A不能为空', trigger: 'blur' }],
                answer2: [{ required: true, message: '选项B不能为空', trigger: 'blur' }],
                rightAnswer: [{ required: true, message: '请设置正确答案', trigger: 'blur' }]
            },
            id: '',
            visible: false,
            allQuestionTypes: null,
        })

        const open = (currentData) => {
            state.visible = true
            if (currentData) {
                state.ruleForm.questionBody = currentData.questionBody
                state.ruleForm.questionTypeId = currentData.questionTypeId
                state.ruleForm.answer1 = currentData.answer1
                state.ruleForm.answer2 = currentData.answer2
                state.ruleForm.answer3 = currentData.answer3
                state.ruleForm.answer4 = currentData.answer4
                state.ruleForm.answer5 = currentData.answer5
                state.ruleForm.answer6 = currentData.answer6
                state.ruleForm.rightAnswer = currentData.rightAnswer
                state.id = currentData.id
            } else {
                state.ruleForm = {
                    questionBody: "",
                    questionTypeId: "",
                    questionClass: 1,
                    answer1: "",
                    answer2: "",
                    answer3: "",
                    answer4: "",
                    answer5: "",
                    answer6: "",
                    rightAnswer: ""
                }
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
                            questionClass: 1,
                            answer1: state.ruleForm.answer1,
                            answer2: state.ruleForm.answer2,
                            answer3: state.ruleForm.answer3,
                            answer4: state.ruleForm.answer4,
                            answer5: state.ruleForm.answer5,
                            answer6: state.ruleForm.answer6,
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
            choiceOptions
        }
    }
}
</script>