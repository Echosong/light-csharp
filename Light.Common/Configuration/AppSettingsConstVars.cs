
namespace Light.Common.Configuration {
    /// <summary>
    /// 配置文件格式化
    /// </summary>
    public class AppSettingsConstVars {

        #region 全局地址================================================================================
        /// <summary>
        /// 系统后端地址
        /// </summary>
        public static readonly string AppConfigAppUrl = AppSettingsHelper.GetContent("AppConfig", "AppUrl");
        /// <summary>
        /// 系统接口地址
        /// </summary>
        public static readonly string AppConfigAppInterFaceUrl = AppSettingsHelper.GetContent("AppConfig", "AppInterFaceUrl");

        /// <summary>
        /// swagger测试密码
        /// </summary>
        public static readonly string AppConfigSwagger = AppSettingsHelper.GetContent("AppConfig", "AppConfigSwagger");

        /// <summary>
        /// 获取当前api地址
        /// </summary>
        public static readonly string AppConfigApiUrl = AppSettingsHelper.GetContent("AppConfig", "AppConfigApiUrl");
        #endregion

        #region 数据库================================================================================
        /// <summary>
        /// 获取数据库连接字符串
        /// </summary>
        public static readonly string DbSqlConnection = AppSettingsHelper.GetContent("ConnectionStrings", "SqlConnection");
        /// <summary>
        /// 获取数据库类型
        /// </summary>
        public static readonly string DbDbType = AppSettingsHelper.GetContent("ConnectionStrings", "DbType");
        #endregion

        #region redis================================================================================

        /// <summary>
        /// 获取redis连接字符串
        /// </summary>
        public static readonly string RedisConfigConnectionString = AppSettingsHelper.GetContent("RedisConfig", "ConnectionString");

        public static readonly bool RedisUseTimedTask = Convert.ToBoolean(AppSettingsHelper.GetContent("RedisConfig", "UseTimedTask"));
        #endregion

        #region 微信相关================================================================================
        //公众号
        public static readonly string WxToken = AppSettingsHelper.GetContent("SenparcWeixinSetting", "Token");
        public static readonly string EncodingAESKey = AppSettingsHelper.GetContent("SenparcWeixinSetting", "EncodingAESKey");
        public static readonly string WeixinAppId = AppSettingsHelper.GetContent("SenparcWeixinSetting", "WeixinAppId");
        public static readonly string WeixinAppSecret = AppSettingsHelper.GetContent("SenparcWeixinSetting", "WeixinAppSecret");

        //小程序
        public static readonly string WxOpenAppId = AppSettingsHelper.GetContent("SenparcWeixinSetting", "WxOpenAppId");
        public static readonly string WxOpenAppSecret = AppSettingsHelper.GetContent("SenparcWeixinSetting", "WxOpenAppSecret");
        public static readonly string WxOpenToken = AppSettingsHelper.GetContent("SenparcWeixinSetting", "WxOpenToken");
        public static readonly string WxOpenEncodingAESKey = AppSettingsHelper.GetContent("SenparcWeixinSetting", "WxOpenEncodingAESKey");

        //微信支付
        public static readonly string TenPayV3_AppId = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_AppId");
        public static readonly string TenPayV3_AppSecret = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_AppSecret");
        public static readonly string TenPayV3_MchId = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_MchId");
        public static readonly string TenPayV3_Key = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_Key");
        public static readonly string TenPayV3_TenpayNotify = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_TenpayNotify");
        public static readonly string TenPayV3_PrivateKey = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_PrivateKey");
        public static readonly string TenPayV3_SerialNumber = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_SerialNumber");
        public static readonly string TenPayV3_ApiV3Key = AppSettingsHelper.GetContent("SenparcWeixinSetting", "TenPayV3_ApiV3Key");

        #endregion

        #region HangFire定时任务================================================================================
        /// <summary>
        /// 登录账号
        /// </summary>
        public static readonly string HangFireLogin = AppSettingsHelper.GetContent("HangFire", "Login");
        /// <summary>
        /// 登录密码
        /// </summary>
        public static readonly string HangFirePassWord = AppSettingsHelper.GetContent("HangFire", "PassWord");


        #endregion

        #region 七牛存储
        public static readonly string QiniuAccessKey = AppSettingsHelper.GetContent("Qiniu", "AccessKey");

        public static readonly string QiniuSecretKey = AppSettingsHelper.GetContent("Qiniu", "SecretKey");

        public static readonly string QiniuBucket = AppSettingsHelper.GetContent("Qiniu", "Bucket");

        public static readonly string QiniuDomain = AppSettingsHelper.GetContent("Qiniu", "Domain");
        #endregion

        #region 华为短信
        public static readonly string BaiduClientId = AppSettingsHelper.GetContent("Baidu", "clientId");
        public static readonly string BaiduClientSecret = AppSettingsHelper.GetContent("Baidu", "clientSecret");
        #endregion

        #region 百度内容审核
        public static readonly string HuaweiApiAddress = AppSettingsHelper.GetContent("Huawei", "apiAddress");
        public static readonly string HuaweiAppKey = AppSettingsHelper.GetContent("Huawei", "appKey");
        public static readonly string HuaweiAppSecret = AppSettingsHelper.GetContent("Huawei", "appSecret");
        #endregion

    }
}
