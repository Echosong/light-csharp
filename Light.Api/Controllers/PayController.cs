using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.Extensions;
using Senparc.Weixin.TenPayV3;
using Senparc.Weixin.TenPayV3.Apis;
using Senparc.Weixin.TenPayV3.Apis.BasePay;
using Senparc.Weixin.TenPayV3.Apis.Entities;
using Senparc.Weixin.TenPayV3.Entities;
using Senparc.Weixin.TenPayV3.Helpers;
using Light.Api.Transfer;
using Light.Common.Configuration;
using Light.Common.Dto.Api;
using Light.Common.Enums;
using Light.Common.Error;
using Light.Common.Filter;
using Light.Common.Utils;
using Light.Entity;
using Light.Service;
using Config = Senparc.Weixin.Config;

namespace Light.Api.Controllers {



    /// <summary>
    /// 支付 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]

    public class PayController : BaseController {
        private readonly IPaymentService _payment;
        private readonly ISmsService _sms;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payment"></param>
        public PayController(IPaymentService payment, Db db, ISmsService sms) {
            _payment = payment;
            _db = db;
            _sms = sms;
        }



        /// <summary>
        /// 转账到零钱
        /// </summary>
        /// <param name="btOrderNo"></param>
        /// <param name="batRemark"></param>
        /// <param name="totalAmount"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        private async Task<BatchesReturnJson> Transfer(string btOrderNo, string batRemark, int totalAmount, string openId) {
            BatchesRequestData batchesRequestData = new BatchesRequestData();
            batchesRequestData.appid = AppSettingsConstVars.WxOpenAppId;
            var sp_billno = string.Format("{0}{1}{2}", AppSettingsConstVars.TenPayV3_MchId/*10位*/, SystemTime.Now.ToString("yyyyMMddHHmmss"),
                    TenPayV3Util.BuildRandomStr(6));
            //这个订单号长度需要注意，要不然会报什么string[]转换错误
            batchesRequestData.out_batch_no = sp_billno;
            batchesRequestData.batch_name = sp_billno;
            batchesRequestData.batch_remark = batRemark;
            batchesRequestData.total_amount = totalAmount;
            batchesRequestData.total_num = 1;

            Transfer_Detail_List[] transfer_Detail_Lists = new Transfer_Detail_List[1];

            Transfer_Detail_List transfer_Detail_List = new Transfer_Detail_List();
            transfer_Detail_List.out_detail_no = sp_billno;
            transfer_Detail_List.transfer_amount = totalAmount;
            transfer_Detail_List.transfer_remark = batRemark;
            transfer_Detail_List.openid = openId;
            //明细转账金额 >= 2,000元，收款用户姓名必填；
            //transfer_Detail_List.user_name = ""
            transfer_Detail_Lists[0] = transfer_Detail_List;

            batchesRequestData.transfer_detail_list = transfer_Detail_Lists;

            var url = string.Format($"{Senparc.Weixin.Config.TenPayV3Host}/{{0}}v3/transfer/batches", Senparc.Weixin.Config.UseSandBoxPay ? "sandboxnew/" : "");
            LogHelper.Info("分发账户"+ url);

            TenPayApiRequest tenPayApiRequest = new TenPayApiRequest();
            var requestAsync = tenPayApiRequest.RequestAsync<BatchesReturnJson>(url, batchesRequestData);
            LogHelper.Info(requestAsync.Result.ToJson());
            return await requestAsync;
        }


        /// <summary>
        /// 小程序支付【购买暂时不需要购买下载，下载那边如果非vip 那么跳转到购买vip】
        /// </summary>
        /// <param name="payProduct"> 注意如果是专家一对一服务 : info:{"mobile":"188000000", "score":100, "sort":1}</param>
        /// <returns></returns>
        [HttpPost]
        [Log("购买vip")]
        [NoPermission]
        public async Task<object> MiniPay(PayProductDto payProduct) {

            try {
                var openId = this._db.Users.Where(t => t.Id == this._user.Id).Select(t => t.OpenId).First();
                var sp_billno = string.Format("{0}{1}{2}", AppSettingsConstVars.TenPayV3_MchId/*10位*/, SystemTime.Now.ToString("yyyyMMddHHmmss"),
                     TenPayV3Util.BuildRandomStr(6));

                FinanceOp financeOp = _payment.Pay(payProduct, _user.Id, sp_billno);
                if (financeOp == null) {
                    throw new BaseException("数据处理错误");
                }

                var body = financeOp.Description;
                if (financeOp.Account == null) {
                    throw new BaseException("金额错误");
                }
                var price = (int)(financeOp.Account * 100);//单位：分
                var notifyUrl = AppSettingsConstVars.TenPayV3_TenpayNotify;
                var basePayApis = new BasePayApis(Config.SenparcWeixinSetting);

                var requestData = new Senparc.Weixin.TenPayV3.Apis.BasePay.TransactionsRequestData(AppSettingsConstVars.WxOpenAppId,
                    AppSettingsConstVars.TenPayV3_MchId,
                    body,
                    sp_billno,
                    new TenpayDateTime(SystemTime.Now.AddMinutes(120).DateTime, false),
                   null, notifyUrl, null, new() { currency = "CNY", total = price },
                    new(openId), null, null, null);
                var result = await basePayApis.JsApiAsync(requestData);
                if (result.prepay_id == null) {
                    throw new BaseException(result.ToJson());
                }
                var packageStr = "prepay_id=" + result.prepay_id;

                var jsApiUiPackage = TenPaySignHelper.GetJsApiUiPackage(AppSettingsConstVars.WxOpenAppId, result.prepay_id);

                return new {
                    success = true,
                    prepay_id = result.prepay_id,
                    appId = Config.SenparcWeixinSetting.WxOpenAppId,
                    timeStamp = jsApiUiPackage.Timestamp,
                    nonceStr = jsApiUiPackage.NonceStr,
                    partnerid = AppSettingsConstVars.TenPayV3_MchId,
                    package = packageStr,
                    signType = "RSA",
                    paySign = jsApiUiPackage.Signature
                };
            } catch (Exception ex) {
                throw new BaseException("全局错误" + ex.Message);
            }
        }
        /// <summary>
        /// 分销直接发送微信余额
        /// </summary>
        /// <param name="finance"></param>
        private void Distribution(FinanceOp finance) {
            var financeOps = this._db.FinanceOps.Where(t => t.RelationId == finance.Id && t.BusinessType == (int)FinanceTypeEnum.分润).ToList();
            if (financeOps.Count == 0) {
                return;
            }
            var userIds = financeOps.Select(t => t.UserId).ToList();
            var userExtends = this._db.Users.Where(t => userIds.Contains(t.Id)).ToList();

            foreach (var op in financeOps) {
                var user = userExtends.FirstOrDefault(t => t.Id == op.UserId);
                if (user != null) {
                    var sp_billno = string.Format("{0}{1}{2}", AppSettingsConstVars.TenPayV3_MchId/*10位*/, SystemTime.Now.ToString("yyyyMMddHHmmss"),
                        TenPayV3Util.BuildRandomStr(6));
                    var result = this.Transfer(sp_billno, op.Description, Convert.ToInt32(op.Account *100), user.OpenId).Result;
                    if (result.ResultCode.ErrorCode == "SUCCESS") {
                        op.State = (int)FinanceOpStateEnum.成功;
                        op.OutOrderNo = sp_billno;
                        var userExtend = this._db.UserExtends.FirstOrDefault(t=>t.UserId == user.Id);
                        userExtend.BalanceTotal += op.Account??0;
                    }
                    else {
                        op.Description += "\r\n 微信提示::"+ result.ResultCode.ErrorMessage;
                    }
                }
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 测试一个分发
        /// </summary>
        /// <param name=""></param>
        [HttpGet]
        public void PayTest(int id) {
            var financeOp = _db.FinanceOps.Find(id);
            Distribution(financeOp);
        }


        /// <summary>
        /// JS-SDK支付回调地址（在下单接口中设置的 notify_url）
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [NoPermission]
        public async Task<IActionResult> PayNotifyUrl() {
            try {
                //获取微信服务器异步发送的支付通知信息
                var resHandler = new TenPayNotifyHandler(HttpContext);
                var orderReturnJson = await resHandler.AesGcmDecryptGetObjectAsync<OrderReturnJson>();

                //记录日志
                LogHelper.Info("微信支付回调::" + orderReturnJson.ToJson(true));

                //获取支付状态
                string trade_state = orderReturnJson.trade_state;

                //验证请求是否从微信发过来（安全）
                NotifyReturnData returnData = new();
                //验证可靠的支付状态
                if (orderReturnJson.VerifySignSuccess == true && trade_state == "SUCCESS") {
                    returnData.code = "SUCCESS";//正确的订单处理
                    if (orderReturnJson.out_trade_no == null) {
                        returnData.code = "FAILD";//错误的订单处理
                        returnData.message = "验证失败";
                    } else {
                        try {
                            FinanceOp financeOp = _payment.Notify(orderReturnJson.transaction_id, orderReturnJson.out_trade_no);
                            Distribution(financeOp);
                        } catch (Exception ex) {
                            returnData.code = "FAILD";//错误的订单处理
                            returnData.message = "验证失败";
                            LogHelper.Error("支付错误", ex);
                        }
                    }
                } else {
                    returnData.code = "FAILD";//错误的订单处理
                    returnData.message = "验证失败";

                    //此处可以给用户发送支付失败提示等
                }
                return Json(returnData);
            } catch (Exception ex) {
                LogHelper.Error("微信回调", ex);
                throw;
            }
        }

    }
}
