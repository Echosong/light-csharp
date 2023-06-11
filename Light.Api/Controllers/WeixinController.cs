using Microsoft.AspNetCore.Mvc;
using NewLife.Serialization;
using Senparc.CO2NET.AspNet.HttpUtility;
using Senparc.NeuChar.MessageHandlers;
using Senparc.Weixin.AspNet.MvcExtension;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.Entities.Request;
using Light.Api.Message;
using Light.Common.Configuration;
using Light.Common.Filter;
using Light.Common.Utils;
using Light.Entity;
using Light.Service;

namespace Light.Api.Controllers {

    /// <summary>
    /// 微信公众号
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public partial class WeixinController : BaseController {
        /// <summary>
        ///与微信公众账号后台的Token设置保持一致，区分大小写。
        /// </summary>
        public static readonly string WxToken = AppSettingsConstVars.WxToken;
        /// <summary>
        /// /与微信公众账号后台的EncodingAESKey设置保持一致，区分大小写。
        /// </summary>
        public static readonly string EncodingAESKey = AppSettingsConstVars.EncodingAESKey;
        /// <summary>
        /// 与微信公众账号后台的AppId设置保持一致，区分大小写。
        /// </summary>
        public static readonly string AppId = AppSettingsConstVars.WeixinAppId;


        readonly Func<string> _getRandomFileName = () => SystemTime.Now.ToString("yyyyMMdd-HHmmss") + Guid.NewGuid().ToString("n").Substring(0, 6);

        private readonly IQiniuService _qiniuService;

        /// <summary>
        /// 初始化注入
        /// </summary>
        /// <param name="qiniuService"></param>
        /// <param name="db"></param>
        public WeixinController(IQiniuService qiniuService, Db db) {
            _qiniuService = qiniuService;
            _db = db;
        }

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url填写如：http://sdk.weixin.senparc.com/weixin
        /// </summary>
        [HttpGet]
        [NoPermission]
        [IgnoreResultAttribute]
        public string Get(string echostr) {
            return echostr; //返回随机字符串则表示验证通过
        }

        /// <summary>
        /// 主动发送
        /// </summary>
        [HttpGet]
        [Route("SendMessage")]
        [NoPermission]
        [IgnoreResultAttribute]
        public void SendMessage(string openId) {
            CustomApi.SendTextAsync(AppId, openId, "发送消息了");
        }

        /// <summary>
        /// 【异步方法】用户发送消息后，微信平台自动Post一个请求到这里，并等待响应XML。
        /// </summary>
        [HttpPost]
        [NoPermission]
        [IgnoreResultAttribute]
        public async Task<ActionResult> Post(string? signature, string? timestamp, string? nonce, string? msg_signature) {
            var postModel = new PostModel {
                Signature = signature,
                Timestamp = timestamp,
                Nonce = nonce,
                Msg_Signature = msg_signature
            };

            LogHelper.Info("服务端收到" + postModel.ToJson());
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, WxToken)) {
                return Content("参数错误！");
            }
            postModel.SetSecretInfo(WxToken, EncodingAESKey, AppId);


            #region 打包 PostModel 信息


            #endregion

            //v4.2.2之后的版本，可以设置每个人上下文消息储存的最大数量，防止内存占用过多，如果该参数小于等于0，则不限制（实际最大限制 99999）
            //注意：如果使用分布式缓存，不建议此值设置过大，如果需要储存历史信息，请使用数据库储存
            var maxRecordCount = 10;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new CustomMessageHandler(await Request.GetRequestMemoryStreamAsync(), postModel, maxRecordCount, _db);//接收消息（第一步）


            #region 设置消息去重设置 + 优先调用同步、异步方法设置

            /* 如果需要添加消息去重功能，只需打开OmitRepeatedMessage功能，SDK会自动处理。
             * 收到重复消息通常是因为微信服务器没有及时收到响应，会持续发送2-5条不等的相同内容的 RequestMessage */
            messageHandler.OmitRepeatedMessage = true;//默认已经是开启状态，此处仅作为演示，也可以设置为 false 在本次请求中停用此功能

            //当同步方法被重写，且异步方法未被重写时，尝试调用同步方法
            messageHandler.DefaultMessageHandlerAsyncEvent = DefaultMessageHandlerAsyncEvent.SelfSynicMethod;
            #endregion

            try {
                //记录 Request 日志（可选）
                messageHandler.SaveRequestMessageLog();
                var ct = new CancellationToken();
                //执行微信处理过程（关键，第二步）
                await messageHandler.ExecuteAsync(ct);
                //返回（第三步）
                return new FixWeixinBugWeixinResult(messageHandler);
            } catch (Exception ex) {
                #region 异常处理

                return Content("");
                #endregion
            }
        }
    }
}
