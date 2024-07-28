namespace SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;

/// <summary>
/// МПКИ (аналог БИПЧа с большим функционалом)
/// </summary>
public interface IPulseCountMeterModule
{
    /// <summary>
    /// Запустить счет импульсов на канале
    /// </summary>
    /// <param name="channelNumber"></param>
    /// <returns></returns>
    Task<bool> StartMeasurePulseCountAsync(ChannelNumber channelNumber);
    
    /// <summary>
    /// Остановить счет импульсов на канале
    /// </summary>
    /// <param name="channelNumber"></param>
    /// <returns></returns>
    Task<bool> StopMeasurePulseCountAsync(ChannelNumber channelNumber);
    
    
}