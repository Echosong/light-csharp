using System.ComponentModel;

namespace Light.Common.Dto {
    public class LogDto {

        /// <summary>
        /// 0 为系统内置 不允许删除
        /// </summary>
        [DisplayName("创建人")]
        public int? CreatorId { get; set; }

        [DisplayName("用户名称")]
        public string? Username { get; set; }

        [DisplayName("请求ip")]
        public string? RequestIp { get; set; }

        [DisplayName("地址")]
        public string? Address { get; set; }


        [DisplayName("描述")]

        public string Description { get; set; } = "";

        [DisplayName("浏览器")]
        public string? Browser { get; set; }

        [DisplayName("请求耗时")]
        public long Time { get; set; }

        [DisplayName("请求方式")]
        public string? Method { get; set; }

        [DisplayName("参数")]

        public string? Params { get; set; }

        [DisplayName("日志类型")]

        public string LogType { get; set; }

        [DisplayName("异常详情")]
        public string? ExceptionDetail { get; set; }
    }
}
