using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 部门 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class RoleController : BaseController {

        public RoleController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 部门 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Role> ListPage(RoleQueryDto queryDto) {
            var where = PredicateExtend.True<Role>();
            if (!string.IsNullOrEmpty(queryDto.Name)) {
                where = where.And(t => t.Name.Contains(queryDto.Name));
            }

            var queryWhere = _db.Roles
                .Where(where);

            var query = queryWhere.OrderBy(t => t.Sort).ThenByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Role>(list, queryDto);
        }

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Role> List() {
            return _db.Roles.ToList();
        }

        /// <summary>
        /// 部门 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Role? Find(int id) {
            return _db.Roles.Find(id);
        }

        /// <summary>
        /// 新增活更新部门
        /// </summary>
        /// <param name="one">部门</param>
		[HttpPost]
        public void Save(Role one) {
            if (one.Id != 0) {
                _db.Roles.Update(one);
            } else {
                _db.Roles.Add(one);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Route("{id?}")]
        public void Delete(int id) {
            var find = _db.Roles.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Roles.Remove(find);
            _db.SaveChanges();
        }
    }
}