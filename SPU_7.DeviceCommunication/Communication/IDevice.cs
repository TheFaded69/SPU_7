using NLog;

namespace SPU_7.DeviceCommunication.Communication;
public interface IDevice
{
    /// <summary>
    /// Уникальный идентификатор устройства
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Текущий протокол связи
    /// </summary>
    DeviceCommunicationProtocol CurrentProtocol { get; set; }

    /// <summary>
    /// Логгер устройства
    /// </summary>
    ILogger Logger { get; set; }

    /// <summary>
    /// Получить значение параметра
    /// </summary>
    /// <typeparam name="T">Тип представления параметра</typeparam>
    /// <param name="parameter">Перечисление параметра</param>
    /// <param name="convertData"></param>
    /// <returns></returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    T? GetParameterValue<T>(Enum parameter);

    /// <summary>
    /// Задать значение параметра
    /// </summary>
    /// <typeparam name="T">Тип представления параметра</typeparam>
    /// <param name="parameter">Перечисление параметра</param>
    /// <param name="convertData"></param>
    /// <returns></returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    bool SetParameterValue<T>(Enum parameter, T value) where T : notnull;

    /// <summary>
    /// Получить значение параметра асинхронно
    /// </summary>
    /// <typeparam name="T">Тип представления параметра</typeparam>
    /// <param name="parameter">Перечисление параметра</param>
    /// <param name="convertData"></param>
    /// <returns></returns>
    Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default);

    /// <summary>
    /// Задать значение параметра асинхронно
    /// </summary>
    /// <typeparam name="T">Тип представления параметра</typeparam>
    /// <param name="parameter">Перечисление параметра</param>
    /// <param name="convertData"></param>
    /// <returns></returns>
    Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default) where T : notnull;
}