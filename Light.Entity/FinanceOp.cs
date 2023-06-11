using System.ComponentModel;
using Light.Common.Attributes;
using Light.Common.Enums;


namespace Light.Entity {
    [AutoEntity(Name = "会员充值，下载付费，分润相关支付业务记录")]
    public class FinanceOp : SysBase {

        [DisplayName("用户id")]
        public int? UserId { get; set; }

        [DisplayName("账号")]
        [AutoEntityField(Query = true)]
        public string? Username { get; set; }

        [DisplayName("账户类型")]
        [AutoEntityField(EnumType = typeof(FinanceTypeEnum), Query = true)]
        public int? BusinessType { get; set; }

        [DisplayName(@"状态")]
        [AutoEntityField(EnumType = typeof(FinanceOpStateEnum), Query = true)]
        public int? State { get; set; }

        [DisplayName("金额")]
        public decimal? Account { get; set; }

        [DisplayName("外部单号")]
        public string? OutOrderNo { get; set; }

        [DisplayName(@"描述，这里可以放信息")]
        public string? Description { get; set; }

        [DisplayName("业务关系id")]
        public int? RelationId { get; set; }

        [AutoEntityField(Name = "支付时间")]
        public DateTime PayTime { get; set; }
    }
}
