using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices.Owen;

internal enum Owen110_224_8R_Parameters
{
    /// <summary>
    /// Название прибора
    /// </summary>
    [Description("Название прибора")]
    DeviceName = 0xD681,

    /// <summary>
    /// Версия прошивки
    /// </summary>
    [Description("Версия прошивки")]
    HardwareVersion = 0x2D5B,

    /// <summary>
    /// Скорость обмена данными
    /// </summary>
    [Description("Скорость обмена")]
    BaudRate = 0xB760,

    /// <summary>
    /// Длина слова данных
    /// </summary>
    [Description("Бит данных")]
    DataBits = 0x523F,

    /// <summary>
    /// Тип контроля четности слова данных
    /// </summary>
    [Description("Чётность")]
    Parity = 0xE8C4,

    /// <summary>
    /// Количество стоп-битов в посылке
    /// </summary>
    [Description("Стоп-битов")]
    StopBits = 0xB72E,

    /// <summary>
    /// Длина сетевого адреса
    /// </summary>
    [Description("Длина сетевого адреса")]
    AddressLength = 0x1ED2,

    /// <summary>
    /// Базовый адрес прибора
    /// </summary>
    [Description("Базовый адрес прибора")]
    Address = 0x9F62,

    /// <summary>
    /// Протокол обмена
    /// </summary>
    [Description("Протокол обмена")]
    Protocol = 0x41F2,

    /// <summary>
    /// Максимальный сетевой таймаут
    /// </summary>
    [Description("Максимальный сетевой таймаут")]
    Timeout = 0xBEC7,

    /// <summary>
    /// Задержка ответа по сети RS 485
    /// </summary>
    [Description("Задержка ответа")]
    ResponseDelay = 0xCBF5,

    /// <summary>
    /// Период ШИМ при управлении ВЭ по RS-485
    /// </summary>
    [Description("Период ШИМ")]
    PWM_RS485 = 0x7BFE,

    /// <summary>
    /// Аварийное значение на ВЭ
    /// </summary>
    [Description("Аварийное значение")]
    AlertOutputValue = 0xDC64,
}