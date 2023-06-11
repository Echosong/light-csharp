<template>
    <div class="vue-box">
        <div class="c-panel">
            <!-- 参数栏 -->
            <el-form :inline="true" size="mini" v-if="user.roleId !=4">
                <el-form-item style="min-width: 0px">
                    <el-button type="primary" icon="el-icon-plus" @click="add">增加{{user.roleId}}1</el-button>
                    <el-button type="primary" icon="el-icon-download" @click="exportFile()">导出</el-button>
                </el-form-item>
                <el-form-item label="账号：" >
                    <el-input v-model="p.username" placeholder="模糊查询"></el-input>
                </el-form-item>
                <el-form-item label="账户类型"  >
                    <input-enum enumName="FinanceTypeEnum" v-model="p.businessType"></input-enum>
                </el-form-item>
                <el-form-item label="状态" >
                    <input-enum enumName="FinanceOpStateEnum" v-model="p.state"></input-enum>
                </el-form-item>
                <el-form-item style="min-width: 0px">
                    <el-button type="primary" icon="el-icon-search" @click="f5();">查询 </el-button>

                </el-form-item>
            </el-form>
            <!-- <div class="c-title">数据列表</div> -->
            <el-table :data="dataList" size="mini" ref="multipleTable">
                <!--start-- 需要时候可以随时开启
				<el-table-column type="selection"></el-table-column>
				<el-table-column label="Id" prop="id"></el-table-column>
				-->
                <el-table-column label="用户id" prop="userId"></el-table-column>
                <el-table-column label="账号" prop="username"></el-table-column>
                <el-table-column label="账户类型" prop="businessTypeEnum"></el-table-column>
                <el-table-column label="状态" prop="stateEnum"></el-table-column>
                <el-table-column label="金额" prop="account"></el-table-column>
                <el-table-column label="外部单号" prop="outOrderNo" width="150"></el-table-column>
                <el-table-column label="备注信息" prop="description" width="280">
                    <template slot-scope="s" >
                        <span v-html="s.row.description"></span>
                    </template>

                </el-table-column>
                <el-table-column label="创建时间" prop="createDateTime"></el-table-column>
                  <el-table-column label="成功时间" prop="updateDateTime"></el-table-column>
                <el-table-column prop="address" label="操作" width="220px">
                    <template slot-scope="s">
                        <el-button class="c-btn" type="primary" icon="el-icon-edit" @click="update(s.row)">修改</el-button>
                        <el-button class="c-btn" type="danger" icon="el-icon-delete" @click="del(s.row)">删除</el-button>
                    </template>
                </el-table-column>
            </el-table>
            <!-- 分页 -->
            <div class="page-box">
                <el-pagination background layout="total, prev, pager, next, sizes, jumper" :current-page.sync="p.page" :page-size.sync="p.pageSize" :total="dataCount" :page-sizes="[1, 15, 20, 30, 40, 50, 100]" @current-change="f5(true)" @size-change="f5(true)">
                </el-pagination>
            </div>
        </div>

        <!-- 增改组件 -->
        <add-or-update ref="add-or-update"></add-or-update>
    </div>
</template>

<script>
import inputEnum from "../../sa-resources/com-view/input-enum.vue";
import addOrUpdate from "./add.vue";
export default {
    components: {
        addOrUpdate,
        inputEnum,
    },
    data() {
        return {
            p: {
                pageSize: 15,
                page: 1,
                username: "",
                businessType: "",
                state: "",
            },
            dataCount: 0,
            dataList: [],
            user:{}
        };
    },
    methods: {
        // 数据刷新
        f5: function () {
            this.sa.put("/FinanceOp/listPage", this.p).then((res) => {
                this.dataList = res.data.content.map((item) => {
                    if(item.businessType === 3){
                        let info =JSON.parse(item.description);
                        item.description = " 专家: "+  (info.name||info.exportId) +"<br>考生电话："+ info.mobile
                          + "<br> 考生分数："+ info.score +"<br>考生排名:"+ info.sort;
                    }

                    return item;
                });
                this.dataCount = res.data.pageable.totalElements;
            });
            this.user = this.sa_admin.user;
            console.log("获取当前登录用户", this.user);
        },
        // 删除
        del: function (data) {
            this.sa.confirm(
                "是否删除，此操作不可撤销",
                function () {
                    this.sa
                        .delete("/FinanceOp/delete/" + data.id)
                        .then((res) => {
                            console.log(res);
                            this.sa.arrayDelete(this.dataList, data);
                            this.sa.ok(res.message);
                        });
                }.bind(this)
            );
        },

        exportFile: function () {
            this.p.pageSize = 1000;
            var titleObj = [
                { key: "userId", name: "用户id" },
                { key: "username", name: "账号" },
                { key: "businessTypeEnum", name: "账户类型" },
                { key: "stateEnum", name: "状态" },
                { key: "account", name: "金额" },
                { key: "outOrderNo", name: "外部单号" },
                { key: "description", name: "描述" },
                { key: "id", name: "id" },
            ];
            this.sa
                .exportFile("/FinanceOp/listPage", this.p, titleObj)
                .then((res) => {
                    console.log(res);
                });
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
        this.f5();
    },
};
</script>

<style>
</style>
