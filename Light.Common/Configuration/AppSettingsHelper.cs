using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Light.Common.Configuration {
    /// <summary>
    /// ��ȡAppsettings������Ϣ
    /// </summary>
    public class AppSettingsHelper {
        static IConfiguration Configuration { get; set; }

        public AppSettingsHelper(string contentPath) {
            string Path = "appsettings.json";
            Configuration = new ConfigurationBuilder().SetBasePath(contentPath).Add(new JsonConfigurationSource { Path = Path, Optional = false, ReloadOnChange = true }).Build();
        }

        /// <summary>
        /// ��װҪ�������ַ�
        /// AppSettingsHelper.GetContent(new string[] { "JwtConfig", "SecretKey" });
        /// </summary>
        /// <param name="sections">�ڵ�����</param>
        /// <returns></returns>
        public static string GetContent(params string[] sections) {
            try {

                if (sections.Any()) {
                    return Configuration[string.Join(":", sections)];
                }
            } catch (Exception) { }

            return "";
        }

    }
}