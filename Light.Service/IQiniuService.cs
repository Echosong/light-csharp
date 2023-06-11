using Microsoft.AspNetCore.Http;
using Light.Common.Dto;

namespace Light.Service {
    public interface IQiniuService {
        /// <summary>
        /// 上传七牛云保持
        /// </summary>
        /// <param name="file"></param>
        /// <param name="extend"></param>
        /// <returns></returns>
        public string Upload(IFormFile file, string extend, int? fileType);

        /// <summary>
        /// 删除文件时候删除七牛云
        /// </summary>
        /// <param name="key"></param>
        public void Remove(int id);


        /// <summary>
        /// 图片鉴黄服务
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public QiniuNropDto Nrop(string url);

        /// <summary>
        /// 图片鉴黄V3
        /// </summary>
        /// <returns></returns>
        public QiniuNropDto NropV3(string image);
    }
}
