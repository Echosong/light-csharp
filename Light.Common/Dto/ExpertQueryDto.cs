using System.ComponentModel;
namespace Light.Common.Dto {
    public class ExpertQueryDto : PageInfo {
        [DisplayName("姓名")]
        public String Name { get; set; }
        [DisplayName("类型")]
        public Int32 Type { get; set; }

    }
}