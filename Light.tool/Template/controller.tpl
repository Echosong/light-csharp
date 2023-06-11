using System.Linq;
using Light.Common;
using Light.Common.Dto;
using Light.Entity;
using Microsoft.AspNetCore.Mvc;
using Light.Admin.Controllers;
using Light.Common.Error;
using Light.Common.Filter;

namespace Light.Admin.Controllers {
    /// <summary>
    /// #{tableInfo}# 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class #{EntityName}#Controller : BaseController {
     
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public #{EntityName}#Controller(Db db) { 
            this._db = db;
        }

        /// <summary>
        /// 分页查询 #{tableInfo}# 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<#{EntityName}#> ListPage(#{EntityName}#QueryDto queryDto) {
			var where = PredicateExtend.True<#{EntityName}#>();
#{LinqWhere}#
            var queryWhere = _db.#{EntityName}#s
				.Where(where);

			var query = queryWhere#{OrderBySort}#
                .Skip(queryDto.pageSize * (queryDto.page -1))
                .Take(queryDto.pageSize);

			var list = query.ToList();
			if(list.Count> 0){
				queryDto.totalElements = queryWhere.Count();
			}
            return new Page<#{EntityName}#>(list, queryDto);
        }

        /// <summary>
        /// #{tableInfo}# 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public #{EntityName}#? Find(int id) {
            return _db.#{EntityName}#s.Find(id);
        }

        /// <summary>
        /// 新增或更新#{tableInfo}#
        /// </summary>
        /// <param name="one">#{tableInfo}#</param>
		[HttpPost]
        [Log("新增或更新#{tableInfo}#")]
        public void Save(#{EntityName}# one) {
             if (one.Id != 0) {
                _db.#{EntityName}#s.Update(one);
            } else {
                _db.#{EntityName}#s.Add(one);
            }
            _db.SaveChanges();
        }

        /// <summary>
        /// 删除#{tableInfo}#
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Route("{id?}")]
        [Log("删除#{tableInfo}#")]
        public void Delete(int id) {
            var find = _db.#{EntityName}#s.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.#{EntityName}#s.Remove(find);
			_db.SaveChanges();
        }
    }
}