using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Light.Common.Enums;

namespace Light.Common.Dto.Api;

public class PayProductDto {
    [Required(ErrorMessage = "必须传入购买信息id")]
    public int ProductId { get; set; }

    [DisplayName("购买类型")]
    public int Type { get; set; } = (int)FinanceTypeEnum.购买会员;

    [DisplayName("补充信息")]
    public string? Info { get; set; }
}