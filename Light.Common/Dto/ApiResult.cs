using System.Net;

namespace Light.Common.Dto {

    public class ApiResult {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public ApiResult(HttpStatusCode code, string message) {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// 状态代码
        /// </summary>
        public HttpStatusCode Code { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; } = true;
    }
}