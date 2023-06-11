using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {

    [AutoEntity(Name = "付费下载")]
    public class Download : SysBase {
        [AutoEntityField(Name = "文件名"), MaxLength(100)]
        public String? Title { get; set; }

        [AutoEntityField(Name = "价格")]
        public decimal Price { get; set; } = 0;

        [AutoEntityField(Name = "文件路径", TypeEnum = HtmlTypeEnum.File), MaxLength(200)]
        public String? Url { get; set; }

        [AutoEntityField(Name = "VIP免费")]
        public int VipFree { get; set; } = 1;

        [AutoEntityField(Name = "首页显示")]
        public int Index { get; set; } = 1;
    }
}
