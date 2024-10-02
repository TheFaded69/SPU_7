using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
using SPU_7.Domain.Extensions;

namespace SPU_7.Domain.Devices;

public interface IMasterDevice : IPressureSensorObservable, ITemperatureSensorObservable, IFlowObservable
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

    /// <summary>
    /// Считать текущий поток с помощью МПКИ
    /// </summary>
    /// <param name="pulseCountMeterModule">Модуль МПКИ</param>
    /// <param name="pulseCountMeterModuleChannelNumber">Номер канала МПКИ</param>
    /// <param name="pulseWeight"></param>
    /// <returns></returns>
    Task<float?> ReadFlowAsync(IPulseCountMeterModule? pulseCountMeterModule, int? pulseCountMeterModuleChannelNumber, float pulseWeight);
    
    float? GetPressureDifference();
    float? GetTemperature();

    float? GetFlow();
}