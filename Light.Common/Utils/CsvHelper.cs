using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Light.Common.Attributes;
using Light.Common.Error;

namespace Light.Common.Utils {
    /// <summary>
    /// 处理csv文件
    /// </summary>
    public class CsvHelper {
        /// <summary>
        /// 导入csv文件读取返回类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<T> Import<T>(IFormFile file) {

            long fileLen = file.Length;
            // 读取文件的 byte[] 
            byte[] bytes = new byte[fileLen];

            var propertyInfos = typeof(T).GetProperties();

            var read = file.OpenReadStream().Read(bytes, 0, (int)fileLen);
            if (read < 0) {
                throw new BaseException("内容错误");
            }

            var content = Encoding.UTF8.GetString(bytes);

            Regex regex = new Regex(@"\t,\n");
            string[] infoLines = regex.Split(content);
            if (infoLines.Length < 3) {
                throw new BaseException("导入数据至少三行");
            }

            var infoHeads = infoLines[1].Replace("\t", "").Split(',');

            List<T> list = new List<T>();

            for (var i = infoLines.Length - 1; i >= 2; i--) {

                if (string.IsNullOrEmpty(infoLines[i])) {
                    continue;
                }
                var items = infoLines[i].Replace("\t", "").Split(',');
                var obj = Activator.CreateInstance<T>();

                for (var j = 0; j < items.Length; j++) {
                    if (infoHeads[j].ToLower() == "id") {
                        continue;
                    }
                    var firstProperty = propertyInfos.FirstOrDefault(t => t.Name == FunctionUtil.UpperFirst(infoHeads[j]));
                    object value = items[j];
                    if (firstProperty == null) {
                        //看是不是 枚举
                        if (infoHeads[j].EndsWith("Enum")) {
                            var info = propertyInfos.FirstOrDefault(t => t.Name == infoHeads[j].Replace("Enum", ""));
                            if (info == null) {
                                continue;
                            }

                            object[] objAttrs = info.GetCustomAttributes(typeof(AutoEntityField), true);
                            if (objAttrs.Length == 0) {
                                continue;
                            }
                            AutoEntityField attr = objAttrs[0] as AutoEntityField;
                            if (attr == null) {
                                continue;
                            }
                            if (attr.EnumType == null) {
                                continue;
                            }
                            var enumFields = Enum.GetNames(attr.EnumType);
                            for (var k = 0; k < enumFields.Length; k++) {
                                if (enumFields[k].Trim() == items[j].Trim()) {
                                    value = Enum.GetValues(attr.EnumType).GetValue(k);
                                }
                            }
                        }

                    }
                    //查看属性是否存在
                    var prop = obj.GetType().GetProperty(FunctionUtil.UpperFirst(infoHeads[j]));
                    if (prop != null) {
                        Type? itemType = Nullable.GetUnderlyingType(prop.PropertyType) == null
                            ? prop.PropertyType
                            : Nullable.GetUnderlyingType(prop.PropertyType);
                        if (itemType != null) prop.SetValue(obj, Convert.ChangeType(value, itemType), null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}