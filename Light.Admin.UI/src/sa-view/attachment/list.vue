<template>
    <div class="vue-box">
        <div class="c-panel">
            <!-- 参数栏 -->
            <el-form :inline="true" size="mini">
                <el-form-item style="min-width: 0px">
                    <el-button type="primary" icon="el-icon-plus" @click="add">增加</el-button>
                    <el-button type="primary" icon="el-icon-download" @click="exportFile()">导出</el-button>
                </el-form-item>
                <el-form-item label="文件名：">
                    <el-input v-model="p.fileName" placeholder="模糊查询"></el-input>
                </el-form-item>
                <el-form-item label="文件存储类型">
                    <input-enum enumName="FileTypeEnum" v-model="p.fileType"></input-enum>
                </el-form-item>
                <el-form-item label="时间：">
                    <el-date-picker v-model="p.startDateTime" type="datetime" value-format="yyyy-MM-dd HH:mm:ss" placeholder="开始日期"></el-date-picker>
                    -
                    <el-date-picker v-model="p.endDateTime" type="datetime" value-format="yyyy-MM-dd HH:mm:ss" placeholder="结束日期"></el-date-picker>
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
                <el-table-column label="文件名" prop="fileName"></el-table-column>
                <el-table-column label="文件扩展名" prop="extend"></el-table-column>
                <el-table-column label="文件存储路径" prop="filePath"></el-table-column>
                <el-table-column label="文件存储类型" prop="fileTypeEnum"></el-table-column>
                <el-table-column label="文档大小" prop="fileSize"></el-table-column>
                <el-table-column label="相对路径" prop="urlPath"></el-table-column>
                <el-table-column label="唯一标识" prop="uuid"></el-table-column>
                <el-table-column label="时间" prop="dateTime"></el-table-column>

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
                fileName: "",
                fileType: "",
                dateTime: "",
            },
            dataCount: 0,
            dataList: [],
        };
    },
    methods: {
        // 数据刷新
        f5: function () {
            this.sa.put("/Attachment/listPage", this.p).then((res) => {
                this.dataList = res.data.content.map((item) => {
                    return item;
                });
                this.dataCount = res.data.pageable.totalElements;
            });
        },
        // 删除
        del: function (data) {
            this.sa.confirm(
                "是否删除，此操作不可撤销",
                function () {
                    this.sa
                        .delete("/Attachment/delete/" + data.id)
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
                { key: "fileName", name: "文件名" },
                { key: "extend", name: "文件扩展名" },
                { key: "filePath", name: "文件存储路径" },
                { key: "fileTypeEnum", name: "文件存储类型" },
                { key: "fileSize", name: "文档大小" },
                { key: "urlPath", name: "相对路径" },
                { key: "uuid", name: "唯一标识" },
                { key: "dateTime", name: "时间" },
                { key: "id", name: "id" },
            ];
            this.sa
                .exportFile("/Attachment/listPage", this.p, titleObj)
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
