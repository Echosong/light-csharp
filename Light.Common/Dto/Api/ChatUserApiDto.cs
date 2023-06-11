using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;

namespace Light.Common.Dto.Api {
    public class ChatUserApiDto {

        public int Id { get; set; }

        [DisplayName("用户id")]
        public int UserId { get; set; }

        [DisplayName("朋友id")]
        public int FriendId { get; set; }

        [DisplayName("账号"), MaxLength(50)]
        public string UserName { get; set; } = "";

        [DisplayName("朋友账号"), MaxLength(50)]
        public string FriendName { get; set; } = "";


        [DisplayName("朋友昵称")]
        public string FriendNick { get; set; } = "";

        [DisplayName("朋友头像")]
        public string FriendAvatar { get; set; } = "";

        [DisplayName("消息"), MaxLength(500)]
        public string? Message { get; set; }

        [DisplayName("状态")]
        public int? State { get; set; } = 1;

        [DisplayName("未读消息条数")]
        public int? MsgNumber { get; set; } = 0;

        [DisplayName("创建时间")]
        [AutoEntityField(List = false)]
        public DateTime? CreateDateTime { get; set; }
    }
}
