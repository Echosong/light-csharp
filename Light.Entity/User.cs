using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    [AutoEntity(Name = "用户表", controller = true)]
    [Microsoft.EntityFrameworkCore.Index("OpenId", IsUnique = true)]
    [Microsoft.EntityFrameworkCore.Index("Username", IsUnique = true)]
    public class User : SysBase {
        [DisplayName("用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        [Phone]
        public string? Username { get; set; }

        [AutoEntityField(Name = "上级名称")]
        [NotMapped]
        public string? PUsername { get; set; } = "顶级";

        [DisplayName("姓名"), MaxLength(100)]
        [MinLength(2)]
        public string? Name { get; set; }

        [DisplayName("状态")]
        [AutoEntityField(Name = "状态", EnumType = typeof(UserStateEnum))]
        public int? State { get; set; }

        [DisplayName("上级")]
        [AutoEntityField(Name = "上级", Query = true)]
        public int ParentId { get; set; } = 0;

        [DisplayName("上上级")]
        [AutoEntityField(Name = "上上级")]
        public int GrandpaId { get; set; }

        [DisplayName("备注"), MaxLength(100)]
        public string? Email { get; set; }

        [DisplayName("注册ip"), MaxLength(100)]
        public string? RegIp { get; set; }

        [DisplayName("登陆ip"), MaxLength(100)]
        public string? LoginIp { get; set; }

        [DisplayName("密码")]
        [AutoEntityField(List = false), MaxLength(100)]
        public string? Password { get; set; }

        [DisplayName("角色")]
        public int RoleId { get; set; }

        [AutoEntityField(Name = "用户类型", Query = true, EnumType = typeof(UserTypeEnum))]
        public int Type { get; set; }

        [DisplayName("部门领导")]
        [AutoEntityField(EnumType = typeof(BaseTypeEnum))]
        public int Lead { get; set; }

        [DisplayName("性别")]
        [AutoEntityField(EnumType = typeof(BaseSexEnum))]
        public int Sex { get; set; }

        [DisplayName("生日"), MaxLength(25)]
        public string? Birthday { get; set; }

        [DisplayName("角色名称不映射")]
        [NotMapped]
        public string? RoleName { get; set; }

        [DisplayName("小程序openId"), MaxLength(100)]
        public string? OpenId { get; set; }




    }
}