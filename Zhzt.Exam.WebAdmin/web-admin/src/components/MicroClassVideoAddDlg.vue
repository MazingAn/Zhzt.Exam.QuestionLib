<template>
    <el-dialog :title="type == 'add' ? '添加微课' : '修改微课'" v-model="visible" width="600px">
        <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="100px">
            <el-form-item label="标题" prop="title">
                <el-input type="text" v-model="ruleForm.title"></el-input>
            </el-form-item>
            <el-form-item label="描述" prop="description">
                <el-input type="text" v-model="ruleForm.description"></el-input>
            </el-form-item>
            <el-form-item label="所属类别" prop="videoCategoryId">
                <el-cascader  placeholder="选择类别" v-model="ruleForm.videoCategoryId" :options="allVideoCategories" :props="cascaderProps"
                    clearable />
            </el-form-item>
            <el-form-item label="视频文件">
                <el-upload drag style="width: 100%;" ref="uploadRef" :auto-upload="true"
                    action="/api/microclasslib/microclassvideo/upload" :before-upload="beforUpload" :data="ruleForm"
                    :on-success="onUploadSuccess">
                    <el-icon class="el-icon--upload">
                        <upload-filled />
                    </el-icon>
                    <div class="el-upload__text">
                        拖拽文件到此或 <em>点击此区域上传文件</em>
                    </div>
                    <template #tip>
                        <div class="el-upload__tip">
                            请上传符合题库模板格式的MP4文件！
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
    name: 'MicroClassVideoAddDlg',
    props: {
        type: String,
        reload: Function,
        allVideoCategories: Array,
    },
    setup(props) {
        const formRef = ref(null)
        const uploadRef = ref(null)
        const state = reactive({
            ruleForm: {
                title: "",
                videoCategoryId : null,
                description: "",
                videoUrl: "",
                thumb:"",
            },
            rules: {
                title: [{ required: true, message: '标题不能为空', trigger: 'change' }],
                videoCategoryId: [{required : true, message:'请务必选择一个分类', trigger:'change'}]
            },
            id: '',
            visible: false,
            allVideoCategories: null,
        })

        const open = (currentData) => {
            state.visible = true
            if (currentData) {
                state.ruleForm.title = currentData.title
                state.ruleForm.description = currentData.description
                state.ruleForm.thumb = currentData.thumb
                state.ruleForm.videoUrl = currentData.videoUrl
                state.ruleForm.videoCategoryId = currentData.videoCategoryId
                state.id = currentData.id
            } else {
                state.ruleForm = {
                    title: "",
                    videoCategoryId : null,
                    description: "",
                    videoUrl: "",
                    thumb:"",
                }
            }
        }

        const close = () => {
            state.visible = false
        }

        const beforUpload = (rawFile) => {
            if (rawFile.type !== 'video/mp4') {
                ElMessage.error('文件格式必须为mp4格式！')
                return false
            } else if (rawFile.size / 1024 / 1024 > 2000) {
                ElMessage.error('请上传2GB以内的文件!')
                return false
            }
            return true
        }

        const onUploadSuccess = (res, up, ups) => {
            if (res.success) {
                state.ruleForm.videoUrl = res.data
            } else {
                ElMessage.error('视频上传失败')
            }
        }

        const submitForm = () => {
            formRef.value.validate((valid, fields) => {
                if (valid) {
                    if (props.type == 'add') {
                        // 添加方法
                        axios.post('/microclasslib/microclassvideo/create', state.ruleForm)
                            .then(() => {
                                ElMessage.success('添加成功')
                                state.visible = false
                                // 接口回调之后，运行重新获取列表方法 reload
                                if (props.reload) props.reload()
                            })
                    } else {
                        // 修改方法
                        axios.put('/microclasslib/microclassvideo/update', {
                            id : state.id,
                            title : state.ruleForm.title,
                            description: state.ruleForm.description,
                            videoCategoryId : state.ruleForm.videoCategoryId,
                            videoUrl: state.ruleForm.videoUrl,
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
            () => props.allVideoCategories,
            (newVal, oldVal) => {
                state.allVideoCategories = newVal
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