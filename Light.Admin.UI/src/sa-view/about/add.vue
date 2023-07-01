<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="650px" top="10vh" :append-to-body="true" :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm" label-width="120px">
                    <el-form-item :label="m.type ===3? '标题':'年份'" prop="title">
                        <el-input v-model="m.title"></el-input>
                    </el-form-item>
                    <el-form-item label="副标" v-if="m.type == 3" prop="info">
                        <el-input v-model="m.info"></el-input>
                    </el-form-item>
                    <el-form-item label="图片" prop="image">
                        <el-upload class="upload-demo" :action="sa.cfg.api_url + '/Attachment/upload?fileType=0&param=0'"
                         :multiple="false" 
                         :limit="10"
                           list-type="picture-card"
                          :on-success="success_image" 
                          :before-remove="remove_image"
                          :file-list="m.imageFile">
                            <el-button size="mini" type="primary">点击上传</el-button>
                        </el-upload>
                    </el-form-item>
                    <el-form-item label="内容" prop="content">
                        <el-input type="textarea" rows="20" placeholder="内容" v-model="m.content"></el-input>
                    </el-form-item>
                    <el-form-item>
                        <span class="c-label">&emsp;</span>
                        <el-button type="primary" icon="el-icon-plus" size="small" @click="ok('ruleForm')">确定
                        </el-button>
                    </el-form-item>
                </el-form>
            </div>
        </div>
    </el-dialog>
</template>

<script>
import inputEnum from "../../sa-resources/com-view/input-enum.vue";
export default {
    components: { inputEnum },
    props: ["params"],
    data() {
        return {
            m: {},
            title: "",
            isShow: false,
            rules: {
                title: [],
                info: [
                    {
                        min: 0,
                        max: 500,
                        message: "长度在 500 个字符",
                        trigger: "blur",
                    },
                ],
                type: [],
                image: [],
                content: [
                    {
                        min: 0,
                        max: 5000,
                        message: "长度在 5000 个字符",
                        trigger: "blur",
                    },
                ],
            },
            fileList: [],
        };
    },
    methods: {
        open: function (data) {
            this.isShow = true;
            let title = data.type == 3?"公司介绍":"发展历程"
            if (data.id) {
                this.title = `修改${title}` ;
                this.sa.get('/About/find/'+data.id).then(res=>{
                  this.m = res.data;
                  this.m.imageFile = JSON.parse(data.image);
                })
            } else {
                this.m = {
                    title: "",
                    info: "",
                    type: data.type,
                    image: "",
                    imageFile: [],
                    content: "",
                };
                this.title = `添加${title}`;
            }
        },
        success_image(response, file, fileList) {
            if (response.code != 200) {
                this.sa.error(response.message);
                return;
            }
            if (!this.m.imageFile) {
                this.m.imageFile = [];
            }
            this.m.imageFile.push(response.data);
            console.log(fileList);
        },
        remove_image(file, fileList) {
            this.m.imageFile = fileList;
        },

        //提交关于我们信息
        ok: function (formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.m.image = JSON.stringify(
                        this.m.imageFile
                            .map((item) => {
                                let a = {};
                                a.name = item.name;
                                a.url = item.url;
                                return a;
                            })
                            .filter((item) => {
                                if (!item.url.startsWith("blob")) {
                                    return item;
                                }
                            })
                    );
                    this.sa.post("/About/save", this.m).then((res) => {
                        console.log(res);
                        this.$parent.f5();
                        this.isShow = false;
                    });
                } else {
                    console.log("error submit!!");
                    return false;
                }
            });
        },
    },
    created() {},
};
</script>