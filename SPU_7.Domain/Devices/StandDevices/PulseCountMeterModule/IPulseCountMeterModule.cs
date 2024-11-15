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
    Task<float?> ReadPulsePeriodAsync(ChannelNumber channelNumber);
    
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<float?> ReadPulseCountAsync();

    /// <summary>
    /// Считать регистр управления
    /// </summary>
    /// <returns></returns>
    Task<uint?> ReadControlRegisterAsync();

    /// <summary>
    /// Включить бит котроль в регистре управления 
    /// </summary>
    /// <returns></returns>
    Task<bool> TurnOnControlBitControlRegisterAsync();
    
    /// <summary>
    /// Выключить бит котроль в регистре управления 
    /// </summary>
    /// <returns></returns>
    Task<bool> TurnOffControlBitControlRegisterAsync();

    /// <summary>
    /// Сброс модуля
    /// </summary>
    /// <returns></returns>
    Task<bool> ResetPulseCountMeterAsync();

    /// <summary>
    /// Считать время измерения (мс)
    /// </summary>
    /// <returns></returns>
    Task<float?> ReadMeasureTimeAsync();

    /// <summary>
    /// Номер модуля
    /// </summary>
    int? PulseCountMeterModuleNumber { get; set; }

    /// <summary>
    /// Считать частоту канала, измеряемую в фоне
    /// </summary>
    /// <param name="pulseCountMeterModuleChannelNumber"></param>
    /// <returns></returns>
    Task<float?> ReadCurrentFrequencyAsync(ChannelNumber pulseCountMeterModuleChannelNumber);

    /// <summary>
    /// Считать усредненную частоту канала за 10 сек, измеряемую в фоне
    /// </summary>
    /// <param name="pulseCountMeterModuleChannelNumber"></param>
    /// <returns></returns>
    Task<float?> ReadAverageFrequencyAsync(ChannelNumber pulseCountMeterModuleChannelNumber);

    Task<CommonCommandStatus?> ReadPulseCountMeterStatusAsync(ChannelNumber pulseCountMeterModuleChannelNumber);
    Task<bool?> ReadPulseCountMeterMeasureStatusAsync(ChannelNumber channel);
}