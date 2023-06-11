using Nelibur.ObjectMapper;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.RedisCache;
using Light.Entity;

namespace Light.Service.Impl {
    internal class LogService : ILogService {
        private readonly Db _db;
        public LogService(Db db) {
            _db = db;
        }

        /// <summary>
        /// 写入日志异步写入到数据
        /// </summary>
        public async Task<int> WriteLogFromRedisToDb() {
            try {
                var logDto = Redis.CreateInstance().PopCache<LogDto>(GlobalConsts.REDIS_QUEUE_LOG);
                TinyMapper.Bind<LogDto, Log>();
                _db.Logs.Add(TinyMapper.Map<Log>(logDto));
                return await _db.SaveChangesAsync();
            } catch (Exception ex) {
                return 0;
            }

        }
    }
}
