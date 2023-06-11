using System.ComponentModel;
namespace Light.Common.Dto {
    public class PermissionQueryDto : PageInfo {
        [DisplayName("权限名称")]
        public string? Name { get; set; }

    }
}