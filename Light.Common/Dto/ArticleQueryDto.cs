using System.ComponentModel;
namespace Light.Common.Dto {
    public class ArticleQueryDto : PageInfo {
        [DisplayName("标题")]
        public String Title { get; set; }
        [DisplayName("分类")]
        public Int32 Type { get; set; }

    }
}