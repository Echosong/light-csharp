using System.ComponentModel;

namespace Light.Common.Dto.Api {
    public class AddressApiDto {
        [DisplayName("经度")]
        public Decimal? Longitude { get; set; } = 0;

        [DisplayName("纬度")]

        public Decimal? Latitude { get; set; } = 0;

        [DisplayName("用户id")]
        public int UserId { get; set; } = 0;
    }
}
