<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="目标用户0位全部,-1 vip" prop="userId">
             <el-input v-model.number="m.userId"></el-input>
</el-form-item>
<el-form-item label="标题" prop="title">
             <el-input v-model="m.title"></el-input>
</el-form-item>
<el-form-item label="内容" prop="description">
 <el-input type="textarea"  rows="2" placeholder="内容"  v-model="m.description"></el-input></el-form-item>
<el-form-item label="状态">
                    <input-enum
                        enumName="NoticeStateEnum"
                        v-model="m.state"
                    ></input-enum>
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
//import E from "wangeditor";
export default {
  components: { inputEnum },
  props: ["params"],
  data() {
    return {
      m: {},
      title:"",
      isShow: false,
      rules: {userId:[],
title:[{ min: 0, max: 255, message: '长度在 255 个字符', trigger: 'blur' },],
description:[],
state:[],},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 消息公告" ;
        
        this.m = data;
      }else{
        this.m = {title:'',
description:'',
state: 0 }
        this.title = "添加 消息公告" ;
      }
      //create_editor
    },
    /*create_editor
    create_editor: function (editName) {
        this.$nextTick(function () {
            let editor = new E(this.$refs[editName]);
            editor.customConfig.debug = true; // debug模式
            editor.customConfig.uploadFileName = 'file'; // 图片流name
            editor.customConfig.withCredentials = true; // 跨域携带cookie
            editor.customConfig.uploadImgShowBase64 = true   	// 使用 base64 保存图片
            editor.customConfig.onchange = (html) => {	// 创建监听，实时传入 
                this.m[editName] = html;
            }
            editor.create();		// 创建编辑器 
            editor.txt.html(this.m[editName]);	// 为编辑器赋值 
        });
    },
     create_editor*/

    

    //提交消息公告信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
            
            this.sa.post("/Notice/save", this.m).then((res) => {
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