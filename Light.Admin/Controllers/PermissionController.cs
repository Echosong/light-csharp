using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 权限表 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class PermissionController : BaseController {

        public PermissionController(Db db) {
            this._db = db;
        }

        /// <summary>
        ///     获取某个用户的权限
        ///     truncate table RolePermissions
        ///     insert into RolePermissions(RoleId, PermissionId) select 1, id from Permissions
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("{id?}")]
        public List<Permission> ListByUser() {
            var permissions = from p in _db.Permissions
                              join rp in _db.RolePermissions on p.Id equals rp.PermissionId
                              where rp.RoleId == _user.RoleId
                              select p;
            return permissions.OrderBy(t => t.Sort).ToList();
        }

        /// <summary>
        ///     根据角色获取权限
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id?}")]
        public List<Permission> ListByRole(int id) {
            var permissions = from p in _db.Permissions
                              join rp in _db.RolePermissions on p.Id equals rp.PermissionId
                              where rp.RoleId == id
                              select p;
            return permissions.OrderBy(t => t.Sort).ToList();
        }

        /// <summary>
        ///     处理绑定关系【前端存储缓存bug】
        /// </summary>
        /// <param name="id"> 角色id</param>
        /// <param name="permissionIds">传入权限需要绑定</param>
        [HttpPost]
        [Route("{id?}")]
        public void UpdateRolePermissions(int id, [FromBody] List<int> permissionIds) {
            _db.RolePermissions.Where(t => t.RoleId == id).BatchDelete();
            var rolePermissions = new List<RolePermission>();
            permissionIds.ForEach(t => {
                var rolePermission = new RolePermission {
                    RoleId = id,
                    PermissionId = t
                };
                rolePermissions.Add(rolePermission);
            });
            _db.RolePermissions.AddRange(rolePermissions);
            _db.SaveChanges();
        }

        /// <summary>
        /// 分页查询 权限表 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Permission> ListPage(PermissionQueryDto queryDto) {
            var where = PredicateExtend.True<Permission>();
            if (!string.IsNullOrEmpty(queryDto.Name)) {
                where = where.And(t => t.Name != null && t.Name.Contains(queryDto.Name));
            }

            var queryWhere = _db.Permissions
                .Where(where);

            var query = queryWhere.OrderBy(t => t.Sort).ThenByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Permission>(list, queryDto);
        }

        /// <summary>
        /// 权限表 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Permission? Find(int id) {
            return _db.Permissions.Find(id);
        }

        /// <summary>
        /// 新增活更新权限表
        /// </summary>
        /// <param name="one">权限表</param>
		[HttpPost]
        public void Save(Permission one) {
            if (one.Id != 0) {
                _db.Permissions.Update(one);
            } else {
                _db.Permissions.Add(one);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        public void Delete(int id) {
            var find = _db.Permissions.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Permissions.Remove(find);
            _db.SaveChanges();
        }
    }
}