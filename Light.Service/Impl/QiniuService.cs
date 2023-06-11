using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using NewLife.Http;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using Light.Common.Configuration;
using Light.Common.Dto;
using Light.Common.Enums;
using Light.Common.Error;
using Light.Common.Utils;
using Light.Entity;

namespace Light.Service.Impl {
    public class QiniuService : IQiniuService {
        private readonly Db _db;
        public QiniuService(Db db) {
            this._db = db;
        }

        readonly string _bucket = AppSettingsConstVars.QiniuBucket;

        /// <summary>
        /// 上传存储到七牛
        /// </summary>
        /// <returns></returns>
        public string Upload(IFormFile file, string extend, int? fileType) {
            Mac mac = new Mac(AppSettingsConstVars.QiniuAccessKey, AppSettingsConstVars.QiniuSecretKey);

            var format = DateTime.Now.ToString("yy/MM");
            var uuid = Guid.NewGuid().ToString("N");
            string key = $"{format}/{uuid}.{extend}";

            // 存储空间名
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = _bucket;

            //putPolicy.SetExpires(3600);
            //putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());

            Qiniu.Storage.Config config = new Qiniu.Storage.Config();
            // 设置上传区域
            config.Zone = Zone.ZONE_CN_East;
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            config.ChunkSize = ChunkUnit.U4096K;
            // 表单上传
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadStream(file.OpenReadStream(), key, token, null);
            Assert.IsTrue(result.Code == 200, "上传到存储错误");

            string url = AppSettingsConstVars.QiniuDomain + key;

            //插入到数据库
            _db.Attachments.Add(new Attachment {
                FileName = file.Name,
                FilePath = key,
                FileSize = file.Length / 1000,
                urlPath = url,
                Extend = extend,
                Uuid = uuid,
                FileType = fileType
            });
            _db.SaveChanges();

            return url;
        }

        /// <summary>
        /// 删除七牛云
        /// </summary>
        public void Remove(int id) {
            var attach = _db.Attachments.Find(id);
            if (attach == null) {
                throw new BaseException("文件不存在");
            }
            Qiniu.Storage.Config config = new Qiniu.Storage.Config();
            config.Zone = Zone.ZONE_CN_East;
            Mac mac = new Mac(AppSettingsConstVars.QiniuAccessKey, AppSettingsConstVars.QiniuSecretKey);
            _db.Attachments.Remove(attach);
            _db.SaveChanges();
            BucketManager bucketManager = new BucketManager(mac, config);
            // 文件名
            string key = attach.FilePath;
            HttpResult deleteRet = bucketManager.Delete(_bucket, key);
            Assert.IsTrue(deleteRet.Code == (int)HttpCode.OK, "存储删除文件错误");


        }

        /// <summary>
        /// 获取 存储key
        /// </summary>
        /// <param name="url">原始URL</param>
        private string UrlSplit(string url) {
            int start = 0;
            string host;
            string path;
            string file;
            Regex regHost = new Regex(@"(http|https):\/\/[\w\-_]+(\.[\w\-_]+)+");
            host = regHost.Match(url, start).Value;
            start += host.Length;

            Regex regPath = new Regex(@"(/(\w|\-)*)+/");
            path = regPath.Match(url, start).Value;
            if (string.IsNullOrEmpty(path)) {
                path = "/";
            }
            start += path.Length;

            int index = url.IndexOf('?', start);
            if (index == -1) {
                file = url.Substring(start);
            } else {
                file = url.Substring(start, index - start);
            }
            return path + file;


        }

        /// <summary>
        /// V2版本审核
        /// 
        /// {"code":200,"message":"OK","entry_id":"6357ea3000018704579be8e14d182011","result":{"suggestion":"pass","scenes":{"pulp":
        /// {"suggestion":"pass","details":[{"suggestion":"pass","label":"normal","score":0.94208306}]}}}}
        /// </summary>
        /// qiniu:///es-sns/3333.jpg
        /// <returns></returns>
        public QiniuNropDto NropV3(string image) {
            var key = this.UrlSplit(image);
            //转七牛协议地址
            image = "qiniu:///" + AppSettingsConstVars.QiniuBucket + key;
            QiniuNropDto nrop = new QiniuNropDto {
                qiniuNropEnum = QiniuNropEnum.异常,
                Review = true
            };

            Mac mac = new Mac(AppSettingsConstVars.QiniuAccessKey, AppSettingsConstVars.QiniuSecretKey);
            String url = "http://ai.qiniuapi.com/v3/image/censor";
            StringDictionary dictionary = new StringDictionary();
            dictionary.Add("Content-Type", "application/json");
            string body = "{ \"data\": {\"uri\": \"" + image + "\"},\"params\": {\"scenes\":[\"pulp\"]}}";

            try {
                string token = Auth.CreateManageTokenV2(mac, "POST", url, dictionary, body);
                HttpManager httpManager = new HttpManager();
                var result = httpManager.PostJson(url, body, token);
                if (result.Code == 200) {
                    JObject jobject = JObject.Parse(result.Text);
                    var suggestion = jobject["result"]?["suggestion"]?.ToString();
                    switch (suggestion) {
                        case "block":
                            nrop.Review = false;
                            nrop.qiniuNropEnum = QiniuNropEnum.色情;
                            break;
                        case "review":
                            nrop.Review = true;
                            nrop.qiniuNropEnum = QiniuNropEnum.性感;
                            break;
                        case "pass":
                            nrop.Review = false;
                            nrop.qiniuNropEnum = QiniuNropEnum.正常;
                            break;
                    }
                    return nrop;
                } else {
                    LogHelper.Error("七牛鉴黄返回错误" + result.Text);
                    return nrop;
                }

            } catch (Exception ex) {
                LogHelper.Error("七牛鉴黄返回错误", ex);
                return nrop;
            }
        }

        /// <summary>
        /// 图片鉴黄服务
        /// 文档说明 https://developer.qiniu.com/dora/kb/1406/image-as-a-yellow-service-interface-protocols
        /// </summary>
        public QiniuNropDto Nrop(string url) {

            QiniuNropDto nrop = new QiniuNropDto {
                qiniuNropEnum = QiniuNropEnum.异常,
                Review = true
            };

            try {
                HttpClient client = new HttpClient();
                string result = client.GetString($"{url}?nrop", new Dictionary<string, string> {
                    { "Content-Type", "application/json" },
                    { "Cache-Contro", "no-store"}

                });
                var jobject = JObject.Parse(result);
                if (Convert.ToInt32(jobject["code"]) != 0) {
                    LogHelper.Debug("七牛鉴黄返回错误" + jobject["message"]);
                    return nrop;
                }
                var jArray = (JArray)jobject["fileList"]!;
                JObject jItem = (JObject)jArray?[0]!;
                nrop.qiniuNropEnum = (QiniuNropEnum)Convert.ToInt32(jItem["label"]);
                nrop.Review = Convert.ToBoolean(jItem["review"]);
                return nrop;

            } catch (Exception ex) {
                LogHelper.Error("七牛鉴黄返回错误", ex);
                return nrop;
            }

        }

    }
}
