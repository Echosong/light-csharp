
<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="客户端信息:"><el-input v-model="m.agent"></el-input>
</el-form-item>
<el-form-item label="客户端ip:"><el-input v-model="m.ip"></el-input>
</el-form-item>
<el-form-item label="请求url:"><el-input v-model="m.url"></el-input>
</el-form-item>
<el-form-item label="业务类型:"><el-input v-model.number="m.type"></el-input>
</el-form-item>
<el-form-item label="操作说明:"><el-input v-model="m.title"></el-input>
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
//import inputEnum from "../../sa-resources/com-view/input-enum.vue";
export default {
  //components: { inputEnum },
  props: ["params"],
  data() {
    return {
      m: {},
      title:"",
      isShow: false,
      rules: {},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 " ;
        
        this.m = data;
      }else{
        this.m = {agent:'',
ip:'',
url:'',
type:'',
title:''}
        this.title = "添加 " ;
      }
    },
    

    //提交信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
            
            this.sa.post("/userLog/save", this.m).then((res) => {
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
