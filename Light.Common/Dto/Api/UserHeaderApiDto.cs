namespace Light.Common.Dto.Api {
    public class UserHeaderApiDto {

        /// <summary>
        /// 账号id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 名称昵称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime? Created { get; set; }

    }

    /// <summary>
    /// 主要是我的里面的相关查询条件使用
    /// </summary>
    public class UserHeaderApiQueryDto : PageInfo {
        public int UserId { get; set; }
    }

}
