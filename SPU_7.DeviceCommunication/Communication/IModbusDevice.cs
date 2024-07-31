using SPU_7.DeviceCommunication.Modbus;

namespace SPU_7.DeviceCommunication.Communication;
public interface IModbusDevice : IDevice
{
    /// <summary>
    /// Реализация протокола Modbus
    /// </summary>
    IModbusDeviceProtocol ModbusProtocol { get; }

    /// <summary>
    /// Карта регистров
    /// </summary>
    IReadOnlyDictionary<Enum, Register> RegisterMap { get; }

    /// <summary>
    /// Получить значение указанного параметра
    /// </summary>
    /// <typeparam name="T">Тип конечного значения</typeparam>
    /// <param name="registerConfiguration">Настройки регистра</param>
    /// <returns>Значение регистра</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    T? GetParameterValue<T>(RegisterConfiguration registerConfiguration);

    /// <summary>
    /// Получить значение указанного параметра асинхронно
    /// </summary>
    /// <typeparam name="T">Тип конечного значения</typeparam>
    /// <param name="registerConfiguration">Настройки регистра</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Задача на получение значения регистра</returns>
    Task<T?> GetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, CancellationToken cancellationToken = default);

    /// <summary>
    /// Задать значение параметра
    /// </summary>
    /// <typeparam name="T">Тип передаваемого значения</typeparam>
    /// <param name="registerConfiguration">Настройки регистра</param>
    /// <param name="value">Значение регистра для записи</param>
    /// <returns></returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    bool SetParameterValue<T>(RegisterConfiguration registerConfiguration, T value) where T : notnull;

    /// <summary>
    /// Задать значение параметра асинхронно
    /// </summary>
    /// <typeparam name="T">Тип передаваемого значения</typeparam>
    /// <param name="registerConfiguration">Настройки регистра</param>
    /// <param name="value">Значение регистра для записи</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns></returns>
    Task<bool> SetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, T value, CancellationToken cancellationToken = default) where T : notnull;
}
