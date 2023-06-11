using Microsoft.AspNetCore.Mvc;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 扩展信息 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserExtendController : BaseController {

        public UserExtendController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 扩展信息 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<UserExtend> ListPage(UserExtendQueryDto queryDto) {
            var where = PredicateExtend.True<UserExtend>();
            if (!string.IsNullOrEmpty(queryDto.Username)) {
                where = where.And(t => t.Username.Contains(queryDto.Username));
            }

            //分站
            if (_user.RoleId == GlobalConsts.AGENT_ROLEID) {
                where = where.And(t => t.SiteId == _user.Id);
            }
            IQueryable<UserExtend> queryWhere;

            if (_user.RoleId == GlobalConsts.USER_ROLEID) {
                queryWhere = (from e in _db.UserExtends
                              join u in _db.Users on e.UserId equals u.Id
                              where u.ParentId == _user.Id
                              select e).Where(where);
            } else {
                queryWhere = _db.UserExtends
                    .Where(where);
            }
            var query = queryWhere.OrderByDescending(t => t.Id)
                   .Skip(queryDto.pageSize * (queryDto.page - 1))
                   .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<UserExtend>(list, queryDto);
        }

        /// <summary>
        /// 扩展信息 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public UserExtend? Find(int id) {
            return _db.UserExtends.Find(id);
        }

        /// <summary>
        /// 新增活更新扩展信息
        /// </summary>
        /// <param name="one">扩展信息</param>
		[HttpPost]
        public void Save(UserExtend one) {
            if (one.Id != 0) {
                _db.UserExtends.Update(one);
            } else {
                _db.UserExtends.Add(one);
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
            var find = _db.UserExtends.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.UserExtends.Remove(find);
            _db.SaveChanges();
        }
    }
}