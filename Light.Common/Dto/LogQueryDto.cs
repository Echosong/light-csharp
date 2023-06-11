using System.ComponentModel;
namespace Light.Common.Dto {
    public class LogQueryDto : PageInfo {
        [DisplayName("请求ip")]
        public String? RequestIp { get; set; }
        [DisplayName("描述")]
        public String? Description { get; set; }
        [DisplayName("日志类型")]
        public String? LogType { get; set; }

    }
}