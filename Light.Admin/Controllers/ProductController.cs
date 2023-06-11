using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 商品表 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ProductController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public ProductController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 商品表 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Product> ListPage(ProductQueryDto queryDto) {
            var where = PredicateExtend.True<Product>();
            if (!string.IsNullOrEmpty(queryDto.Title)) {
                where = where.And(t => t.Title.Contains(queryDto.Title));
            }

            var queryWhere = _db.Products
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Product>(list, queryDto);
        }

        /// <summary>
        /// 商品表 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Product? Find(int id) {
            return _db.Products.Find(id);
        }

        /// <summary>
        /// 新增活更新商品表
        /// </summary>
        /// <param name="one">商品表</param>
		[HttpPost]
        public void Save(Product one) {
            if (one.Id != 0) {
                _db.Products.Update(one);
            } else {
                _db.Products.Add(one);
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
            var find = _db.Products.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Products.Remove(find);
            _db.SaveChanges();
        }
    }
}