using System.Net;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Common.RedisCache;
using Light.Common.Utils;

namespace Light.Common.Filter {
    public class AuthFilterForAdmin : ActionFilterAttribute {

        /// <summary>
        /// 防止重复点击
        /// </summary>
        private static bool RepeatSubmit(string token, string localPath, string method) {
            if (method.ToUpper() == "GET" || method.ToUpper() == "PUT") {
                return false;
            }
            string key = $"{token}_{localPath}";
            var redis = Redis.CreateInstance();
            if (redis.Exists(key)) {
                return true;
            } else {
                //提交数据 更新或者修改的 考虑到分页list 用的也是put 所以put 也做下处理
                long timeOut = 1000;

                redis.SetCache(key, 1, TimeSpan.FromMilliseconds(timeOut));
                return false;
            }
        }

        /// <summary>
        /// 判断是否需要放开权限限定
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsOpenPermissionRequired(ActionExecutingContext context) {
            bool isNoPermissionRequired = false;
            //获取请求进来的控制器与方法
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null) {
                //判断请求的控制器有没有NoPermissionRequiredAttribute（不需要权限判断）
                isNoPermissionRequired = controllerActionDescriptor.ControllerTypeInfo.IsDefined(typeof(NoPermissionAttribute), true);
                if (!isNoPermissionRequired) {
                    //判断请求的方法有没有NoPermissionRequiredAttribute
                    isNoPermissionRequired = controllerActionDescriptor.MethodInfo.IsDefined(typeof(NoPermissionAttribute), true);
                }
            }
            return isNoPermissionRequired;
        }


        /// <summary>
        /// 权限判断
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context) {
            //放开权限
            if (IsOpenPermissionRequired(context)) {
                return;
            }
            var request = context.HttpContext.Request;
            var token = context.HttpContext.Request.Headers["token"].ToString();
            var noLogin = true;

            if (!string.IsNullOrEmpty(token)) {

                if (token == AppSettingsConstVars.AppConfigSwagger) {
                    return;
                }
                try {
                    var user = Redis.CreateInstance().GetCache<UserDto>(token);
                    if (user != null) {
                        noLogin = false;
                    }
                } catch (Exception e) {
                    LogHelper.Error("未登录", e);
                }
            }
            //没有登录
            if (noLogin) {
                throw new BaseException(HttpStatusCode.Gone, "未登录");
            } else {
                if (RepeatSubmit(token, request.Path, request.Method)) {
                    throw new BaseException(HttpStatusCode.AlreadyReported, "http重复请求");
                }
            }
            //模型验证码不通过
            if (!context.ModelState.IsValid) {
                //获取验证失败的模型字段
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .Select(e => e.Value.Errors.First().ErrorMessage)
                    .ToList();

                var str = string.Join("|", errors);
                if (string.IsNullOrEmpty(str)) return;
                throw new BaseException(HttpStatusCode.Forbidden, str);
            }
        }
    }
}
