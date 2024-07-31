using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Gates;

public class KPK_RS485 : ModbusDevice, IModbusDevice
{
    public KPK_RS485(ICommunicationChannel? deviceCommunication = null)
        : base(deviceCommunication, ModbusExtensions.CreateRegisterMap<KPK_RS485_RegisterMap>(), DeviceEndianess.ABCD)
    {

    }
}