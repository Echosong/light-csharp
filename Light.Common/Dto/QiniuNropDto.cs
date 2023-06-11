using Light.Common.Enums;

namespace Light.Common.Dto {
    public class QiniuNropDto {
        /// <summary>
        /// 返回图片的尺度
        /// </summary>
        public QiniuNropEnum qiniuNropEnum { get; set; }

        /// <summary>
        /// 是否需要人审核
        /// </summary>
        public bool Review { get; set; }
    }
}
