using Hangfire;
using Light.Common.Utils;

namespace Light.Job {
    public class HangfireDispose {
        #region 配置服务

        public static void HangfireService() {
            try {

                //自动取消订单任务
                RecurringJob.AddOrUpdate<LogJob>(s => s.Execute(), "0/10 * * * * ? ", TimeZoneInfo.Utc); // 每10秒写入一次日志

                RecurringJob.AddOrUpdate<MessageJob>(s => s.Execute(), "0/5 * * * * ? ", TimeZoneInfo.Utc); // 每5秒发送消息整理
            } catch (Exception ex) {
                LogHelper.Error("定时器错误", ex);
            }
        }

        #endregion
    }
}