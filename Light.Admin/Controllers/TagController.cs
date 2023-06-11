using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 标签 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class TagController : BaseController {
        public TagController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 标签 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Tag> ListPage(TagQueryDto queryDto) {
            var where = PredicateExtend.True<Tag>();
            if (!string.IsNullOrEmpty(queryDto.Name)) {
                where = where.And(t => t.Name.Contains(queryDto.Name));
            }

            var queryWhere = _db.Tags
                .Where(where);

            var query = queryWhere.OrderBy(t => t.Sort).ThenByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Tag>(list, queryDto);
        }

        /// <summary>
        /// 标签 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Tag? Find(int id) {
            return _db.Tags.Find(id);
        }

        /// <summary>
        /// 新增活更新标签
        /// </summary>
        /// <param name="one">标签</param>
		[HttpPost]
        public void Save(Tag one) {
            if (one.Id != 0) {
                _db.Tags.Update(one);
            } else {
                _db.Tags.Add(one);
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
            var find = _db.Tags.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Tags.Remove(find);
            _db.SaveChanges();
        }
    }
}