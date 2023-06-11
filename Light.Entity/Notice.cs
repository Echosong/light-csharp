using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;


namespace Light.Entity {
    [AutoEntity(Name = "消息公告")]
    public class Notice : SysBase {
        [DisplayName("目标用户0位全部,-1 vip")]
        public int UserId { get; set; } = 0;

        [DisplayName("标题"), MaxLength(255)]
        public string? Title { get; set; }

        [DisplayName("内容")]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.TextArea)]
        public string? Description { get; set; }

        [DisplayName("状态")]
        [AutoEntityField(EnumType = typeof(NoticeStateEnum))]
        public int State { get; set; }
    }
}
