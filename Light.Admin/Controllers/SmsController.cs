using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Common.Filter;
using Light.Entity;
using Light.Service;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 短信记录（微信推送消息） 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class SmsController : BaseController {
        private readonly ISmsService _smsService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public SmsController(Db db, ISmsService smsService) {
            this._db = db;
            this._smsService = smsService;
        }

        [HttpGet]
        [NoPermission]
        public async void Test() {
            _smsService.SendMessage();

        }

        /// <summary>
        /// 分页查询 短信记录（微信推送消息） 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Sms> ListPage(SmsQueryDto queryDto) {
            var where = PredicateExtend.True<Sms>();
            if (!string.IsNullOrEmpty(queryDto.Mobile)) {
                where = where.And(t => t.Mobile.Contains(queryDto.Mobile));
            }
            if (queryDto.State != 0) {
                where = where.And(t => t.State == queryDto.State);
            }
            if (queryDto.Type != 0) {
                where = where.And(t => t.Type == queryDto.Type);
            }
            if (!string.IsNullOrEmpty(queryDto.Code)) {
                where = where.And(t => t.Code.Contains(queryDto.Code));
            }

            var queryWhere = _db.Smss
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Sms>(list, queryDto);
        }

        /// <summary>
        /// 短信记录（微信推送消息） 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Sms? Find(int id) {
            return _db.Smss.Find(id);
        }

        /// <summary>
        /// 新增活更新短信记录（微信推送消息）
        /// </summary>
        /// <param name="one">短信记录（微信推送消息）</param>
		[HttpPost]
        public void Save(Sms one) {
            if (one.Id != 0) {
                _db.Smss.Update(one);
            } else {
                var template = _db.Templates.FirstOrDefault(t => t.Code == one.Code);
                Assert.IsNotNull(template, "模板不存在");
                one.TemplateId = template.Id;
                _db.Smss.Add(one);
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
            var find = _db.Smss.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Smss.Remove(find);
            _db.SaveChanges();
        }
    }
}