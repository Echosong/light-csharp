using Light.Common.Dto;
using Light.Entity;

namespace Light.Service {
    public interface IArticleService {
        /// <summary>
        /// 获取文章信息
        /// </summary>
        /// <param name="queryDto"></param>
        /// <returns></returns>
        public Page<Article> ListPage(ArticleQueryDto queryDto);
    }
}
