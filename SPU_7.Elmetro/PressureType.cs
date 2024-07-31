using System.ComponentModel;

namespace DeviceCommunication;
public enum PressureType
{
    [Description("")]
    DefaultPressure,

    [Description("Абсолютное давление")]
    AbsolutePressure,

    [Description("Избыточное давление")]
    ExcessPressure,

    [Description("")]
    RelativePressure,
}