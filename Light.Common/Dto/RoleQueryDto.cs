using System.ComponentModel;
namespace Light.Common.Dto {
    public class RoleQueryDto : PageInfo {
        [DisplayName("角色名称")]
        public String Name { get; set; }

    }
}