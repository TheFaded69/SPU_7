using System.ComponentModel;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

public enum ElmetroDeviceType
{
    /// <summary>
    /// ЭЛМЕТРО-Паскаль-02
    /// </summary>
    ElmetroPascal00Type = 0,

    /// <summary>
    /// ЭЛМЕТРО-Паскаль-02
    /// </summary>
    ElmetroPascal02Type = 1,

    /// <summary>
    /// ЭЛМЕТРО-Паскаль-04
    /// </summary>
    ElmetroPascal04Type = 2,

    /// <summary>
    /// ЭЛМЕТРО-Паскаль / МЕТРАН-530.
    /// </summary>
    ElmetroPascalMetranType = 3,
}

public enum ElmetroDeviceTypeFlags
{
    /// <summary>
    /// Биты 2-0 не используются и равны нулю
    /// </summary>
    [Description("Зарезервировано/Неизвестно")]
    None = 0,

    /// <summary>
    /// исполнение модуля давления.
    /// 1 – взрывобезопасное;
    /// 0 – общепромышленное
    /// </summary>
    [Description("Взрывозащищённое исполнение")]
    HaveExplosionProtection = 0x08,

    /// <summary>
    /// Маска для получения значения перечисления ElmetroDeviceType
    /// </summary>
    [Description("Тип устройства")]
    DeviceTypeMask = 0x70,

    /// <summary>
    /// Установлен, если тип модуля – без возможности оперативной подстройки смещения нуля (т.е. обнуление возможно только через механизм пользовательской коррекции),
    /// и сброшен, если модуль поддерживает оперативную подстройку смещения нуля
    /// </summary>
    [Description("Без динамической подстройки смещения нуля")]
    NonInteractiveMode = 0x80
}