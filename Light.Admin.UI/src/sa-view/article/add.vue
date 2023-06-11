<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="850px" top="10vh" :append-to-body="true"
               :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                         label-width="120px">
                    <el-form-item label="标题" prop="title">
                        <el-input v-model="m.title"></el-input>
                    </el-form-item>
                    <el-form-item label="分类">
                        <input-enum
                            enumName="ArticleTypeEnum"
                            v-model="m.type"
                        ></input-enum>
                    </el-form-item>
                    <el-form-item label="图片" prop="img">
                        <el-upload
                            class="upload-demo"
                            :action="sa.cfg.api_url + '/Attachment/upload?fileType=0&param=0'"
                            :multiple="false"
                            list-type="picture-card"
                            :limit="10"
                            :on-success="success_img" :before-remove="remove_img" :file-list="m.imgFile">
                            <el-button size="mini" type="primary">点击上传</el-button>
                            <div slot="tip" class="el-upload__tip">上传图片</div>
                        </el-upload>
                    </el-form-item>
                    <el-form-item label="作者" prop="author">
                        <el-input v-model="m.author"></el-input>
                    </el-form-item>
                    <el-form-item label="时间：" prop="time">
                        <el-date-picker
                            v-model="m.time"
                            type="datetime"
                            value-format="yyyy-MM-dd HH:mm:ss"
                            placeholder="时间"
                        ></el-date-picker>
                    </el-form-item>
                    <el-form-item label="简介" prop="info">
                        <el-input type="textarea" rows="2" placeholder="简介" v-model="m.info"></el-input>
                    </el-form-item>
                    <el-form-item label="内容" prop="content">
                        <div class="editor-box">
                            <div id="content" ref="content" style="text-align:left"></div>
                        </div>
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
import E from "wangeditor";

export default {
    components: {inputEnum},
    props: ["params"],
    data() {
        return {
            m: {},
            title: "",
            isShow: false,
            rules: {
                title: [{required: true, message: '请输入标题', trigger: 'blur'}, {
                    min: 0,
                    max: 200,
                    message: '长度在 200 个字符',
                    trigger: 'blur'
                },],
                type: [{required: true, message: '请输入分类', trigger: 'blur'},],
                img: [{min: 0, max: 200, message: '长度在 200 个字符', trigger: 'blur'},],
                author: [{min: 0, max: 20, message: '长度在 20 个字符', trigger: 'blur'},],
                time: [],
                info: [{min: 0, max: 1000, message: '长度在 1000 个字符', trigger: 'blur'},],
                content: [{required: true, message: '请输入内容', trigger: 'blur'},],
            },
            fileList: [],
        }
    },
    methods: {
        open: function (data) {
            this.isShow = true;
            if (data){
                this.sa.get("/Article/Find/"+data.id).then(res=>{
                    let data = res.data;
                    try {
                        data.imgFile = JSON.parse(data.img);
                    } catch (e) {
                        data.imgFile = [{"url": data.img}]
                    }
                    this.m = data;
                    this.create_editor("content");
                })
                this.title = "修改 文章";
            } else {
                this.m = {
                    title: '',
                    type: 0,
                    img: '',
                    imgFile: [],
                    author: '',
                    time: '',
                    info: '',
                    content: ''
                }
                this.title = "添加 文章";
                this.create_editor("content");
            }

        },

        create_editor: function (editName) {
            this.$nextTick(function () {
                let editor = new E(this.$refs[editName]);
                editor.customConfig.debug = true; // debug模式
                editor.customConfig.uploadFileName = 'file'; // 图片流name
                editor.customConfig.withCredentials = true; // 跨域携带cookie
                editor.customConfig.uploadImgServer = 'upload'; //设置上传文件的服务器路径
                editor.customConfig.onchange = (html) => {	// 创建监听，实时传入
                    this.m[editName] = html;
                }
                editor.create();		// 创建编辑器
                editor.txt.html(this.m[editName]);	// 为编辑器赋值
            });
        },


        success_img(response, file, fileList) {
            if (response.code != 200) {
                this.sa.error(response.message);
                return;
            }
            if (!this.m.imgFile) {
                this.m.imgFile = [];
            }
            this.m.imgFile.push(response.data);
            console.log(fileList);
        },
        remove_img(file, fileList) {
            this.m.imgFile = fileList;
        },

        //提交文章信息
        ok: function (formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.m.img = JSON.stringify(this.m.imgFile.map(item => {
                        let a = {};
                        a.name = item.name;
                        a.url = item.url;
                        return a;
                    }).filter(item => {
                        if (!item.url.startsWith("blob")) {
                            return item;
                        }
                    }));
                    this.sa.post("/Article/save", this.m).then((res) => {
                        console.log(res);
                        this.$parent.f5();
                        this.isShow = false;
                    });
                } else {
                    console.log("error submit!!");
                    return false;
                }
            });
        }
    },
    created() {
    },
};
</script>