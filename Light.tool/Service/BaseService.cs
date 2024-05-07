using Light.Common.Attributes;
using Light.Common.Configuration;

namespace Light.Tool.Service {
    /// <summary>
    /// 基础处理类
    /// </summary>
    public class BaseService {
        /// <summary>
        /// 当前Entity
        /// </summary>
        protected Type clazz;

        /// <summary>
        /// 表名
        /// </summary>
        protected string tableName;

        /// <summary>
        /// 表说明
        /// </summary>
        protected string tableInfo = "";

        /// <summary>
        /// 类上特性
        /// </summary>
        protected AutoEntity Auto = new AutoEntity();

        /// <summary>
        /// 特性信息
        /// </summary>
        protected AutoEntityField AutoEntity;

        /// <summary>
        /// 模板地址
        /// </summary>
        private string _templatePath = AppSettingsHelper.GetContent("Path", "templatePath");

        /// <summary>
        /// 前端路径
        /// </summary>
        protected string VuePath = AppSettingsHelper.GetContent("Path", "vuePath");

        /// <summary>
        /// 控制器路径
        /// </summary>
        protected string ControllerPath = AppSettingsHelper.GetContent("Path", "controllerPath");

        /// <summary>
        /// dto 路径
        /// </summary>
        protected string DtoPath = AppSettingsHelper.GetContent("Path", "dtoPath");

        /// <summary>
        /// 基础路径
        /// </summary>
        protected string BasePath = AppSettingsHelper.GetContent("Path", "basePath");

        protected string TemplatePath { get => _templatePath; set => _templatePath = value; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="clazz"></param>
        public BaseService(Type clazz) {
            this.clazz = clazz;
            this.tableName = clazz.Name;
            var autos = clazz.GetCustomAttributes(typeof(AutoEntity), true);
            if (autos.Length > 0) {
                var auto = autos[0] as AutoEntity;
                if (auto != null) {
                    this.tableInfo = auto.Name;
                    this.Auto = auto;
                }
            }
        }

        /// <summary>
        /// 魔板读取替换基础信息
        /// </summary>
        /// <param name="tplName">模板路径名</param>
        /// <returns>初步处理后模板内容</returns>
        protected string ReplaceTpl(string tplName) {
            string tplContent = File.ReadAllText(BasePath + TemplatePath + tplName);
            tplContent = tplContent.Replace("#{EntityName}#", tableName);
            tplContent = tplContent.Replace("#{tableInfo}#", tableInfo);
            return tplContent;
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected string LowerFirst(string message) {
            return message.Substring(0, 1).ToLower() + message.Substring(1);
        }


    }
}