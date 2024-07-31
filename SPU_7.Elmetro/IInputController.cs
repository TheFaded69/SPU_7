using DeviceCommunication.Communication;

namespace DeviceCommunication.Devices;
public interface IInputController : IDevice
{
    /// <summary>
    /// Количество входов
    /// </summary>
    public int InputsCount { get; }

    #region Синхронные методы

    /// <summary>
    /// Получить состояние входа по индексу
    /// </summary>
    /// <param name="inputIndex">Индекс входа прибора</param>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    bool? GetInputState(int inputIndex);

    /// <summary>
    /// Получить значения входов в качестве маски битов
    /// </summary>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    T? GetInputsMask<T>() where T : struct;

    #endregion

    #region Асинхронные методы

    /// <summary>
    /// Получить состояние входа по индексу асинхронно
    /// </summary>
    /// <param name="inputIndex">Индекс входа прибора</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    Task<bool?> GetInputStateAsync(int inputIndex, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получить состояние входов в качестве маски битов асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    Task<T?> GetInputsMaskAsync<T>(CancellationToken cancellationToken = default) where T : struct;

    /// <summary>
    /// Получить список масок состояния входов асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    //Task<IList<T>?> GetInputsMasksAsync<T>(CancellationToken cancellationToken = default);

    #endregion
}