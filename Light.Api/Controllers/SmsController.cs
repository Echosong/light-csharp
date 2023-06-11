using Microsoft.AspNetCore.Mvc;
using Light.Common.Error;
using Light.Common.Filter;
using Light.Entity;
using Light.Service;

namespace Light.Api.Controllers {
    /// <summary>
    /// 短信业务
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class SmsController : BaseController {

        private readonly ISmsService _smsService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        /// <param name="smsService"></param>
        public SmsController(Db db, ISmsService smsService) {
            this._db = db;
            this._smsService = smsService;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="mobile">发送消息</param>
        [HttpGet]
        [NoPermission]
        public void SendCode(string mobile) {
            _smsService.SendCode(mobile);
        }

        /// <summary>
        /// 验证码验证
        /// </summary>
        /// <param name="mobile">手机</param>
        /// <param name="code">验证码</param>
        [HttpGet]
        [NoPermission]
        public void Verification(string mobile, string code) {
            try {
                _smsService.Verification(mobile, code);
            }
            catch (Exception e) {
                throw new BaseException("验证码错误");
            }
        }
    }
}
