using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Light.Common.Dto;
using Light.Common.Enums;
using Light.Common.Filter;
using Light.Common.Utils;
using Light.Entity;
using Light.Service;

namespace Light.Admin.Controllers {
    /// <summary>
    /// 文件管理 控制器
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class AttachmentController : BaseController {



        private IQiniuService _qiniuService;

        /// <summary>
        /// 注入相关代码
        /// </summary>
        /// <param name="db"></param>
        /// <param name="_qiniuService"></param>
        public AttachmentController(Db db, IQiniuService _qiniuService) {
            this._db = db;
            this._qiniuService = _qiniuService;
        }


        /// <summary>
        ///     上传文件 (目前先支支持一个)
        /// </summary>
        /// <param name="fileType"></param>
        /// <param name="param"></param>
        [HttpPost]
        [NoPermission]
        public Dictionary<string, string> Upload(IFormFile file, string param = "") {
            Assert.IsTrue(file.Length > 10, "不存在上传文件信息");
            var dictionary = new Dictionary<string, string>();
            var limitExName = new List<string>() {
                "html", "htm", "js"
            };
            var fileNames = file.FileName.Split('.');
            var extend = fileNames[fileNames.Length - 1];
            Assert.IsTrue(!limitExName.Contains(extend), "不能保护敏感文件");
            string url = _qiniuService.Upload(file, extend, (int)FileTypeEnum.后台上传);
            dictionary = new Dictionary<string, string> {
                    {"name", file.FileName},
                    {"url", url},
                    {"params", param}
                };
            LogHelper.Info("上传文件成功");
            return dictionary;
        }

        /// <summary>
        /// wangEdit 返回图片信息
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>

        [HttpPost]
        [IgnoreResult]
        [NoPermission]
        [Route("/upload")]
        public Object Upload(IFormFile file) {
            var dictionary = this.Upload(file, "");

            return new {
                errno = 0,
                data = new string[]{ dictionary["url"]}
            };
        }



        /// <summary>
        /// 分页查询 文件管理 
        /// </summary>
        /// <param name="queryDto">分页条件</param>
        /// <returns></returns>
        [HttpPut]
        public Page<Attachment> ListPage(AttachmentQueryDto queryDto) {
            var where = PredicateExtend.True<Attachment>();
            if (!string.IsNullOrEmpty(queryDto.FileName)) {
                where = where.And(t => t.FileName.Contains(queryDto.FileName));
            }
            if (queryDto.FileType != null) {
                where = where.And(t => t.FileType == queryDto.FileType);
            }
            if (queryDto.StartDateTime != null) {
                where = where.And(t => t.DateTime > queryDto.StartDateTime);
            }
            if (queryDto.EndDateTime != null) {
                where = where.And(t => t.DateTime < queryDto.EndDateTime);
            }

            var queryWhere = _db.Attachments
                .Where(where);

            var query = queryWhere.OrderByDescending(t => t.Id)
                .Skip(queryDto.pageSize * (queryDto.page - 1))
                .Take(queryDto.pageSize);

            var list = query.ToList();
            if (list.Count > 0) {
                queryDto.totalElements = queryWhere.Count();
            }
            return new Page<Attachment>(list, queryDto);
        }

        /// <summary>
        /// 文件管理 单条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet]
        [Route("{id?}")]
        public Attachment? Find(int id) {
            return _db.Attachments.Find(id);
        }

        /// <summary>
        /// 新增活更新文件管理
        /// </summary>
        /// <param name="one">文件管理</param>
		[HttpPost]
        public void Save(Attachment one) {
            if (one.Id != 0) {
                _db.Attachments.Update(one);
            } else {
                _db.Attachments.Add(one);
            }
            _db.SaveChanges();
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete]
        [Route("{id?}")]
        public void Delete(int id) {
            _qiniuService.Remove(id);
        }

    }
}