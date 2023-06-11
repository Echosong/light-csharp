using Light.Entity;

namespace Light.Service {
    public interface IConfigService {
        /// <summary>
        /// 查询某个key对应的配置
        /// </summary>
        /// <returns></returns>
        public List<Config>? GetConfigs();

        /// <summary>
        /// 批量更新配置
        /// </summary>
        /// <param name="configs"></param>
        public void BatchSave(List<Config> configs);

        /// <summary>
        /// 获取某个具体配置
        /// </summary>
        /// <returns></returns>
        public Config GetConfig(string group, string key);

        /// <summary>
        /// 获取某个key对应配置
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Config GetConfigByKey(string key);


    }
}
