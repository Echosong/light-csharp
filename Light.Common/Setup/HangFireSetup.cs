using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Light.Common.Configuration;

namespace Light.Common.Setup {
    /// <summary>
    /// 增加HangFire到单独配置
    /// </summary>
    public static class HangFireSetup {
        public static void AddHangFireSetup(this IServiceCollection services) {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //注册Hangfire定时任务
            var isEnabledRedis = AppSettingsConstVars.RedisUseTimedTask;
            if (isEnabledRedis) {
                services.AddHangfire(x => x.UseRedisStorage(AppSettingsConstVars.RedisConfigConnectionString));
            }


            services.AddHangfireServer(options => {
                options.Queues = new[] { GlobalConsts.HangFireQueuesConfig.@default.ToString(),
                    GlobalConsts.HangFireQueuesConfig.apis.ToString(),
                    GlobalConsts.HangFireQueuesConfig.web.ToString(),
                    GlobalConsts.HangFireQueuesConfig.recurring.ToString() };
                options.ServerTimeout = TimeSpan.FromMinutes(4);
                options.SchedulePollingInterval = TimeSpan.FromSeconds(2);//秒级任务需要配置短点，一般任务可以配置默认时间，默认15秒
                options.ShutdownTimeout = TimeSpan.FromMinutes(30); //超时时间
                options.WorkerCount = Math.Max(Environment.ProcessorCount, 20); //工作线程数，当前允许的最大线程，默认20
            });

        }
    }
}
