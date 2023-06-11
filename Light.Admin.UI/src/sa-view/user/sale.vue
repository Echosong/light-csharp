<template>
    <div class="vue-box">
        <div class="c-panel">
            <!-- 参数栏 -->
            <el-form :inline="true" size="mini">
              
                <el-form-item label="账号:">
                    <el-input v-model="p.username" placeholder="模糊查询"></el-input>
                </el-form-item>

                <el-form-item label="上级账号：">
                    <el-input v-model="p.pusername" placeholder="上级账号"></el-input>
                </el-form-item>

                <el-form-item label="姓名：">
                    <el-input v-model="p.name" placeholder="模糊查询"></el-input>
                </el-form-item>

            
                <el-form-item label="等级:">
                    <input-enum enumName="userLevelEnum" v-model="p.level" ></input-enum>
                </el-form-item>

                <el-form-item style="min-width: 0">
                    <el-button type="primary" icon="el-icon-search" @click="f5();">查询 </el-button>

                    <el-button class="c-btn" type="warning" icon="el-icon-s-promotion" @click="setQrcode({id:0})">二维码</el-button>
                </el-form-item>
            </el-form>
            <!-- <div class="c-title">数据列表</div> -->
            <el-table :data="dataList" size="mini">
                <el-table-column type="selection"></el-table-column>
                <el-table-column label="头像">
                    <template slot-scope="s">
                        <el-image style="width: 60px; height: 60px; border-radius: 30px;" v-for="item in (s.row.avatarFile)" :key="item.id" :src="item.url" :preview-src-list="[item.url]">
                        </el-image>
                    </template>
                </el-table-column>
                <el-table-column label="账号" prop="username">
                    <template slot-scope="s"><span style="color: #2D8CF0; cursor: pointer"   @click="goTo(s.row.username)">{{s.row.username}}</span> </template>
                </el-table-column>
                <el-table-column label="昵称" prop="name"></el-table-column>
                <el-table-column label="类型" prop="typeEnum"></el-table-column>
                <el-table-column label="等级" prop="levelEnum"></el-table-column>
                <el-table-column label="上级" prop="pUsername"></el-table-column>
                <el-table-column label="注册时间" prop="createDateTime"></el-table-column>
                <el-table-column label="最近活动时间" prop="updateDateTime"></el-table-column>
                <el-table-column label="分销奖金" prop="balanceTotal"></el-table-column>
                <el-table-column label="备注" prop="email"></el-table-column>

                <el-table-column prop="address" label="操作" width="250px">
                    <template slot-scope="s">
                        <el-button class="c-btn" type="primary" icon="el-icon-edit" @click="update(s.row)">备注</el-button>
                        <el-button class="c-btn" type="warning" icon="el-icon-s-promotion" @click="setQrcode(s.row)">二维码</el-button>
                    </template>
                </el-table-column>

            </el-table>
            <!-- 分页 -->
            <div class="page-box">
                <el-pagination background layout="total, prev, pager, next, sizes, jumper" :current-page.sync="p.page" :page-size.sync="p.pageSize" :total="dataCount" :page-sizes="[1, 15, 20, 30, 40, 50, 100]" @current-change="f5(true)" @size-change="f5(true)">
                </el-pagination>
            </div>
        </div>

        <el-dialog title="推广二维码" :visible.sync="dialogTableVisible"  width="400px" top="10vh" :append-to-body="true"  custom-class="full-dialog">
             <img :src="qrcode" style="width: 320px; height: 300px; margin-left: 30px;" />
        </el-dialog>

        <!-- 增改组件 -->
        <add-or-update ref="add-or-update" :roles="roles"></add-or-update>
        <set-user ref="set-user"  :roles="roles"></set-user>
    </div>
</template>

<script>
import InputEnum from '../../sa-resources/com-view/input-enum.vue';
//import inputEnum from "../../sa-resources/com-view/input-enum.vue";
import addOrUpdate from "./add.vue";
import setUser from "./user-add.vue"
export default {
    components: {
        addOrUpdate,
        setUser,
        InputEnum
        //inputEnum,
    },
    data() {
        return {
            p: { pageSize: 15, page: 1 , pusername: '', type: '', username:'',level:''},
            dataCount: 0,
            dataList: [],
            roles:[],
            dialogTableVisible:false,
            qrcode: ''
        };
    },
    methods: {
        setu:function(row){
            this.$refs["set-user"].open(row);
        },

        // 数据刷新
        f5: function () {
            this.sa.put("/user/listSale", this.p).then((res) => {
                console.log("我在这里", res);
                this.dataList = res.data.content.map((item) => {
                    if(item.roleId){
                        item.roleName = (this.roles.find(t=>t.id === item.roleId)).name
                    }
                    if(item.avatar && item.avatar.indexOf('[')!==-1){
                        item.avatarFile = JSON.parse(item.avatar);
                    }else{
                        item.avatarFile =  [{url: item.avatar}];
                    }
                    return item;
                });
                this.dataCount = res.data.pageable.totalElements;
            });
           
        },
        //更新
        update(row) {
            row.saleShow = true;
            this.$refs["add-or-update"].open(row);
        },

        goTo(username){
          this.p.pusername = username;
          this.p.page = 1;
          this.f5();
        },

        setQrcode(row){
            this.sa.get('/user/GetQrCode/'+row.id).then(res=>{
                this.qrcode = res.data;
                this.dialogTableVisible = true
            })
        },
    },
    created: function () {
        this.sa.get("/role/list").then((res) => {
                this.roles = res.data;
                this.f5();
        });
    },
};
</script>

<style>
</style>
