using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Entity {
    /// <summary>
    /// 各类模板 短信  转发
    /// </summary>
    [AutoEntity(Name = "转发短信模板")]
    public class Template : TemplateBase {

        [DisplayName("限定参数"), MaxLength(500)]
        public string Params { get; set; } = string.Empty;

        [DisplayName("模板说明"), MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [DisplayName("魔板内容"), MaxLength(500)]
        [AutoEntityField(Name = "模板内容", TypeEnum = HtmlTypeEnum.TextArea)]
        public string Content { get; set; } = String.Empty;


    }
}
