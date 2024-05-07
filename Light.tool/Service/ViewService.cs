using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Light.Common.Attributes;
using Light.Common.Dto;
using Light.Common.Enums;
using Light.Entity;
using File = System.IO.File;

namespace Light.Tool.Service {
    public class ViewService : BaseService, IService {
        private string _targetPath = "";

        public ViewService(Type clazz) : base(clazz) {
        }

        public void Start() {
            _targetPath = VuePath + "/sa-view/" + LowerFirst(tableName);

            if (Auto.viewFrom) FormView();
            if (Auto.viewList) ListView();
        }

        /// <summary>
        ///     写入配置文件 router.js
        /// </summary>
        /// <param name="typeStr"></param>
        private void WriteConfigJs(string typeStr) {
            var setLine = new HashSet<string>();

            var name = LowerFirst(clazz.Name);
            var perms = $"{{perms:\"{name}-{typeStr}\", view: () => import('@/sa-view/{name}/{typeStr}.vue')}},";

            var configPath = VuePath + "/router.js";
            var readLines = File.ReadLines(configPath);
            foreach (var readLine in readLines)
                if (readLine.StartsWith("]")) {
                    setLine.Add(perms);
                    setLine.Add("]");
                } else {
                    setLine.Add(readLine.Trim());
                }

            File.WriteAllLines(configPath, setLine);
        }


        /// <summary>
        ///     列表处理
        /// </summary>
        private void ListView() {
            var tplContent = ReplaceTpl("list.tpl");
            var targetListPath = _targetPath + "/list.vue";

            //文件存在先备份
            if (File.Exists(targetListPath)) {
                Console.WriteLine($"{targetListPath} 已经存在不做处理");
                return;
                //  var bakPath = _targetPath + "/list_bak"+ new Random().Next(1000) + ".vue";
                // File.Move(targetListPath, bakPath);
            }

            //表格list
            var tableColumns = new List<string> {
                //"<el-table-column type=\"selection\"></el-table-column>"
            };
            //分页查询参数
            var paramsList = new List<string>();
            //查询条件表单
            var fromList = new List<string>();
            //文件上传js处理
            var fileList = new List<string>();
            //导出字段头
            var exportList = new List<string>();

            //每个字段依次处理
            foreach (var property in clazz.GetProperties()) {

                if (typeof(SysBase).GetProperties().Any(s => s.Name == property.Name) && property.Name != "Id") {
                    continue;
                }

                var disObjects = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                var columnName = "";
                if (disObjects.Length > 0) {
                    var display = disObjects[0] as DisplayNameAttribute;
                    columnName = display?.DisplayName;
                }

                var typeName = LowerFirst(property.Name);

                var autoObjects = property.GetCustomAttributes(typeof(AutoEntityField), true);
                var auto = new AutoEntityField();
                if (autoObjects.Length > 0) auto = autoObjects[0] as AutoEntityField;

                if (auto == null) continue;

                if (String.IsNullOrEmpty(columnName)) {
                    columnName = auto.Name;
                }

                //需要list 表格
                if (auto.List) tableColumns.Add(ElTableColumn(columnName, auto, property));
                //需要建立查询条件
                if (auto.Query) {
                    //如果是日期类型
                    paramsList.Add(property.PropertyType == typeof(DateTime)
                        ? $"start{property.Name}:"
                        : $"{typeName}:''");
                    fromList.Add(ElForm(columnName, auto, property));
                }
                exportList.Add(ElExportTitleObj(columnName, auto, property));

                if (auto.TypeEnum == HtmlTypeEnum.Upload || auto.TypeEnum == HtmlTypeEnum.File)
                    fileList.Add($" try{{ item.{typeName}File = JSON.parse(item.{typeName}); }}catch(e){{ item.{typeName}File = [{{\"url\": item.{typeName}}}]}}");
            }

            var pageInfo = new PageInfo();
            var queryParam = $"pageSize:{pageInfo.pageSize},page:{pageInfo.page}";
            if (paramsList.Count > 0)
                queryParam = "{" + queryParam + ", " + string.Join(",", paramsList) + "}";
            else
                queryParam = "{" + queryParam + "}";

            //分页条件
            tplContent = tplContent.Replace("#{queryPageParams}#", queryParam);

            //表格字符替换
            tplContent = tplContent.Replace("#{el-table-column}#",
                string.Join("\r\n", tableColumns));
            //查询表单处理
            var elContent = string.Join("\r\n", fromList);
            tplContent = tplContent.Replace("#{el-form-item}#", elContent);

            //处理枚举下来组件
            if (elContent.Contains("<input-enum")) {
                tplContent = tplContent.Replace(
                    "//import inputEnum from \"../../sa-resources/com-view/input-enum.vue\";",
                    "import inputEnum from \"../../sa-resources/com-view/input-enum.vue\";");
                tplContent = tplContent.Replace("//inputEnum,", "inputEnum,");
            }

            tplContent = tplContent.Replace("//map_file", string.Join("\r\n", fileList));

            tplContent = tplContent.Replace("//title_obj", $"[{string.Join(",", exportList)}]");

            if (!Directory.Exists(_targetPath))
                Directory.CreateDirectory(_targetPath);

            //覆盖写入文件
            File.WriteAllText(targetListPath, tplContent);
            WriteConfigJs("list");
        }

        /// <summary>
        ///     列表格段字符处理
        /// </summary>
        /// <returns></returns>
        private string ElTableColumn(string columnName, AutoEntityField autoEntityField, PropertyInfo field) {
            //id 已经内置到模板里面了
            if (field.Name == "Id") {
                return "";
            }
            var typeName = LowerFirst(field.Name);
            if (autoEntityField.EnumType != null) typeName = typeName + "Enum";
            string returnValue;
            //图片字段处理
            if (autoEntityField.TypeEnum == HtmlTypeEnum.File) {
                returnValue = $" <el-table-column  label=\"{columnName}\"  >\n" +
                              "    <template slot-scope=\"s\">\n" +
                              $"        <el-link  v-for=\"item in (s.row.{typeName}File)\"  " +
                              ":key=\"item.name\" :href=\"item.url\"  type=\"primary\" >{{item.name}}</el-link>\n" +
                              "     </template>\n" +
                              " </el-table-column> ";
            } else if (autoEntityField.TypeEnum == HtmlTypeEnum.Upload) {
                returnValue = $" <el-table-column  label=\"{columnName}\"  >\n" +
                             "    <template slot-scope=\"s\">\n" +
                             $"         <el-image style=\"width: 60px; height: 60px; margin-left: 10px; \"  v-for=\"item in (s.row.{typeName}File)\"  " +
                             ":key=\"item.id\" :src=\"item.url\" :preview-src-list=\"[item.url]\"  type=\"primary\" >{{item.name}}</el-image>\n" +
                             "     </template>\n" +
                             " </el-table-column> ";
            } else if (autoEntityField.TypeEnum == HtmlTypeEnum.TextArea || autoEntityField.TypeEnum == HtmlTypeEnum.TextEdit) {
                returnValue = $" <el-table-column  label=\"{columnName}\" :show-overflow-tooltip=\"true\"   prop=\"{typeName}\" ></el-table-column>";
            } else {
                returnValue = $" <el-table-column  label=\"{columnName}\"   prop=\"{typeName}\" ></el-table-column>";
            }
            return returnValue;
        }

        /// <summary>
        /// 获取导出 的字段头
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="autoEntityField"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private string ElExportTitleObj(string columnName, AutoEntityField autoEntityField, PropertyInfo field) {
            var returnVale = string.Empty;
            var typeName = LowerFirst(field.Name);
            if (autoEntityField.Export) {
                if (autoEntityField.EnumType != null) {
                    returnVale = $"{{key:\"{typeName}Enum\",name:\"{columnName}\" }}";
                } else {
                    returnVale = $"{{key:\"{typeName}\",name:\"{columnName}\" }}";
                }
            }
            return returnVale;
        }

        /// <summary>
        ///     列表表单段字符处理
        /// </summary>
        /// <returns></returns>
        private string ElForm(string columnName, AutoEntityField autoEntityField, PropertyInfo field) {
            var returnValue = string.Empty;
            var typeName = LowerFirst(field.Name);
            if (field.PropertyType == typeof(string))
                returnValue = $"<el-form-item label=\"{columnName}：\">\n" +
                              $" <el-input v-model=\"p.{typeName}\" placeholder=\"模糊查询\"></el-input>\n" +
                              "</el-form-item>";
            if (field.PropertyType == typeof(DateTime?) || field.PropertyType == typeof(DateTime))
                returnValue = $"<el-form-item label=\"{columnName}：\">\n" +
                              "          <el-date-picker\n" +
                              $"            v-model=\"p.start{field.Name}\"\n" +
                              "            type=\"datetime\"\n" +
                              "            value-format=\"yyyy-MM-dd HH:mm:ss\"\n" +
                              "            placeholder=\"开始日期\"\n" +
                              "          ></el-date-picker>\n" +
                              "          -\n" +
                              "          <el-date-picker\n" +
                              $"            v-model=\"p.end{field.Name}\"\n" +
                              "            type=\"datetime\"\n" +
                              "            value-format=\"yyyy-MM-dd HH:mm:ss\"\n" +
                              "            placeholder=\"结束日期\"\n" +
                              "          ></el-date-picker>\n" +
                              "        </el-form-item>";
            if (autoEntityField.EnumType != null)
                returnValue = $"<el-form-item label=\"{columnName}\">\n" +
                              "                    <input-enum\n" +
                              $"                        enumName=\"{autoEntityField.EnumType.Name}\"\n" +
                              $"                        v-model=\"p.{typeName}\"\n" +
                              "                    ></input-enum>\n" +
                              "                </el-form-item>";
            return returnValue;
        }

        /// <summary>
        ///     表单页面
        /// </summary>
        private void FormView() {
            var tplContent = ReplaceTpl("add.tpl");
            var targetListPath = _targetPath + "/add.vue";

            //文件存在先备份
            if (File.Exists(targetListPath)) {
                Console.WriteLine($"{targetListPath} 已经存在不做处理");
                return;
            }

            var elFormItems = new List<string>();
            var ms = new List<string>();
            var uploadCallback = new List<string>();
            var replaceFiles = new List<string>();
            var replaceOld = new List<string>();
            var rulesFields = new List<string>();
            //富文本编辑器是否有多个
            var textEdits = new List<string>();

            //每个字段依次处理
            foreach (var property in clazz.GetProperties()) {

                if (typeof(SysBase).GetProperties().Any(s => s.Name == property.Name)) {
                    continue;
                }

                var disObjects = property.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                var columnName = "";
                if (disObjects.Length > 0) {
                    var display = disObjects[0] as DisplayNameAttribute;
                    columnName = display?.DisplayName;
                }

                rulesFields.Add(Validation(property, columnName));

                var fieldName = LowerFirst(property.Name);

                var autoObjects = property.GetCustomAttributes(typeof(AutoEntityField), true);
                var auto = new AutoEntityField();
                if (autoObjects.Length > 0) auto = autoObjects[0] as AutoEntityField;

                if (auto == null) continue;
                if (String.IsNullOrEmpty(columnName)) {
                    columnName = auto.Name;
                }
                if (auto.TypeEnum == HtmlTypeEnum.Upload || auto.TypeEnum == HtmlTypeEnum.File) {
                    var successUploadFun = $"success_{fieldName}(response, file, fileList) {{\n" +
                                           "          if(response.code != 200){\n" +
                                           "            this.sa.error(response.message);\n" +
                                           "            return;\n" +
                                           "          }\n" +
                                           $"          if(!this.m.{fieldName}File){{\n" +
                                           $"            this.m.{fieldName}File = [];\n" +
                                           "          }" +
                                           $"          this.m.{fieldName}File.push(response.data);\n" +
                                           "          console.log(fileList);\n" +
                                           "        },";
                    var removeUploadFun = $"remove_{fieldName}(file, fileList){{\n" +
                                          $"    this.m.{fieldName}File = fileList;\n" +
                                          "},";
                    uploadCallback.Add(successUploadFun);
                    uploadCallback.Add(removeUploadFun);
                    var replaceOlderFile =
                        $"       this.m.{fieldName} =JSON.stringify(this.m.{fieldName}File.map(item=>{{\n" +
                        "              let a = {};\n" +
                        "              a.name = item.name;\n" +
                        "              a.url = item.url;\n" +
                        "              return a;" +
                        "       }).filter(item=>{ if(!item.url.startsWith(\"blob\")){return item;}}) );";
                    replaceOld.Add(replaceOlderFile);
                    replaceFiles.Add($" try{{ data.{fieldName}File = JSON.parse(data.{fieldName}); }}catch(e){{ data.{fieldName}File = [{{\"url\": data.{fieldName}}}]}}");

                }



                //枚举格式
                if (auto.EnumType != null) {
                    elFormItems.Add($"<el-form-item label=\"{columnName}\">\n" +
                                    "                    <input-enum\n" +
                                    $"                        enumName=\"{auto.EnumType.Name}\"\n" +
                                    $"                        v-model=\"m.{fieldName}\"\n" +
                                    "                    ></input-enum>\n" +
                                    "                </el-form-item>");
                    ms.Add($"{fieldName}: 0 ");
                    continue;
                }

                //时间格式
                if (property.PropertyType == typeof(DateTime?) || property.PropertyType == typeof(DateTime)) {
                    elFormItems.Add($"<el-form-item label=\"{columnName}：\"  prop=\"{fieldName}\" >\n" +
                                    " <el-date-picker\n" +
                                    $"            v-model=\"m.{fieldName}\"\n" +
                                    "            type=\"datetime\"\n" +
                                    "            value-format=\"yyyy-MM-dd HH:mm:ss\"\n" +
                                    $"            placeholder=\"{columnName}\"\n" +
                                    "          ></el-date-picker>" +
                                    " </el-form-item>");
                    ms.Add($"{fieldName}:''");
                    continue;
                }

                if (property.PropertyType == typeof(string)) ms.Add($"{fieldName}:''");

                var htmlTypeStr = "";

                switch (auto.TypeEnum) {
                    case HtmlTypeEnum.Radio:
                        htmlTypeStr = $"<el-switch v-model=\"m.{fieldName} \" ></el-switch>";
                        break;
                    case HtmlTypeEnum.TextArea:
                        htmlTypeStr =
                            $"<el-input type=\"textarea\"  rows=\"2\" placeholder=\"{columnName}\"  v-model=\"m.{fieldName}\"></el-input>";
                        break;
                    case HtmlTypeEnum.File:
                        htmlTypeStr = "<el-upload\n" +
                                      "  class=\"upload-demo\"\n" +
                                      "  :action=\"sa.cfg.api_url + '/Attachment/upload?fileType=0&param=0'\"\n" +
                                      "  :multiple=\"false\"\n" +
                                      "  :limit=\"10\"\n" +
                                      $"   :on-success=\"success_{fieldName}\"" +
                                      $"   :before-remove=\"remove_{fieldName}\"" +
                                      $"  :file-list=\"m.{fieldName}File\">\n" +
                                      "  <el-button size=\"mini\" type=\"primary\">点击上传</el-button>\n" +
                                      $"  <div slot=\"tip\" class=\"el-upload__tip\">上传{columnName}</div>\n" +
                                      "</el-upload>";
                        ms.Add($"{fieldName}File:[]");
                        break;
                    case HtmlTypeEnum.Upload:
                        htmlTypeStr = "<el-upload\n" +
                                      "  class=\"upload-demo\"\n" +
                                      "  :action=\"sa.cfg.api_url + '/Attachment/upload?fileType=0&param=0'\"\n" +
                                      "  :multiple=\"false\"\n" +
                                      " list-type=\"picture-card\"\n" +
                                      "  :limit=\"10\"\n" +
                                      $"   :on-success=\"success_{fieldName}\"" +
                                      $"   :before-remove=\"remove_{fieldName}\"" +
                                      $"  :file-list=\"m.{fieldName}File\">\n" +
                                      "  <el-button size=\"mini\" type=\"primary\">点击上传</el-button>\n" +
                                      $"  <div slot=\"tip\" class=\"el-upload__tip\">上传{columnName}</div>\n" +
                                      "</el-upload>";
                        ms.Add($"{fieldName}File:[]");
                        break;
                    case HtmlTypeEnum.TextEdit:
                        htmlTypeStr = $"<div class=\"editor-box\"> <div id = \"{fieldName}\" ref= \"{fieldName}\" style = \"text-align:left\" >" +
                            "</div ></div > ";
                        textEdits.Add($"this.create_editor(\"{fieldName}\");");
                        break;
                    case HtmlTypeEnum.Input:
                        string vmodel = "v-model";
                        if (property.PropertyType == typeof(int) || property.PropertyType == typeof(long)) {
                            vmodel = "v-model.number";
                        }
                        htmlTypeStr = $"            <el-input {vmodel}=\"m.{fieldName}\"></el-input>\n";

                        break;
                }

                elFormItems.Add($"<el-form-item label=\"{columnName}\" prop=\"{fieldName}\">\n {htmlTypeStr}</el-form-item>");
            }

            var elContent = string.Join("\r\n", elFormItems);
            if (elContent.Contains("<input-enum")) {
                tplContent = tplContent.Replace(
                    "//import inputEnum from \"../../sa-resources/com-view/input-enum.vue\";",
                    "import inputEnum from \"../../sa-resources/com-view/input-enum.vue\";");
                tplContent = tplContent.Replace("//components: { inputEnum },", "components: { inputEnum },");
            }

            if (textEdits.Count > 0) {
                tplContent = tplContent.Replace("//import E from \"wangeditor\";", "import E from \"wangeditor\";");
                tplContent = tplContent.Replace("/*create_editor", "").Replace("create_editor*/", "");
                tplContent = tplContent.Replace("//create_editor", string.Join("\r\n", textEdits));
                tplContent = tplContent.Replace("width=\"550px\"", "width=\"850px\"");
            }

            tplContent = tplContent.Replace("//rule_fields", string.Join("\r\n", rulesFields));
            tplContent = tplContent.Replace("#{el-form-item}#", elContent);
            tplContent = tplContent.Replace("#{data_init}#", string.Join(",\r\n", ms));
            tplContent = tplContent.Replace("//upload_functions", string.Join("\r\n", uploadCallback));
            tplContent = tplContent.Replace("//replace_file", string.Join("\r\n", replaceFiles));
            tplContent = tplContent.Replace("//replace_old", string.Join("\r\n", replaceOld));
            //处理宽度
            tplContent = tplContent.Replace("width=\"550px\"", "width=" + Auto.fromWidth + "px");

            if (!Directory.Exists(_targetPath))
                Directory.CreateDirectory(_targetPath);
            //覆盖写入文件
            File.WriteAllText(targetListPath, tplContent);
            WriteConfigJs("add");
        }

        private string Validation(PropertyInfo property, string columnName) {
            var fieldName = LowerFirst(property.Name);
            var ruleBuilder = new StringBuilder("");
            //必须字段
            var requireAttributes = property.GetCustomAttributes(typeof(RequiredAttribute), true);
            if (requireAttributes.Length > 0)
                ruleBuilder.Append($"{{required: true, message: '请输入{columnName}', trigger: 'blur' }},");

            var maxAttributes = property.GetCustomAttributes(typeof(MaxLengthAttribute), true);
            if (maxAttributes.Length > 0)
                if (maxAttributes[0] is MaxLengthAttribute max)
                    ruleBuilder.Append(
                        $"{{ min: 0, max: {max.Length}, message: '长度在 {max.Length} 个字符', trigger: 'blur' }},");

            var emailObjects = property.GetCustomAttributes(typeof(EmailAddressAttribute), true);

            if (emailObjects.Length > 0)
                ruleBuilder.Append("{ type: 'email', message: '请输入正确的邮箱地址', trigger: ['blur', 'change'] },");

            //正则表达式

            var ranges = property.GetCustomAttributes(typeof(RangeAttribute), true);
            if (ranges.Length > 0)
                if (ranges[0] is RangeAttribute range)
                    ruleBuilder.Append(
                        $"{{pattern:/^\\d{{{range.Minimum},{range.Maximum}}}$/, message:'{columnName}必须为数字类型', trigger: 'blur'}},");

            var regulars = property.GetCustomAttributes(typeof(RegularExpressionAttribute), true);
            if (regulars.Length > 0)
                if (regulars[0] is RegularExpressionAttribute regular)
                    ruleBuilder.Append($"{{pattern:/{regular.Pattern}/, message:'', trigger: 'blur'}}");

            var s = ruleBuilder.ToString();

            return $"{fieldName}:[{s}],";
        }
    }
}