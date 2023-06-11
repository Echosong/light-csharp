using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    public class TemplateBase : SysBase {
        [DisplayName("模板类型")]
        [AutoEntityField(Query = true, EnumType = typeof(TemplateTypeEnum))]
        public int Type { get; set; } = 0;

        [DisplayName("模板编码"), MaxLength(100)]
        [AutoEntityField(Query = true)]
        public string Code { get; set; } = string.Empty;
    }
}
