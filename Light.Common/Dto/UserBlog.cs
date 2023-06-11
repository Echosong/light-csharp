using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Common.Dto {
    public class UserBlog {
        [DisplayName("账号"), MaxLength(100)]
        [AutoEntityField(Query = true)]
        public string? Username { get; set; }

        [DisplayName("关联用户id")]
        public int UserId { get; set; }

        [DisplayName("头像"), MaxLength(300)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.Upload)]
        public string? Avatar { get; set; }

        [DisplayName("个性签名"), MaxLength(255)]
        public string? Signlogo { get; set; }

        [DisplayName("昵称")]
        public string? Name { get; set; }

        [DisplayName("等级")]
        public int? Level { get; set; } = 0;

        public int Sex { get; set; } = 0;
    }
}
