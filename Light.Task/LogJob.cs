using Light.Service;

namespace Light.Job {
    public class LogJob {

        private readonly ILogService _log;
        public LogJob(ILogService log) {
            _log = log;
        }

        public async System.Threading.Tasks.Task Execute() {
            while (true) {
                var count = await _log.WriteLogFromRedisToDb();
                if (count == 0) {
                    break;
                }
                Thread.Sleep(50);
            }
        }
    }
}
