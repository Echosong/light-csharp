using Microsoft.AspNetCore.Mvc;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Enums;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 资金操作记录 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class FinanceOpController : BaseController {
        public FinanceOpController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 资金操作记录 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<FinanceOp> ListPage(FinanceOpQueryDto queryDto) {
            var where = PredicateExtend.True<FinanceOp>();

            where = where.And(t => t.Account != 0);

            //分站主
            if (_user.RoleId == GlobalConsts.USER_ROLEID) {
                where = where.And(t => t.UserId == _user.Id && t.BusinessType ==(int) FinanceTypeEnum.分润);

            }
            if (!string.IsNullOrEmpty(queryDto.Username)) {
                where = where.And(t => t.Username.Contains(queryDto.Username));
            }
            if (queryDto.BusinessType != null) {
                where = where.And(t => t.BusinessType == queryDto.BusinessType);
            }

            if (queryDto.State != 0) {
                where = where.And(t => t.State == queryDto.State);
            }

            var queryWhere = _db.FinanceOps
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<FinanceOp>(list, queryDto);
        }

        /// <summary>
        /// 资金操作记录 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public FinanceOp? Find(int id) {
            return _db.FinanceOps.Find(id);
        }

        /// <summary>
        /// 新增活更新资金操作记录
        /// </summary>
        /// <param name="one">资金操作记录</param>
		[HttpPost]
        public void Save(FinanceOp one) {
            if (one.Id != 0) {
                _db.FinanceOps.Update(one);
            } else {
                _db.FinanceOps.Add(one);
            }
            _db.SaveChanges();
        }

    }
}