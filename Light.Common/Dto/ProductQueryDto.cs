using System.ComponentModel;
namespace Light.Common.Dto {
    public class ProductQueryDto : PageInfo {
        [DisplayName("商品标题")]
        public String Title { get; set; }

    }
}