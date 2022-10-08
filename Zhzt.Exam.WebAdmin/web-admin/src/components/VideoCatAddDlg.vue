<template>
    <el-dialog :title="type == 'add' ? '添加类别' : '修改类别'" v-model="visible" width="400px">
        <el-form :model="ruleForm" :rules="rules" ref="formRef" label-width="100px" >
            <el-form-item label="类别名称" prop="name">
                <el-input type="text" v-model="ruleForm.name"></el-input>
            </el-form-item>
            <el-form-item label="上级类别" prop="parentId">
                <el-cascader placeholder="选择类别" v-model="ruleForm.parentId" :options="allCategories" :props="cascaderProps" clearable />
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
import { onMounted, reactive, ref, toRefs, watch } from 'vue'
import axios from '../utils/axios'

const cascaderProps = {
    checkStrictly: true,
    emitPath: false
}

export default {
    name: 'VideoCatAddDlg',
    props: {
        type: String,
        reload: Function,
        allCategories: Array,
    },
    setup(props) {
        const formRef = ref(null)
        const state = reactive({
            ruleForm: {
                name: '',
                parentId: '0'
            },
            rules: {
                name: [{ required: true, message: '名称不能为空', trigger: 'change' }],
                parentId:[{ required: true, message: '请选择一个分类', trigger: 'blur'}]
            },
            id:'',
            visible: false,
            allCategories: null
        })

        const open = (currentData) => {
            state.visible = true
            if (currentData) {
                state.ruleForm.name = currentData.name
                state.ruleForm.parentId = currentData.parentId
                state.id = currentData.id
            } else {
                state.ruleForm = {
                    name: '',
                    parentId: '0',
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
                        axios.post('/microclasslib/videocategory/create', {
                            name : state.ruleForm.name,
                            parentId: state.ruleForm.parentId
                        })
                        .then(() => {
                            ElMessage.success('添加成功')
                            state.visible = false
                            // 接口回调之后，运行重新获取列表方法 reload
                            if (props.reload) props.reload()
                        })
                    } else {
                        // 修改方法
                        axios.put('/microclasslib/videocategory/update', {
                            id: state.id,
                            name: state.ruleForm.name,
                            parentId: state.ruleForm.parentId,
                        }).then(() => {
                            ElMessage.success('修改成功')
                            state.visible = false
                            // 接口回调之后，运行重新获取列表方法 reload
                            if (props.reload) props.reload()
                        })
                    }
                }else{
                    ElMessage.error("请确保内容填写正确")
                }
            })
        }


        const convertQuestionType = (questionList) => {
            if(questionList){
                return questionList.map((val) => {
                    var rObj = {}
                    rObj['key'] = val.id
                    rObj['value'] = val.id
                    rObj['label'] = val.name
                    rObj['children'] = convertQuestionType(val.child)
                    rObj['disabled'] = false
                    return rObj
                })
            }
        }

        const disableSelf = (theOptions, id, forceDisable=false)=>{
            if(theOptions){
                return theOptions.map((val) => {
                    var rObj = {}
                    rObj['key'] = val.value
                    rObj['value'] = val.value
                    rObj['label'] = val.label
                    if(forceDisable){
                        rObj['disabled'] = true
                    }else{                    
                        rObj['disabled'] = (val.value === id)
                    }
                    rObj['children'] = disableSelf(val.children, id, rObj['disabled'])
                    return rObj
                })
            }else{
                return []
            }
        }

        const unDisableAll = (theOptions)=>{
            if(theOptions){
                return theOptions.map((val) => {
                    var rObj = {}
                    rObj['key'] = val.value
                    rObj['value'] = val.value
                    rObj['label'] = val.label
                    rObj['disabled'] = false
                    rObj['children'] = disableSelf(val.children)
                    return rObj
                })
            }else{
                return []
            }
        }

        watch(
            () => props.allCategories,
            (newVal, oldVal) => {
                state.allCategories = newVal
            }
        )

        return {
            ...toRefs(state),
            formRef,
            open,
            close,
            convertQuestionType,
            cascaderProps,
            submitForm,
            disableSelf,
            unDisableAll
        }
    }
}
</script>