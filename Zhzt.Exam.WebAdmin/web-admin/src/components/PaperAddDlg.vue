<template>
    <el-dialog :title="type == 'add' ? '添加试卷' : '修改试卷'" v-model="visible">
        <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="150px">
            <el-form-item label="试卷名称" prop="name">
                <el-input type="text" v-model="ruleForm.name"></el-input>
            </el-form-item>
            <el-form-item label="所属科目" prop="ruleForm.subject.subjectId">
                <el-cascader v-model="ruleForm.subject.subjectId" :options="allQuestionTypes" :props="cascaderProps"
                    clearable />
            </el-form-item>
            <el-form-item label="总分设置" prop="ruleForm.pagerConfig.totalScore">
                <el-input-number v-model="ruleForm.pagerConfig.totalScore" :precision="2" :step="5" :max="1000" />
            </el-form-item>
            <el-form-item label="选择题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.singleChoiceTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.singleChoiceTotalScore" :precision="2"
                            :step="5" :max="1000" />
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.singleChoiceCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.singleChoiceCount" :step="1"
                            :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>


            <el-form-item label="多项选择题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.multiChoiceTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.multiChoiceTotalScore" :precision="2" :step="5"
                            :max="1000" />
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.multiChoiceCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.multiChoiceCount" :step="1"
                            :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="判断题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.judgeTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.judgeTotalScore" :precision="2" :step="5"
                            :max="1000" />
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.judgeCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.judgeCount" :step="1"
                            :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="填空题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.blankFillTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.blankFillTotalScore" :precision="2" :step="5"
                            :max="1000" />
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.blankFillCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.blankFillCount" :step="1"
                            :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="问答题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.quesAnswereTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.quesAnswereTotalScore" :precision="2" :step="5"
                            :max="1000" />
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.quesAnswerCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.quesAnswerCount" :step="1"
                            :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="重新抽取题目" prop="ruleForm.pagerConfig.reGenerateQuestions">
                <el-switch v-model="ruleForm.pagerConfig.reGenerateQuestions" inline-prompt active-text="是"
                    inactive-text="否" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="visible = false">取 消</el-button>
                <el-button type="primary" @click="submitForm">确 定</el-button>
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
                id: null,
                name: "",
                creator: {},
                subject: {
                    name: "",
                    subjectId: "",
                    parentId: "",
                    children: []
                },
                pagerConfig: {
                    totalScore: 0,
                    singleChoiceCount: 0,
                    singleChoiceTotalScore: 0,
                    multiChoiceCount: 0,
                    multiChoiceTotalScore: 0,
                    judgeCount: 0,
                    judgeTotalScore: 0,
                    blankFillCount: 0,
                    blankFillTotalScore: 0,
                    quesAnswerCount: 0,
                    quesAnswereTotalScore: 0
                },
                singleChoiceQuestions: [],
                multiChoiceQuestions: [],
                judgeQuestions: [],
                blankFillQuestions: [],
                quesAnswerQuestions: [],
                reGenerateQuestions: false,
            },
            rules: {
                name: [{ required: true, message: '试卷名称不能为空', trigger: 'change' }],
                'ruleForm.subject.subjectId': [{ required: true, message: '请选择一个科目类别', trigger: 'blur' }]
            },
            id: '',
            visible: false,
            allQuestionTypes: null,
        })

        const open = (currentData) => {
            state.visible = true
            if (currentData) {
                state.ruleForm = currentData
            } else {
                state.ruleForm = {
                    id: null,
                    name: "",
                    creator: {},
                    subject: {
                        name: "",
                        subjectId: "",
                        parentId: "",
                        children: []
                    },
                    pagerConfig: {
                        totalScore: 0,
                        singleChoiceCount: 0,
                        singleChoiceTotalScore: 0,
                        multiChoiceCount: 0,
                        multiChoiceTotalScore: 0,
                        judgeCount: 0,
                        judgeTotalScore: 0,
                        blankFillCount: 0,
                        blankFillTotalScore: 0,
                        quesAnswerCount: 0,
                        quesAnswereTotalScore: 0
                    },
                    singleChoiceQuestions: [],
                    multiChoiceQuestions: [],
                    judgeQuestions: [],
                    blankFillQuestions: [],
                    quesAnswerQuestions: [],
                    reGenerateQuestions: false,
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
                        axios.post('/paperlib/paper/create', state.ruleForm)
                            .then(() => {
                                ElMessage.success('添加成功')
                                state.visible = false
                                // 接口回调之后，运行重新获取列表方法 reload
                                if (props.reload) props.reload()
                            })
                    } else {
                        // 修改方法
                        axios.put('/paperlib/paper/update', ruleForm).then(() => {
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
        }
    }
}
</script>