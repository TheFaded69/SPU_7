using System.ComponentModel;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

public enum PrecisionMeasureUnits
{
    /// <summary>
    /// Проценты
    /// </summary>
    [Description("%")]
    Percent = 0,

    /// <summary>
    /// Паскали
    /// </summary>
    [Description("Па")]
    Pascal = 1,

    /// <summary>
    /// Без определённых единиц измерения
    /// </summary>
    [Description("")]
    NoUnits = 2,
}