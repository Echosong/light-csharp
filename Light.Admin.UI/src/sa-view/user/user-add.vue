<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="500px" top="10vh" :append-to-body="true" :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm" label-width="120px">

                    <el-form-item label="姓名：" prop="name" required>
                        <el-input v-model="m.name"></el-input>
                    </el-form-item>

                    <el-form-item label="手机号：" v-if="!m.id" prop="username" required>
                        <el-input v-model="m.username"></el-input>
                    </el-form-item>

                    <el-form-item label="上级：">
                        <el-select v-model="m.parentId" filterable >
                            <el-option label="请选择" value="0"></el-option>
                            <el-option v-for="item in pUsers" :key="item.id" :label="item.username+'-'+item.typeEnum " :value="item.id">
                            </el-option>
                        </el-select>
                    </el-form-item>
                   
                    <el-form-item>
                        <span class="c-label">&emsp;</span>
                        <el-button type="primary" icon="el-icon-plus" size="mini" @click="ok('ruleForm')">确定</el-button>
                    </el-form-item>
                </el-form>
            </div>
        </div>
    </el-dialog>
</template>

<script>
export default {
    props: {
      roles: {type:Array, default:[]}
    },
    data() {
        return {
            pUsers:[],
            m: {
                username: "", // 从菜单配置文件里传递过来的参数
                password: "",
                name: "",
                email: "",
                roleId: 0,
                sex: 1,
                state: 0
            },
            rules: {
                name: [
                    {
                        required: true,
                        message: "请输入姓名",
                        trigger: "blur"
                    }
                ],
                username: [
                    { required: true, message: "请输入账号", trigger: "blur" },
                    {
                        min: 10,
                        max: 12,
                        message: "账号必须为手机号",
                        trigger: "blur"
                    }
                ],
                password: [
                    { required: true, message: "请输入密码", trigger: "blur" },
                    { main: 6, max: 24, message: "密码长度必须大于等于6个字符" }
                ]
            },
            isShow: false,
            title: "新增用户",
        };
    },
    methods: {
        open: function(data) {
            this.isShow = true;
            if (data.id) {
                data.password = "";
                this.title = "修改用户";
                this.m = data;
            }
            this.parent();
        },

        parent:function(){
            this.sa.get("/user/GetParents").then(res=>{
                this.pUsers = res.data.map(item=>{
                   var find = this.roles.find(t=>t.id === item.roleId);
                   if(find) {
                       item.roleName =find.name;
                   }else{
                       item.roleName = "";
                   }
                    return item;
                });
            })
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
    },
    created() {}
};
</script>


