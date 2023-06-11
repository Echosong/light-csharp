using Microsoft.AspNetCore.Mvc;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 评论 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class CommentController : BaseController {
        public CommentController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 评论 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Comment> ListPage(CommentQueryDto queryDto) {
            var where = PredicateExtend.True<Comment>();

            var queryWhere = _db.Comments
                .Where(where);
            //分站主
            if (_user.RoleId == GlobalConsts.AGENT_ROLEID) {
                where = where.And(t => t.SiteId == _user.Id);
            }

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Comment>(list, queryDto);
        }

        /// <summary>
        /// 评论 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Comment? Find(int id) {
            return _db.Comments.Find(id);
        }

        /// <summary>
        /// 新增活更新评论
        /// </summary>
        /// <param name="one">评论</param>
		[HttpPost]
        public void Save(Comment one) {
            if (one.Id != 0) {
                _db.Comments.Update(one);
            } else {
                _db.Comments.Add(one);
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
            var find = _db.Comments.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Comments.Remove(find);
            _db.SaveChanges();
        }
    }
}