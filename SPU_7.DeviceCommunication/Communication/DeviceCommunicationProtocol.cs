using System.ComponentModel;

namespace SPU_7.DeviceCommunication.Communication;

public enum DeviceCommunicationProtocol
{
    [Description("Неизвестный")]
    Unknown = 0,
    [Description("Elmetro")]
    Elmetro = 1,
    [Description("Modbus")]
    Modbus = 2,
    [Description("ОВЕН")]
    Owen = 3,
}