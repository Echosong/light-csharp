using System.Collections.Specialized;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Utilities;
using Newtonsoft.Json;
using Senparc.Weixin.MP.AdvancedAPIs;
using Light.Common.Configuration;
using Light.Common.Enums;
using Light.Common.RedisCache;
using Light.Common.Utils;
using Light.Entity;

namespace Light.Service.Impl {
    public class SmsService : ISmsService {

        /// <summary>
        /// 数据库
        /// </summary>
        private readonly Db _db;
        private readonly Redis _redis;

        /// <summary>
        /// 注入信息
        /// </summary>
        /// <param name="db"></param>
        public SmsService(Db db, Redis redis) {
            _db = db;
            _redis = redis;
        }

        /// <summary>
        /// 验证码是否正确
        /// </summary>
        public void Verification(string mobile, string code) {
            Assert.IsTrue(!string.IsNullOrEmpty(mobile), "手机    号错误");
            Assert.IsTrue(!string.IsNullOrEmpty(code), "验证码错误");
           // if (code != "888888") {
                var redisCode = _redis.GetCache<string>(mobile);
                Assert.IsEquals(redisCode, code, "验证码错误");
           // }
        }

        /// <summary>
        /// 构造请求body
        /// </summary>
        /// <param name="keyValues"></param>
        /// <returns></returns>
        private string BuildQueryString(NameValueCollection keyValues) {
            StringBuilder temp = new StringBuilder();
            foreach (string item in keyValues.Keys) {
                temp.Append(item).Append("=").Append(HttpUtility.UrlEncode(keyValues.Get(item))).Append("&");
            }
            return temp.Remove(temp.Length - 1, 1).ToString();
        }


        /// <summary>
        /// 构造X-WSSE参数值
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private string BuildWSSEHeader(string appKey, string appSecret) {
            string now = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"); //Created
            string nonce = Guid.NewGuid().ToString().Replace("-", ""); //Nonce

            byte[] material = Encoding.UTF8.GetBytes(nonce + now + appSecret);
            byte[] hashed = SHA256.Create().ComputeHash(material);
            string hexdigest = BitConverter.ToString(hashed).Replace("-", "");
            string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hexdigest)); //PasswordDigest

            return String.Format("UsernameToken Username=\"{0}\",PasswordDigest=\"{1}\",Nonce=\"{2}\",Created=\"{3}\"",
                            appKey, base64, nonce, now);
        }

        /// <summary>
        /// 发送手机短信
        /// https://support.huaweicloud.com/devg-msgsms/sms_04_0005.html
        /// </summary>
        /// <param name="template"></param>
        /// <param name="user"></param>
        /// <param name="data"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SendSmsMessage(Sms sms) {

            //必填,请参考"开发准备"获取如下数据,替换为实际值
            string apiAddress = AppSettingsConstVars.HuaweiApiAddress; //APP接入地址(在控制台"应用管理"页面获取)+接口访问URI
            string appKey = AppSettingsConstVars.HuaweiAppKey; //APP_Key
            string appSecret = AppSettingsConstVars.HuaweiAppSecret; //APP_Secret
            //code 存在格式是 模板id,通道号
            string[] code = sms.Code.Split(",");
            if (code.Length < 2) { return; }
            string templateId = code[0]; //模板ID
            string sender = code[1]; //"1069368924410004072"; //国内短信签名通道号或国际/港澳台短信通道号


            //条件必填,国内短信关注,当templateId指定的模板类型为通用模板时生效且必填,必须是已审核通过的,与模板类型一致的签名名称
            //国际/港澳台短信不用关注该参数
            string signature = "华为云短信测试"; //签名名称

            //必填,全局号码格式(包含国家码),示例:+86151****6789,多个号码之间用英文逗号分隔
            string receiver = sms.Mobile;

            //选填,短信状态报告接收地址,推荐使用域名,为空或者不填表示不接收状态报告
            string statusCallBack = "";


            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(sms.Param);
            string templateParas = JsonConvert.SerializeObject(data.Values.ToArray());

            try {

                // 为防止因HTTPS证书认证失败造成API调用失败,需要先忽略证书信任问题
                HttpClient client = new HttpClient();
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                //请求Headers
                client.DefaultRequestHeaders.Add("Authorization", "WSSE realm=\"SDP\",profile=\"UsernameToken\",type=\"Appkey\"");
                client.DefaultRequestHeaders.Add("X-WSSE", BuildWSSEHeader(appKey, appSecret));
                //请求Body
                var body = new Dictionary<string, string>() {
                    {"from", sender},
                    {"to", receiver},
                    {"templateId", templateId},
                    {"templateParas", templateParas},
                    {"statusCallback", statusCallBack},
                    {"signature", signature} //使用国内短信通用模板时,必须填写签名名称
                };

                HttpContent content = new FormUrlEncodedContent(body);

                var response = client.PostAsync(apiAddress, content).Result;
                Console.WriteLine(response.StatusCode); //打印响应结果码
                var res = response.Content.ReadAsStringAsync().Result;
                LogHelper.Info("华为短信发送成功" + res);

            } catch (Exception e) {
                LogHelper.Error("华为短信发送", e);
            }
        }

        /// <summary>
        /// 发送微信模板信息
        /// </summary>
        /// <param name="template"></param>
        /// <param name="user"></param>
        public async void SendWxMessage(Sms sms) {
            //如果传过来是手机号，那么找对于的openid
            string mobile = sms.Mobile ?? string.Empty;
            if (mobile.Length == 11) {
                var userExtent = _db.UserExtends.FirstOrDefault(t => t.Username == sms.Mobile);
                mobile = userExtent.OpenId;
            }

            //template.Content 填写模板编码就行
            if (sms.Type == (int)TemplateTypeEnum.推送消息) {
                var result = await CustomApi.SendTextAsync(AppSettingsConstVars.WeixinAppId, mobile, sms.Message);
                LogHelper.Info(result);
            } else {
                //发送模板消息
                Dictionary<string, string> dic = JsonConvert.DeserializeObject<Dictionary<string, string>>(sms.Param);
                await TemplateApi.SendTemplateMessageAsync(AppSettingsConstVars.WeixinAppId, mobile, sms.Code, dic["url"], dic);
            }
        }

     


        /// <summary>
        /// 集中发送消息入库
        /// </summary>
        public async void SendMessage() {
            var smsList = _db.Smss.Where(t => t.State == (int)SimStateEnum.新建).Take(20).ToList();
            foreach (var sms in smsList) {
                switch ((TemplateTypeEnum)sms.Type) {
                    case TemplateTypeEnum.推送消息:
                        this.SendWxMessage(sms);
                        break;
                    case TemplateTypeEnum.推送模板消息:
                        this.SendWxMessage(sms);
                        break;
                    case TemplateTypeEnum.通知短信:
                        this.SendSmsMessage(sms);
                        break;
                }
                sms.State = (int)SimStateEnum.发送;
            }
            _db.SaveChanges();
        }


        /// <summary>
        /// 插入消息
        /// </summary>
        /// <param name="templateCode"></param>
        /// <param name="param"></param>
        public void CreateMessage(string toUser, string templateCode, Dictionary<string, string> param) {
            var template = _db.Templates.AsNoTracking().FirstOrDefault(t => t.Code == templateCode);
            Assert.IsTrue(template != null, "模板不存在，请先添加魔板");
            String content = template.Content;
            if (param != null) {
                if (param.Count > 0) {
                    foreach (var item in param) {
                        content = content.Replace("${" + item.Key + "}", item.Value);
                    }
                }
            }
            _db.Smss.Add(new Sms {
                Mobile = toUser,
                Code = templateCode,
                TemplateId = template.Id,
                Message = content,
                State = (int)SimStateEnum.新建,
                Type = template.Type,
                Param = param == null ? String.Empty : JsonConvert.SerializeObject(param)
            });
            _db.SaveChanges();
        }

        /// <summary>
        /// 发送短信验证码 这里我没做限制，
        /// 看华为短信那边是不是有限制
        /// </summary>
        /// <param name="code"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SendCode(string mobile) {
            Random rd = new Random();
            string code = rd.Next(100000, 999999).ToString();
            _redis.SetCache<string>(mobile, code);
            this.CreateMessage(mobile, "7f2879c32c7a4a4c8575d6fd8ba91e00,8823021422494", new Dictionary<string, string> {
                {"code", code }
            });
        }
    }
}
