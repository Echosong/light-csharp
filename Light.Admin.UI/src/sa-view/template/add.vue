<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true" :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm" label-width="120px">
                    <el-form-item label="模板类型"  >
                        <input-enum enumName="TemplateTypeEnum" v-model="m.type"></input-enum>
                    </el-form-item>
                    <el-form-item label="第三方编码" prop="code">
                        <el-input v-model="m.code" v-if="m.code" disabled="true"></el-input>
                        <el-input v-model="m.code" v-else ></el-input>
                    </el-form-item>
                    <el-form-item label="模板参数" prop="title">
                        <el-input v-model="m.params"></el-input>
                    </el-form-item>
                    <el-form-item label="模板说明" prop="title">
                        <el-input v-model="m.title"></el-input>
                    </el-form-item>
                    <el-form-item label="魔板内容" prop="content">
                        <el-input type="textarea" rows="2" placeholder="魔板内容" v-model="m.content"></el-input>
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
                type: [],
                code: [
                    {
                        min: 0,
                        max: 50,
                        message: "长度在 50 个字符",
                        trigger: "blur",
                    },
                ],
                title: [
                    {
                        min: 0,
                        max: 200,
                        message: "长度在 200 个字符",
                        trigger: "blur",
                    },
                ],
                content: [
                    {
                        min: 0,
                        max: 500,
                        message: "长度在 500 个字符",
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
            if (data) {
                this.title = "修改 转发短信模板";

                this.m = data;
            } else {
                this.m = { type: 0, code: "", title: "", content: "" };
                this.title = "添加 转发短信模板";
            }
        },

        //提交转发短信模板信息
        ok: function (formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.sa.post("/Template/save", this.m).then((res) => {
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