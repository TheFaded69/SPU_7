using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
using SPU_7.Domain.Extensions;

namespace SPU_7.Domain.Devices;

public interface IMasterDevice : IPressureSensorObservable, ITemperatureSensorObservable
{
    /// <summary>
    /// Считать давление с ДД привязанного к позиции СГ
    /// </summary>
    /// <returns></returns>
    Task<float?> ReadPressureAsync();
    
    /// <summary>
    /// Считать температуру с ДT привязанного к позиции СГ
    /// </summary>
    /// <returns></returns>
    Task<float?> ReadTemperatureAsync();

    //Task<float?> ReadCurrentFlow();
    float? GetPressureDifference();
    float? GetTemperature();
}