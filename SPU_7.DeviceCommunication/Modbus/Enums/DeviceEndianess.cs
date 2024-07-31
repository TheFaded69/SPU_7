using System.ComponentModel;

namespace SPU_7.DeviceCommunication.Modbus.Enums;

public enum DeviceEndianess
{
    /// <summary>
    /// Порядок начиная со старшего байта
    /// </summary>
    [Description("BigEndian")]
    ABCD,

    /// <summary>
    /// Порядок начиная со старшего байта
    /// </summary>
    [Description("MidBigEndian")]
    BADC,

    /// <summary>
    /// Порядок начиная с младшего байта
    /// </summary>
    [Description("LittleEndian")]
    DCBA,

    /// <summary>
    /// Порядок начиная с младшего байта
    /// </summary>
    [Description("MidLittleEndian")]
    CDAB
}