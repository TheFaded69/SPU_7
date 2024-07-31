using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices.Owen;

/// <summary>
/// Перечисление хэш кодов команд СИ8 в Протоколе Овен
/// </summary>
public enum OwenIC8_Parameters : ushort
{
    [Description("Счётчик импульсов")]
    ImpulseCount = 0xC173,

    [Description("Показания расходомера")]
    FlowValue = 0x8FC2,

    [Description("Показания таймера")]
    TimerValue = 0xE69C,
}