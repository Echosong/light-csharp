using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 聊天记录 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ChatController : BaseController {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public ChatController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 聊天记录 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Chat> ListPage(ChatQueryDto queryDto) {
            var where = PredicateExtend.True<Chat>();

            var queryWhere = _db.Chats
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Chat>(list, queryDto);
        }

        /// <summary>
        /// 聊天记录 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Chat? Find(int id) {
            return _db.Chats.Find(id);
        }

        /// <summary>
        /// 新增活更新聊天记录
        /// </summary>
        /// <param name="one">聊天记录</param>
		[HttpPost]
        public void Save(Chat one) {
            if (one.Id != 0) {
                _db.Chats.Update(one);
            } else {
                _db.Chats.Add(one);
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
            var find = _db.Chats.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Chats.Remove(find);
            _db.SaveChanges();
        }
    }
}