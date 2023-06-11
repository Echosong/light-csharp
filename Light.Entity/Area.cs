using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Light.Common.Attributes;

namespace Light.Entity {

    [AutoEntity(Name = "地区表")]
    public class Area : SysBase {

        /// <summary>
        /// 父级ID
        /// </summary>
        [Display(Name = "父级ID")]
        [AutoEntityField(Name = "父级ID", Query = true)]
        public System.Int32? ParentId { get; set; }

        /// <summary>
        /// 地区深度
        /// </summary>
        [Display(Name = "地区深度")]
        [AutoEntityField(Name = "地区深度")]
        public System.Int32? Depth { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        [Display(Name = "地区名称")]
        [AutoEntityField(Name = "地区名称", Len = 100, Query = true), MaxLength(100)]
        [StringLength(50, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.String Name { get; set; }

        /// <summary>
        /// 邮编
        /// </summary>
        [Display(Name = "邮编")]
        [AutoEntityField(Name = "邮编", Len = 20, Query = true)]
        [StringLength(10, ErrorMessage = "【{0}】不能超过{1}字符长度")]
        public System.String PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// 地区排序
        /// </summary>
        [Display(Name = "地区排序")]
        [AutoEntityField(Name = "地区排序")]
        [Required(ErrorMessage = "请输入{0}")]
        public System.Int32 Sort { get; set; }


        /// <summary>
        /// 子节点
        /// </summary>
        [NotMapped]
        public List<Area> Children { get; set; }
    }
}
