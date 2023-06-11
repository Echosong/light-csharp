<template>
    <el-dialog v-if="m" :title="title" :visible.sync="isShow" width="550px" top="10vh" :append-to-body="true"
        :destroy-on-close="true" :close-on-click-modal="false" custom-class="full-dialog">
        <div class="vue-box">
            <!-- 参数栏 -->
            <div class="c-panel">
                <el-form size="mini" v-if="m" ref="ruleForm" :rules="rules" :model="m" class="demo-ruleForm"
                    label-width="120px">
                    <el-form-item label="类型">
                    <input-enum
                        enumName="TagTypeEnum"
                        v-model="m.type"
                    ></input-enum>
                </el-form-item>
<el-form-item label="标签名称" prop="name">
             <el-input v-model="m.name"></el-input>
</el-form-item>
<el-form-item label="描述" prop="description">
 <el-input type="textarea"  rows="2" placeholder="描述"  v-model="m.description"></el-input></el-form-item>
<el-form-item label="排序" prop="sort">
             <el-input v-model.number="m.sort"></el-input>
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
      rules: {type:[{required: true, message: '请输入类型', trigger: 'blur' },],
name:[{required: true, message: '请输入标签名称', trigger: 'blur' },{ min: 0, max: 255, message: '长度在 255 个字符', trigger: 'blur' },],
description:[{ min: 0, max: 500, message: '长度在 500 个字符', trigger: 'blur' },],
sort:[],},
      fileList:[],
    }
  },
  methods: {
    open: function (data) {
      this.isShow = true;
      if (data) {
        this.title = "修改 标签" ;
        
        this.m = data;
      }else{
        this.m = {type: 0 ,
name:'',
description:''}
        this.title = "添加 标签" ;
      }
    },
    

    //提交标签信息
    ok: function (formName) {
      this.$refs[formName].validate((valid) => {
        if (valid) {
            
            this.sa.post("/Tag/save", this.m).then((res) => {
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