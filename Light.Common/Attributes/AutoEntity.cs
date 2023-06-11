
namespace Light.Common.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class AutoEntity : Attribute {
        /// <summary>
        ///     字段名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     是否生成dto
        /// </summary>
        public bool dto { get; set; } = true;

        /// <summary>
        ///     是否生成前端list
        /// </summary>
        public bool viewList { get; set; } = true;


        /// <summary>
        ///     前端表单
        /// </summary>
        public bool viewFrom { get; set; } = true;

        /// <summary>
        /// 表单宽度
        /// </summary>
        public int fromWidth { get; set; } = 600;

        /// <summary>
        ///     控制器
        /// </summary>
        public bool controller { get; set; } = true;
    }
}