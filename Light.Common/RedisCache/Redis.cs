using Newtonsoft.Json;
using StackExchange.Redis;
using Light.Common.Configuration;

namespace Light.Common.RedisCache {
    /// <summary>
    /// 简单单列redis
    /// </summary>
    public class Redis {
        private static Redis _singleton;
        private static readonly object SingletonLock = new object();

        private readonly IDatabase _db;


        public Redis() {
            string redisConfiguration = AppSettingsConstVars.RedisConfigConnectionString;
            var configuration = ConfigurationOptions.Parse(redisConfiguration, true);
            var redisClient = ConnectionMultiplexer.Connect(configuration);
            _db = redisClient.GetDatabase(5);
        }

        public static Redis CreateInstance() {
            if (_singleton == null) //双if +lock
                lock (SingletonLock) {
                    if (_singleton == null) _singleton = new Redis();
                }

            return _singleton;
        }

        public bool SetCache<T>(string key, T value) {
            return _db.StringSet(key, JsonConvert.SerializeObject(value));
        }

        public bool SetCache<T>(string key, T value, TimeSpan timeSpan) {
            return _db.StringSet(key, JsonConvert.SerializeObject(value), timeSpan);
        }

        public async Task<long> PushCache<T>(string key, T value) {
            return await _db.ListLeftPushAsync(key, JsonConvert.SerializeObject(value));
        }

        /// <summary>
        /// 查询附件的人
        /// </summary>
        /// <param name="key">位置信息key</param>
        /// <param name="member">用户id</param>
        /// <returns></returns>
        public GeoRadiusResult[] GeoRadius(string key, int member) {
            var geo = _db.GeoRadius(key, member, 100, GeoUnit.Kilometers, 1000, Order.Ascending);
            return geo;
        }

        /// <summary>
        /// 插入用户的位置
        /// </summary>
        /// <param name="key">位置信息key</param>
        /// <param name="member">用户userid</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        public bool GeoAdd(string key, int member, double longitude, double latitude) {
            return _db.GeoAdd(key, longitude, latitude, member);
        }

        /// <summary>
        /// 设置hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <param name="hashValue"></param>
        /// <returns></returns>
        public async Task<bool> SetHash<T>(string key, string hashKey, T hashValue) {
            return await _db.HashSetAsync(key, hashKey, JsonConvert.SerializeObject(hashValue));
        }

        /// <summary>
        /// 删除hah
        /// </summary>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public bool RemoveHash(string key, string hashKey) {
            return _db.HashDelete(key, hashKey);
        }

        /// <summary>
        /// 获取hash
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="hashKey"></param>
        /// <returns></returns>
        public async Task<T?> GetHash<T>(string key, string hashKey) {
            if (!_db.KeyExists(key)) {
                return default(T);
            }
            if (!_db.HashExists(key, hashKey)) {
                return default(T);
            }
            var result = await _db.HashGetAsync(key, hashKey);
            return JsonConvert.DeserializeObject<T>(result);
        }


        public T PopCache<T>(string key) {
            var redisValue = _db.ListRightPop(key);
            return JsonConvert.DeserializeObject<T>(redisValue);
        }

        public bool Exists(string key) {
            return _db.KeyExists(key);
        }

        public T? GetCache<T>(string key) {
            if (!_db.KeyExists(key)) {
                return default(T);
            }
            var redisValue = _db.StringGet(key);
            return JsonConvert.DeserializeObject<T>(redisValue);
        }

        public bool Delete(string key) {
            return _db.KeyDelete(key);
        }
    }
}