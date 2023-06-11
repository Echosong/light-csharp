using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    /// <summary>
    /// 角色表
    /// </summary>
    [AutoEntity(Name = "部门")]
    public class Role : SysBase {
        [DisplayName("角色名称")]
        [AutoEntityField(Query = true), MaxLength(200)]
        [Required]
        public string Name { get; set; }

        [DisplayName("描述"), MaxLength(800)]
        [AutoEntityField(Name = "描述", Len = 1000, TypeEnum = HtmlTypeEnum.TextArea)]
        public string Description { get; set; }

        [DisplayName("排序")]
        public int? Sort { get; set; } = 0;

        [DisplayName("上级id")]
        public int ParentId { get; set; }

        [DisplayName("角色编码"), MaxLength(20)]
        public string Code { get; set; }
    }
}