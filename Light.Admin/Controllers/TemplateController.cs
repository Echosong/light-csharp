using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 转发短信模板 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class TemplateController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public TemplateController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 转发短信模板 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Template> ListPage(TemplateQueryDto queryDto) {
            var where = PredicateExtend.True<Template>();
            if (queryDto.Type != 0) {
                where = where.And(t => t.Type == queryDto.Type);
            }
            if (!string.IsNullOrEmpty(queryDto.code)) {
                where = where.And(t => t.Code.Contains(queryDto.code));
            }

            var queryWhere = _db.Templates
                .Where(where);

            var query = queryWhere.OrderBy(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Template>(list, queryDto);
        }

        /// <summary>
        /// 转发短信模板 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Template? Find(int id) {
            return _db.Templates.Find(id);
        }

        /// <summary>
        /// 新增活更新转发短信模板
        /// </summary>
        /// <param name="one">转发短信模板</param>
		[HttpPost]
        public void Save(Template one) {
            if (string.IsNullOrEmpty(one.Code)) {
                one.Code = _db.GetCode(typeof(Template));
            }
            if (one.Id != 0) {
                _db.Templates.Update(one);
            } else {
                _db.Templates.Add(one);
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
            var find = _db.Templates.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Templates.Remove(find);
            _db.SaveChanges();
        }
    }
}