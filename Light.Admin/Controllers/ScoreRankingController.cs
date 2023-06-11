using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 一分一段表 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ScoreRankingController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public ScoreRankingController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 一分一段表 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<ScoreRanking> ListPage(ScoreRankingQueryDto queryDto) {
            var where = PredicateExtend.True<ScoreRanking>();

            var queryWhere = _db.ScoreRankings
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<ScoreRanking>(list, queryDto);
        }

        /// <summary>
        /// 一分一段表 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public ScoreRanking? Find(int id) {
            return _db.ScoreRankings.Find(id);
        }

        /// <summary>
        /// 新增活更新一分一段表
        /// </summary>
        /// <param name="one">一分一段表</param>
		[HttpPost]
        public void Save(ScoreRanking one) {
            if (one.Id != 0) {
                _db.ScoreRankings.Update(one);
            } else {
                _db.ScoreRankings.Add(one);
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
            var find = _db.ScoreRankings.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.ScoreRankings.Remove(find);
            _db.SaveChanges();
        }
    }
}