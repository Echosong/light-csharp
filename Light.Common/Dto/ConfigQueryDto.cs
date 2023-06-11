using System.ComponentModel;
namespace Light.Common.Dto {
    public class ConfigQueryDto : PageInfo {
        [DisplayName("唯一标识")]
        public String Code { get; set; }
        [DisplayName("名称")]
        public String Name { get; set; }

    }
}