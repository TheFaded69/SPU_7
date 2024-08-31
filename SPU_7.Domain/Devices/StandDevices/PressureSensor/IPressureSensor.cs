namespace SPU_7.Domain.Devices.StandDevices.PressureSensor;

public interface IPressureSensor
{
    float? Pressure { get; set; }

    /// <summary>
    /// Считать давление с ДД
    /// </summary>
    /// <returns>Давление</returns>
    Task<float?> ReadPressureAsync();

    /// <summary>
    /// Сброс на ноль ДД
    /// </summary>
    /// <returns></returns>
    Task<bool> ResetToZeroAsync();
}