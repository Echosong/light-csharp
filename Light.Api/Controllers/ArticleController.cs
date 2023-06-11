using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Filter;
using Light.Common.Utils;
using Light.Entity;
using Light.Service;

namespace Light.Api.Controllers {

    /// <summary>
    /// 文章相关
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ArticleController : BaseController {

        private readonly IArticleService _articleService;

        /// <summary>
        /// 注入
        /// </summary>
        /// <param name="db"></param>
        ///  <param name="articleService"></param>
        public ArticleController(Db db, IArticleService articleService) {
            this._db = db;
            this._articleService = articleService;
        }

        /// <summary>
        /// 获取业务分类
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NoPermission]
        public List<Dictionary<string, object>> GetArticleCategories() {
            return FunctionUtil.GetEnums("ArticleTypeEnum");
        }


        /// <summary>
        /// 某篇文章下面的推荐
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [NoPermission]
        public List<Article> GetRecommend(int id) {
            var article = this._db.Articles.FirstOrDefault(t => t.Id == id);
            if (article == null) {
                return new List<Article>();
            }
            return this._db.Articles.Where(t => t.Type == article.Type).OrderByDescending(t => t.Id)
                .Select(t => new Article() { Id = t.Id, Title = t.Title, Author = t.Author, Info = t.Info, Img = t.Img, CreateDateTime = t.CreateDateTime })
                .ToList();
        }


        /// <summary>
        /// 获取文章列表【我得页面可用直接设置categoryId = 1 来拉到所有页面数据】
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [NoPermission]
        public Page<Article> GetArticles(ArticleQueryDto queryDto) {
            return _articleService.ListPage(queryDto);
        }

        /// <summary>
        /// 获取单个文章信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [NoPermission]
        public Article? GetArticle(int id) {
            return this._db.Articles.FirstOrDefault(c => c.Id == id);
        }


    }
}
