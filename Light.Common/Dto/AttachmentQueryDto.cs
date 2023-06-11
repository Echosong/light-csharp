using System.ComponentModel;
namespace Light.Common.Dto {
    public class AttachmentQueryDto : PageInfo {
        [DisplayName("文件名")]
        public string? FileName { get; set; }
        [DisplayName("文件存储类型")]
        public Int32? FileType { get; set; }
        [DisplayName("时间")]
        public DateTime? DateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }

    }
}