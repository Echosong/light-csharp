using System.ComponentModel;
using System.Text;
using Light.Common.Attributes;
using Light.Common.Configuration;

namespace Light.Tool.Service;

public class DictionaryService : BaseService, IService {

    public DictionaryService(Type clazz) : base(clazz) {

    }

    /// <summary>
    /// 用来全局存储
    /// </summary>
    private static StringBuilder _dictionaryStr = new StringBuilder();

    private static string tplContent = string.Empty;


    public void Start() {
        var fields = clazz.GetProperties();
        _dictionaryStr.Append("<div class=\"content\">\r\n");
        _dictionaryStr.Append($"<h2>{tableName}({tableInfo})</h2>");

        _dictionaryStr.Append($"<table><tr><th>名称</th><th>数据类型</th> <th>长度</th><th>允许空值</th><th>默认值</th><th>说明</th></tr>\r\n");

        foreach (var propInfo in fields) {
            var disObjects = propInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            var columnName = "";
            if (disObjects.Length > 0) {
                var display = disObjects[0] as DisplayNameAttribute;
                columnName = display?.DisplayName;
            }
            var typeName = propInfo.PropertyType.Name;
            var underlyingType = Nullable.GetUnderlyingType(propInfo.PropertyType);
            if (underlyingType != null) typeName = underlyingType.Name + "?";

            var blank = Activator.CreateInstance(clazz);
            object defaultObject = propInfo.GetValue(blank, null);
            string defaultValue = string.Empty;


            int len = 4;
            var objAttrs = propInfo.GetCustomAttributes(typeof(AutoEntityField), true);
            if (objAttrs.Length > 0) {
                var attr = objAttrs[0] as AutoEntityField;
                if (string.IsNullOrEmpty(columnName)) {
                    columnName = attr?.Name;
                    len = attr.Len;
                }
            }

            _dictionaryStr.Append("<tr>");
            _dictionaryStr.Append($"<td>{propInfo.Name}</td>");
            _dictionaryStr.Append($"<td>{typeName}</td>");
            _dictionaryStr.Append($"<td>{len}</td>");
            string isNull = underlyingType == null ? "N" : "Y";
            _dictionaryStr.Append($"<td>{isNull}</td>");
            _dictionaryStr.Append($"<td>{defaultValue}</td>");
            _dictionaryStr.Append($"<td>{columnName}</td>");
            _dictionaryStr.Append("</tr>");
        }

        _dictionaryStr.Append("</table></div>");

        if (string.IsNullOrEmpty(tplContent)) {
            tplContent = ReplaceTpl("data.tpl");
        }
    }

    /// <summary>
    /// 写入文档
    /// </summary>
    public static void WriteDictionaryFile() {
        string fileName = AppSettingsHelper.GetContent("Path", "basePath") + "/doc/data.html";
        if (File.Exists(fileName)) {

            File.Delete(fileName);
        }
        var replaceTpl = tplContent.Replace("#{tableDiv}#", _dictionaryStr.ToString());

        replaceTpl = replaceTpl.Replace("{{dateTime}}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        File.WriteAllText(fileName, replaceTpl);
    }
}