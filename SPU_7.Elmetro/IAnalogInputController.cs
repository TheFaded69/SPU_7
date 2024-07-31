using DeviceCommunication.Communication;

namespace DeviceCommunication.Devices;
public interface IAnalogInputController : IDevice
{
    /// <summary>
    /// Количество аналоговых входов
    /// </summary>
    int AnalogInputsCount { get; }

    /// <summary>
    /// Получить значение на входе
    /// </summary>
    /// <param name="inputIndex">Индекс входа</param>
    /// <returns>Значение регистра</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    public float? GetInputValue(int inputIndex);

    /// <summary>
    /// Получить значение на входе асинхронно
    /// </summary>
    /// <param name="inputIndex">Индекс входа</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Значение регистра></returns>
    public Task<float?> GetInputValueAsync(int inputIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить значения на входах
    /// </summary>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    public IList<float>? GetInputValues();

    /// <summary>
    /// Получить значения на входах асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    public Task<IList<float>?> GetInputValuesAsync(CancellationToken cancellationToken = default);
}