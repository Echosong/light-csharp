using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    [AutoEntity(Name = "全局字典配置表")]
    public class Config : SysBase {

        [DisplayName("分组")]
        public string Group { get; set; }

        [DisplayName("唯一标识")]
        [AutoEntityField(Query = true), MaxLength(200)]
        public string? Code { get; set; }

        [DisplayName("配置值"), MaxLength(1000)]
        public string Value { get; set; }

        [DisplayName("名称"), MaxLength(255)]
        [AutoEntityField(Query = true)]
        public string? Name { get; set; }

        [DisplayName("类型")]
        [AutoEntityField(EnumType = typeof(HtmlTypeEnum))]
        public int Type { get; set; }

        [DisplayName("描述"), MaxLength(1000)]
        public string? Description { get; set; }
    }
}