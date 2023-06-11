<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="文件名" prop="fileName">
             <el-input v-model="m.fileName"></el-input>
</el-form-item>
<el-form-item label="文件扩展名" prop="extend">
             <el-input v-model="m.extend"></el-input>
</el-form-item>
<el-form-item label="文件存储路径" prop="filePath">
             <el-input v-model="m.filePath"></el-input>
</el-form-item>
<el-form-item label="文件存储类型">
                    <input-enum
                        enumName="FileTypeEnum"
                        v-model="m.fileType"
                    ></input-enum>
                </el-form-item>
<el-form-item label="文档大小" prop="fileSize">
             <el-input v-model="m.fileSize"></el-input>
</el-form-item>
<el-form-item label="相对路径" prop="urlPath">
             <el-input v-model="m.urlPath"></el-input>
</el-form-item>
<el-form-item label="唯一标识" prop="uuid">
             <el-input v-model="m.uuid"></el-input>
</el-form-item>
<el-form-item label="时间："  prop="dateTime" >
 <el-date-picker
            v-model="m.dateTime"
            type="datetime"
            value-format="yyyy-MM-dd HH:mm:ss"
            placeholder="时间"
          ></el-date-picker> </el-form-item>
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
      rules: {fileName:[{ min: 0, max: 200, message: '长度在 200 个字符', trigger: 'blur' },],
extend:[{ min: 0, max: 50, message: '长度在 50 个字符', trigger: 'blur' },],
filePath:[{ min: 0, max: 255, message: '长度在 255 个字符', trigger: 'blur' },],
fileType:[],
fileSize:[],
urlPath:[{ min: 0, max: 100, message: '长度在 100 个字符', trigger: 'blur' },],
uuid:[{ min: 0, max: 100, message: '长度在 100 个字符', trigger: 'blur' },],
dateTime:[],},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 文件管理" ;
        
        this.m = data;
      }else{
        this.m = {fileName:'',
extend:'',
filePath:'',
fileType: 0 ,
urlPath:'',
uuid:'',
dateTime:''}
        this.title = "添加 文件管理" ;
      }
    },
    

    //提交文件管理信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
            
            this.sa.post("/Attachment/save", this.m).then((res) => {
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