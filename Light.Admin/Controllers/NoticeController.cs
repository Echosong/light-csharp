using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 消息公告 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class NoticeController : BaseController {
        public NoticeController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 消息公告 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Notice> ListPage(NoticeQueryDto queryDto) {
            var where = PredicateExtend.True<Notice>();

            var queryWhere = _db.Notices
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Notice>(list, queryDto);
        }

        /// <summary>
        /// 消息公告 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        public Notice? Find(int id) {
            return _db.Notices.Find(id);
        }

        /// <summary>
        /// 新增活更新消息公告
        /// </summary>
        /// <param name="one">消息公告</param>
		[HttpPost]
        public void Save(Notice one) {
            if (one.Id != 0) {
                _db.Notices.Update(one);
            } else {
                _db.Notices.Add(one);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        public void Delete(int id) {
            var find = _db.Notices.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Notices.Remove(find);
            _db.SaveChanges();
        }
    }
}