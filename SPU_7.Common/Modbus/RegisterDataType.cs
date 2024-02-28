// ReSharper disable InconsistentNaming

namespace SPU_7.Common.Modbus;

/// <summary>
/// Тип данных регистра
/// </summary>
public enum RegisterDataType
{
    /// <summary>
    /// 16 битное беззнаковое целое
    /// </summary>
    UInt16 = 0,

    /// <summary>
    /// 32 битное беззнаковое целое
    /// </summary>
    UInt32 = 1,

    /// <summary>
    /// 64 битное беззнаковое целое
    /// </summary>
    UInt64 = 2,

    /// <summary>
    /// Регистр флагов 16 бит
    /// </summary>
    Flags16 = 3,

    /// <summary>
    /// Регистр флагов 32 бит
    /// </summary>
    Flags32 = 4,

    /// <summary>
    /// Вещественное 32 бит
    /// </summary>
    Float = 5,

    /// <summary>
    /// Вещественное 64 бит
    /// </summary>
    Double = 6,

    /// <summary>
    /// Перечисление 16 бит
    /// </summary>
    Enum16 = 7,

    /// <summary>
    /// Перечисление 32 бит
    /// </summary>
    Enum32 = 8,

    /// <summary>
    /// Массив символов ASCII
    /// </summary>
    CharArray = 9,

    /// <summary>
    /// Массив байт
    /// </summary>
    ByteArray = 10,

    /// <summary>
    /// IP адрес AAA.BBB.CCC.DDD
    /// </summary>
    IpV4 = 11,

    /// <summary>
    /// <para><b>Время по порядку (количество байт):</b><br/>
    /// Миллисекунды (1),<br/> Секунды (1),<br/> Минуты (1),<br/> Часы (1),<br/> Год (2),<br/> Месяц (1),<br/> День (1)</para>
    /// </summary>
    TDateTime = 12,

    /// <summary>
    /// Формат Unixtime
    /// </summary>
    UnixTime = 13,

    /// <summary>
    /// Телефон, "+" - это '0xA', а '0xF' - конец
    /// </summary>
    PhoneBCD = 14,

    /// <summary>
    /// Повтор ожидания соединения
    /// </summary>
    TWaitConnect = 15,

    /// <summary>
    /// Повтор выхода на связь
    /// </summary>
    TConnection = 16,

    /// <summary>
    /// Регистр флагов 64 бит
    /// </summary>
    Flags64 = 17,

    /// <summary>
    /// 16 битное знаковое целое
    /// </summary>
    Int16 = 18,
}