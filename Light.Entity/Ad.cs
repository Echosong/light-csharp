using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;


namespace Light.Entity {

    [AutoEntity(Name = "广告banner")]
    public class Ad : SysBase {
        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int Sort { get; set; }

        [DisplayName("标题"), MaxLength(255)]
        [AutoEntityField(Query = true)]
        public string? Title { get; set; }

        [DisplayName("图片"), MaxLength(255)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.Upload)]
        public string? Image { get; set; }

        [DisplayName(@"位置")]
        [AutoEntityField(EnumType = typeof(AdPostionEnum))]
        public int? Postion { get; set; }

        [DisplayName(@"链接")]
        public string? Url { get; set; }
    }
}
