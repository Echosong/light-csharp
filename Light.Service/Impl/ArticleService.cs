using Light.Common.Dto;
using Light.Common.Enums;
using Light.Entity;

namespace Light.Service.Impl {
    public class ArticleService : IArticleService {
        private readonly Db _db;
        public ArticleService(Db db) { _db = db; }

        public Page<Article> ListPage(ArticleQueryDto queryDto) {
            var where = PredicateExtend.True<Article>();
            if (!string.IsNullOrEmpty(queryDto.Title)) {
                where = where.And(t => t.Title.Contains(queryDto.Title));
            }
            if (queryDto.Type != 0) {
                where = where.And(t => t.Type == queryDto.Type);
            } else {
                where = where.And(t => t.Type != (int)ArticleTypeEnum.我的服务);
            }

            var queryWhere = _db.Articles
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.Select(t => new Article() { Id = t.Id, Title = t.Title, Author = t.Author, Img = t.Img, Type = t.Type, CreateDateTime = t.CreateDateTime, Info = t.Info }).ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Article>(list, queryDto);
        }
    }
}
