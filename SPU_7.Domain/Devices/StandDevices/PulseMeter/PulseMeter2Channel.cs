using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Extensions;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.PulseMeter;

/// <summary>
/// Блок измерения периода и частоты на 2 канала
/// </summary>
public class PulseMeter2Channel : ModbusUnitProcessor<PulseMeter2ChannelRegisterMap>, IPulseMeter2Channel
{
    public PulseMeter2Channel(IModbusProcessor modbusProcessor, IRegisterMapEnum<PulseMeter2ChannelRegisterMap> registerMap, int pulseMeterAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)pulseMeterAddress;
    }

    /// <summary>
    /// Прочитать сетевой адрес модуля. Необходимое условие - модуль должен быть
    /// один на шине Modbus, т.к. запрос делается на широковещательный адрес 0!!!
    /// </summary>
    public async Task<uint?> GetNetAddressAsync()
    {
        // запоминаем текущий адрес
        var moduleAddress = ModuleAddress;
        // меняем на 0
        ModuleAddress = 0;
        // получаем адрес устройства
        var netAddress = await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.NetAddressRegister);
        // восстанавливаем
        ModuleAddress = moduleAddress;
        return (uint?) netAddress;
    }

    /// <summary>
    /// Записать новый сетевой адрес модуля.
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public async Task<bool> SetNetAddressAsync(byte address)
    {
        var result = await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.NetAddressRegister, BitConverter.GetBytes((uint) address).SwapBytes());
        if (result)
        {
            ModuleAddress = address;
        }
        return result;
    }

    /// <summary>
    /// Прочитать имя прошивки (платы)
    /// </summary>
    public async Task<string> GetFirmwareNameAsync() => (string) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.FirmwareName);

    /// <summary>
    /// Прочитать версию метролог. значимого ПО
    /// </summary>
    public async Task<float?> GetMzpoVersionAsync() => (float) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MzpoVersion);

    /// <summary>
    /// Прочитать версию метролог. незначимого ПО
    /// </summary>
    public async Task<float?> GetMnzpoVersionAsync() => (float) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MnzpoVersion);

    /// <summary>
    /// Прочитать значение регистра управления
    /// </summary>
    public async Task<uint?> GetControlRegisterAsync() => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.ControlRegister);

    /// <summary>
    /// Выключить LCD экран
    /// </summary>
    public async Task<bool> DisableLcdAsync()
    {
        var controlRegister = await GetControlRegisterAsync();
        if (controlRegister == null) return false;
        controlRegister &= ~(uint)0x1000;
        return await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.ControlRegister, BitConverter.GetBytes((uint)controlRegister).SwapBytes());
    }

    /// <summary>
    /// Включить LCD экран
    /// </summary>
    public async Task<bool> EnableLcdAsync()
    {
        var controlRegister = await GetControlRegisterAsync();
        if (controlRegister == null) return false;
        controlRegister |= 0x1000;
        return await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.ControlRegister, BitConverter.GetBytes((uint)controlRegister).SwapBytes());
    }

    /// <summary>
    /// Получить частоту
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetFrequencyAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.FrequencyChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.FrequencyChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить корректированную частоту
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<float?> GetFrequencyCorrectedAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (float?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.FrequencyCorrectedChannel1),
            PulseMeterChannel.Channel2 => (float?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.FrequencyCorrectedChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    
    /// <summary>
    /// Получить усреднённую частоту
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<float?> GetAverageFrequencyAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (float?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.AverageFrequencyChannel1),
            PulseMeterChannel.Channel2 => (float?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.AverageFrequencyChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить значение регистра управления подканалами
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetSubChannelsControlRegisterAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.SubChannelsControlChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.SubChannelsControlChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Записать значение в регистр управления подканалами
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="value">значение</param>
    public async Task<bool> SetSubChannelsControlRegisterAsync(PulseMeterChannel channel, uint value) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.SubChannelsControlChannel1, BitConverter.GetBytes(value).SwapBytes()),
            PulseMeterChannel.Channel2 => await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.SubChannelsControlChannel2, BitConverter.GetBytes(value).SwapBytes()),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить значение регистра режима
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetModeRegisterAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.ModeChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.ModeChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Записать значение в регистр режима
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="value">значение</param>
    public async Task<bool> SetModeRegisterAsync(PulseMeterChannel channel, uint value) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.ModeChannel1, BitConverter.GetBytes(value).SwapBytes()),
            PulseMeterChannel.Channel2 => await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.ModeChannel2, BitConverter.GetBytes(value).SwapBytes()),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Установить режим канала модуля
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="mode">режим</param>
    public async Task<bool> SetChannelModeAsync(PulseMeterChannel channel, PulseMeter2ChannelMode mode)
    {
        var modeRegisterValue = await GetModeRegisterAsync(channel);
        if (modeRegisterValue == null) return false;
        modeRegisterValue &= 0x1C00;
        var register = channel switch
        {
            PulseMeterChannel.Channel1 => PulseMeter2ChannelRegisterMap.ModeChannel1,
            PulseMeterChannel.Channel2 => PulseMeter2ChannelRegisterMap.ModeChannel2,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
        var value = mode switch
        {
            PulseMeter2ChannelMode.Off => (uint) modeRegisterValue,
            PulseMeter2ChannelMode.MeasureEvery1PeriodFall => (uint) modeRegisterValue | 0x0002,
            PulseMeter2ChannelMode.MeasureEvery1PeriodRise => (uint) modeRegisterValue | 0x0003,
            PulseMeter2ChannelMode.MeasureEvery4PeriodRise => (uint) modeRegisterValue | 0x0004,
            PulseMeter2ChannelMode.MeasureEvery16PeriodRise => (uint) modeRegisterValue | 0x0005,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
        return await WriteRegisterAsync(register, BitConverter.GetBytes(value).SwapBytes());
    }

    /// <summary>
    /// Получить системное время
    /// </summary>
    public async Task<uint?> GetSystemTimeAsync() => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.SystemTime);

    /// <summary>
    /// Записать системное время
    /// </summary>
    public async Task<bool> SetSystemTimeAsync(uint value) => 
        await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.SystemTime, BitConverter.GetBytes(value).SwapBytes());

    /// <summary>
    /// Получить макс. количество импульсов в измерении
    /// </summary>
    public async Task<uint?> GetPulseAmountMaxAsync() => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.PulseAmountMax);

    /// <summary>
    /// Записать макс. количество импульсов в измерении
    /// </summary>
    public async Task<bool> SetPulseAmountMaxAsync(ushort value) => 
        await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.PulseAmountMax, BitConverter.GetBytes((uint)value).SwapBytes());

    /// <summary>
    /// Запуск измерения периода
    /// </summary>
    /// <param name="channel">канал</param>
    /// <param name="periodAmount">количество измеряемых периодов</param>
    public async Task<bool> StartPeriodMeasureAsync(PulseMeterChannel channel, uint periodAmount)
    {
        var pulseAmountMax = await GetPulseAmountMaxAsync();
        if (pulseAmountMax == null) return false;
        if (periodAmount > pulseAmountMax) periodAmount = (uint) pulseAmountMax;
        return channel switch
        {
            PulseMeterChannel.Channel1 => await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.PeriodMeasureStartChannel1, BitConverter.GetBytes(periodAmount).SwapBytes()),
            PulseMeterChannel.Channel2 => await WriteRegisterAsync(PulseMeter2ChannelRegisterMap.PeriodMeasureStartChannel2, BitConverter.GetBytes(periodAmount).SwapBytes()),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
    }

    /// <summary>
    /// Получить статус канала
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<PulseMeter2ChannelState> GetChannelStatusAsync(PulseMeterChannel channel)
    {
        var state = channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.StatusChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.StatusChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };
        if (state == null || !Enum.IsDefined(typeof(PulseMeter2ChannelState), state)) return PulseMeter2ChannelState.None;
        return (PulseMeter2ChannelState) (uint) state;
    }

    /// <summary>
    /// Получить время начала измерения (ms)
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetStartMeasureTimeAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MeasureTimeBeginChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MeasureTimeBeginChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить время окончания измерения (ms)
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetEndMeasureTimeAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MeasureTimeEndChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MeasureTimeEndChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить количество измерений (ms)
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetMeasurementAmountAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MeasureAmountChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MeasureAmountChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить текущее значение периода (разрешение 4 us), ms
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetCurrentPeriodAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.CurrentPeriodChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.CurrentPeriodChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить среднее значение периода (разрешение 4 us), ms
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetAveargePeriodAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.AveragePeriodChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.AveragePeriodChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить корректированное среднее значение периода, ms
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<float?> GetCorrectedAveargePeriodAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (float?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.AveragePeriodCorrectedChannel1),
            PulseMeterChannel.Channel2 => (float?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.AveragePeriodCorrectedChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить запускаемый счётчик импульсов
    /// </summary>
    /// <param name="channel">канал</param>
    public async Task<uint?> GetLaunchCounterAsync(PulseMeterChannel channel) =>
        channel switch
        {
            PulseMeterChannel.Channel1 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.LaunchCounterChannel1),
            PulseMeterChannel.Channel2 => (uint?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.LaunchCounterChannel2),
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, null)
        };

    /// <summary>
    /// Получить время измерения кол-ва импульсов, ms
    /// </summary>
    public async Task<float?> GetMeasureCounterTimeAsync() => (float?) await ReadRegisterAsync(PulseMeter2ChannelRegisterMap.MeasureCounterTime);
}