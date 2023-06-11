using System.ComponentModel;
using Light.Common.Attributes;

namespace Light.Entity {
    public class SysBase {
        [DisplayName("id")]
        public int Id { get; set; }

        [DisplayName("创建时间")]
        [AutoEntityField(List = false)]
        public DateTime? CreateDateTime { get; set; }

        [DisplayName("更新时间")]
        [AutoEntityField(List = false)]
        public DateTime? UpdateDateTime { get; set; }

        /// <summary>
        /// 0 为系统内置 不允许删除
        /// </summary>
        [DisplayName("创建人")]
        [AutoEntityField(List = false)]
        public int? CreatorId { get; set; }

        [DisplayName("更新人")]
        [AutoEntityField(List = false)]
        public int? UpdaterId { get; set; }

        [DisplayName("版本号")]
        [AutoEntityField(List = false)]
        public int? Version { get; set; }

        [DisplayName("多站点")]
        public int SiteId { get; set; } = 0;

    }
}