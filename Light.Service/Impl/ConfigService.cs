using Light.Common.Configuration;
using Light.Common.Error;
using Light.Common.RedisCache;
using Light.Entity;

namespace Light.Service.Impl {

    public class ConfigService : IConfigService {
        private readonly Redis _redis;
        private readonly Db _db;

        /// <summary>
        /// 构造注入
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="db"></param>
        public ConfigService(Redis redis, Db db) {
            _redis = redis;
            _db = db;
        }

        /// <summary>
        /// 批量新增修改
        /// </summary>
        /// <param name="configs"></param>
        public void BatchSave(List<Config> configs) {
            _redis.Delete(GlobalConsts.REDIS_CONFIG);
            foreach (var one in configs) {
                if (one.Id != 0) {
                    _db.Configs.Update(one);
                } else {
                    _db.Configs.Add(one);
                }
                _db.SaveChanges();
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        public List<Config>? GetConfigs() {
            List<Config>? configs = _redis.GetCache<List<Config>>(GlobalConsts.REDIS_CONFIG);
            if (configs == null) {
                configs = _db.Configs.ToList();
                _redis.SetCache<List<Config>>(GlobalConsts.REDIS_CONFIG, configs);
            }
            return configs;

        }

        /// <summary>
        /// 获取自定信息
        /// </summary>
        /// <param name="group"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Config GetConfig(string group, string key) {
            _redis.Delete(GlobalConsts.REDIS_CONFIG);
            return (GetConfigs() ?? throw new BaseException("配置文件不存在")).FirstOrDefault(t => t.Group == group && t.Code == key) ?? throw new BaseException("不存在");
        }


        /// <summary>
        /// 获取信息by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Config GetConfigByKey(string key) {
            _redis.Delete(GlobalConsts.REDIS_CONFIG);
            var configs = GetConfigs();
            if (configs is { Count: > 0 }) {
                return configs.FirstOrDefault(t => t.Code == key) ?? new Config();
            } else {
                return new Config();
            }
        }
    }
}
