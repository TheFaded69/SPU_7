using DeviceCommunication.Communication;

namespace DeviceCommunication.Devices;
public interface ITemperatureSensor : IDevice
{
    /// <summary>
    /// Получить температуру
    /// </summary>
    /// <returns></returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    float? GetTemperature();

    /// <summary>
    /// Получить температуру асинхронно
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<float?> GetTemperatureAsync(CancellationToken cancellationToken = default);
}