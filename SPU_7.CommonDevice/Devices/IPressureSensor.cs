using SPU_7.DeviceCommunication.Communication;

namespace SPU_7.CommonDevice.Devices;
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
    public Task<float?> ReadPressureAsync(PressureType pressureType = PressureType.DefaultPressure, CancellationToken cancellationToken = default);
}