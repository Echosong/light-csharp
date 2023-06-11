using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Light.Common.Dto;
using Light.Common.RedisCache;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 基础类
    /// </summary>
    public class BaseController : Controller {

        /// <summary>
        /// 全局当前用户
        /// </summary>
        protected UserDto _user = new UserDto() {
            Id = 3,
            Username = "18800000000",
            RoleId = 1,
            Name = "张三"
        };

        /// <summary>
        /// 全局数据上下文
        /// </summary>
        protected Db _db;

        /// <summary>
        ///     获取http 上的token
        /// </summary>
        protected string Token = "";

        /// <summary>
        ///     获取客户端IP地址
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        protected string GetIp() {
            try {
                if (this.Request.HttpContext.Connection.RemoteIpAddress == null) {
                    return "127.0.0.1";
                }
                return this.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            } catch (Exception) {
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// 获取客户端来的token
        /// </summary>
        /// <returns></returns>
        protected string GetToken() {
            var token = this.HttpContext.Request.Headers["token"].ToString();
            return String.IsNullOrEmpty(token) ? "" : token;
        }

        /// <summary>
        /// 执行前处理
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context) {
            Token = GetToken();
            if (!string.IsNullOrEmpty(Token)) {
                try {
                    var user = Redis.CreateInstance().GetCache<UserDto>(Token);
                    if (user != null) {
                        _user = user;
                        _db.UserId = user.Id;
                        _db.SiteId = user.SiteId;
                    }
                } catch (Exception ex) {
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
