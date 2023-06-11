using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 这个表记录的时候朋友发送消息个userid,并且userid 还没读的个数 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ChatUserController : BaseController {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="db"></param>
        public ChatUserController(Db db) {
            this._db = db;
        }

        /// <summary>
        /// 分页查询 这个表记录的时候朋友发送消息个userid,并且userid 还没读的个数 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<ChatUser> ListPage(ChatUserQueryDto queryDto) {
            var where = PredicateExtend.True<ChatUser>();

            var queryWhere = _db.ChatUsers
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<ChatUser>(list, queryDto);
        }

        /// <summary>
        /// 这个表记录的时候朋友发送消息个userid,并且userid 还没读的个数 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public ChatUser? Find(int id) {
            return _db.ChatUsers.Find(id);
        }

        /// <summary>
        /// 新增活更新这个表记录的时候朋友发送消息个userid,并且userid 还没读的个数
        /// </summary>
        /// <param name="one">这个表记录的时候朋友发送消息个userid,并且userid 还没读的个数</param>
		[HttpPost]
        public void Save(ChatUser one) {
            if (one.Id != 0) {
                _db.ChatUsers.Update(one);
            } else {
                _db.ChatUsers.Add(one);
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
            var find = _db.ChatUsers.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.ChatUsers.Remove(find);
            _db.SaveChanges();
        }
    }
}