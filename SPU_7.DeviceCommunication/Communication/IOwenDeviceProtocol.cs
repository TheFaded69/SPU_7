using SPU_7.DeviceCommunication.OwenProtocol;

namespace SPU_7.DeviceCommunication.Communication;

public interface IOwenDeviceProtocol
{
    /// <summary>
    /// Коммуникатор для протокола ОВЕН
    /// </summary>
    public ICommunicationChannel? CommunicationChannel { get; set; }

    /// <summary>
    /// Адрес в сети Овен
    /// </summary>
    int Address { get; set; }

    /// <summary>
    /// Количество попыток пересылки команды
    /// </summary>
    int RetryCount { get; set; }

    /// <summary>
    /// Получить данные параметра
    /// </summary>
    /// <param name="parameter">Параметр для запроса</param>
    /// <returns>Значение параметра или null, если не удалось прочитать</returns>
    public OwenResponse? GetParameterData(Enum parameter);

    /// <summary>
    /// Получить данные параметра асинхронно
    /// </summary>
    /// <param name="parameter">Параметр для запроса</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Задача на получение значения параметра или null, если не удалось прочитать</returns>
    public Task<OwenResponse?> GetParameterDataAsync(Enum parameter, CancellationToken cancellationToken = default);
}