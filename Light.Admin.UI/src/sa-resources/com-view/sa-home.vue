<style scoped>
.vue-box {
    background-color: #eee;
    font-size: 14px;
    font-family: "Helvetica Neue", Helvetica, "PingFang SC", "Hiragino Sans GB",
    "Microsoft YaHei", "微软雅黑", Arial, sans-serif;
}

.content-box {
    margin: 1em;
    padding: 1em;
    background-color: #fff;
    color: #666;
    border-radius: 10px;
}

.el-tag {
    border-radius: 1px;
}

/*800之下*/
@media (max-width: 800px) {
    .kapian {
        width: 100% !important;
        margin-right: 0px !important;
    }

    .saoma {
        display: none;
    }
}

/* markdown格式 */
.md-contetn {
    padding-left: 1.5em;
    line-height: 26px;
}
</style>
<style>
/* 多个ul时，切换那个啥 */
.md-contetn h4 {
    margin-left: -0.5em;
}

.md-contetn ul,
.md-contetn ol {
    padding-left: 1em;
}

.md-contetn pre {
    padding: 5px;
    background-color: #eee;
    font-family: "times new roman";
}

.title {
    width: 120px;
    font-size: 18px;
    font-family: PingFangSC-Semibold, PingFang SC;
    font-weight: 600;
    color: #2d2d2d;
    line-height: 29px;
    padding-left: 20px;
    border-left: 3px solid #4983f4;
}

.info {
    color: #ccc;
    width: 120px;
    font-size: 14px;
    padding-left: 60px;
}

.header {
    margin: 20px;
    margin-bottom: 10px;
}

.body {
    margin: 80px;
}

.project {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
    align-items: flex-start;
}

.project-title {
    font-size: 16px;
    font-weight: 600;
    justify-content: flex-start;
    align-items: flex-start;
    display: flex;
}

.project-number {
    display: flex;
    width: 92px;
    height: 40px;
    font-size: 24px;
    font-family: PingFangSC-Semibold, PingFang SC;
    font-weight: 600;
    color: #4983f4;
    line-height: 40px;
    justify-content: center;
    align-items: center;
    width: 200px;
}

.index-task-title {
    font-size: 16px;
    font-family: PingFangSC-Semibold, PingFang SC;
    font-weight: 600;
    margin-top: 10px;
    display: flex;
    flex-direction: row;
}

.index-task-title-item {
    margin-left: 20px;
    margin-right: 20px;
}

.project-task-add {
    margin: 10px;
    height: 67px;
    background: #ffffff;
    border-radius: 8px;
    border: 1px solid #e2e2e2;
    display: flex;
    padding-left: 60px;
    justify-content: flex-start;
    align-items: center;
    color: #979797;
    flex-direction: row;
    margin-bottom: 20px;
}

.project-task-add span {
    margin-left: 20px;
    font-size: 20px;
}

.index-item {
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    border-bottom: #eee 2px solid;
    padding: 10px;
    flex-direction: column;
    margin-top: 10px;
}

.index-item-circle {
    height: 18px;
    width: 18px;
    border-radius: 9px;
    color: #fff;
    margin-right: 10px;
    display: flex;
    justify-content: center;
    align-items: center;
}

.index-item-b {
    margin-top: 12px;
    display: flex;
    flex-direction: row;
    margin-left: 30px;
}

.el-radio__inner {
    height: 24px;
    width: 24px;
}

.active {
    color: #4983f4;
}

.index-item-title {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 90%;
}

.index-item-dev {
    display: flex;
    flex-direction: row;
    justify-content: left;
}

.index-item-info {
    word-break: break-all;
    white-space: pre-wrap;
    width: 90%;
}

.index-item-username {
    margin-left: 20px;
    width: 250px;
    font-weight: bold;
}

.content-card {
    display: flex;
    justify-content: center;
    align-items: center;
}

.finance {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
}

.finance-item {
    width: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    flex-direction: column;
}

.finance-title {
    font-size: 14px;
    font-weight: bold;
    margin: 10px;
}

.finance-money {
    font-size: 16px;
    font-weight: bold;
}

.finance-info {
    margin-top: 10px;
    margin-bottom: 20px;
    color: #999;
}

.finance-ge {
    width: 1px;
    background-color: #ddd;
    height: 50px;
}

.finance-item img {
    width: 50px;
}

.kuai-list {
    width: 100%;
    min-height: 50px;
    margin-top: 10px;
    display: flex;
    justify-content: left;
    flex-wrap: wrap;
}
</style>

<template>
    <div class="vue-box">

        <div class="header">
            <el-row>
                <el-col :span="12">
                    <div class="title">数据看板</div>
                </el-col>

            </el-row>
        </div>
        <div class="content-box" v-if="user.roleId != 4">
            <el-row :gutter="50">
                <el-col :span="6">
                    <div class="item">
                        <div class="info">支付总金额</div>
                        <div class="project-number">¥ {{ moneyInfo["支付总金额"] }}</div>
                    </div>


                </el-col>
                <el-col :span="6">

                    <div class="item">
                        <div class="info">分销利润</div>
                        <div class="project-number" style="color: #2B9939">¥ {{ moneyInfo["总分销金额"] }}</div>
                    </div>

                </el-col>
                <el-col :span="6">

                    <div class="item">
                        <div class="info">招商利润</div>
                        <div class="project-number" style="color: blueviolet">¥ {{ moneyInfo["总招商金额"] }}</div>
                    </div>
                </el-col>

                <el-col :span="6">
                    <div class="item">
                        <div class="info">VIP人数</div>
                        <div class="project-number" style="color: #CA4242">{{ moneyInfo["viP人数"] }} 人</div>
                    </div>
                </el-col>
            </el-row>
        </div>

        <div class="content-box" v-if="user.roleId == 4">
            <el-row :gutter="50">
                <el-col :span="4">
                    <div class="item">
                        <div class="info">今日收入(元)</div>
                        <div class="project-number">¥ {{ moneyInfo["今日收入"] }}</div>
                    </div>

                </el-col>
                <el-col :span="4">

                    <div class="item">
                        <div class="info">近一周收入(元)</div>
                        <div class="project-number" style="color: #2B9939">¥ {{ moneyInfo["近一周收入"] }}</div>
                    </div>

                </el-col>
                <el-col :span="4">
                    <div class="item">
                        <div class="info">近一月收入(元)</div>
                        <div class="project-number" style="color: blueviolet">¥ {{ moneyInfo["近一月收入"] }}</div>
                    </div>
                </el-col>

                <el-col :span="4">
                    <div class="item">
                        <div class="info">今日新增用户</div>
                        <div class="project-number" style="color: #CA4242">{{ moneyInfo["今日新增用户"] }} 人</div>
                    </div>
                </el-col>

                <el-col :span="4">
                    <div class="item">
                        <div class="info">近一周新增用户</div>
                        <div class="project-number" style="color: #007aff">{{ moneyInfo["近一周新增用户"] }} 人</div>
                    </div>
                </el-col>

                <el-col :span="4">
                    <div class="item">
                        <div class="info">近一月新增用户</div>
                        <div class="project-number" style="color: #2d2d2d">{{ moneyInfo["近一月新增用户"] }} 人</div>
                    </div>
                </el-col>

            </el-row>
        </div>


        <el-row>
            <el-col :span="12">
                <div class="content-box">
                    <el-row>
                        <el-col>
                            <div class="project">

                            </div>
                            <div class="content-card">
                                <div id="orders" style="width: 100%; height:320px; "></div>
                            </div>
                        </el-col>
                    </el-row>
                </div>
            </el-col>

            <el-col :span="12">
                <div class="content-box">
                    <el-row>
                        <el-col>
                            <div class="project">

                            </div>
                            <div class="content-card">
                                <div id="users" style="width: 100%; height:320px; "></div>
                            </div>
                        </el-col>
                    </el-row>
                </div>
            </el-col>
        </el-row>

    </div>

</template>

<script>
import * as echarts from "echarts";

export default {
    data() {
        return {
            enums: [],
            all: 0,
            value: 0,
            p: {pageSize: 15, page: 1, state: 0, userId: null},
            dataCount: 0,
            dataList: [],
            allData: {},
            financeOut: 0,
            financeIn: 1,
            tableData: [],
            moneyInfo: {
                "支付总金额": 0,
                "总分销金额": 0,
                "总招商金额": 0,
                "viP人数": 0,
                "今日收入":0,
                "近一周收入":0,
                "近一月收入":0,
                "今日新增用户":0,
                "近一周新增用户":0,
                "近一月新增用户":0
            },
            user:{}
        };
    },
    created() {
        this.user = this.sa_admin.user;

        this.f5();
    },
    mounted() {
        var orderChart = echarts.init(document.getElementById("orders"));
        var usersChart = echarts.init(document.getElementById("users"));
        this.sa.get("/Home/Statistics").then(res => {
            let data = res.data;
            this.moneyInfo = data["总统计"];

            let orderInfo = data["订单统计"];
            let orderkeys = Object.keys(orderInfo);
            let orderValues = orderkeys.map(k => orderInfo[k]);
            // 指定图表的配置项和数据
            var option = {
                title: {
                    text: '订单统计'
                },
                tooltip: {},
                legend: {
                    data: ['销量']
                },
                xAxis: {
                    data: orderkeys
                },
                yAxis: {},
                series: [
                    {
                        name: '销量',
                        type: 'bar',
                        data: orderValues
                    }
                ]
            };
            orderChart.setOption(option);
            let userInfo = data["用户统计"];
            let userKeys = Object.keys(userInfo);
            let userValues = userKeys.map(k => userInfo[k]);
            option = {
                title: {
                    text: '新增会员'
                },
                xAxis: {
                    type: 'category',
                    data: userKeys
                },
                yAxis: {
                    type: 'value'
                },
                series: [
                    {
                        data: userValues,
                        type: 'line'
                    }
                ]
            };
            usersChart.setOption(option);
        })


    },

    methods: {
        f5: function () {
            this.user = this.sa_admin.user;
        },
        send(index) {
            this.p.state = index;
            this.f5();
        },
        success(row) {
        },
        quick(row) {
            console.log(row.link);
            this.sa_admin.navigateTo(row.link);
        },
    },
};
</script>

