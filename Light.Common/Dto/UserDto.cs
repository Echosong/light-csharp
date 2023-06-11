using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Common.Dto {
    public class UserDto {
        public int Id { get; set; }

        [DisplayName("创建时间")]
        [AutoEntityField(List = false)]
        public DateTime? CreateDateTime { get; set; }

        [DisplayName("更新时间")]
        [AutoEntityField(List = false)]
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 分站id
        /// </summary>
        public int SiteId { get; set; }

        [DisplayName("账号")]
        public string? Username { get; set; }

        [DisplayName("昵称")]
        public string? Name { get; set; }

        public int RoleId { get; set; }

        [DisplayName("状态")]
        public int? State { get; set; }

        [DisplayName("注册ip")]
        public string? RegIp { get; set; }

        [DisplayName("登陆ip")]
        public string? LoginIp { get; set; }

        [DisplayName("角色名称不映射")]
        public string? RoleName { get; set; }

        [DisplayName("微信登录openid")]
        public string? OpenId { get; set; }

        [DisplayName("头像")]
        public string? Avatar { get; set; }

        [DisplayName("性别")]
        public int Sex { get; set; }

        [DisplayName("地址")]
        public string? Address { get; set; }

        [DisplayName("code")]
        public string? Code { get; set; }

        [DisplayName("联合id")]
        public string? Unionid { get; set; }

        public int ParentId { get; set; }

        [DisplayName("生日"), MaxLength(20)]
        public string? Birthday { get; set; }


        [DisplayName("备注")]
        public string? Email { get; set; }

        [DisplayName("个性签名"), MaxLength(255)]
        public string? Signlogo { get; set; }


        [DisplayName(@"等级")]
        [AutoEntityField(EnumType = typeof(UserLevelEnum))]
        public int? Level { get; set; }

        [DisplayName("会员到期时间")]
        public DateTime? Expire { get; set; } = new DateTime();

        [DisplayName("查看次数")]
        public int? HitCount { get; set; }

        [DisplayName("金币数量")]
        public decimal? Balance { get; set; }

        [DisplayName("累计收入")]
        public Decimal BalanceTotal { get; set; } = 0;
        [AutoEntityField(Name = "用户类型", Query = true, EnumType = typeof(UserTypeEnum))]
        public int Type { get; set; }

        public string? PUsername { get; set; }

    }

    public class SimpleUser {
        public string Name { get; set; } = "";
        public int Id { get; set; }
        public string Username { get; set; } = "";

        public int RoleId { get; set; }

        public int Type { get; set; }

        public string? TypeEnum { get; set; }
    }
}
