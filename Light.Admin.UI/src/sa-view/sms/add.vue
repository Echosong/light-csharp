<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="接受对象" prop="mobile">
             <el-input v-model="m.mobile"></el-input>
</el-form-item>
<el-form-item label="消息内容" prop="message">
 <el-input type="textarea"  rows="2" placeholder="消息内容"  v-model="m.message"></el-input></el-form-item>
<el-form-item label="原始参数" prop="param">
 <el-input type="textarea"  rows="2" placeholder="原始参数"  v-model="m.param"></el-input></el-form-item>
<el-form-item label="状态">
                    <input-enum
                        enumName="SimStateEnum"
                        v-model="m.state"
                    ></input-enum>
                </el-form-item>
<el-form-item label="模板" prop="templateId">
             <el-input v-model.number="m.templateId"></el-input>
</el-form-item>
<el-form-item label="模板类型">
                    <input-enum
                        enumName="TemplateTypeEnum"
                        v-model="m.type"
                    ></input-enum>
                </el-form-item>
<el-form-item label="模板编码" prop="code">
             <el-input v-model="m.code"></el-input>
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
      rules: {mobile:[{ min: 0, max: 100, message: '长度在 100 个字符', trigger: 'blur' },],
message:[{ min: 0, max: 500, message: '长度在 500 个字符', trigger: 'blur' },],
param:[{ min: 0, max: 500, message: '长度在 500 个字符', trigger: 'blur' },],
state:[],
templateId:[],
type:[],
code:[{ min: 0, max: 50, message: '长度在 50 个字符', trigger: 'blur' },],},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 短信记录（微信推送消息）" ;
        
        this.m = data;
      }else{
        this.m = {mobile:'',
message:'',
param:'',
state: 0 ,
type: 0 ,
code:''}
        this.title = "添加 短信记录（微信推送消息）" ;
      }
    },
    

    //提交短信记录（微信推送消息）信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
            
            this.sa.post("/Sms/save", this.m).then((res) => {
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