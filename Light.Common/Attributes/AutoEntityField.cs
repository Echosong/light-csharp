using Light.Common.Enums;

namespace Light.Common.Attributes {
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoEntityField : Attribute {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段说明
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// 字段长度
        /// </summary>
        public int Len { get; set; } = 255;

        /// <summary>
        /// 是否不在列表展示 默认是 true
        /// </summary>
        public bool List { get; set; } = true;

        /// <summary>
        /// 查询是否做完查询条件，默认是false
        /// </summary>
        public bool Query { get; set; } = false;

        /// <summary>
        /// 允许导出字段
        /// </summary>
        public bool Export { get; set; } = true;

        /// <summary>
        /// 表单展现类型
        /// </summary>
        public HtmlTypeEnum TypeEnum { get; set; } = HtmlTypeEnum.Input;

        /// <summary>
        /// 状态啥的相关枚举名称
        /// </summary>
        public Type? EnumType { get; set; }

    }
}