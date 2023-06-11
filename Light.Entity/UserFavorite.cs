using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    [AutoEntity(Name = "用户收藏信息")]
    public class UserFavorite : SysBase {
        [AutoEntityField(Name = "用户id")]
        public int UserId { get; set; }

        [AutoEntityField(Name = "用户名", Len = 100), MaxLength(100)]
        public string UserName { get; set; }

        [AutoEntityField(Name = "类型", EnumType = typeof(FavoriteTypeEnum))]
        public int Type { get; set; }

        [AutoEntityField(Name = "相关id")]
        public int RelateId { get; set; }

        [AutoEntityField(Name = "标题")]
        public string Name { get; set; } = string.Empty;


    }
}
