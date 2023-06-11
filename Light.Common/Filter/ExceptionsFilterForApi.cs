
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Common.Utils;

namespace Light.Common.Filter {
    public class ExceptionsFilterForApi : IExceptionFilter {

        public void OnException(ExceptionContext context) {
            LogHelper.Error(context.Exception.Message);

            //处理各种异常
            var jm = new ApiResult(HttpStatusCode.InternalServerError, context.Exception.Message);

            if (context.Exception is BaseException) {
                var baseExcption = context.Exception as BaseException;
                if (baseExcption != null) {
                    jm = new ApiResult(baseExcption.Code, baseExcption.Message);
                }
            }

            var serializerSettings = new JsonSerializerSettings {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            var ret = JsonConvert.SerializeObject(jm, Formatting.None, serializerSettings);

            context.Result = new ContentResult {
                StatusCode = (int)HttpStatusCode.OK,
                ContentType = "application/json;charset=utf-8",
                Content = ret
            };
        }
    }
}
