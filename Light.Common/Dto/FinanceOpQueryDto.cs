using System.ComponentModel;
namespace Light.Common.Dto {
    public class FinanceOpQueryDto : PageInfo {
        [DisplayName("账号")]
        public String Username { get; set; }
        [DisplayName("账户类型")]
        public Int32? BusinessType { get; set; }


        public int State { get; set; }

    }
}