namespace SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;

/// <summary>
/// МПКИ (аналог БИПЧа с большим функционалом)
/// </summary>
public interface IPulseCountMeterModule
{
    /// <summary>
    /// Запустить счет импульсов на всех каналах
    /// </summary>
    /// <param name="channelNumber"></param>
    /// <returns></returns>
    Task<bool> StartMeasurePulseCountAsync();
    
    /// <summary>
    /// Запустить счет импульсов на канале
    /// </summary>
    /// <param name="channelNumber"></param>
    /// <returns></returns>
    Task<bool> StartMeasurePulseCountAsync(ChannelNumber channelNumber);
    
    /// <summary>
    /// Остановить счет импульсов на всех каналах
    /// </summary>
    /// <param name="channelNumber"></param>
    /// <returns></returns>
    Task<bool> StopMeasurePulseCountAsync();
    
    /// <summary>
    /// Остановить счет импульсов на канале
    /// </summary>
    /// <param name="channelNumber"></param>
    /// <returns></returns>
    Task<bool> StopMeasurePulseCountAsync(ChannelNumber channelNumber);

    /// <summary>
    /// Получить статус командного широковещательного регистра
    /// </summary>
    /// <returns></returns>
    Task<CommonCommandStatus?> GetCommonCommandStatusAsync();
    
    /// <summary>
    /// Считать количество импульсов на канале
    /// </summary>
    /// <param name="channelNumber">Номер канала</param>
    /// <returns></returns>
    Task<uint?> ReadPulsePeriodAsync(ChannelNumber channelNumber);
    
    /// <summary>
    /// Считать длительность импульса на канале
    /// </summary>
    /// <param name="channelNumber">Номер канала</param>
    /// <returns></returns>
    Task<float?> ReadPulseDurationAsync(ChannelNumber channelNumber);

    /// <summary>
    /// Выбрать профиль настроек на нужный канал
    /// </summary>
    /// <param name="channelNumber"></param>
    /// <returns></returns>
    Task<bool> SetPulseCountMeterModuleChannelSettingsAsync(ChannelNumber channelNumber);
}