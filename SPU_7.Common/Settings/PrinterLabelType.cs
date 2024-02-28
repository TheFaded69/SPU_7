using System.ComponentModel;
using SPU_7.Common.Attributes;
using SPU_7.Common.Stand;

namespace SPU_7.Common.Settings
{
    public enum PrinterLabelType
    {
        None = 0,

        [Description("Гранд БК Вер. 5")]
        [StandType(StandType.MetrologyStand_v25)]
        GrandBKv5 = 1,

        [Description("Гранд БК Вер. 10")]
        [StandType(StandType.MetrologyStand_v25)]
        GrandBKv10 = 2,

        [Description("Гранд 4-10")]
        [StandType(StandType.MetrologyStand_v25)]
        Grand4_10 = 3,
    }
}
