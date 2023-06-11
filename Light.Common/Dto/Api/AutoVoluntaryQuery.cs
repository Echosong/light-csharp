using System.ComponentModel;
using Light.Common.Attributes;
using Light.Common.Enums;

namespace Light.Common.Dto.Api;

public class AutoVoluntaryQuery {
    /// <summary>
    /// 默认为0
    /// </summary>
    public int Id { get; set; }
    public List<int> AreaIds { get; set; }

    public Int32 AreaId { get; set; }
    [DisplayName("办学层次")]
    public String RunLevel { get; set; }
    [DisplayName("办学类型")]
    public String RunCategory { get; set; }
    [DisplayName("办学特色")]
    public String RunFeature { get; set; }
    [DisplayName("院校归属")]
    public String RunBelong { get; set; }
    [DisplayName("办学性质")]
    public String RunType { get; set; }

    [DisplayName("专业id")]
    public List<int> MajorIds { get; set; }

    [DisplayName("收藏院校id")]
    public List<int> UniversityIds { get; set; }

    [AutoEntityField(EnumType = typeof(BatchEnum))]
    public int Batch { get; set; }

}