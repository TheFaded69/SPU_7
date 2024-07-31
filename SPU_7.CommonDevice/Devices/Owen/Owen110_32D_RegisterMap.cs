using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;
public enum Owen110_32D_RegisterMap
{
    [RegisterConfiguration(0x0063, 2, RegisterDataType.UInt32)]
    InputsState,
}