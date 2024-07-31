using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices;
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