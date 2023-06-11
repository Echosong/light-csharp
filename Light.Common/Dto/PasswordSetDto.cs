using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Light.Common.Dto {
    public class PasswordSetDto {
        /// <summary>
        /// 新设置密码
        /// </summary>
        [DisplayName("原始密码")]
        [MinLength(6)]
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [MinLength(6)]
        [DisplayName("新密码")]
        public string Password { get; set; }

        /// <summary>
        /// 重复密码
        /// </summary>
        [DisplayName("重复密码")]
        public string RePassword { get; set; }
    }
}
