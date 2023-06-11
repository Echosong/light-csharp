namespace Light.Common.Dto {
    /// <summary>
    /// 分页信息
    /// </summary>
    public class PageInfo {

        private int _page = 1;

        /// <summary>
        /// 页数，默认0,第一页
        /// </summary>
        public int page { get => _page < 1 ? 1 : _page; set => _page = value; }

        /// <summary>
        /// 每页个数,默认10
        /// </summary>
        public int pageSize { get; set; } = 10;

        /// <summary>
        /// 共有多少条
        /// </summary>
        public int totalElements { get; set; }

        /// <summary>
        /// 共有多少页
        /// </summary>
        public int totalPages { get; set; }


    }
}
