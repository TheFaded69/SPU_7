using System.Collections;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.TurboDevices;
public class TDDeviceBlockInfo : TDBlockEnumerator
{
    public TDDeviceBlockInfo(IEnumerator inputData, DeviceEndianess endianess) : base(inputData)
    {
        DataBlockId = InputDataEnumerator.ConvertRegisterData<uint>(RegisterDataType.UInt32, endianess);
        FormatVersion = InputDataEnumerator.TakeValue<byte>();
        BlockSize = InputDataEnumerator.TakeValue<byte>();
    }

    /// <summary>
    /// Идентификатор блока данных
    /// </summary>
    public uint DataBlockId { get; }

    /// <summary>
    /// Версия формата блока данных
    /// </summary>
    public byte FormatVersion { get; }

    /// <summary>
    /// Размер блока данных
    /// </summary>
    public byte BlockSize { get; }
}