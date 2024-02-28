using System.ComponentModel;
using SPU_7.Common.Attributes;
using SPU_7.Common.Stand;

namespace SPU_7.Common.Settings
{
    /// <summary>
    /// Состояние устройств на стенде
    /// </summary>
    public enum DeviceStateType
    {
        None = 0,

        [Description("Новые")]
        [StandType(StandType.MetrologyStand_v25)]
        New = 1,

        [Description("Ремонтные")]
        [StandType(StandType.MetrologyStand_v25)]
        Repair = 2,

        [Description("Экспериментные")]
        [StandType(StandType.MetrologyStand_v25)]
        Experiment = 3,
    }
}
