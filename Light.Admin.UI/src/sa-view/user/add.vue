<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true" :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm" label-width="120px">
                    <template v-if="!saleShow">
                        <el-form-item label="用户名" prop="username">
                            <el-input v-model="m.username"></el-input>
                        </el-form-item>
                        <el-form-item label="姓名" prop="name">
                            <el-input v-model="m.name"></el-input>
                        </el-form-item>
                        <el-form-item label="状态">
                            <input-enum enumName="UserStateEnum" v-model="m.state"></input-enum>
                        </el-form-item>

                        <el-form-item label="注册ip" prop="regIp">
                            <el-input v-model="m.regIp"></el-input>
                        </el-form-item>
                        <el-form-item label="登陆ip" prop="loginIp">
                            <el-input v-model="m.loginIp"></el-input>
                        </el-form-item>
                        <el-form-item label="用户类型" prop="password">
                            <input-enum enumName="userTypeEnum" v-model="m.type" ></input-enum>
                        </el-form-item>
                        <el-form-item label="角色">
                            <el-select v-model="m.roleId">
                                <el-option label="请选择" value="0" disabled></el-option>
                                <el-option v-for="item in roles" :key="item.id" :label="item.name" :value="item.id">
                                </el-option>
                            </el-select>
                        </el-form-item>
                        <el-form-item label="性别">
                            <input-enum enumName="BaseSexEnum" v-model="m.sex"></input-enum>
                        </el-form-item>
                    </template>
                    <el-form-item label="描述" prop="email">
                        <el-input type="textarea" rows="2" placeholder="描述" v-model="m.email"></el-input>
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
    props: {
      roles: {type:Array, default:[]}
    },

    data() {
        return {
            m: {},
            title: "",
            isShow: false,
            saleShow:false,
            rules: {
                username: [
                    {
                        required: true,
                        message: "请输入用户名",
                        trigger: "blur",
                    },
                ],
                name: [],
                state: [],
                email: [

                ],
                regIp: [],
                loginIp: [],
                password: [],
                roleId: [],
                lead: [],
                sex: [],
                roleName: [],
            },
            fileList: [],
        };
    },
    methods: {
        open: function (data) {
            this.isShow = true;
            if (data) {

                this.saleShow = data.saleShow
                this.title = data.saleShow?'设置备注': "修改 用户表";
                this.m = data;
            } else {
                this.m = {
                    username: "",
                    name: "",
                    state: 0,
                    email: "",
                    regIp: "",
                    loginIp: "",
                    password: "",
                    lead: 0,
                    sex: 0,
                    roleName: "",
                    type:''
                };
                this.title = "添加 用户表";
            }
        },

        //提交用户表信息
        ok: function (formName) {
            this.$refs[formName].validate((valid) => {
                if (valid) {
                    this.sa.post("/User/save", this.m).then((res) => {
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
       
    }
};
</script>