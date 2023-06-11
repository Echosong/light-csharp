using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 省分控线表 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProvinceScoreController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public ProvinceScoreController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 省分控线表 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<ProvinceScore> ListPage(ProvinceScoreQueryDto queryDto) {
            var where = PredicateExtend.True<ProvinceScore>();

            var queryWhere = _db.ProvinceScores
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<ProvinceScore>(list, queryDto);
        }

        /// <summary>
        /// 省分控线表 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public ProvinceScore? Find(int id) {
            return _db.ProvinceScores.Find(id);
        }

        /// <summary>
        /// 新增活更新省分控线表
        /// </summary>
        /// <param name="one">省分控线表</param>
		[HttpPost]
        public void Save(ProvinceScore one) {
            if (one.Id != 0) {
                _db.ProvinceScores.Update(one);
            } else {
                _db.ProvinceScores.Add(one);
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
            var find = _db.ProvinceScores.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.ProvinceScores.Remove(find);
            _db.SaveChanges();
        }
    }
}