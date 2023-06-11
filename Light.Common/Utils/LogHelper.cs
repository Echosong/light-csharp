using NLog;

namespace Light.Common.Utils {
    /// <summary>
    /// log4 日志
    /// </summary>
    public class LogHelper {


        public static void Debug(object message, Exception e) {

            NLogUtil.WriteFileLog(LogLevel.Debug, LogType.Web, message != null ? message.ToString() : "", e.Message);

        }

        public static void Debug(object message) {
            NLogUtil.WriteFileLog(LogLevel.Debug, LogType.Web, "", message != null ? message.ToString() : "");
        }

        public static void Info(object message) {
            NLogUtil.WriteAll(LogLevel.Info, LogType.Web, "", message != null ? message.ToString() : "");
        }

        public static void Warn(object message) {
            NLogUtil.WriteFileLog(LogLevel.Warn, LogType.Web, "", message != null ? message.ToString() : "");
        }

        public static void Error(object message) {
            NLogUtil.WriteFileLog(LogLevel.Error, LogType.Web, "", message != null ? message.ToString() : "");
        }
        public static void Error(object message, Exception e) {
            NLogUtil.WriteFileLog(LogLevel.Error, LogType.Web, message != null ? message.ToString() : "", e.Message);

        }
    }
}