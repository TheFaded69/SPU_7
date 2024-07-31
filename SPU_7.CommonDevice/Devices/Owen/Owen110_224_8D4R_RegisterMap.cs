using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;

public enum Owen110_224_8D4R_RegisterMap
{
    /// <summary>
    /// Период ШИМ на выходе №1 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0000, 1, RegisterDataType.UInt16)]
    Output1,

    /// <summary>
    /// Период ШИМ на выходе №2 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0001, 1, RegisterDataType.UInt16)]
    Output2,

    /// <summary>
    /// Период ШИМ на выходе №3 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0002, 1, RegisterDataType.UInt16)]
    Output3,

    /// <summary>
    /// Период ШИМ на выходе №4 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0003, 1, RegisterDataType.UInt16)]
    Output4,

    /// <summary>
    /// Безопасное состояние выхода №1 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0010, 1, RegisterDataType.UInt16)]
    AlertOutput1,

    /// <summary>
    /// Безопасное состояние выхода №2 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0011, 1, RegisterDataType.UInt16)]
    AlertOutput2,

    /// <summary>
    /// Безопасное состояние выхода №3 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0012, 1, RegisterDataType.UInt16)]
    AlertOutput3,

    /// <summary>
    /// Безопасное состояние выхода №4 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0013, 1, RegisterDataType.UInt16)]
    AlertOutput4,

    /// <summary>
    /// Период ШИМ на выходе №1 (0...900с)
    /// </summary>
    [RegisterConfiguration(0x0020, 1, RegisterDataType.UInt16)]
    OutputPWM1,

    /// <summary>
    /// Период ШИМ на выходе №2 (0...900с)
    /// </summary>
    [RegisterConfiguration(0x0021, 1, RegisterDataType.UInt16)]
    OutputPWM2,

    /// <summary>
    /// Период ШИМ на выходе №3 (0...900с)
    /// </summary>
    [RegisterConfiguration(0x0022, 1, RegisterDataType.UInt16)]
    OutputPWM3,

    /// <summary>
    /// Период ШИМ на выходе №4 (0...900с)
    /// </summary>
    [RegisterConfiguration(0x0023, 1, RegisterDataType.UInt16)]
    OutputPWM4,

    /// <summary>
    /// Максимальный сетевой тайм-аут (0...600с)
    /// </summary>
    [RegisterConfiguration(0x0030, 1, RegisterDataType.UInt16)]
    MaxNetworkTimeout,

    /// <summary>
    /// Состояние всех выходов сразу (Битовая маска)
    /// </summary>
    [RegisterConfiguration(0x0032, 1, RegisterDataType.UInt16)]
    OutputsState,

    /// <summary>
    /// Состояние всех входов сразу (Битовая маска)
    /// </summary>
    [RegisterConfiguration(0x0033, 1, RegisterDataType.UInt16)]
    InputsState,

    /// <summary>
    /// Счётчик входа №1
    /// </summary>
    [RegisterConfiguration(0x0040, 1, RegisterDataType.UInt16)]
    DigitalCounter1,

    /// <summary>
    /// Счётчик входа №2
    /// </summary>
    [RegisterConfiguration(0x0041, 1, RegisterDataType.UInt16)]
    DigitalCounter2,

    /// <summary>
    /// Счётчик входа №3
    /// </summary>
    [RegisterConfiguration(0x0042, 1, RegisterDataType.UInt16)]
    DigitalCounter3,

    /// <summary>
    /// Счётчик входа №4
    /// </summary>
    [RegisterConfiguration(0x0043, 1, RegisterDataType.UInt16)]
    DigitalCounter4,

    /// <summary>
    /// Счётчик входа №5
    /// </summary>
    [RegisterConfiguration(0x0044, 1, RegisterDataType.UInt16)]
    DigitalCounter5,

    /// <summary>
    /// Счётчик входа №6
    /// </summary>
    [RegisterConfiguration(0x0045, 1, RegisterDataType.UInt16)]
    DigitalCounter6,

    /// <summary>
    /// Счётчик входа №7
    /// </summary>
    [RegisterConfiguration(0x0046, 1, RegisterDataType.UInt16)]
    DigitalCounter7,

    /// <summary>
    /// Счётчик входа №8
    /// </summary>
    [RegisterConfiguration(0x0047, 1, RegisterDataType.UInt16)]
    DigitalCounter8,
}