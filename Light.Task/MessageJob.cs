using Light.Common.Utils;
using Light.Service;

namespace Light.Job {
    public class MessageJob {
        private readonly ISmsService _smsService;
        public MessageJob(ISmsService smsService) {
            _smsService = smsService;
        }

        /// <summary>
        /// 执行函数
        /// </summary>
        /// <returns></returns>
        public void Execute() {
            try {
                _smsService.SendMessage();
            } catch (Exception ex) {
                LogHelper.Error("消息定时错误", ex);
            }
        }

    }
}
