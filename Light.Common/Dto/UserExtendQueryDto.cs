using System.ComponentModel;
namespace Light.Common.Dto {
    public class UserExtendQueryDto : PageInfo {
        [DisplayName("账号")]
        public String Username { get; set; }

    }
}