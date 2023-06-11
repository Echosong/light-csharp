using System.ComponentModel;

namespace Light.Common.Configuration {
    public class GlobalConsts {

        /// <summary>
        /// 通过redis异步写日志
        /// </summary>
        public const string REDIS_QUEUE_LOG = "REDIS_QUEUE_LOG_VOLUNTARY";

        /// <summary>
        /// 系统配置功能缓存
        /// </summary>
        public const string REDIS_CONFIG = "REDIS_CONFIG_VOLUNTARY";

        /// <summary>
        /// notice 通知信息
        /// </summary>
        public const string REDIS_NOTICE = "redis_notice__VOLUNTARY";

        /// <summary>
        /// 缓存聊天的socket对象hash
        /// </summary>
        public const string CHAT_WEBSOCKET_HASH = "CHAT_WEBSOCKET_HASH";

        /// <summary>
        /// 是否需要机器审核
        /// </summary>
        public const string REVIEW_BOOL = "review_bool";

        /// <summary>
        /// 前端用户所对应的角色
        /// </summary>
        public const int NORMAL_ROLEID = 2;

        /// <summary>
        /// 超级管理员
        /// </summary>
        public const int ADMIN_ROLEID = 1;

        /// <summary>
        /// 代理
        /// </summary>
        public const int AGENT_ROLEID = 3;

        /// <summary>
        /// 分销商
        /// </summary>
        public const int USER_ROLEID = 4;

        /// <summary>
        /// 瓜田助手用户
        /// </summary>
        public const int OFFICIAL_USER_ID = 17;

        /// <summary>
        /// 存储客户位置
        /// </summary>
        public const string GEO_ADDRESS_ROLEID = "GEO_ADDRESS_ROLEID";

        public const string PREFIX = "GUA";

        public const string Default = "";

        /// <summary>
        /// 配置不需要登录就能请求的路由
        /// </summary>
        public static List<string> OpenRouteList = new List<string>() {
            "/attachment/SimpleUpload",
             "/attachment/upload",
            "/user/login",
            "/permission/find"
        };

        #region HangFire定时任务相关

        public enum HangFireQueuesConfig {
            /// <summary>
            /// 默认
            /// </summary>
            [Description("默认")]
            @default = 1,
            /// <summary>
            /// 接口
            /// </summary>
            [Description("接口")]
            apis = 2,
            /// <summary>
            /// 网站
            /// </summary>
            [Description("网站")]
            web = 3,
            /// <summary>
            /// 循环时间
            /// </summary>
            [Description("循环时间")]
            recurring = 4,
        }

        #endregion
    }
}
