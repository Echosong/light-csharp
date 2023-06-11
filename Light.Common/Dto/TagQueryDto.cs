using System.ComponentModel;
namespace Light.Common.Dto {
    public class TagQueryDto : PageInfo {
        [DisplayName("标签名称")]
        public String Name { get; set; }

    }
}