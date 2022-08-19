<template>
    <el-dialog :title="type == 'add' ? '添加试卷' : '修改试卷'" v-model="visible" width="650px">
        <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="150px">
            <el-form-item label="试卷名称" prop="name">
                <el-input type="text" v-model="ruleForm.name"></el-input>
            </el-form-item>
            <el-form-item label="所属科目" prop="ruleForm.subject.subjectId">
                <el-cascader v-model="ruleForm.subject.subjectId" placeholder="选择科目" :options="allQuestionTypes" :props="cascaderProps"
                    @change="handleSubjectChange" clearable />
            </el-form-item>
            <el-form-item label="总分设置" prop="ruleForm.pagerConfig.totalScore">
                <el-input-number v-model="ruleForm.pagerConfig.totalScore" :precision="2" :step="5" :max="1000" />
            </el-form-item>
            <el-form-item label="单项选择题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.singleChoiceTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.singleChoiceTotalScore" :precision="2" :step="5"
                            :max="1000"  @change="handleScoreChange" />
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.singleChoiceCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.singleChoiceCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>


            <el-form-item label="多项选择题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.multiChoiceTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.multiChoiceTotalScore" :precision="2" :step="5"
                            :max="1000" @change="handleScoreChange" />
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.multiChoiceCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.multiChoiceCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="判断题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.judgeTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.judgeTotalScore" :precision="2" :step="5"
                            :max="1000"  @change="handleScoreChange"/>
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.judgeCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.judgeCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="计算题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.computeTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.computeTotalScore" :precision="2" :step="5"
                            :max="1000"  @change="handleScoreChange"/>
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.computeCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.computeCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="名词解释题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.nounParsingTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.nounParsingTotalScore" :precision="2" :step="5"
                            :max="1000"  @change="handleScoreChange"/>
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.nounParsingCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.nounParsingCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="填空题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.blankFillTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.blankFillTotalScore" :precision="2" :step="5"
                            :max="1000"  @change="handleScoreChange"/>
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.blankFillCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.blankFillCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="问答题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.quesAnswereTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.quesAnswereTotalScore" :precision="2" :step="5"
                            :max="1000"  @change="handleScoreChange"/>
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.quesAnswerCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.quesAnswerCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="论述题设置">
                <el-col :span="11">
                    <el-form-item label="总分" prop="ruleForm.pagerConfig.essayTotalScore" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.essayTotalScore" :precision="2" :step="5"
                            :max="1000"  @change="handleScoreChange"/>
                    </el-form-item>
                </el-col>
                <el-col :span="2"></el-col>
                <el-col :span="11">
                    <el-form-item label="数量" prop="ruleForm.pagerConfig.essayCount" label-width="50">
                        <el-input-number v-model="ruleForm.pagerConfig.essayCount" :step="1" :max="1000" />
                    </el-form-item>
                </el-col>
            </el-form-item>

            <el-form-item label="重新抽取题目" prop="ruleForm.reGenerateQuestions">
                <el-switch v-model="ruleForm.reGenerateQuestions" inline-prompt active-text="是" inactive-text="否" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="visible = false">取 消</el-button>
                <el-button type="primary" @click="submitForm" :loading="isLoading">
                    <template #loading>
                        <div class="custom-loading">
                            <svg class="circular" viewBox="-10, -10, 50, 50">
                                <path class="path" d="
                            M 30 15
                            L 28 17
                            M 25.61 25.61
                            A 15 15, 0, 0, 1, 15 30
                            A 15 15, 0, 1, 1, 27.99 7.5
                            L 15 15
                        " style="stroke-width: 4px; fill: rgba(0, 0, 0, 0)" />
                            </svg>
                        </div>
                    </template>
                    确定
                </el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script>
import { ElMessage, ElMessageBox } from 'element-plus'
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
                    quesAnswereTotalScore: 0,
                    nounParsingCount: 0,
                    nounParsingTotalScore: 0,
                    essayCount: 0,
                    essayTotalScore: 0,
                    computeCount: 0,
                    computeTotalScore: 0
                },
                singleChoiceQuestions: [],
                multiChoiceQuestions: [],
                judgeQuestions: [],
                blankFillQuestions: [],
                quesAnswerQuestions: [],
                nounParsingQuestions: [],
                essayQuestions: [],
                computeQuestions: [],
                reGenerateQuestions: false,
                subjectIdBeforEdit: ''
            },
            rules: {
                name: [{ required: true, message: '试卷名称不能为空', trigger: 'change' }],
                'subject.subjectId': [{ required: true, message: '请选择一个科目类别', trigger: 'blur' }]
            },
            id: '',
            visible: false,
            allQuestionTypes: null,
            isLoading: false
        })

        const open = (currentData) => {
            state.visible = true
            if (currentData) {
                state.ruleForm = currentData
                state.subjectIdBeforEdit = currentData.subject.subjectId
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
                        quesAnswereTotalScore: 0,
                        nounParsingCount: 0,
                        nounParsingTotalScore: 0,
                        essayCount: 0,
                        essayTotalScore: 0,
                        computeCount: 0,
                        computeTotalScore: 0
                    },
                    singleChoiceQuestions: [],
                    multiChoiceQuestions: [],
                    judgeQuestions: [],
                    blankFillQuestions: [],
                    quesAnswerQuestions: [],
                    nounParsingQuestions: [],
                    essayQuestions: [],
                    computeQuestions: [],
                    reGenerateQuestions: false,
                }
            }
        }

        const close = () => {
            state.visible = false
        }

        const handleSubjectChange = (value) => {
            for (var i = 0; i < state.allQuestionTypes.length; i++) {
                let node = state.allQuestionTypes[i]
                let ret = findSubjectNodeById(node, value)
                if (ret !== undefined) {
                    state.ruleForm.subject.name = ret.name ?? "未知"
                    state.ruleForm.subject.parentId = ret.parentId ?? null
                    state.ruleForm.subject.children = ret.children ?? []
                    break
                }
            }
        }

        const findSubjectNodeById = (node, id) => {
            if (node.value === id) {
                let ret = {}
                ret["subjectId"] = node.value
                ret["name"] = node.label
                ret["parentId"] = node.parentId
                ret["children"] = node.children
                return ret
            } else {
                if (node?.children?.length > 0) {
                    for (var i = 0; i < node?.children?.length ?? 0; i++) {
                        let ret2 = findSubjectNodeById(node?.children[i], id)
                        if (ret2 !== undefined) return ret2
                    }
                } else {
                    return undefined
                }
            }
        }

        const submitForm = () => {
            formRef.value.validate((valid, fields) => {
                if (valid) {
                    state.isLoading = true
                    if (props.type == 'add') {
                        // 添加方法
                        axios.post('/paperlib/paper/create', state.ruleForm)
                            .then(() => {
                                ElMessage.success('添加成功')
                                state.visible = false
                                // 接口回调之后，运行重新获取列表方法 reload
                                if (props.reload) props.reload()
                                state.isLoading = false
                            }).catch(err => {
                                ElMessageBox.alert("更新数据失败,问题库中似乎没有足够的试题以供抽取！");
                                state.isLoading = false
                            })
                    } else {
                        // 修改方法
                        if (state.subjectIdBeforEdit !== state.ruleForm.subject.subjectId) {
                            state.ruleForm.reGenerateQuestions = true;
                        }
                        axios.put('/paperlib/paper/update', state.ruleForm).then(() => {
                            ElMessage.success('修改成功')
                            state.visible = false
                            // 接口回调之后，运行重新获取列表方法 reload
                            if (props.reload) props.reload()
                            state.isLoading = false
                        }).catch(err => {
                            ElMessageBox.alert("更新数据失败,问题库中似乎没有足够的试题以供抽取！");
                            state.isLoading = false
                        })
                    }
                } else {
                    ElMessage.error("请确保内容填写正确")
                }
            })
        }

        const handleScoreChange = ()=>{
            state.ruleForm.pagerConfig.totalScore = 
                state.ruleForm.pagerConfig.singleChoiceTotalScore + 
                state.ruleForm.pagerConfig.multiChoiceTotalScore +
                state.ruleForm.pagerConfig.judgeTotalScore +  
                state.ruleForm.pagerConfig.blankFillTotalScore +   
                state.ruleForm.pagerConfig.quesAnswereTotalScore +   
                state.ruleForm.pagerConfig.essayTotalScore +   
                state.ruleForm.pagerConfig.nounParsingTotalScore+   
                state.ruleForm.pagerConfig.computeTotalScore  
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
            handleSubjectChange,
            handleScoreChange
        }
    }
}
</script>

<style scoped>
.el-button .custom-loading .circular {
  margin-right: 6px;
  width: 18px;
  height: 18px;
  animation: loading-rotate 2s linear infinite;
}

.el-button .custom-loading .circular .path {
  animation: loading-dash 1.5s ease-in-out infinite;
  stroke-dasharray: 90, 150;
  stroke-dashoffset: 0;
  stroke-width: 2;
  stroke: var(--el-button-text-color);
  stroke-linecap: round;
}
</style>