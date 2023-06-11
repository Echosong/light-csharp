using System.ComponentModel;
namespace Light.Common.Dto {
    public class SmsQueryDto : PageInfo {
        [DisplayName("接受对象")]
        public String Mobile { get; set; }
        [DisplayName("状态")]
        public Int32 State { get; set; }
        [DisplayName("模板类型")]
        public Int32 Type { get; set; }
        [DisplayName("模板编码")]
        public String Code { get; set; }

    }
}