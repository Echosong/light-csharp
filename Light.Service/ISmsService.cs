using Light.Entity;

namespace Light.Service {
    public interface ISmsService {


        /// <summary>
        /// 短信验证码验证
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="code"></param>

        public void Verification(string mobile, string code);

        /// <summary>
        /// 集中发送入库
        /// </summary>
        public void SendMessage();


        /// <summary>
        /// 创建消息先插入表，然后统一job发送消息短信
        /// </summary>
        /// <param name="templateCode"></param>
        /// <param name="param"></param>
        public void CreateMessage(string toUser, string templateCode, Dictionary<string, string> param);


        /// <summary>
        /// 发送 微信公众号推送信息
        /// </summary>
        /// <param name="template"></param>
        /// <param name="user"></param>
        public void SendWxMessage(Sms sms);

        /// <summary>
        /// 发送 手机短信
        /// </summary>
        /// <param name="template"></param>
        /// <param name="user"></param>
        public void SendSmsMessage(Sms sms);

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="mobile"></param>
        public void SendCode(string mobile);
    }
}
