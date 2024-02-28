namespace SPU_7.Domain.Devices.StandDevices.PulseMeter;

public interface IPulseMeter2Channel
{
    /// <summary>
    /// Прочитать сетевой адрес модуля. Необходимое условие - модуль должен быть
    /// один на шине Modbus, т.к. запрос делается на широковещательный адрес 0!!!
    /// </summary>
    Task<uint?> GetNetAddressAsync();

    /// <summary>
    /// Записать новый сетевой адрес модуля.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    Task<bool> SetNetAddressAsync(byte address);

    /// <summary>
    /// Прочитать имя прошивки (платы)
    /// </summary>
    Task<string> GetFirmwareNameAsync();

    /// <summary>
    /// Прочитать версию метролог. значимого ПО
    /// </summary>
    Task<float?> GetMzpoVersionAsync();

    /// <summary>
    /// Прочитать версию метролог. незначимого ПО
    /// </summary>
    Task<float?> GetMnzpoVersionAsync();

    /// <summary>
    /// Прочитать значение регистра управления
    /// </summary>
    Task<uint?> GetControlRegisterAsync();

    /// <summary>
    /// Выключить LCD экран
    /// </summary>
    Task<bool> DisableLcdAsync();

    /// <summary>
    /// Включить LCD экран
    /// </summary>
    Task<bool> EnableLcdAsync();

    /// <summary>
    /// Получить частоту
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetFrequencyAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить корректированную частоту
    /// </summary>
    /// <param name="channel">канал</param>
    Task<float?> GetFrequencyCorrectedAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить усреднённую частоту
    /// </summary>
    /// <param name="channel">канал</param>
    Task<float?> GetAverageFrequencyAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить значение регистра управления подканалами
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetSubChannelsControlRegisterAsync(PulseMeterChannel channel);

    /// <summary>
    /// Записать значение в регистр управления подканалами
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="value">значение</param>
    Task<bool> SetSubChannelsControlRegisterAsync(PulseMeterChannel channel, uint value);

    /// <summary>
    /// Получить значение регистра режима
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetModeRegisterAsync(PulseMeterChannel channel);

    /// <summary>
    /// Записать значение в регистр режима
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="value">значение</param>
    Task<bool> SetModeRegisterAsync(PulseMeterChannel channel, uint value);

    /// <summary>
    /// Установить режим канала модуля
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="mode">режим</param>
    Task<bool> SetChannelModeAsync(PulseMeterChannel channel, PulseMeter2ChannelMode mode);

    /// <summary>
    /// Получить системное время
    /// </summary>
    Task<uint?> GetSystemTimeAsync();

    /// <summary>
    /// Записать системное время
    /// </summary>
    Task<bool> SetSystemTimeAsync(uint value);

    /// <summary>
    /// Получить макс. количество импульсов в измерении
    /// </summary>
    Task<uint?> GetPulseAmountMaxAsync();

    /// <summary>
    /// Записать макс. количество импульсов в измерении
    /// </summary>
    Task<bool> SetPulseAmountMaxAsync(ushort value);

    /// <summary>
    /// Запуск измерения периода
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="periodAmount">количество измеряемых периодов</param>
    Task<bool> StartPeriodMeasureAsync(PulseMeterChannel channel, uint periodAmount);

    /// <summary>
    /// Получить статус канала
    /// </summary>
    /// <param name="channel">канал</param>
    Task<PulseMeter2ChannelState> GetChannelStatusAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить время начала измерения (ms)
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetStartMeasureTimeAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить время окончания измерения (ms)
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetEndMeasureTimeAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить количество измерений (ms)
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetMeasurementAmountAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить текущее значение периода (разрешение 4 us), ms
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetCurrentPeriodAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить среднее значение периода (разрешение 4 us), ms
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetAveargePeriodAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить корректированное среднее значение периода, ms
    /// </summary>
    /// <param name="channel">канал</param>
    Task<float?> GetCorrectedAveargePeriodAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить запускаемый счётчик импульсов
    /// </summary>
    /// <param name="channel">канал</param>
    Task<uint?> GetLaunchCounterAsync(PulseMeterChannel channel);

    /// <summary>
    /// Получить время измерения кол-ва импульсов, ms
    /// </summary>
    Task<float?> GetMeasureCounterTimeAsync();

    byte ModuleAddress { get; set; }
}