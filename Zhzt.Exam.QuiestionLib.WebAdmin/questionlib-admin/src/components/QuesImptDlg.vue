<template>
    <el-dialog :title="'导入题库'" v-model="visible" width="600px">
        <el-form :model="ruleForm" ref="formRef" label-width="100px">
            <el-form-item label="所属科目" prop="questionTypeId">
                <el-cascader v-model="ruleForm.questionTypeId" :options="allQuestionTypes" :props="cascaderProps"
                    clearable />
            </el-form-item>
            <el-form-item label="题库文件">
                <el-upload drag style="width: 100%;"
                    action=cache.fileUploadAction>
                    <el-icon class="el-icon--upload">
                        <upload-filled />
                    </el-icon>
                    <div class="el-upload__text">
                        拖拽文件到此或 <em>点击此区域上传文件</em>
                    </div>
                    <template #tip>
                        <div class="el-upload__tip">
                            请上传符合题库模板格式的excel文件！
                        </div>
                    </template>
                </el-upload>
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
import cache from '../utils/cache'

const cascaderProps = {
    checkStrictly: true,
    emitPath: false
}


export default {
    name: 'QuesImptDlg',
    props: {
        reload: Function,
        allQuestionTypes: Array,
    },
    setup(props) {
        const formRef = ref(null)
        const state = reactive({
            ruleForm: {
                questionTypeId : ""
            },
            id: '',
            visible: false,
            allQuestionTypes: null
        })

        const open = () => {
            state.visible = true

        }

        const close = () => {
            state.visible = false
        }

        const submitForm = () => {

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
            cahce
        }
    }
}
</script>