using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Light.Common.Enums;
using Light.Common.Filter;
using Light.Common.Utils;
using Light.Entity;
using Light.Service;

namespace Light.Api.Controllers {

    /// <summary>
    /// 常规内容业务控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class OtherController : BaseController {

        private readonly IConfigService _configService;
        private readonly IQiniuService _iqiniuService;
        private readonly ISmsService _msService;
        private readonly IPaymentService _paymentService;

        /// <summary>
        /// 初始化常规业务
        /// </summary>
        /// <param name="db"></param>
        /// <param name="configService"></param>
        /// <param name="qiniuService"></param>
        /// <param name="smsService"></param>
        /// <param name="paymentService"></param>
        public OtherController(Db db, IConfigService configService, IQiniuService qiniuService, ISmsService smsService, IPaymentService paymentService) {
            _db = db;
            _configService = configService;
            _iqiniuService = qiniuService;
            _msService = smsService;
            _paymentService = paymentService;
        }

        private long _time = DateTime.Now.Ticks;

        /// <summary>
        /// 发送短信
        /// </summary>
        [HttpGet]
        [NoPermission]
        public void SendMessage() {
            _msService.CreateMessage("18317033205", "22111000009", null);
            //_msService.CreateMessage("18317033205", "eb149b293c4a4b938b8e757c541f41b6,1069368924410004072", new Dictionary<string, string> { { "code","65321" } });

        }

        /// <summary>
        ///     上传文件 (目前先支支持一个)
        /// </summary>
        /// <param name="file"></param>
        /// <param name="param"></param>
        [HttpPost]
        public Dictionary<string, string> Upload(IFormFile file, string? param = "") {
            Assert.IsTrue(file.Length > 10, "不存在上传文件信息");
            var dictionary = new Dictionary<string, string>();
            var limitExName = new List<string>() {
                "html", "htm", "js"
            };
            var fileNames = file.FileName.Split('.');
            var extend = fileNames[fileNames.Length - 1];
            Assert.IsTrue(!limitExName.Contains(extend), "不能保护敏感文件");
            string url = _iqiniuService.Upload(file, extend, (int)FileTypeEnum.九宫格图片);
            dictionary = new Dictionary<string, string> {
                    {"name", file.FileName},
                    {"url", url},
                    {"params", param}
                };
            LogHelper.Info("上传文件成功");
            return dictionary;
        }

        /// <summary>
        /// 获取配置key为空时候获取全部
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{key?}")]
        [NoPermission]
        public List<Config>? Config(string? key) {
            var configs = _configService.GetConfigs();
            if (!string.IsNullOrEmpty(key)) {
                return configs.Where(t => t.Code == key).ToList();
            } else {
                return configs;
            }
        }

        /// <summary>
        /// 获取所有广告位置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [NoPermission]
        public List<Ad> ListAd() {
            return _db.Ads.OrderByDescending(t => t.Sort).ToList();
        }

        /// <summary>
        ///     约定通用方法（表得某个字段需要枚举时候，那么用(表名+字段+Enum) 比如
        ///     UserSateEnum 就表示 user 模型下 state 字段 对应得枚举）
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        [HttpGet]
        [NoPermission]
        public List<Dictionary<string, object>> GetEnums(string? enumName) {
            return FunctionUtil.GetEnums(enumName ?? "");
        }

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="parentId">上级id</param>
        /// <returns></returns>
        [HttpGet]
        [NoPermission]
        public List<Area> GeAreas(int parentId, int children = 0) {
            var areas = _db.Areas.Where(t => t.ParentId == parentId).ToList();
            if (children != 0) {
                var ints = areas.Select(a => a.Id).ToList();
                var citys = _db.Areas.Where(t => ints.Contains(t.ParentId ?? 0)).ToList();
                areas.ForEach(item => {
                    item.Children = citys.Where(t => t.ParentId == item.Id).ToList();
                });
            }
            return areas;
        }

        [HttpGet]
        [NoPermission]
        public void Test() {
            
        }

    }
}
