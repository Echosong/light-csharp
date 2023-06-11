using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    [AutoEntity(Name = "文件管理")]
    public class Attachment : SysBase {

        [AutoEntityField(Query = true)]
        [DisplayName("文件名"), MaxLength(200)]
        public string FileName { get; set; }

        [DisplayName("文件扩展名"), MaxLength(50)]
        public string Extend { get; set; }

        [DisplayName("文件存储路径"), MaxLength(255)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.Upload)]
        public string FilePath { get; set; }

        [DisplayName("文件存储类型")]
        [AutoEntityField(EnumType = typeof(FileTypeEnum), Query = true)]
        public int? FileType { get; set; }

        [DisplayName("文档大小")]
        public long? FileSize { get; set; }

        [DisplayName("相对路径"), MaxLength(100)]
        public string urlPath { get; set; }

        [DisplayName("唯一标识"), MaxLength(100)]
        public string Uuid { get; set; }

        [DisplayName("时间")]
        [AutoEntityField(Query = true)]
        public DateTime? DateTime { get; set; }

    }
}