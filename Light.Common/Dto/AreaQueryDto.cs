using System.ComponentModel;
namespace Light.Common.Dto {
    public class AreaQueryDto : PageInfo {
        [DisplayName("父级ID")]
        public Int32? ParentId { get; set; }
        [DisplayName("地区名称")]
        public String Name { get; set; }
        [DisplayName("邮编")]
        public String PostalCode { get; set; }

    }
}