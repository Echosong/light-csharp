using System.Diagnostics;
using SM23Crypto;

namespace Light.Test {
    public class BaiduTest {

        public enum BatchEnum {
            本一批 = 1,
            本二批,
            专科批
        }

        public static void Start() {

            decimal price = Convert.ToDecimal((7.8 * 80 / 100).ToString("#0.0"));

            Console.WriteLine(price);
            /*

             string content = "你好一起找小姐，我是你爹,百度默认文本反作弊库 免费翻墙";
             var result = File.ReadAllText("E:/1.json");

             var resultJObject = JObject.Parse(result);
             //审核不通过需要替换敏感字符
             if (Convert.ToInt32(resultJObject["conclusionType"]) == 2) {
                 foreach (var item in resultJObject["data"]) {
                     foreach (var hit in item["hits"]) {

                         foreach (string word in hit["words"]) {
                             Console.WriteLine(word);
                             if (word.Length > 0) {
                                content = content.Replace(word, "****");
                             }
                         }
                     }
                 }
             }

             Console.WriteLine(content);
            */

        }

    }
}
