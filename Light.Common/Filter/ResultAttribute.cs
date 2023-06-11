using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Light.Common.Dto;

namespace Light.Common.Filter {
    public class ResultAttribute : Attribute, IResultFilter {

        /// <summary>
        /// action 执行
        /// </summary>
        /// <param name="context"></param>
        public void OnResultExecuted(ResultExecutedContext context) {

        }


        /// <summary>
        /// 判断是否需要放开权限限定
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsignoreResult(ResultExecutingContext context) {
            bool ignoreResult = false;
            //获取请求进来的控制器与方法
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null) {
                //判断请求的控制器有没有NoPermissionRequiredAttribute（不需要权限判断）
                ignoreResult = controllerActionDescriptor.ControllerTypeInfo.IsDefined(typeof(IgnoreResultAttribute), true);
                if (!ignoreResult) {
                    //判断请求的方法有没有NoPermissionRequiredAttribute
                    ignoreResult = controllerActionDescriptor.MethodInfo.IsDefined(typeof(IgnoreResultAttribute), true);
                }
            }
            return ignoreResult;
        }

        public void OnResultExecuting(ResultExecutingContext context) {
            if (IsignoreResult(context)) {
                return;
            }
            var result = new ApiResult(HttpStatusCode.OK, "成功");
            var _ms = ((Controller)(context.Controller)).ModelState;
            //模型验证码处理
            if (!_ms.IsValid) {
                var _FirstErrorField = _ms.FirstOrDefault();
                string strHtmlId = _FirstErrorField.Key;
                string strErrorMessage = _FirstErrorField.Value.Errors.FirstOrDefault().ErrorMessage;//这个数据你想怎么给JS都行.
                result = new ApiResult(HttpStatusCode.InternalServerError, strErrorMessage);

            } else {
                try {
                    ObjectResult rep = (ObjectResult)context.Result;
                    result.Data = rep.Value;
                } catch (Exception) {

                }
            }
            var serializerSettings = new JsonSerializerSettings {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            var ret = JsonConvert.SerializeObject(result, Formatting.None, serializerSettings);
            context.Result = new ContentResult {
                // 返回状态码设置为200，表示成功
                StatusCode = (int)HttpStatusCode.OK,
                // 设置返回格式
                ContentType = "application/json;charset=utf-8",
                Content = ret
            };

        }
    }
}
