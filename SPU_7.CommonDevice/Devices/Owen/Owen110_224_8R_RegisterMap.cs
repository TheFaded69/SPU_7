using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;
public enum Owen110_224_8R_RegisterMap
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
    /// Период ШИМ на выходе №5 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0004, 1, RegisterDataType.UInt16)]
    Output5,

    /// <summary>
    /// Период ШИМ на выходе №6 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0005, 1, RegisterDataType.UInt16)]
    Output6,

    /// <summary>
    /// Период ШИМ на выходе №7 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0006, 1, RegisterDataType.UInt16)]
    Output7,

    /// <summary>
    /// Период ШИМ на выходе №8 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0007, 1, RegisterDataType.UInt16)]
    Output8,

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
    /// Безопасное состояние выхода №5 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0014, 1, RegisterDataType.UInt16)]
    AlertOutput5,

    /// <summary>
    /// Безопасное состояние выхода №6 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0015, 1, RegisterDataType.UInt16)]
    AlertOutput6,

    /// <summary>
    /// Безопасное состояние выхода №7 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0016, 1, RegisterDataType.UInt16)]
    AlertOutput7,

    /// <summary>
    /// Безопасное состояние выхода №8 (0...1000)
    /// </summary>
    [RegisterConfiguration(0x0017, 1, RegisterDataType.UInt16)]
    AlertOutput8,

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
}