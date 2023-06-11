<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="用户" prop="username">
             <el-input v-model="m.username"></el-input>
</el-form-item>
<el-form-item label="用户昵称" prop="userNick">
             <el-input v-model="m.userNick"></el-input>
</el-form-item>
<el-form-item label="用户id" prop="userId">
             <el-input v-model.number="m.userId"></el-input>
</el-form-item>
<el-form-item label="博客归属用户" prop="blogUserId">
             <el-input v-model="m.blogUserId"></el-input>
</el-form-item>
<el-form-item label="博客id" prop="blogId">
             <el-input v-model.number="m.blogId"></el-input>
</el-form-item>
<el-form-item label="回复的客户id" prop="replyId">
             <el-input v-model="m.replyId"></el-input>
</el-form-item>
<el-form-item label="回复人昵称" prop="replyName">
             <el-input v-model="m.replyName"></el-input>
</el-form-item>
<el-form-item label="评论的回复" prop="commentId">
             <el-input v-model="m.commentId"></el-input>
</el-form-item>
<el-form-item label="内容" prop="content">
 <el-input type="textarea"  rows="2" placeholder="内容"  v-model="m.content"></el-input></el-form-item>
<el-form-item label="评论点赞数" prop="goodCount">
             <el-input v-model="m.goodCount"></el-input>
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
//import E from "wangeditor";
export default {
  //components: { inputEnum },
  props: ["params"],
  data() {
    return {
      m: {},
      title:"",
      isShow: false,
      rules: {username:[],
userNick:[{ min: 0, max: 100, message: '长度在 100 个字符', trigger: 'blur' },],
userId:[],
blogUserId:[],
blogId:[],
replyId:[],
replyName:[{ min: 0, max: 100, message: '长度在 100 个字符', trigger: 'blur' },],
commentId:[],
content:[{ min: 0, max: 2000, message: '长度在 2000 个字符', trigger: 'blur' },],
goodCount:[],},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 评论" ;
        
        this.m = data;
      }else{
        this.m = {username:'',
userNick:'',
replyName:'',
content:''}
        this.title = "添加 评论" ;
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

    

    //提交评论信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
            
            this.sa.post("/Comment/save", this.m).then((res) => {
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