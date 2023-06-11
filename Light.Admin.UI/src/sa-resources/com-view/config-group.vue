
<template>
   
    <el-form size="mini" label-width="230px">
        <template>
            <el-form-item :label="item.name + ':'" :key="item.id" v-for="(item, index) in configList">
                <el-input v-if="item.type == 0" v-model="item.value" :placeholder="item.description"></el-input>
                <el-input v-if="item.type == 6" v-model="item.value" :placeholder="item.description" type="textarea" class="text-area" :rows="2"></el-input>
                <el-select v-if="item.type == 1" v-model="item.value">
                    <el-option label="请选择" value=""></el-option>
                    <el-option v-for="op in item.options" :key="op.v" :label="op.n" :value="op.n">{{ op.n }}
                    </el-option>
                </el-select>
    
                <el-switch v-if="item.type == 2" v-model="item.value" active-color="#13ce66">
                </el-switch>
    
                <el-input v-if="item.type == 5" type="textarea" :rows="4" style="width: 60%" :placeholder="item.description" v-model="item.value">
                </el-input>
    
                <template v-if="item.type == 3">
                    <el-checkbox-group v-model="item.value">
                        <el-checkbox v-for="op in item.options" :key="op.v" :label="op.n">{{ op.n }}
                        </el-checkbox>
                    </el-checkbox-group>
                </template>
    
                <el-upload v-if="item.type == 4" class="upload-demo" :action="sa.cfg.api_url + '/Attachment/upload?fileType=0&param='+index" :multiple="false" :on-success="success" :on-remove="remove" list-type="picture-card" :file-list="item.fileList">
                    <el-button size="small" type="primary">{{
                item.name
            }}</el-button>
                    <div slot="tip" class="el-upload__tip">
                        {{ item.description }}
                    </div>
                </el-upload>
            </el-form-item>
        </template>
    
        <el-form-item>
            <el-button type="primary" size="mini" icon="el-icon-check" @click="ok">保存修改</el-button>
          
        </el-form-item>
    </el-form>
     </template>
     
     <script>
     export default {
         props:{
            configList: {default:[]}
         },
         methods:{
            ok(){
                console.log('子组件方式.....');
                this.$emit('ok');
            },
            
    
            success(response, file, fileList) {
                //服务端返回做了个传递
                let index = parseInt(response.data.params);
                let obj = this.configList[index];
                obj.fileList.push({ url: response.data.url, id: obj.id });
                obj.value = JSON.stringify(obj.fileList);
            },
    
            //注意关键是id，其他地方可以用index 进行处理，这样一个页面只需要一个success 和 remove处理方法了
            remove(file, fileList) {
                let obj = this.configList.filter((item) => item.id == file.id)[0];
                obj.value = JSON.stringify(fileList);
                obj.fileList = fileList;
            },
    
    
             goto(project){
                 
                 this.sa_admin.navigateTo("project-detail", project);
             }
         }
     }
     </script>
     
     <style>
     
     </style>
    
    