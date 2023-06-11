using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;

namespace Light.Entity {
    /// <summary>
    ///     系统日志
    /// </summary>
    [AutoEntity(Name = "系统日志")]
    public class Log : SysBase {

        [DisplayName("用户名称"), MaxLength(50)]
        public string? Username { get; set; }

        [DisplayName("请求ip"), MaxLength(200)]
        [AutoEntityField(Query = true)]
        public string? RequestIp { get; set; }

        [DisplayName("地址"), MaxLength(200)]
        public string? Address { get; set; }


        [DisplayName("描述")]
        [MaxLength(2000)]
        [AutoEntityField(Query = true)]
        public string Description { get; set; } = "";

        [DisplayName("浏览器"), MaxLength(800)]
        public string? Browser { get; set; }

        [DisplayName("请求耗时")]
        public long Time { get; set; }

        [DisplayName("请求方式"), MaxLength(50)]
        public string? Method { get; set; }

        [DisplayName("参数")]
        [MaxLength(2000)]
        public string? Params { get; set; }

        [DisplayName("日志类型")]
        [AutoEntityField(Query = true)]
        public string? LogType { get; set; }

        [DisplayName("异常详情")]
        [StringLength(2000)]
        public string? ExceptionDetail { get; set; }
    }
}