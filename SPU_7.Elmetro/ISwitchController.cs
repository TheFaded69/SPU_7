using DeviceCommunication.Communication;

namespace DeviceCommunication.Devices;
public interface ISwitchController : IDevice
{
    /// <summary>
    /// Количество выходов
    /// </summary>
    int OutputsCount { get; }

    #region Синхронные методы

    /// <summary>
    /// Включить все выходы
    /// </summary>
    /// <returns>Успешно ли произведена операция</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    bool EnableAll();

    /// <summary>
    /// Выключить все выходы
    /// </summary>
    /// <returns>Успешно ли произведена операция</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    bool DisableAll();

    /// <summary>
    /// Получить состояние выхода по индексу
    /// </summary>
    /// <param name="outputIndex">Индекс выхода прибора</param>
    /// <returns>Состояние входа</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    bool? GetOutputState(int outputIndex);

    /// <summary>
    /// Задать логическое значение выхода
    /// </summary>
    /// <param name="outputIndex">Индекс выхода прибора</param>
    /// <param name="value">Значение для задания</param>
    /// <returns>Успешно ли произведена операция</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    bool SetOutput(int outputIndex, bool value);

    /// <summary>
    /// Получить состояние выходов в качестве маски битов
    /// </summary>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    T? GetOutputsMask<T>() where T : struct;

    #endregion

    #region Асинхронные методы

    /// <summary>
    /// Включить все выходы асинхронно
    /// </summary>
    /// <returns>Успешно ли произведена операция</returns>
    Task<bool> EnableAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Выключить все выходы асинхронно
    /// </summary>
    /// <returns>Успешно ли произведена операция</returns>
    Task<bool> DisableAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить состояние выхода по индексу асинхронно
    /// </summary>
    /// <param name="outputIndex">Индекс выхода прибора</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Состояние входа</returns>
    Task<bool?> GetOutputStateAsync(int outputIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Задать логическое значение выхода асинхронно
    /// </summary>
    /// <param name="outputIndex">Индекс выхода прибора</param>
    /// <param name="value">Значение для задания</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Успешно ли произведена операция</returns>
    Task<bool> SetOutputAsync(int outputIndex, bool value, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Получить состояние выходов в качестве маски битов асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    Task<T?> GetOutputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct;

    /// <summary>
    /// Получить список масок состояния выходов асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    //Task<IList<T>?> GetOutputsMasksAsync<T>(CancellationToken cancellationToken = default);

    #endregion
}