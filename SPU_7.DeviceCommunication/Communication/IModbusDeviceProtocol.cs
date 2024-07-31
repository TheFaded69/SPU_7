using System.ComponentModel;
using NLog;
using SPU_7.DeviceCommunication.Modbus;
using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;
using SPU_7.DeviceCommunication.Modbus.Response;

namespace SPU_7.DeviceCommunication.Communication;

public interface IModbusDeviceProtocol : INotifyPropertyChanged
{
    /// <summary>
    /// Коммуникатор для протокола Modbus
    /// </summary>
    ICommunicationChannel? CommunicationChannel { get; set; }

    /// <summary>
    /// Адрес в сети Modbus
    /// </summary>
    int Address { get; set; }

    /// <summary>
    /// Количество попыток пересылки команды
    /// </summary>
    int RetryCount { get; set; }

    /// <summary>
    /// Использовать ли преамбулу
    /// </summary>
    bool UsePreamble { get; set; }

    /// <summary>
    /// Данные для преамбулы
    /// </summary>
    byte[] Preamble { get; set; }

    /// <summary>
    /// Задержка после преамбулы, в мс
    /// </summary>
    int DelayAfterPreamble { get; set; }

    /// <summary>
    /// Тип расположения байт в устройстве
    /// </summary>
    DeviceEndianess Endianess { get; }

    /// <summary>
    /// Происходит ли сейчас получение 
    /// </summary>
    bool IsReceive { get; }

    /// <summary>
    /// Происходит ли сейчас отправка
    /// </summary>
    bool IsTransmit { get; }

    /// <summary>
    /// Происходит ли запрос данных
    /// </summary>
    bool IsRequesting { get; }

    /// <summary>
    /// Чтение значения регистра с логированием
    /// </summary>
    /// <param name="rc">Настройки регистра</param>
    /// <param name="logger">Лог событий</param>
    /// <returns>Значение регистра или null если не удалось прочитать</returns>
    T? ReadRegisterValue<T>(RegisterConfiguration rc, ILogger logger);

    /// <summary>
    /// Чтение значения регистра асинхронно с логированием
    /// </summary>
    /// <typeparam name="T">Тип значения в C#</typeparam>
    /// <param name="rc">Настройки регистра</param>
    /// <param name="logger">Лог событий</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Задача на получение значения регистра или null если не удалось прочитать</returns>
    Task<T?> ReadRegisterValueAsync<T>(RegisterConfiguration rc, ILogger logger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Запись значения регистра
    /// </summary>
    /// <param name="rc">Настройки регистра</param>
    /// <param name="value">Значение для записи</param>
    /// <param name="logger">Лог событий</param>
    /// <returns>Успешно ли записалось значение регистра</returns>
    bool WriteRegisterValue(RegisterConfiguration rc, object value, ILogger logger);

    /// <summary>
    /// Запись значения регистра асинхронно
    /// </summary>
    /// <param name="rc">Настройки регистра</param>
    /// <param name="value">Значение для записи</param>
    /// <param name="logger">Лог событий</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Задача на ожидание результата записи значения регистра</returns>
    Task<bool> WriteRegisterValueAsync(RegisterConfiguration rc, object value, ILogger logger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сделать запрос по протоколу Modbus
    /// </summary>
    /// <param name="request">Тип запроса</param>
    /// <param name="logger">Лог событий</param>
    /// <returns>Готовый ответ</returns>
    ModbusBaseResponse? DoRequest(ModbusBaseRequest request, ILogger logger);

    /// <summary>
    /// Сделать запрос по протоколу Modbus асинхронно
    /// </summary>
    /// <param name="request">Тип запроса</param>
    /// <param name="logger">Лог событий</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Готовый ответ</returns>
    Task<ModbusBaseResponse?> DoRequestAsync(ModbusBaseRequest request, ILogger logger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Прочитать блоки регистров непрерывно насколько это возможно (Только для устройств поддерживающих такой тип чтения)
    /// </summary>
    /// <param name="registers">Регистры для чтения</param>
    /// <param name="modbusFunction">Выбранная функция чтения</param>
    /// <param name="logger">Логгер событий</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Успешно ли удалось прочитать значения всех блоков</returns>
    Task<bool> ReadRegistersBlocksAsync(IList<Register> registers, ModbusFunction modbusFunction, ILogger logger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Записать блоки регистров непрерывно насколько это возможно (Только для устройств поддерживающих такой тип записи)
    /// </summary>
    /// <param name="registers">Регистры для записи</param>
    /// <param name="logger">Логгер событий</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Успешно ли записались значения всех блоков</returns>
    Task<bool> WriteRegistersBlocksAsync(IList<Register> registers, ILogger logger, CancellationToken cancellationToken = default);

    /// <summary>
    /// Записать блоки регистров непрерывно насколько это возможно (Только для устройств поддерживающих такой тип записи)
    /// </summary>
    /// <param name="registers">Регистры для записи</param>
    /// <param name="logger">Логгер событий</param>
    /// <param name="actionBefore">Асинхронное действие перед каждой командой записи</param>
    /// <param name="actionAfter">Асинхронное действие после каждой команды записи</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    Task<bool> WriteRegistersBlocksAsync(IList<Register> registers, ILogger logger,
        Func<Task>? actionBefore = null, Func<Task>? actionAfter = null, CancellationToken cancellationToken = default);
}