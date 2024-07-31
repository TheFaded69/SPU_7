using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices.PS_UNI;

public enum PS_UNI_MeasureUnits
{
    //MultiplierAttribute(1e-6) []
    [Description("Па")]
    Pa,
    //MultiplierAttribute(1e-3) []
    [Description("КПа")]
    KPa,
    //MultiplierAttribute(1) []
    [Description("МПа")]
    MPa,
}