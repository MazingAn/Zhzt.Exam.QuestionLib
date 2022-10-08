<template>
    <el-dialog :name="type == 'add' ? '添加文档' : '修改文档'" v-model="visible" width="600px">
        <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="100px">
            <el-form-item label="标题" prop="name">
                <el-input type="text" v-model="ruleForm.name"></el-input>
            </el-form-item>
            <el-form-item label="所属类别" prop="categoryId">
                <el-cascader  placeholder="选择类别" v-model="ruleForm.categoryId" :options="allDocumentCategories" :props="cascaderProps"
                    clearable />
            </el-form-item>
            <el-form-item label="文档">
                <el-upload drag style="width: 100%;" ref="uploadRef" :auto-upload="true"
                    action="/api/documentlib/document/upload" :before-upload="beforUpload" :data="ruleForm"
                    :on-success="onUploadSuccess">
                    <el-icon class="el-icon--upload">
                        <upload-filled />
                    </el-icon>
                    <div class="el-upload__text">
                        拖拽文件到此或 <em>点击此区域上传文件</em>
                    </div>
                    <template #tip>
                        <div class="el-upload__tip">
                            支持doc/docx/ppt/pptx/xls/xlsx/pdf等格式的文件！
                        </div>
                    </template>
                </el-upload>
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

export default {
    name: 'DOcumentAddDlg',
    props: {
        type: String,
        reload: Function,
        allDocumentCategories: Array,
    },
    setup(props) {
        const formRef = ref(null)
        const uploadRef = ref(null)
        const state = reactive({
            ruleForm: {
                name: "",
                categoryId : null,
                docUrl: "",
                thumb:"",
            },
            rules: {
                name: [{ required: true, message: '标题不能为空', trigger: 'change' }],
                categoryId: [{required : true, message:'请务必选择一个分类', trigger:'change'}]
            },
            id: '',
            visible: false,
            allDocumentCategories: null,
        })

        const open = (currentData) => {
            state.visible = true
            if (currentData) {
                state.ruleForm.name = currentData.name
                state.ruleForm.thumb = currentData.thumb
                state.ruleForm.docUrl = currentData.docUrl
                state.ruleForm.categoryId = currentData.categoryId
                state.id = currentData.id
            } else {
                state.ruleForm = {
                    name: "",
                    categoryId : null,
                    docUrl: "",
                    thumb:"",
                }
            }
        }

        const close = () => {
            state.visible = false
        }

        const beforUpload = (rawFile) => {
            // if (rawFile.type !== 'video/mp4') {
            //     ElMessage.error('文件格式必须为mp4格式！')
            //     return false
            // } else if (rawFile.size / 1024 / 1024 > 2000) {
            //     ElMessage.error('请上传2GB以内的文件!')
            //     return false
            // }
            return true
        }

        const onUploadSuccess = (res, up, ups) => {
            if (res.success) {
                state.ruleForm.docUrl = res.data
            } else {
                ElMessage.error('视频上传失败')
            }
        }

        const submitForm = () => {
            formRef.value.validate((valid, fields) => {
                if (valid) {
                    if (props.type == 'add') {
                        // 添加方法
                        axios.post('/documentlib/document/create', state.ruleForm)
                            .then(() => {
                                ElMessage.success('添加成功')
                                state.visible = false
                                // 接口回调之后，运行重新获取列表方法 reload
                                if (props.reload) props.reload()
                            })
                    } else {
                        // 修改方法
                        axios.put('/documentlib/document/update', {
                            id : state.id,
                            name : state.ruleForm.name,
                            categoryId : state.ruleForm.categoryId,
                            docUrl: state.ruleForm.docUrl,
                            thumb : state.ruleForm.thumb
                        })
                        .then(() => {
                            ElMessage.success('修改成功')
                            state.visible = false
                            // 接口回调之后，运行重新获取列表方法 reload
                            if (props.reload) props.reload()
                        })
                    }
                    uploadRef.value.clearFiles()
                } else {
                    ElMessage.error("请确保内容填写正确")
                }
            })
        }
        
        watch(
            () => props.allDocumentCategories,
            (newVal, oldVal) => {
                state.allDocumentCategories = newVal
            }
        )

        return {
            ...toRefs(state),
            formRef,
            open,
            close,
            cascaderProps,
            submitForm,
            beforUpload,
            onUploadSuccess,
            uploadRef
        }
    }
}
</script>