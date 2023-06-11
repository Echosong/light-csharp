using Microsoft.AspNetCore.Mvc;
using Light.Common.Dto;
using Light.Common.Error;
using Light.Entity;
using Light.Service;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 全局字典配置表 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class ConfigController : BaseController {

        private readonly IConfigService _configService;
        private readonly IUserService _userService;

        /// <summary>
        /// 配置文件
        /// </summary>
        /// <param name="db"></param>
        /// <param name="configService"></param>
        public ConfigController(Db db, IUserService userService, IConfigService configService) {
            _db = db;
            _userService = userService;
            _configService = configService;
        }

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<Config> List() {
            return _db.Configs.ToList();
        }

        /// <summary>
        /// 分页查询 全局字典配置表 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Config> ListPage(ConfigQueryDto queryDto) {
            var where = PredicateExtend.True<Config>();
            if (!string.IsNullOrEmpty(queryDto.Code)) {
                where = where.And(t => t.Code.Contains(queryDto.Code));
            }
            if (!string.IsNullOrEmpty(queryDto.Name)) {
                where = where.And(t => t.Name.Contains(queryDto.Name));
            }

            var queryWhere = _db.Configs
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Config>(list, queryDto);
        }

        /// <summary>
        /// 全局字典配置表 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        public Config? Find(int id) {
            return _db.Configs.Find(id);
        }

        /// <summary>
        /// 新增活更新全局字典配置表
        /// </summary>
        /// <param name="ones">全局字典配置表</param>
		[HttpPost]
        public void Save(List<Config> ones) {
            _configService.BatchSave(ones);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        public void Delete(int id) {
            var find = _db.Configs.Find(id);
            if (find == null) {
                throw new BaseException("数据不存在");
            }
            _db.Configs.Remove(find);
            _db.SaveChanges();
        }
    }
}