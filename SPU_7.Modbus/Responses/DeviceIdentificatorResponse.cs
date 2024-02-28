using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Responses;

/// <summary>
/// Ответ на команду запроса идентификатора прибора (<b>0x11</b>)
/// </summary>
public class DeviceIdentificatorResponse : BaseResponse
{
    /// <summary>
    /// количество байт
    /// </summary>
    [SerializationOrder(2)]
    public byte ByteCount { get; set; }

    /// <summary>
    /// Идентификатор блока данных
    /// </summary>
    [SerializationOrder(3)]
    public uint MainDataBlockId { get; set; }

    /// <summary>
    /// Номер версии формата
    /// </summary>
    [SerializationOrder(4)]
    public byte MainDataFormatVersion { get; set; }

    /// <summary>
    /// Размер блока
    /// </summary>
    [SerializationOrder(5)]
    public byte MainDataDataSize { get; set; }

    /// <summary>
    /// Идентификатор типа устройства в целом
    /// </summary>
    [SerializationOrder(6)]
    public byte DeviceTypeId { get; set; }

    /// <summary>
    /// Идентификатор типа модуля устройства.
    /// Флаговый регистр, где каждый бит означает одну из функций / модуль / возможность прибора.
    /// </summary>
    [SerializationOrder(7)]
    public uint DeviceFeaturesId { get; set; }

    /// <summary>
    /// Карта регистров (модель): главная версия (до точки, например 4.39 - 4)
    /// </summary>
    [SerializationOrder(8)]
    public byte RegisterMapMajorVersion { get; set; }

    /// <summary>
    /// Карта регистров (модель): дополнительная версия (после точки, например 4.39 - 39)
    /// </summary>
    [SerializationOrder(9)]
    public byte RegisterMapMinorVersion { get; set; }

    /// <summary>
    /// Версия МЗЧ ПО: главная версия (до точки, например 4.39 - 4)
    /// </summary>
    [SerializationOrder(10)]
    public byte SoftwareMajorVersion { get; set; }

    /// <summary>
    /// Версия МЗЧ ПО: дополнительная версия (после точки, например 4.39 - 39)
    /// </summary>
    [SerializationOrder(11)]
    public byte SoftwareMinorVersion { get; set; }

    /// <summary>
    /// CRC МЗЧ ПО
    /// </summary>
    [SerializationOrder(12)]
    [ByteOrder(ByteOrderType.LittleEndian_DCBA)]
    public uint SoftwareCrc { get; set; }

    /// <summary>
    /// Версия аппаратной части. ASCII 20 символов.
    /// </summary>
    [SerializationOrder(13)]
    public char[] HardwareVersion { get; set; } = new char[20];

    /// <summary>
    /// Заводской номер устройства. ASCII 20 символов
    /// </summary>
    [SerializationOrder(14)]
    public char[] DeviceSerialNumber { get; set; } = new char[20];

    /// <summary>
    /// Текущий год по прибору
    /// </summary>
    [SerializationOrder(15)]
    public ushort CurrentYear { get; set; }

    /// <summary>
    /// Текущий месяц по прибору
    /// </summary>
    [SerializationOrder(16)]
    public byte CurrentMonth { get; set; }

    /// <summary>
    /// Текущий день по прибору
    /// </summary>
    [SerializationOrder(17)]
    public byte CurrentDay { get; set; }

    /// <summary>
    /// Текущий час по прибору
    /// </summary>
    [SerializationOrder(18)]
    public byte CurrentHour { get; set; }

    /// <summary>
    /// Текущие минуты по прибору
    /// </summary>
    [SerializationOrder(19)]
    public byte CurrentMinutes { get; set; }

    /// <summary>
    /// Текущие секунды по прибору
    /// </summary>
    [SerializationOrder(20)]
    public byte CurrentSeconds { get; set; }

    /// <summary>
    /// Часовой пояс
    /// </summary>
    [SerializationOrder(21)]
    public byte TimeZone { get; set; }

    /// <summary>
    /// Канал интерфейса связи
    /// </summary>
    [SerializationOrder(22)]
    public byte CommunicationChannel { get; set; }

    /// <summary>
    /// Режим работы: включен
    /// </summary>
    [SerializationOrder(23)]
    public byte OperatingMode { get; set; }

    /// <summary>
    /// Идентификатор блока дополнительных данных
    /// </summary>
    [SerializationOrder(24)]
    public uint AdditionalDataBlockId { get; set; }

    /// <summary>
    /// Номер версии формата
    /// </summary>
    [SerializationOrder(25)]
    public byte AdditionalDataFormatVersion { get; set; }

    /// <summary>
    /// Размер блока
    /// </summary>
    [SerializationOrder(26)]
    public byte AdditionalDataDataSize { get; set; }

    /// <summary>
    /// Уникальный серийный номер МК (8 байт)
    /// </summary>
    [SerializationOrder(27)]
    public byte[] ProcessorUid { get; } = new byte[8];
}