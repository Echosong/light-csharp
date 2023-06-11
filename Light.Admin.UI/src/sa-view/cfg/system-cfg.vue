/* eslint-disable */
<style scoped>
.text-area {
    width: 500px;
}
</style>

<template>
    <div class="vue-box">
        <!-- 参数栏 -->
        <div class="c-panel">
            <el-tabs v-model="activeName" @tab-click="handleClick">
                <el-tab-pane label="平台配置" name="site">
                    <config-group :configList="configSite" @ok="ok"></config-group>
                </el-tab-pane>
                <el-tab-pane label="业务配置" name="app">
                    <config-group :configList="configApp" @ok="ok"></config-group>
                </el-tab-pane>

            </el-tabs>

        </div>
    </div>
</template>

<script>
import configGroup from "../../sa-resources/com-view/config-group.vue";
export default {
    components: { configGroup },
    data() {
        return {
            configList: [],
            m: null,
            checkboxs: [],
            activeName: 'site',
            file: "",
            configApp:[],
            configSite:[]
        };
    },
    methods: {
        handleClick() {},
        // f5
        f5: function () {
            this.sa.get("/config/list").then((res) => {
                this.configList = res.data.map((item) => {
                    if (item.description == null) {
                        item.description = "";
                    }
                    if (item.description.indexOf(",") != -1) {
                        var arrStr = item.description.split(",");
                        var options = [];
                        arrStr.forEach((element, index) => {
                            options.push({ n: element, v: index });
                        });
                        item.options = options;
                    }
                    if (item.type == 3) {
                        item.values = (item.value || "").split(",");
                    }

                    if (item.type == 2) {
                        item.value = item.value == "true" ? true : false;
                    }
                    if (item.type == 4) {
                        if (item.value && item.value.length > 3) {
                            if (item.value.indexOf("[") != -1) {
                                item.fileList = JSON.parse(item.value);
                            } else {
                                item.fileList = [
                                    { url: item.value, id: item.id },
                                ];
                            }
                        } else {
                            item.fileList = [];
                        }
                    }

                    return item;
                });

                this.configList.forEach(item=>{
                    if(item.group == 'app'){
                        this.configApp.push(item);
                    }else{
                        this.configSite.push(item);
                    }
                })

                console.log('app', this.configApp, "app");
                console.log('site', this.configSite);
            });
        },

       

        // ok
        ok: function () {
            debugger;
            this.sa.post("/config/save", this.configList).then((res) => {
                console.log(res);
                this.sa.ok("保存成功");
            });
        },
    },
    created() {
        this.f5();
    },
};
</script>
