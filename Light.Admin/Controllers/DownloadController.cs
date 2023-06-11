using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 付费下载 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class DownloadController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public DownloadController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 付费下载 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Download> ListPage(DownloadQueryDto queryDto) {
            var where = PredicateExtend.True<Download>();

            var queryWhere = _db.Downloads
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Download>(list, queryDto);
        }

        /// <summary>
        /// 付费下载 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Download? Find(int id) {
            return _db.Downloads.Find(id);
        }

        /// <summary>
        /// 新增活更新付费下载
        /// </summary>
        /// <param name="one">付费下载</param>
		[HttpPost]
        public void Save(Download one) {
            if (one.Id != 0) {
                _db.Downloads.Update(one);
            } else {
                _db.Downloads.Add(one);
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
            var find = _db.Downloads.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Downloads.Remove(find);
            _db.SaveChanges();
        }
    }
}