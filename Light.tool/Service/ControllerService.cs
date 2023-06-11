using System.Text;
using Light.Common.Attributes;

namespace Light.Tool.Service {
    public class ControllerService : BaseService, IService {

        public ControllerService(Type clazz) : base(clazz) {

        }

        public void Start() {
            if (!Auto.controller) {
                return;
            }
            var replaceTpl = this.ReplaceTpl("controller.tpl");
            string path = basePath + controllerPath + tableName + "Controller.cs";
            if (File.Exists(path)) {
                Console.WriteLine($"{path} 已经存在不做处理");
                return;
                // string pathBak = basePath + controllerPath + tableName + "Controller_"+ new Random().Next(1000) +".cs";
                // File.Move(path, pathBak);
            }

            // 查询条件
            var fields = clazz.GetProperties();
            var fieldStr = new StringBuilder();
            bool flgSort = false;


            foreach (var propInfo in fields) {
                var objAttrs = propInfo.GetCustomAttributes(typeof(AutoEntityField), true);
                if (propInfo.Name == "Sort") {
                    flgSort = true;
                }
                if (objAttrs.Length > 0) {
                    var attr = objAttrs[0] as AutoEntityField;
                    if (attr == null) continue;
                    if (attr.Query) {
                        string linqWhere = $"\t\t\tif (queryDto.{propInfo.Name} != null) {{ \r\n" +
                                    $"\t\t\t\twhere = where.And(t => t.{propInfo.Name} == queryDto.{propInfo.Name}); \r\n" +
                                    "\t\t\t} \r\n";

                        if (propInfo.PropertyType == typeof(string)) {
                            linqWhere = $"\t\t\tif (!string.IsNullOrEmpty(queryDto.{propInfo.Name})) {{ \r\n" +
                                               $"\t\t\t\twhere = where.And(t => t.{propInfo.Name}.Contains(queryDto.{propInfo.Name})); \r\n" +
                                               "\t\t\t} \r\n";
                        }

                        if (propInfo.PropertyType == typeof(int)) {
                            linqWhere = $"\t\t\tif (queryDto.{propInfo.Name} != 0) {{ \r\n" +
                                               $"\t\t\t\twhere = where.And(t => t.{propInfo.Name} == queryDto.{propInfo.Name}); \r\n" +
                                               "\t\t\t} \r\n";
                        }

                        if (propInfo.PropertyType == typeof(DateTime?)) {
                            linqWhere = $"\t\t\tif (queryDto.Start{propInfo.Name} != null) {{ \r\n" +
                                        $"\t\t\t\twhere = where.And(t => t.{propInfo.Name} > queryDto.Start{propInfo.Name}); \r\n" +
                                        "\t\t\t} \r\n";

                            linqWhere += $"\t\t\tif (queryDto.End{propInfo.Name} != null) {{ \r\n" +
                                        $"\t\t\t\twhere = where.And(t => t.{propInfo.Name} < queryDto.End{propInfo.Name}); \r\n" +
                                        "\t\t\t} \r\n";

                        }
                        fieldStr.Append(linqWhere);
                    }
                }
            }

            replaceTpl = replaceTpl.Replace("#{OrderBySort}#", flgSort ? ".OrderBy(t=>t.Sort).ThenByDescending(t => t.Id)" : ".OrderByDescending(t => t.Id)");
            replaceTpl = replaceTpl.Replace("#{LinqWhere}#", fieldStr.ToString());

            File.WriteAllText(path, replaceTpl);
        }
    }
}