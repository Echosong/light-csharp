<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="用户id" prop="userId">
             <el-input v-model="m.userId"></el-input>
</el-form-item>
<el-form-item label="账号" prop="username">
             <el-input v-model="m.username"></el-input>
</el-form-item>
<el-form-item label="账户类型">
                    <input-enum
                        enumName="FinanceTypeEnum"
                        v-model="m.businessType"
                    ></input-enum>
                </el-form-item>
<el-form-item label="状态">
                    <input-enum
                        enumName="FinanceOpStateEnum"
                        v-model="m.state"
                    ></input-enum>
                </el-form-item>
<el-form-item label="金额" prop="account">
             <el-input v-model="m.account"></el-input>
</el-form-item>
<el-form-item label="外部单号" prop="outOrderNo">
             <el-input v-model="m.outOrderNo"></el-input>
</el-form-item>
<el-form-item label="描述" prop="description">
             <el-input v-model="m.description"></el-input>
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
      title:"",
      isShow: false,
      rules: {userId:[],
username:[],
businessType:[],
state:[],
account:[],
outOrderNo:[],
description:[],},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 资金操作记录" ;
        
        this.m = data;
      }else{
        this.m = {username:'',
businessType: 0 ,
state: 0 ,
outOrderNo:'',
description:''}
        this.title = "添加 资金操作记录" ;
      }
    },
    

    //提交资金操作记录信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
            
            this.sa.post("/FinanceOp/save", this.m).then((res) => {
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