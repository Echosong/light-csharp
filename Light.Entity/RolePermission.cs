using Light.Common.Attributes;

namespace Light.Entity {
    [AutoEntity(Name = "角色权限", controller = false, viewFrom = false, viewList = false, dto = false)]
    public class RolePermission : SysBase {

        public int RoleId { get; set; }

        public int PermissionId { get; set; }
    }
}