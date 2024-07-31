using DeviceCommunication.Communication;

namespace DeviceCommunication.Devices;
public interface IPressureSensor : IDevice
{
    /// <summary>
    /// Получить давление
    /// </summary>
    /// <param name="pressureType">Тип давления</param>
    /// <returns></returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    public float? GetPressure(PressureType pressureType = PressureType.DefaultPressure);

    /// <summary>
    /// Получить давление асинхронно
    /// </summary>
    /// <param name="pressureType">Тип давления</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<float?> GetPressureAsync(PressureType pressureType = PressureType.DefaultPressure, CancellationToken cancellationToken = default);
}