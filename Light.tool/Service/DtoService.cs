using System.ComponentModel;
using System.Text;
using Light.Common.Attributes;
using Light.Common.Utils;

namespace Light.Tool.Service {
    public class DtoService : BaseService, IService {
        public DtoService(Type clazz) : base(clazz) {
        }

        public void Start() {
            if (!Auto.dto) return;

            var replaceTpl = ReplaceTpl("queryDto.tpl");

            var path = BasePath + DtoPath + tableName + "QueryDto.cs";

            if (File.Exists(path)) {
                Console.WriteLine($"{path} 已经存在不做处理");
                return;
                //var pathBak = basePath + dtoPath + tableName + "QueryDto_" + new Random().Next(1000) + ".cs";
                //File.Move(path, pathBak);
            }

            var fields = clazz.GetProperties();
            var fieldStr = new StringBuilder();

            foreach (var propInfo in fields) {
                var objAttrs = propInfo.GetCustomAttributes(typeof(AutoEntityField), true);
                if (objAttrs.Length > 0) {
                    var attr = objAttrs[0] as AutoEntityField;
                    if (attr == null) continue;
                    if (attr.Query) {
                        var typeName = propInfo.PropertyType.Name;
                        var underlyingType = Nullable.GetUnderlyingType(propInfo.PropertyType);
                        if (underlyingType != null) typeName = underlyingType.Name + "?";

                        var displays = propInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                        if (displays.Length == 0) {
                            LogHelper.Info(typeName);
                            fieldStr.Append($"\t\t[DisplayName(\"{attr.Name}\")]\r\n");
                        } else {

                            if (displays[0] is DisplayNameAttribute display) {
                                fieldStr.Append($"\t\t[DisplayName(\"{display.DisplayName}\")]\r\n");
                            }
                        }
                        var publicStr = $"\t\tpublic {typeName} {propInfo.Name} {{get;set;}} \r\n";
                        fieldStr.Append(publicStr);

                        if (typeName.StartsWith("DateTime")) {
                            publicStr = $"\t\tpublic {typeName} Start{propInfo.Name} {{get;set;}} \r\n";
                            fieldStr.Append(publicStr);

                            publicStr = $"\t\tpublic {typeName} End{propInfo.Name} {{get;set;}} \r\n";
                            fieldStr.Append(publicStr);
                        }
                    }
                }
            }

            replaceTpl = replaceTpl.Replace("#{fields}#", fieldStr.ToString());


            File.WriteAllText(path, replaceTpl);
        }
    }
}