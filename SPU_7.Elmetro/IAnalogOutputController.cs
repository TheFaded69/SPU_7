using DeviceCommunication.Communication;

namespace DeviceCommunication.Devices;
public interface IAnalogOutputController : IDevice
{
    /// <summary>
    /// Количество аналоговых выходов
    /// </summary>
    int AnalogOutputCount { get; }

    #region Синхронные методы

    /// <summary>
    /// Получить значение заданное на выходе
    /// </summary>
    /// <param name="outputIndex">Индекс выхода</param>
    /// <returns>Значение регистра</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    public float? GetOutputValue(int outputIndex);

    /// <summary>
    /// Задать значение на выходе
    /// </summary>
    /// <param name="outputIndex">Индекс выхода</param>
    /// <param name="value">Значение регистра</param>
    /// <returns>Выполнена ли операция успешно</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    public bool SetOutputValue(int outputIndex, float value);

    #endregion

    #region Асинхронные методы

    /// <summary>
    /// Получить значение заданное на выходе асинхронно
    /// </summary>
    /// <param name="outputIndex">Индекс выхода</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Значение регистра></returns>
    public Task<float?> GetOutputValueAsync(int outputIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Задать значение на выходе асинхронно
    /// </summary>
    /// <param name="outputIndex">Индекс выхода</param>
    /// <param name="value">Значение регистра</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Выполнена ли операция успешно</returns>
    public Task<bool> SetOutputValueAsync(int outputIndex, float value, CancellationToken cancellationToken = default);

    #endregion
}