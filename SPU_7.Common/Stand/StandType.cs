using System.ComponentModel;

namespace SPU_7.Common.Stand
{
    public enum StandType
    {
        None = 0,

        [Description("СПУ-5 v.25")]
        MetrologyStand_v25 = 1,

        [Description("СПУ-5 v.32")]
        MetrologyStand_v32 = 2,

        [Description("СПУ-5 v.40")]
        MetrologyStand_v40 = 3,

    }
}
