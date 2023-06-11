using Microsoft.AspNetCore.Mvc;
using Light.Admin.Controllers;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Common.Filter;
using Light.Entity;

namespace Light.Controllers {
    /// <summary>
    /// 文章 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ArticleController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public ArticleController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 文章 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Article> ListPage(ArticleQueryDto queryDto) {
            var where = PredicateExtend.True<Article>();
            if (!string.IsNullOrEmpty(queryDto.Title)) {
                where = where.And(t => t.Title.Contains(queryDto.Title));
            }
            if (queryDto.Type != 0) {
                where = where.And(t => t.Type == queryDto.Type);
            }

            var queryWhere = _db.Articles
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Select(t=>new Article(){Id = t.Id, Img = t.Img, Title = t.Title, Time = t.Time,
                    CreateDateTime = t.CreateDateTime, Author = t.Author, Info = t.Info,Type = t.Type})
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Article>(list, queryDto);
        }

        /// <summary>
        /// 文章 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Article? Find(int id) {
            return _db.Articles.Find(id);
        }

        /// <summary>
        /// 新增或更新文章
        /// </summary>
        /// <param name="one">文章</param>
		[HttpPost]
        [Log("新增或更新文章")]
        public void Save(Article one) {
            if (one.Id != 0) {
                _db.Articles.Update(one);
            } else {
                _db.Articles.Add(one);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Route("{id?}")]
        [Log("删除文章")]
        public void Delete(int id) {
            var find = _db.Articles.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Articles.Remove(find);
            _db.SaveChanges();
        }
    }
}