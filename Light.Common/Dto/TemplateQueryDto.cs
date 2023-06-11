using System.ComponentModel;
namespace Light.Common.Dto {
    public class TemplateQueryDto : PageInfo {
        [DisplayName("模板类型")]
        public Int32 Type { get; set; }
        [DisplayName("模板编码")]
        public String code { get; set; }

    }
}