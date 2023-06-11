using Microsoft.AspNetCore.Mvc;
using Light.Admin.Controllers;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Common.Filter;
using Light.Entity;

namespace Light.Controllers {
    /// <summary>
    /// 地区表 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class AreaController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public AreaController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 地区表 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Area> ListPage(AreaQueryDto queryDto) {
            var where = PredicateExtend.True<Area>();
            if (queryDto.ParentId != null) {
                where = where.And(t => t.ParentId == queryDto.ParentId);
            }
            if (!string.IsNullOrEmpty(queryDto.Name)) {
                where = where.And(t => t.Name.Contains(queryDto.Name));
            }
            if (!string.IsNullOrEmpty(queryDto.PostalCode)) {
                where = where.And(t => t.PostalCode.Contains(queryDto.PostalCode));
            }

            var queryWhere = _db.Areas
                .Where(where);

            var query = queryWhere.OrderBy(t => t.Sort).ThenByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Area>(list, queryDto);
        }

        /// <summary>
        /// 地区表 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Area? Find(int id) {
            return _db.Areas.Find(id);
        }

        /// <summary>
        /// 新增或更新地区表
        /// </summary>
        /// <param name="one">地区表</param>
		[HttpPost]
        [Log("新增或更新地区表")]
        public void Save(Area one) {
            if (one.Id != 0) {
                _db.Areas.Update(one);
            } else {
                _db.Areas.Add(one);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除地区表
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Route("{id?}")]
        [Log("删除地区表")]
        public void Delete(int id) {
            var find = _db.Areas.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Areas.Remove(find);
            _db.SaveChanges();
        }
    }
}