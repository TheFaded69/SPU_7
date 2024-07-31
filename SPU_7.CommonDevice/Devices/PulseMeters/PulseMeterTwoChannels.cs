using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;
using SPU_7.DeviceCommunication.Modbus.Request;

namespace SPU_7.CommonDevice.Devices.PulseMeters;
/// <summary>
/// БИПЧ с двумя каналами
/// </summary>
public class PulseMeterTwoChannels : ModbusDevice, IModbusDevice
{
    public PulseMeterTwoChannels(ICommunicationChannel? communicationChannel = null)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<PMTC_RegisterMap>(), DeviceEndianess.CDAB)
    {
        Logger = LogManager.GetLogger(nameof(PulseMeterTwoChannels));
    }

    #region Получение параметров из устройства

    /// <summary>
    /// Прочитать сетевой адрес модуля. Необходимое условие - модуль должен быть
    /// один на шине Modbus, т.к. запрос делается на широковещательный адрес!!!
    /// </summary>
    public uint? GetNetworkAddress()
    {
        var oldAddress = ModbusProtocol.Address;
        ModbusProtocol.Address = 0;
        var maddr = GetParameterValue<uint?>(PMTC_RegisterMap.NetworkAddress);
        ModbusProtocol.Address = oldAddress;
        return maddr;
    }

    /// <summary>
    /// Прочитать сетевой адрес модуля. Необходимое условие - модуль должен быть
    /// один на шине Modbus, т.к. запрос делается на широковещательный адрес!!!
    /// </summary>
    public async Task<uint?> GetNetworkAddressAsync()
    {
        var oldAddress = ModbusProtocol.Address;
        ModbusProtocol.Address = 0;
        var maddr = await GetParameterValueAsync<uint?>(PMTC_RegisterMap.NetworkAddress);
        ModbusProtocol.Address = oldAddress;
        return maddr;
    }

    /// <summary>
    /// Получить название прошивки платы
    /// </summary>
    public string? GetFirmwareString() => GetParameterValue<string?>(PMTC_RegisterMap.FirmwareString);

    /// <summary>
    /// Получить название прошивки платы асинхронно
    /// </summary>
    public Task<string?> GetFirmwareStringAsync() => GetParameterValueAsync<string?>(PMTC_RegisterMap.FirmwareString);

    /// <summary>
    /// Получить версию метрологически значимого ПО
    /// </summary>
    public float? GetMetrologySoftwareVersion() => GetParameterValue<float?>(PMTC_RegisterMap.MetrologySoftwareVersion);

    /// <summary>
    /// Получить версию метрологически значимого ПО асинхронно
    /// </summary>
    public Task<float?> GetMetrologySoftwareVersionAsync() => GetParameterValueAsync<float?>(PMTC_RegisterMap.MetrologySoftwareVersion);

    /// <summary>
    /// Получить версию ПО
    /// </summary>
    public float? GetSoftwareVersion() => GetParameterValue<float?>(PMTC_RegisterMap.SoftwareVersion);

    /// <summary>
    /// Получить версию ПО асинхронно
    /// </summary>
    public Task<float?> GetSoftwareVersionAsync() => GetParameterValueAsync<float?>(PMTC_RegisterMap.SoftwareVersion);

    /// <summary>
    /// Получить системное время
    /// </summary>
    public uint? GetSystemTime() => GetParameterValue<uint?>(PMTC_RegisterMap.SystemTime);

    /// <summary>
    /// Получить системное время асинхронно
    /// </summary>
    public Task<uint?> GetSystemTimeAsync() => GetParameterValueAsync<uint?>(PMTC_RegisterMap.SystemTime);

    /// <summary>
    /// Получить значение регистра управления
    /// </summary>
    public uint? GetControlRegister() => GetParameterValue<uint?>(PMTC_RegisterMap.ControlRegister);

    /// <summary>
    /// Получить значение регистра управления асинхронно
    /// </summary>
    public Task<uint?> GetControlRegisterAsync() => GetParameterValueAsync<uint?>(PMTC_RegisterMap.ControlRegister);

    /// <summary>
    /// Получить частоту на выбранном канале
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetFrequency(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.FrequencyChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.FrequencyChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить частоту на выбранном канале асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetFrequencyAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.FrequencyChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.FrequencyChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить частоту на выбранном канале скорректированную
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetCorrectedFrequency(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.FrequencyCorrectedChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.FrequencyCorrectedChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить частоту на выбранном канале скорректированную асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetCorrectedFrequencyAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.FrequencyCorrectedChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.FrequencyCorrectedChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить среднюю частоту на выбранном канале
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetAverageFrequency(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.AverageFrequencyChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.AverageFrequencyChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить среднюю частоту на выбранном канале асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetAverageFrequencyAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.AverageFrequencyChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.AverageFrequencyChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить значение регистра управления подканалами
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetSubChannelControlRegister(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.SubChannelsControlChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.SubChannelsControlChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить значение регистра управления подканалами асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetSubChannelControlRegisterAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.SubChannelsControlChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.SubChannelsControlChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить значение регистра режима
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetMode(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.ModeChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.ModeChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить значение регистра режима асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetModeAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.ModeChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.ModeChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить режим канала
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public PulseMeterChannelMode? GetChannelMode(PulseMeterChannel channel) => GetMode(channel) is uint cm ? (PulseMeterChannelMode)(cm & 0x07) : null;

    /// <summary>
    /// Получить режим канала асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public async Task<PulseMeterChannelMode?> GetChannelModeAsync(PulseMeterChannel channel) => await GetModeAsync(channel) is uint cm ? (PulseMeterChannelMode)(cm & 0x07) : null;

    /// <summary>
    /// Получить максимальное кол-во импульсов
    /// </summary>
    public uint? GetMaxImpulseCount() => GetParameterValue<uint?>(PMTC_RegisterMap.MaxImpulseCount);

    /// <summary>
    /// Получить максимальное кол-во импульсов асинхронно
    /// </summary>
    public Task<uint?> GetMaxImpulseCountAsync() => GetParameterValueAsync<uint?>(PMTC_RegisterMap.MaxImpulseCount);

    /// <summary>
    /// Получить состояние канала
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public PulseMeterChannelState GetChannelStatus(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.StatusChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.StatusChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    } is uint cs ? (PulseMeterChannelState)cs : PulseMeterChannelState.Unknown;

    /// <summary>
    /// Получить состояние канала асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public async Task<PulseMeterChannelState> GetChannelStatusAsync(PulseMeterChannel channel)  => await (channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.StatusChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.StatusChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    }) is uint cs ? (PulseMeterChannelState)cs : PulseMeterChannelState.Unknown;

    /// <summary>
    /// Получить кол-во замеров канала
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetMeasureCount(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.MeasureCountChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.MeasureCountChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить кол-во замеров канала асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetMeasureCountAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.MeasureCountChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.MeasureCountChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить время начала измерения канала
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetStartMeasureTime(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.BeginMeasureTimeChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.BeginMeasureTimeChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить время начала измерения канала асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetStartMeasureTimeAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.BeginMeasureTimeChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.BeginMeasureTimeChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить время окончания измерения канала
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetEndMeasureTime(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.EndMeasureTimeChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.EndMeasureTimeChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить время окончания измерения канала асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetEndMeasureTimeAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.EndMeasureTimeChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.EndMeasureTimeChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить значение периода измерения канала (Разрешение 4 мкс), мс
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetCurrentPeriod(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.CurrentPeriodChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.CurrentPeriodChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить значение периода измерения канала асинхронно (Разрешение 4 мкс), мс
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetCurrentPeriodAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.CurrentPeriodChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.CurrentPeriodChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить среднее значение периода для канала (Разрешение 4 мкс), мс
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetAveragePeriod(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.AveragePeriodChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.AveragePeriodChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить среднее значение периода для канала асинхронно (Разрешение 4 мкс), мс
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetAveragePeriodAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.AveragePeriodChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.AveragePeriodChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить среднее значение периода скорректированое для канала (Разрешение 4 мкс), мс
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public float? GetAvgCorrectedPeriod(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<float?>(PMTC_RegisterMap.AverageCorrectedPeriodChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<float?>(PMTC_RegisterMap.AverageCorrectedPeriodChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить среднее значение периода скорректированое для канала асинхронно (Разрешение 4 мкс), мс
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<float?> GetAvgCorrectedPeriodAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<float?>(PMTC_RegisterMap.AverageCorrectedPeriodChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<float?>(PMTC_RegisterMap.AverageCorrectedPeriodChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить счётчик импульсов
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public uint? GetLaunchCounter(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<uint?>(PMTC_RegisterMap.LaunchCounterChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<uint?>(PMTC_RegisterMap.LaunchCounterChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить счётчик импульсов асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<uint?> GetLaunchCounterAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.LaunchCounterChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<uint?>(PMTC_RegisterMap.LaunchCounterChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить свободнобегущий счётчик импульсов
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public ushort? GetFreeRunningCounter(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValue<ushort?>(PMTC_RegisterMap.FreeRunningCounterChannel1),
        PulseMeterChannel.Channel2 => GetParameterValue<ushort?>(PMTC_RegisterMap.FreeRunningCounterChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить свободнобегущий счётчик импульсов асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<ushort?> GetFreeRunningCounterAsync(PulseMeterChannel channel) => channel switch
    {
        PulseMeterChannel.Channel1 => GetParameterValueAsync<ushort?>(PMTC_RegisterMap.FreeRunningCounterChannel1),
        PulseMeterChannel.Channel2 => GetParameterValueAsync<ushort?>(PMTC_RegisterMap.FreeRunningCounterChannel2),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Получить время измерения кол-ва импульсов
    /// </summary>
    public float? GetMeasuredTime() => GetParameterValue<float?>(PMTC_RegisterMap.CounterMeasureTime);

    /// <summary>
    /// Получить время измерения кол-ва импульсов асинхронно
    /// </summary>
    public Task<float?> GetMeasuredTimeAsync() => GetParameterValueAsync<float?>(PMTC_RegisterMap.CounterMeasureTime);

    #endregion

    #region Задание параметров устройства

    /// <summary>
    /// Записать новый сетевой адрес модуля
    /// </summary>
    /// <param name="address">Новый адрес</param>
    /// <returns>Удалось ли успешно задать новый адрес</returns>
    public bool SetNetworkAddress(byte address)
    {
        var result = SetParameterValue(PMTC_RegisterMap.NetworkAddress, (uint)address);
        if (result) ModbusProtocol.Address = address;
        return result;
    }

    /// <summary>
    /// Записать новый сетевой адрес модуля асинхронно
    /// </summary>
    /// <param name="address">Новый адрес</param>
    /// <returns>Удалось ли успешно задать новый адрес</returns>
    public async Task<bool> SetNetworkAddressAsync(byte address)
    {
        var result = await SetParameterValueAsync(PMTC_RegisterMap.NetworkAddress, (uint)address);
        if (result) ModbusProtocol.Address = address;
        return result;
    }

    /// <summary>
    /// Задать системное время
    /// </summary>
    /// <param name="time">Время в мс</param>
    public bool SetSystemTime(uint time) => SetParameterValue(PMTC_RegisterMap.SystemTime, time);

    /// <summary>
    /// Задать системное время асинхронно
    /// </summary>
    /// <param name="time">Время в мс</param>
    public Task<bool> SetSystemTimeAsync(uint time) => SetParameterValueAsync(PMTC_RegisterMap.SystemTime, time);

    /// <summary>
    /// Задать максимальное кол-во импульсов
    /// </summary>
    /// <param name="impulseCount">Максимальное кол-во импульсов за одно измерение</param>
    public bool SetMaxImpulseCount(uint impulseCount) => SetParameterValue(PMTC_RegisterMap.MaxImpulseCount, impulseCount);

    /// <summary>
    /// Задать максимальное кол-во импульсов асинхронно
    /// </summary>
    /// <param name="impulseCount">Максимальное кол-во импульсов за одно измерение</param>
    public Task<bool> SetMaxImpulseCountAsync(uint impulseCount) => SetParameterValueAsync(PMTC_RegisterMap.MaxImpulseCount, impulseCount);

    /// <summary>
    /// Задать значение регистра управления подканалами
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public bool SetSubChannelControlRegister(PulseMeterChannel channel, uint value) => channel switch
    {
        PulseMeterChannel.Channel1 => SetParameterValue(PMTC_RegisterMap.SubChannelsControlChannel1, value),
        PulseMeterChannel.Channel2 => SetParameterValue(PMTC_RegisterMap.SubChannelsControlChannel2, value),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Задать значение регистра управления подканалами асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<bool> SetSubChannelControlRegisterAsync(PulseMeterChannel channel, uint value) => channel switch
    {
        PulseMeterChannel.Channel1 => SetParameterValueAsync(PMTC_RegisterMap.SubChannelsControlChannel1, value),
        PulseMeterChannel.Channel2 => SetParameterValueAsync(PMTC_RegisterMap.SubChannelsControlChannel2, value),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Задать регистр режима измерения канала
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public bool SetMode(PulseMeterChannel channel, uint value) => channel switch
    {
        PulseMeterChannel.Channel1 => SetParameterValue(PMTC_RegisterMap.ModeChannel1, value),
        PulseMeterChannel.Channel2 => SetParameterValue(PMTC_RegisterMap.ModeChannel2, value),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Задать регистр режима измерения канала асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public Task<bool> SetModeAsync(PulseMeterChannel channel, uint value) => channel switch
    {
        PulseMeterChannel.Channel1 => SetParameterValueAsync(PMTC_RegisterMap.ModeChannel1, value),
        PulseMeterChannel.Channel2 => SetParameterValueAsync(PMTC_RegisterMap.ModeChannel2, value),
        _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
    };

    /// <summary>
    /// Задать режим измерения канала
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public bool SetChannelMode(PulseMeterChannel channel, PulseMeterChannelMode mode) =>
        GetMode(channel) is uint cm && SetMode(channel, cm & 0x1C00 | (uint)mode);

    /// <summary>
    /// Задать режим измерения канала асинхронно
    /// </summary>
    /// <param name="channel">Канал устройства</param>
    /// <exception cref="ArgumentOutOfRangeException">Значение перечисления каналов за пределами допустимого</exception>
    public async Task<bool> SetChannelModeAsync(PulseMeterChannel channel, PulseMeterChannelMode mode) =>
        await GetModeAsync(channel) is uint cm && await SetModeAsync(channel, cm & 0x1C00 | (uint)mode);

    #endregion

    #region Управление устройством

    /// <summary>
    /// Включить подсветку экрана
    /// </summary>
    public bool EnableLCD() => GetControlRegister() is uint cr && SetParameterValue(PMTC_RegisterMap.ControlRegister, cr | 0x1000);

    /// <summary>
    /// Включить подсветку экрана асинхронно
    /// </summary>
    public async Task<bool> EnableLCDAsync() => await GetControlRegisterAsync() is uint cr && await SetParameterValueAsync(PMTC_RegisterMap.ControlRegister, cr | 0x1000);

    /// <summary>
    /// Отключить подсветку экрана
    /// </summary>
    public bool DisableLCD() => GetControlRegister() is uint cr && SetParameterValue(PMTC_RegisterMap.ControlRegister, cr & ~0x1000);

    /// <summary>
    /// Отключить подсветку экрана асинхронно
    /// </summary>
    public async Task<bool> DisableLCDAsync() => await GetControlRegisterAsync() is uint cr && await SetParameterValueAsync(PMTC_RegisterMap.ControlRegister, cr & ~0x1000);

    /// <summary>
    /// Запуск измерения периода
    /// </summary>
    /// <param name="channel">Канал для измерения</param>
    /// <param name="periodCount">Кол-во периодов</param>
    public bool StartMeasure(PulseMeterChannel channel, uint periodCount)
    {
        if (GetMaxImpulseCount() is not uint icMax) return false;
        if (periodCount > icMax) periodCount = icMax;
        return channel switch
        {
            PulseMeterChannel.Channel1 => SetParameterValue(PMTC_RegisterMap.StartPeriodMeasureChannel1, periodCount),
            PulseMeterChannel.Channel2 => SetParameterValue(PMTC_RegisterMap.StartPeriodMeasureChannel2, periodCount),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
        };
    }

    /// <summary>
    /// Запуск измерения периода асинхронно
    /// </summary>
    /// <param name="channel">Канал для измерения</param>
    /// <param name="periodCount">Кол-во периодов</param>
    public async Task<bool> StartMeasureAsync(PulseMeterChannel channel, uint periodCount)
    {
        if (await GetMaxImpulseCountAsync() is not uint icMax) return false;
        if (periodCount > icMax) periodCount = icMax;
        return channel switch
        {
            PulseMeterChannel.Channel1 => await SetParameterValueAsync(PMTC_RegisterMap.StartPeriodMeasureChannel1, periodCount),
            PulseMeterChannel.Channel2 => await SetParameterValueAsync(PMTC_RegisterMap.StartPeriodMeasureChannel2, periodCount),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, ""),
        };
    }

    /// <summary>
    /// 
    /// </summary>
    public void SwitchMeasure()
    {
        if (ModbusProtocol.Address > 247) throw new ArgumentException("Modbus адрес за пределами допустимого!");
        var request = new WriteMultipleRegistersRequest((byte)ModbusProtocol.Address, 0x001C, 2, new byte[] { 0x00, 0x01, 0x00, 0x00 }, ModbusProtocol.Endianess);
        if (ModbusProtocol.CommunicationChannel == null) return;
        var oldRequestTimeout = ModbusProtocol.CommunicationChannel.RequestTimeout;
        ModbusProtocol.CommunicationChannel.RequestTimeout = 8000;
        try {
            ModbusProtocol.CommunicationChannel.TryProcessRequestAndResponse(() =>
            {
                ModbusProtocol.CommunicationChannel.Write(request.Data);
                return 0;
            });
        }
        finally {
            ModbusProtocol.CommunicationChannel.RequestTimeout = oldRequestTimeout;
        }
    }

    #endregion
}