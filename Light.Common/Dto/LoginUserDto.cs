using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Light.Common.Dto {

    public class LoginUserDto {
        [Phone]
        [DisplayName("账号")]
        public string Username { get; set; }

        [DisplayName("密码")]
        [MaxLength(20), MinLength(6)]
        public string Password { get; set; }

        [DisplayName("请输入验证码")]
        [MinLength(3)]
        public string? Code { get; set; }

        [DisplayName("登陆客户端ip")]
        public string? LoginIp { get; set; }
    }
}