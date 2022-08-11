<template>
    <el-dialog :title="'导入题库'" v-model="visible" width="600px">
        <el-form :model="ruleForm" ref="formRef" label-width="100px">
            <el-form-item label="所属科目" prop="questionTypeId">
                <el-cascader v-model="ruleForm.questionTypeId" :options="allQuestionTypes" :props="cascaderProps"
                    clearable />
            </el-form-item>
            <el-form-item label="题库文件">
                <el-upload drag style="width: 100%;"
                ref="uploadRef"
                 :auto-upload="false" 
                 action="/api/question/import" 
                 :before-upload="beforExcelUpload"
                 :data="ruleForm"
                 :on-success="onUploadSuccess">
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
        const uploadRef = ref(null)
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
            state.ruleForm.questionTypeId = ""
            state.visible = false
        }

        const submitForm = () => {
            uploadRef.value.submit()
        }

        const beforExcelUpload = (rawFile) => {
            if (state.ruleForm.questionTypeId == ""){
                ElMessage.error('请务必选择一个科目')
                return false
            }
            if (rawFile.type !== 'application/vnd.ms-excel') {
                 ElMessage.error('文件格式必须为excel格式!')
                 return false
            } else if (rawFile.size / 1024 / 1024 > 200) {
                 ElMessage.error('请上传200MB以内的文件!')
                 return false
            }
            return true
        }

        const onUploadSuccess = (res,up,ups)=>{
            if(res.success){
                ElMessage.success(`成功导入${res.data}项`)
            }else{
                ElMessage.error('数据导入失败')
            }
            if(props.reload){props.reload()}
            close()
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
            beforExcelUpload,
            onUploadSuccess,
            uploadRef,
        }
    }
}
</script>