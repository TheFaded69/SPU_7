using System.Collections;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.TurboDevices;
public class TDAdditionalInfo : TDBlockEnumerator
{
    public TDAdditionalInfo(IEnumerator inputData, DeviceEndianess endianess) : base(inputData)
    {
        HardwareSerialNumber = InputDataEnumerator.ConvertRegisterData<ulong>(RegisterDataType.UInt64, endianess);
    }

    /// <summary>
    /// Уникальный серийный номер МК
    /// </summary>
    public ulong HardwareSerialNumber { get; }
}