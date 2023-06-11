using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    [AutoEntity(Name = "文章")]
    public class Article : SysBase {
        /// <summary> 标题 </summary>
        [DisplayName("标题")]
        [Required]
        [AutoEntityField(Query = true), MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [DisplayName("分类")]
        [Required]
        [AutoEntityField(Query = true, EnumType = typeof(ArticleTypeEnum))]
        public int Type { get; set; }

        [DisplayName("图片")]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.Upload), MaxLength(200)]
        public string Img { get; set; } = string.Empty;

        [AutoEntityField(Name = "作者"), MaxLength(20)]
        public string Author { get; set; } = string.Empty;

        [DisplayName("时间")]
        public DateTime Time { get; set; }

        [DisplayName("简介"), MaxLength(1000)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.TextArea)]
        public string Info { get; set; } = string.Empty;

        [DisplayName("内容")]
        [Required]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.TextEdit, List = false)]
        public string Content { get; set; } = string.Empty;
    }
}
