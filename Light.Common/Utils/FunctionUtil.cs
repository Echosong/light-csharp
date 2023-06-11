using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Text;

namespace Light.Common.Utils {
    public class FunctionUtil {
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string LowerFirst(string message) {
            return message.Substring(0, 1).ToLower() + message.Substring(1);
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string UpperFirst(string message) {
            return message.Substring(0, 1).ToUpper() + message.Substring(1);
        }

        /// <summary>
        /// 二维码信息
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="JsonStr"></param>
        /// <returns></returns>
        public static string PostHttpDownLoad(string Url, string JsonStr, string fileName) {

            byte[] byteArray = null;
            if (string.IsNullOrEmpty(fileName)) {
                fileName = Guid.NewGuid() + ".png";
            }
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\qrcode", fileName);
            byteArray = Encoding.UTF8.GetBytes(JsonStr.Replace("\r", "\\r").Replace("\n", "\\n").Replace("\t", "\\t")); //转化
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            request.Timeout = 60000;
            request.Method = "POST";
            request.ContentType = "application/json"; ;
            request.ContentLength = byteArray.Length;
            Stream myRequestStream = request.GetRequestStream();
            myRequestStream.Write(byteArray, 0, byteArray.Length);//写入参数
            myRequestStream.Close();

            HttpWebResponse myrp = (System.Net.HttpWebResponse)request.GetResponse();
            long totalBytes = myrp.ContentLength;

            Stream st = myrp.GetResponseStream();

            Stream so = new FileStream(filePath, System.IO.FileMode.Create);
            long totalDownloadedByte = 0;
            byte[] by = new byte[1024];
            int osize = st.Read(by, 0, (int)by.Length);
            while (osize > 0) {
                totalDownloadedByte = osize + totalDownloadedByte;
                so.Write(by, 0, osize);
                osize = st.Read(by, 0, (int)by.Length);
            }
            so.Close();
            st.Close();

            return "qrcode/"+fileName;

        }

        /// <summary>
        /// 枚举转 字典
        /// </summary>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> GetEnums(string enumName = "") {

            var dictionary = new List<Dictionary<string, object>>();
            try {
                var dllNameArr = Assembly.GetExecutingAssembly().FullName?.Split(",");
                IEnumerable<Type?> q;
                if (dllNameArr == null || dllNameArr.Length == 0) {
                    return dictionary;
                }
                if (!string.IsNullOrEmpty(enumName)) {
                    q = from t in Assembly.Load(dllNameArr[0]).GetTypes()
                        where t.IsEnum && t.Name == UpperFirst(enumName)
                        select t;
                } else {
                    q = from t in Assembly.Load(dllNameArr[0]).GetTypes()
                        where t.IsEnum
                        select t;
                }
                var enumerable = q as Type[] ?? q.ToArray();
                if (enumerable.FirstOrDefault() == null) return dictionary;

                if (enumerable.Length == 1) {
                    var one = enumerable.FirstOrDefault();
                    if (one == null) {
                        return dictionary;
                    }
                    for (var i = 0; i < one.GetFields().Length; i++) {
                        try {
                            var fieldInfo = one.GetFields()[i];
                            var itemDic = new Dictionary<string, object> {
                                { "code",  fieldInfo.GetRawConstantValue()??"" },
                                { "name", fieldInfo.Name }
                            };
                            dictionary.Add(itemDic);
                        }
                        catch {
                            // ignored
                        }
                    }
                } else {
                    foreach (var type in enumerable) {
                        var itemList = new List<Dictionary<string, object>>();
                        if (type == null) {
                            continue;
                        }
                        for (var i = 0; i < type.GetFields().Length; i++) {
                            try {
                                var fieldInfo = type.GetFields()[i];
                                var itemDic = new Dictionary<string, object> {
                                    { "code", fieldInfo.GetRawConstantValue() ?? "" },
                                    { "name", fieldInfo.Name }
                                };
                                itemList.Add(itemDic);
                            } catch {
                                // ignored
                            }
                        }
                        dictionary.Add(new Dictionary<string, object>() { { type.Name, itemList } });
                    }
                }
            } catch (Exception exception) {
                LogHelper.Error(exception.Message);
            }

            return dictionary;
        }
    }
}