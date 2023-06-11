using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;

namespace Light.Entity {
    [AutoEntity(Name = "权限表")]
    public class Permission : SysBase {

        [DisplayName("权限名称")]
        [AutoEntityField(Query = true)]
        public string? Name { get; set; }

        [DisplayName("权限描述")]
        [MaxLength(500)]
        public string? Description { get; set; }

        [DisplayName("权限标识")]
        public string? Url { get; set; }

        [Required]
        [DisplayName("路径标识")]
        public string? Perms { get; set; }

        [DisplayName("父级id")]
        public int ParentId { get; set; }

        [DisplayName("类型")]
        [DefaultValue(0)]
        [Range(1, 5, ErrorMessage = "验证码")]
        public int Type { get; set; }

        [DisplayName("排序")]
        public int Sort { get; set; } = 0;

        [DisplayName("图标")]
        public string Icon { get; set; }

        [DisplayName("是否显示")]
        public bool Show { get; set; } = true;
    }
}