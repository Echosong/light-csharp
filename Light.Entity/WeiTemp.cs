using Light.Common.Attributes;

namespace Light.Entity {

    [AutoEntity(Name = "微信公众号关注临时存储", controller = false, dto = false, viewFrom = false, viewList = false)]
    public class WeiTemp : SysBase {
        public string OpenId { get; set; } = string.Empty;

        public string? UnionId { get; set; } = string.Empty;
    }
}
