
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Light.Common.Attributes;
using Light.Common.Dto;

namespace Light.Entity {

    /// <summary>
    /// 分页主体数据处理
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Page<T> {
        /// <summary>
        /// 构造初始化函数
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pageable"></param>
        public Page(List<T> content, PageInfo pageable) {
            this.Content = FilterContent(content);
            pageable.totalPages = (int)Math.Ceiling((double)pageable.totalElements / pageable.pageSize);
            this.Pageable = pageable;
        }
        /// <summary>
        /// 设置列表
        /// </summary>
        public List<Dictionary<string, Object>> Content { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public PageInfo Pageable { set; get; }

        /// <summary>
        /// 过滤处理列表返回数据，主要是枚举数据
        /// </summary>
        /// <typeparam name="TE"></typeparam>
        /// <param name="list"></param>
        private List<Dictionary<string, Object>> FilterContent<TE>(List<TE> list) {
            //存储是否list需要隐藏，如果这里包含，那么就需要把值设置为null
            HashSet<string> listProp = new HashSet<string>();

            List<Dictionary<string, Object>> listDic = new List<Dictionary<string, Object>>();
            foreach (var item in list) {
                Dictionary<string, Object> dictionary = new Dictionary<string, Object>();
                var type = typeof(T);
                var fields = type.GetProperties();
                foreach (var propInfo in fields) {
                    object[] objAttrs = propInfo.GetCustomAttributes(typeof(AutoEntityField), true);
                    if (objAttrs.Length > 0) {
                        AutoEntityField? attr = objAttrs[0] as AutoEntityField;
                        if (attr == null) {
                            continue;
                        }
                        var baseFields = typeof(SysBase).GetProperties();
                        var count = baseFields.Count(t => t.Name == propInfo.Name);
                        //不需要列表显示(SysBase里面的是需要显示的)
                        if (!attr.List && count == 0) {
                            listProp.Add(propInfo.Name);
                        }

                        if (attr.EnumType == null) {
                            continue;
                        }
                        int index;
                        try {
                            index = (int)(propInfo.GetValue(item) ?? 0);
                        } catch (Exception) {
                            index = -1;
                        }

                        try {
                            var enumFields = attr.EnumType.GetFields();
                            var first = enumFields.Skip(1)
                                .FirstOrDefault(t => (int)(t.GetValue(null) ?? 0) == index);

                            if (first != null) {
                                dictionary.Add(propInfo.Name + "Enum", first.Name);
                            } else {
                                dictionary.Add(propInfo.Name + "Enum", "");
                            }
                        }
                        catch (Exception) {
                            // ignored
                        }
                    }
                }
                if (dictionary.Count > 0) {
                    listDic.Add(dictionary);
                }
            }
            var serializerSettings = new JsonSerializerSettings {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DateFormatString = "yyyy-MM-dd HH:mm:ss"
            };
            var jsonStr = JsonConvert.SerializeObject(list, Formatting.None, serializerSettings);
            var jsonArray = JsonConvert.DeserializeObject<List<Dictionary<string, Object>>>(jsonStr);


            //把内容替换为空其他的不处理了
            foreach (var jToken in jsonArray) {
                foreach (var key in jToken.Keys) {
                    if (listProp.Contains(key)) {
                        jToken[key] = "";
                    }
                }
            }

            //处理枚举字段
            if (listDic.Count > 0) {
                for (var i = listDic.Count - 1; i >= 0; i--) {
                    foreach (var pair in listDic[i]) {
                        jsonArray[i][pair.Key.Substring(0, 1).ToLower() + pair.Key.Substring(1)] = pair.Value;
                    }
                }
            }

            return jsonArray;
        }

    }
}
