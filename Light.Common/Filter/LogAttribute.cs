using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.RedisCache;

namespace Light.Common.Filter {
    public class LogAttribute : ActionFilterAttribute {
        private readonly string _title;

        private long _time;
        /// <summary>
        /// 日志记录注入
        /// </summary>
        /// <param name="title"></param>
        public LogAttribute(string title) {
            _title = title;
        }

        private LoginUserDto _loginUser = new LoginUserDto {
            Username = "匿名"
        };

        private string param;

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public long GetTimeStamp() {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }
        public override void OnActionExecuting(ActionExecutingContext context) {
            _time = GetTimeStamp();
            GetFirstParamObject(context);
        }

        /// <summary>
        /// 获取首参数的值
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns></returns>
        private void GetFirstParamObject(ActionExecutingContext context) {

            var paramNames = context.ActionDescriptor.Parameters;
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var parameter in paramNames) {
                var parameterName = parameter.Name;//获取Action方法中参数的名字
                var parameterType = parameter.ParameterType;//获取Action方法中参数的类型

                if (parameterType == typeof(LoginUserDto)) {
                    try {
                        _loginUser = context.ActionArguments[parameterName] as LoginUserDto;
                    } catch (Exception ex) { }
                }
                if (parameterName != null) {
                    var value = context.ActionArguments[parameterName];
                    stringBuilder.Append(JsonConvert.SerializeObject(value));
                }

            }
            param = stringBuilder.ToString();
        }

        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="filterContext"></param>
        override public async void OnActionExecuted(ActionExecutedContext filterContext) {
            var request = filterContext.HttpContext.Request;

            var token = request.Headers["token"].ToString();
            var user = new UserDto();
            if (!string.IsNullOrEmpty(token))
                try { user = Redis.CreateInstance().GetCache<UserDto>(token); } catch (Exception ex) { };
            string ip = "127.0.0.1";
            if (request.HttpContext.Connection.RemoteIpAddress != null) {
                ip = request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
            if (user == null) {
                user = new UserDto();
                user.Id = 1;
                user.Username = _loginUser.Username;
            } else if (user.Id == 0) {
                user.Id = 1;
                user.Username = _loginUser.Username;
            }
            var log = new LogDto {
                LogType = _title,
                Params = param,
                Time = GetTimeStamp() - _time,
                CreatorId = user.Id,
                Username = user.Username,
                RequestIp = ip,
                Browser = request.Headers.UserAgent.ToString(),
                Address = request.Headers.Host.ToString() + filterContext.HttpContext.Request.Path,
                Method = request.Method.ToLower(),
                Description = _title
            };
            //异步写日志到数据库
            await Redis.CreateInstance().PushCache<LogDto>(GlobalConsts.REDIS_QUEUE_LOG, log);
        }
    }
}
