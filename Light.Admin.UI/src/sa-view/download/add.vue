<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="文件名" prop="title">
             <el-input v-model="m.title"></el-input>
</el-form-item>
<el-form-item label="价格" prop="price">
             <el-input v-model="m.price"></el-input>
</el-form-item>
<el-form-item label="文件路径" prop="url">
 <el-upload
  class="upload-demo"
  :action="sa.cfg.api_url + '/Attachment/upload?fileType=0&param=0'"
  :multiple="false"
  :limit="10"
   :on-success="success_url"   :before-remove="remove_url"  :file-list="m.urlFile">
  <el-button size="mini" type="primary">点击上传</el-button>
  <div slot="tip" class="el-upload__tip">上传文件路径</div>
</el-upload></el-form-item>
<el-form-item label="VIP免费" prop="vipFree">
             <el-input v-model.number="m.vipFree"></el-input>
</el-form-item>
<el-form-item label="首页显示" prop="index">
             <el-input v-model.number="m.index"></el-input>
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
      rules: {title:[{ min: 0, max: 100, message: '长度在 100 个字符', trigger: 'blur' },],
price:[],
url:[{ min: 0, max: 200, message: '长度在 200 个字符', trigger: 'blur' },],
vipFree:[],
index:[],},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 付费下载" ;
        data.urlFile = JSON.parse(data.url);
        this.m = data;
      }else{
        this.m = {title:'',
url:'',
urlFile:[]}
        this.title = "添加 付费下载" ;
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

    success_url(response, file, fileList) {
          if(response.code != 200){
            this.sa.error(response.message);
            return;
          }
          if(!this.m.urlFile){
            this.m.urlFile = [];
          }          this.m.urlFile.push(response.data);
          console.log(fileList);
        },
remove_url(file, fileList){
    this.m.urlFile = fileList;
},

    //提交付费下载信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
                   this.m.url =JSON.stringify(this.m.urlFile.map(item=>{
              let a = {};
              a.name = item.name;
              a.url = item.url;
              return a;       }).filter(item=>{ if(!item.url.startsWith("blob")){return item;}}) );
            this.sa.post("/Download/save", this.m).then((res) => {
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