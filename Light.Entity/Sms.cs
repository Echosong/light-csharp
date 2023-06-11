using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {

    /// <summary>
    /// 短信魔板
    /// </summary>
    [AutoEntity(Name = "短信记录（微信推送消息）")]
    [Index("Code", IsUnique = false)]
    public class Sms : TemplateBase {
        [DisplayName("接受对象"), MaxLength(100)]
        [AutoEntityField(Query = true)]
        public string? Mobile { get; set; }

        [DisplayName("消息内容"), MaxLength(500)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.TextArea)]
        public string? Message { get; set; }

        [DisplayName("原始参数"), MaxLength(500)]
        [AutoEntityField(TypeEnum = HtmlTypeEnum.TextArea)]
        public string? Param { get; set; }

        [DisplayName("状态")]
        [AutoEntityField(Query = true, EnumType = typeof(SimStateEnum))]
        public int State { get; set; } = 0;

        [DisplayName("模板")]
        public int TemplateId { get; set; }
    }
}
