using NewLife.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Light.Common.Configuration;
using Light.Common.Error;
using Light.Common.RedisCache;
using Light.Common.Utils;

namespace Light.Service.Impl {
    /// <summary>
    /// 百度内容审核
    /// </summary>
    public class BaiduTextCensorService : IBaiduTextCensorService {

        /// <summary>
        /// 获取地址
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public async Task<string> GetAddress(string address) {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync("https://api.map.baidu.com/reverse_geocoding/v3/?ak=98OpFyS7jlPPMCb94wl5Xf06wGmb6Lcy&output=json&coordtype=wgs84ll&location=" + address);
        }


        /// <summary>
        /// 替换敏感字符
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="BaseException"></exception>
        public string TextCensor(string content) {
            string? token = string.Empty;
            try {
                token = Redis.CreateInstance().GetCache<string>("access_token");
                if (string.IsNullOrEmpty(token)) {
                    token = GetAccessToken();
                }
            } catch (Exception ex) {
                token = GetAccessToken();
            }

            if (string.IsNullOrEmpty(token)) {
                return content;
            }
            string host = "https://aip.baidubce.com/rest/2.0/solution/v1/text_censor/v2/user_defined?access_token=" + token;
            HttpClient httpClient = new HttpClient();
            String str = "text=" + content;
            var result = httpClient.PostForm(host, str);
            try {
                var resultJObject = JObject.Parse(result);
                //审核不通过需要替换敏感字符
                if (Convert.ToInt32(resultJObject["conclusionType"]) == 2) {
                    foreach (var item in resultJObject["data"]) {
                        foreach (var hit in item["hits"]) {
                            foreach (string word in hit["words"]) {
                                if (word != null && word.Length > 0) {
                                    content = content.Replace(word, "***");
                                }
                            }
                        }
                    }
                }
                return content;
            } catch (Exception e) {
                LogHelper.Error("百度内容审核", e);
                return content;
            }
        }

        /// <summary>
        /// 配置client
        /// </summary>
        private string clientId = AppSettingsConstVars.BaiduClientId;
        private string clientSecret = AppSettingsConstVars.BaiduClientSecret;

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        private string? GetAccessToken() {
            String authHost = "https://aip.baidubce.com/oauth/2.0/token";
            HttpClient client = new HttpClient();
            List<KeyValuePair<String, String>> paraList = new List<KeyValuePair<string, string>>();
            paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
            paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            String result = response.Content.ReadAsStringAsync().Result;
            try {
                Dictionary<string, string?> keyValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                Redis.CreateInstance().SetCache<string>("access_token", keyValues["access_token"], TimeSpan.FromDays(20));
                return keyValues["access_token"];
            } catch (Exception ex) {
                LogHelper.Error("百度内容审核" + result, ex);
                return "";
            }

        }
    }
}
