<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true" :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm" label-width="120px">
                
                    <el-form-item label="标题" prop="title">
                        <el-input v-model="m.title"></el-input>
                    </el-form-item>
                    <el-form-item label="图片" prop="image">
                        <el-upload class="upload-demo" :action="sa.cfg.api_url + '/Attachment/upload?fileType=0&param=0'"
                         :multiple="false" :limit="10" :on-success="success_image" :before-remove="remove_image"
                         list-type="picture-card"
                          :file-list="m.imageFile">
                            <el-button size="mini" type="primary">点击上传</el-button>
                            <div slot="tip" class="el-upload__tip">上传图片</div>
                        </el-upload>
                    </el-form-item>
                    <el-form-item label="位置">
                        <input-enum enumName="AdPostionEnum" v-model="m.postion"></input-enum>
                    </el-form-item>
                    <el-form-item label="链接" prop="url">
                        <el-input v-model="m.url"></el-input>
                    </el-form-item>
                    <el-form-item label="排序" prop="sort">
                        <el-input v-model.number="m.sort"></el-input>
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
                sort: [],
                title: [
                    {
                        min: 0,
                        max: 255,
                        message: "长度在 255 个字符",
                        trigger: "blur",
                    },
                ],
                image: [
                    {
                        min: 0,
                        max: 255,
                        message: "长度在 255 个字符",
                        trigger: "blur",
                    },
                ],
                postion: [
                    {
                        min: 0,
                        max: 255,
                        message: "长度在 255 个字符",
                        trigger: "blur",
                    },
                ],
                url: [],
            },
            fileList: [],
        };
    },
    methods: {
        open: function (data) {
            this.isShow = true;
            if (data) {
                this.title = "修改 广告banner";
                data.imageFile = JSON.parse(data.image);
                this.m = data;
            } else {
                this.m = {
                    title: "",
                    image: "",
                    imageFile: [],
                    postion: 0,
                    url: "",
                };
                this.title = "添加 广告banner";
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

        //提交广告banner信息
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
                    this.sa.post("/Ad/save", this.m).then((res) => {
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