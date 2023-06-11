using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 广告banner 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class AdController : BaseController {

        public AdController(Db db) {
            this._db = db;
            this._db.UserId = _user.Id;

        }



        /// <summary>
        /// 分页查询 广告banner 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Ad> ListPage(AdQueryDto queryDto) {
            var where = PredicateExtend.True<Ad>();
            if (!string.IsNullOrEmpty(queryDto.Title)) {
                where = where.And(t => t.Title.Contains(queryDto.Title));
            }

            var queryWhere = _db.Ads
                .Where(where);

            var query = queryWhere.OrderBy(t => t.Sort).ThenByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Ad>(list, queryDto);
        }

        /// <summary>
        /// 广告banner 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Ad? Find(int id) {
            return _db.Ads.Find(id);
        }

        /// <summary>
        /// 新增活更新广告banner
        /// </summary>
        /// <param name="one">广告banner</param>
		[HttpPost]
        public void Save(Ad one) {
            if (one.Id != 0) {
                _db.Ads.Update(one);
            } else {
                _db.Ads.Add(one);
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
            var find = _db.Ads.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Ads.Remove(find);
            _db.SaveChanges();
        }
    }
}