using System.ComponentModel.DataAnnotations;

namespace Light.Common.Dto.Api {
    public class UserInfoApiDto {
        public string? Username { get; set; }

        [Required]
        public int Sex { get; set; }

        public string? Birthday { get; set; }

        public string? Signlogo { get; set; }

        [Required(ErrorMessage = "昵称不能为空")]
        public string Avatar { get; set; }

        [Required(ErrorMessage = "账户不能为空")]
        public string Name { get; set; }


    }
}
