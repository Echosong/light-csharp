namespace Light.Common.Dto.Api;

public class UserOrderDto {
    /// <summary>
    /// 记录id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 订单用户
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// 订单标题
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 购买时间
    /// </summary>
    public DateTime? PayTime { get; set; }

    /// <summary>
    /// 到期时间
    /// </summary>
    public DateTime? Expire { get; set; } = new DateTime();

}