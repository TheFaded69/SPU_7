using System.Collections;
using System.Text;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.TurboDevices;
public class TDDeviceId : TDBlockEnumerator
{
    public TDDeviceId(IEnumerator inputData, DeviceEndianess endianess) : base(inputData)
    {
        DeviceTypeId = InputDataEnumerator.TakeValue<byte>();
        ModuleTypeId = InputDataEnumerator.ConvertRegisterData<uint>(RegisterDataType.UInt32, endianess);
        RegisterMapMajorVersion = InputDataEnumerator.TakeValue<byte>();
        RegisterMapMinorVersion = InputDataEnumerator.TakeValue<byte>();
        MetrologicSoftwareMajorVersion = InputDataEnumerator.TakeValue<byte>();
        MetrologicSoftwareMinorVersion = InputDataEnumerator.TakeValue<byte>();
        MetrologicSoftwareCRC = InputDataEnumerator.ConvertRegisterData<uint>(RegisterDataType.UInt32, endianess);
        HardwareVersion = InputDataEnumerator.TakeToArray<byte>(20).ToHexString()/*.ConvertRegisterData(RegisterDataType.CharArray, endianess)*/;
        SerialNumber = Extensions.MemoryExtensions.GetEncodedString(InputDataEnumerator.TakeToArray<byte>(20), Encoding.UTF8);
        Year = InputDataEnumerator.ConvertRegisterData<ushort>(RegisterDataType.UInt16, endianess);
        Month = InputDataEnumerator.TakeValue<byte>();
        Day = InputDataEnumerator.TakeValue<byte>();
        Hour = InputDataEnumerator.TakeValue<byte>();
        Minutes = InputDataEnumerator.TakeValue<byte>();
        Seconds = InputDataEnumerator.TakeValue<byte>();
        TimeZone = InputDataEnumerator.TakeValue<byte>();
        CommunicationChannel = InputDataEnumerator.TakeValue<byte>();
        WorkMode = InputDataEnumerator.TakeValue<byte>();
    }

    /// <summary>
    /// Идентификатор типа устройства
    /// </summary>
    public byte DeviceTypeId { get; }

    /// <summary>
    /// Идентификатор типа модуля устройства
    /// </summary>
    public uint ModuleTypeId { get; }

    /// <summary>
    /// Карта регистров (модель): главная версия
    /// </summary>
    public byte RegisterMapMajorVersion { get; }

    /// <summary>
    /// Карта регистров (модель): дополнительная версия 
    /// </summary>
    public byte RegisterMapMinorVersion { get; }

    /// <summary>
    /// Карта регистров (модель): главная версия
    /// </summary>
    public byte MetrologicSoftwareMajorVersion { get; }

    /// <summary>
    /// Карта регистров (модель): дополнительная версия 
    /// </summary>
    public byte MetrologicSoftwareMinorVersion { get; }

    /// <summary>
    /// CRC МЗЧ ПО
    /// </summary>
    public uint MetrologicSoftwareCRC { get; }

    /// <summary>
    ///  Версия аппаратной части
    /// </summary>
    public string HardwareVersion { get; } = string.Empty;

    /// <summary>
    /// Заводской номер устройства
    /// </summary>
    public string SerialNumber { get; } = string.Empty;

    /// <summary>
    /// Текущий год устройства
    /// </summary>
    public ushort Year { get; }

    /// <summary>
    /// Текущий месяц устройства
    /// </summary>
    public byte Month { get; }

    /// <summary>
    /// Текущий день устройства
    /// </summary>
    public byte Day { get; }

    /// <summary>
    /// Текущий час устройства
    /// </summary>
    public byte Hour { get; }

    /// <summary>
    /// Текущие минуты устройства
    /// </summary>
    public byte Minutes { get; }

    /// <summary>
    /// Текущие секунды устройства
    /// </summary>
    public byte Seconds { get; }

    /// <summary>
    /// Часовой пояс (+TimeZone)
    /// </summary>
    public byte TimeZone { get; }

    /// <summary>
    /// Канал интерфейса связи
    /// </summary>
    public byte CommunicationChannel { get; }

    /// <summary>
    /// Режим работы
    /// </summary>
    public byte WorkMode { get; }
}