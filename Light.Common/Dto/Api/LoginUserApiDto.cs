using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Light.Common.Dto.Api {
    public class LoginUserApiDto {

        /// <summary>
        /// 登录账号
        /// </summary>
        [Phone(ErrorMessage = "需要输入手机号码格式")]
        public string Username { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [StringLength(6, ErrorMessage = "{0}长度未6位"), DisplayName("验证码")]
        public string Code { get; set; }


        public string? LoginIp { get; set; }

        /// <summary>
        /// 推荐人id
        /// </summary>
        public int ParentId { get; set; } = 0;
    }
}
