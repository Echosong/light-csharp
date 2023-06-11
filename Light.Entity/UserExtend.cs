using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {

    [AutoEntity(Name = "扩展信息")]
    [Index("UserId", IsUnique = true)]
    [Index("UnionId", IsUnique = false)]
    public class UserExtend : SysBase {

        [DisplayName("账号"), MaxLength(100)]
        [AutoEntityField(Query = true)]
        public string? Username { get; set; }

        [DisplayName("关联用户id")]
        public int UserId { get; set; }

        [DisplayName("头像"), MaxLength(300)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.Upload)]
        public string? Avatar { get; set; }

        [DisplayName("地址")]
        public string? Address { get; set; }

        [DisplayName("账户金币")]
        public Decimal? Balance { get; set; } = 0;

        [DisplayName("累计收入")]
        public Decimal BalanceTotal { get; set; } = 0;

        [DisplayName("经度")]
        public Decimal? Longitude { get; set; } = 0;

        [DisplayName("纬度")]
        public Decimal? Latitude { get; set; } = 0;

        [DisplayName(@"等级")]
        [AutoEntityField(EnumType = typeof(UserLevelEnum))]
        public int? Level { get; set; } = (int)UserLevelEnum.普通会员;

        [DisplayName("个性签名"), MaxLength(255)]
        public string? Signlogo { get; set; }

        [DisplayName("查看数据")]
        public int? HitCount { get; set; } = 0;

        [DisplayName("在线状态")]
        [AutoEntityField(EnumType = typeof(UserOnlineEnum))]
        public int? Online { get; set; } = (int)UserOnlineEnum.在线;

        [DisplayName("会员到期时间")]
        public DateTime? Expire { get; set; } = new DateTime();

        [DisplayName("微信公众号openId"), MaxLength(100)]
        public string? OpenId { get; set; } = string.Empty;

        [DisplayName("小程序公众号联合键"), MaxLength(100)]
        public string? UnionId { get; set; }

    }
}
