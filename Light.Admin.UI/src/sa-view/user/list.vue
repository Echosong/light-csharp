<template>
    <div class="vue-box">
        <div class="c-panel">
            <!-- 参数栏 -->
            <el-form :inline="true" size="mini">
                <el-form-item style="min-width: 0px">
                    <el-button type="primary" icon="el-icon-plus" @click="add">增加</el-button>
                </el-form-item>
                <el-form-item label="用户名：">
                    <el-input v-model="p.username" placeholder="模糊查询"></el-input>
                </el-form-item>

                <el-form-item label="上级账号：">
                    <el-input v-model="p.pusername" placeholder="上级账号"></el-input>
                </el-form-item>

                <el-form-item label="姓名：">
                    <el-input v-model="p.name" placeholder="模糊查询"></el-input>
                </el-form-item>

                <el-form-item label="类型：">
                    <input-enum enumName="userTypeEnum" v-model="p.type" ></input-enum>
                </el-form-item>

                <el-form-item style="min-width: 0px">
                    <el-button type="primary" icon="el-icon-search" @click="f5();">查询 </el-button>
                </el-form-item>
            </el-form>
            <!-- <div class="c-title">数据列表</div> -->
            <el-table :data="dataList" size="mini">
                <el-table-column type="selection"></el-table-column>
                <el-table-column label="用户名" prop="username"></el-table-column>
                <el-table-column label="姓名" prop="name"></el-table-column>
                <el-table-column label="类型" prop="typeEnum"></el-table-column>
                <el-table-column label="上级" prop="pUsername"></el-table-column>
                <el-table-column label="登陆ip" prop="loginIp"></el-table-column>
                <el-table-column label="角色" prop="roleName"></el-table-column>
                <el-table-column label="小程序openId" prop="openId"></el-table-column>
                <el-table-column label="备注" prop="email"></el-table-column>
                <el-table-column label="性别" prop="sex">
                    <template slot-scope="s">
                        <span>{{s.row.sex == 1?"男":"女"}}</span>
                    </template>
                </el-table-column>
                <el-table-column label="状态" prop="">
                    <template slot-scope="s">
                        <div @click="resetState(s.row)">
                            <span v-if="s.row.state == 2" style=" cursor: pointer;text-decoration:underline">正常</span>
                            <span v-if="s.row.state == 3" style="color:red; cursor: pointer;text-decoration:underline">禁用</span>
                        </div>
                    </template>

                </el-table-column>
                
                <el-table-column label="注册时间" prop="createDateTime"></el-table-column>
                <el-table-column prop="address" label="操作" width="350px">
                    <template slot-scope="s">
                        <el-button class="c-btn" type="primary" icon="el-icon-edit" @click="update(s.row)">修改</el-button>
                        <el-button class="c-btn" type="success" icon="el-icon-setting" @click="setu(s.row)">设置上级</el-button>
                        <el-button class="c-btn" type="warning" icon="el-icon-odometer"  @click="upset(s.row)">重置密码</el-button>
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
            p: { pageSize: 15, page: 1 , pusername: '', type: '', username:''},
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
            this.sa.put("/user/listPage", this.p).then((res) => {
                console.log("我在这里", res);
                this.dataList = res.data.content.map((item) => {
                    if(item.roleId){
                        item.roleName = (this.roles.find(t=>t.id == item.roleId)).name
                    }
                    return item;
                });
                this.dataCount = res.data.pageable.totalElements;
            });
           
        },
        upset(row){
            this.sa.confirm("确定要重置密码？", ()=>{
                this.sa.get('/user/resetPassword?id='+ row.id).then(res=>{
                    this.sa.ok("密码重置为123456")
                })
            })
        },
        resetState(row){
            this.sa.confirm("确定要重置状态？", ()=>{
                this.sa.get('/user/ResetState?id='+ row.id).then(res=>{
                    row.state = row.state == 3?2:3;
                })
            })
        },
        // 删除
        del: function (data) {
            this.sa.confirm(
                "是否删除，此操作不可撤销",
                function () {
                    this.sa.delete("/user/" + data.id).then((res) => {
                        console.log(res);
                        this.sa.arrayDelete(this.dataList, data);
                        this.sa.ok(res.message);
                    });
                }.bind(this)
            );
        },
        setQrcode(row){
            this.sa.get('/user/GetQrCode/'+row.id).then(res=>{
                this.qrcode = res.data;
                this.dialogTableVisible = true
            })
        },
        //更新
        update(row) {
            this.$refs["add-or-update"].open(row);
        },
        //添加
        add: function () {
            this.$refs["add-or-update"].open(0);
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
