using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 系统日志 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class LogController : BaseController {

        public LogController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 系统日志 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Log> ListPage(LogQueryDto queryDto) {
            var where = PredicateExtend.True<Log>();
            if (!string.IsNullOrEmpty(queryDto.RequestIp)) {
                where = where.And(t => t.RequestIp.Contains(queryDto.RequestIp));
            }
            if (!string.IsNullOrEmpty(queryDto.Description)) {
                where = where.And(t => t.Description.Contains(queryDto.Description));
            }
            if (!string.IsNullOrEmpty(queryDto.LogType)) {
                where = where.And(t => t.LogType.Contains(queryDto.LogType));
            }

            var queryWhere = _db.Logs
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Log>(list, queryDto);
        }

        /// <summary>
        /// 系统日志 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Log? Find(int id) {
            return _db.Logs.Find(id);
        }

        /// <summary>
        /// 新增活更新系统日志
        /// </summary>
        /// <param name="one">系统日志</param>
		[HttpPost]
        public void Save(Log one) {
            if (one.Id != 0) {
                _db.Logs.Update(one);
            } else {
                _db.Logs.Add(one);
            }
            _db.SaveChanges();
        }
    }
}