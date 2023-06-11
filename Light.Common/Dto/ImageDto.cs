namespace Light.Common.Dto {
    public class ImageDto {
        /// <summary>
        /// 图片名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 如果是视频这里存储封面
        /// </summary>
        public string? Img { get; set; }


        /// <summary>
        /// 图片|视频地址
        /// </summary>
        public string? Url { get; set; }
    }
}
