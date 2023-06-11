using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    [AutoEntity(Name = "标签")]
    public class Tag : SysBase {
        [DisplayName("类型")]
        [Required]
        [AutoEntityField(EnumType = typeof(TagTypeEnum))]
        public int Type { get; set; }

        [DisplayName("标签名称"), MaxLength(255)]
        [Required]
        [AutoEntityField(Query = true)]
        public string? Name { get; set; }

        [DisplayName("描述"), MaxLength(500)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.TextArea)]
        public string? Description { get; set; }

        [DisplayName("排序")]
        public int Sort { get; set; } = 0;

        [DisplayName("热度")]
        public int HitCount { get; set; } = 1;
    }
}
