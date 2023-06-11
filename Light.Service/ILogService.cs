namespace Light.Service {
    public interface ILogService {
        public Task<int> WriteLogFromRedisToDb();
    }
}
