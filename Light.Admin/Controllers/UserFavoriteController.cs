using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 用户收藏信息 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserFavoriteController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public UserFavoriteController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 用户收藏信息 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<UserFavorite> ListPage(UserFavoriteQueryDto queryDto) {
            var where = PredicateExtend.True<UserFavorite>();

            var queryWhere = _db.UserFavorites
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<UserFavorite>(list, queryDto);
        }

        /// <summary>
        /// 用户收藏信息 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public UserFavorite? Find(int id) {
            return _db.UserFavorites.Find(id);
        }

        /// <summary>
        /// 新增活更新用户收藏信息
        /// </summary>
        /// <param name="one">用户收藏信息</param>
		[HttpPost]
        public void Save(UserFavorite one) {
            if (one.Id != 0) {
                _db.UserFavorites.Update(one);
            } else {
                _db.UserFavorites.Add(one);
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
            var find = _db.UserFavorites.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.UserFavorites.Remove(find);
            _db.SaveChanges();
        }
    }
}