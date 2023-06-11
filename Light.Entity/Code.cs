using System.ComponentModel;
using Light.Common.Attributes;

namespace Light.Entity {
    [AutoEntity(Name = "专门用来生产单据号表", dto = false, controller = false, viewFrom = false, viewList = false)]
    public class Code {

        [DisplayName("id")]
        public int Id { get; set; }

        [DisplayName("表名称")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("单号")]
        public long Number { get; set; }
    }
}