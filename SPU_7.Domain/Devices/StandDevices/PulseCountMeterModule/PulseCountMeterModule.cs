using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Extensions;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;

/// <summary>
/// МПКИ
/// </summary>
public class PulseCountMeterModule : ModbusUnitProcessor<PulseMeterCountModuleRegisterMap>, IPulseCountMeterModule 
{
    public PulseCountMeterModule(IModbusProcessor modbusProcessor, 
        IRegisterMapEnum<PulseMeterCountModuleRegisterMap> registerMap,
        int pulseMeterAddress)
        : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)pulseMeterAddress;
    }

    public int? PulseCountMeterModuleNumber { get; set; }
    
    public async Task<float?> ReadCurrentFrequencyAsync(ChannelNumber pulseCountMeterModuleChannelNumber)
    {
        return pulseCountMeterModuleChannelNumber switch
        {
            ChannelNumber.First => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.CurrentFrequencyFirstRegister),
            ChannelNumber.Second => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.CurrentFrequencySecondRegister),
            ChannelNumber.Third => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.CurrentFrequencyThirdRegister),
            ChannelNumber.Fourth => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.CurrentFrequencyFourthRegister),
            _ => throw new ArgumentOutOfRangeException(nameof(pulseCountMeterModuleChannelNumber), pulseCountMeterModuleChannelNumber, null)
        };
    }
    
    public async Task<float?> ReadAverageFrequencyAsync(ChannelNumber pulseCountMeterModuleChannelNumber)
    {
        return pulseCountMeterModuleChannelNumber switch
        {
            ChannelNumber.First => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.AverageFrequencyFirstRegister),
            ChannelNumber.Second => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.AverageFrequencySecondRegister),
            ChannelNumber.Third => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.AverageFrequencyThirdRegister),
            ChannelNumber.Fourth => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.AverageFrequencyFourthRegister),
            _ => throw new ArgumentOutOfRangeException(nameof(pulseCountMeterModuleChannelNumber), pulseCountMeterModuleChannelNumber, null)
        };
    }

    public async Task<bool> StartMeasurePulseCountAsync() =>
        await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandRegister,
            BitConverter.GetBytes((uint)1).SwapBytes().ToArray());

    public async Task<bool> StartMeasurePulseCountAsync(ChannelNumber channelNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> StopMeasurePulseCountAsync() =>
        await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandRegister,
            BitConverter.GetBytes((uint)0).SwapBytes().ToArray());

    public async Task<bool> StopMeasurePulseCountAsync(ChannelNumber channelNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<CommonCommandStatus?> GetCommonCommandStatusAsync()
    {
        var status = (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandStatusRegister);

        return status != null ? (CommonCommandStatus)status : null;
    }

    public async Task<float?> ReadPulsePeriodAsync(ChannelNumber channelNumber) => channelNumber switch
    {
        ChannelNumber.First => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel1),
        ChannelNumber.Second => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel2),
        ChannelNumber.Third => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel3),
        ChannelNumber.Fourth => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel4),
        _ => throw new ArgumentOutOfRangeException(nameof(channelNumber), channelNumber, "Не поддерживаемый номер канала у МПКИ")
    };

    public async Task<float?> ReadPulseDurationAsync(ChannelNumber channelNumber) => channelNumber switch
        {
            ChannelNumber.First => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulseDurationRegisterChannel1),
            ChannelNumber.Second => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulseDurationRegisterChannel2),
            ChannelNumber.Third => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulseDurationRegisterChannel3),
            ChannelNumber.Fourth => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulseDurationRegisterChannel4),
            _ => throw new ArgumentOutOfRangeException(nameof(channelNumber), channelNumber, "Не поддерживаемый номер канала у МПКИ")
        };

    public async Task<bool> SetPulseCountMeterModuleChannelSettingsAsync(ChannelNumber channelNumber)
    {
        switch (channelNumber)
        {
            case ChannelNumber.First:
                await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                    BitConverter.GetBytes((uint)1).SwapBytes().ToArray());
                break;
            case ChannelNumber.Second:
                await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                    BitConverter.GetBytes((uint)2).SwapBytes().ToArray());
                break;

            case ChannelNumber.Third:
               await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                    BitConverter.GetBytes((uint)3).SwapBytes().ToArray());
               break;

            case ChannelNumber.Fourth:
               await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                    BitConverter.GetBytes((uint)4).SwapBytes().ToArray());
               break;

            default:
                throw new ArgumentOutOfRangeException(nameof(channelNumber), channelNumber,
                    "Не поддерживаемый номер канала у МПКИ");
        }
        //Применяем настройки
        return await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandRegister,
            BitConverter.GetBytes((uint)2).SwapBytes().ToArray());

        
    }

    public async Task<float?> ReadPulseCountAsync() => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulseCountRegister);
    public async Task<uint?> ReadControlRegisterAsync() => (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.ControlRegister);

    public async Task<bool> TurnOnControlBitControlRegisterAsync()
    {
        var controlRegister = await ReadControlRegisterAsync();

        controlRegister |= (1u << 4);

        return await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.ControlRegister,
            BitConverter.GetBytes((uint)controlRegister).SwapBytes().ToArray());
    }

    public async Task<bool> TurnOffControlBitControlRegisterAsync()
    {
        var controlRegister = await ReadControlRegisterAsync();

        controlRegister &= ~(1u << 4);

        return await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.ControlRegister,
            BitConverter.GetBytes((uint)controlRegister).SwapBytes().ToArray());
    }

    public async Task<bool> ResetPulseCountMeterAsync()
    {
        await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
            BitConverter.GetBytes((uint)0).SwapBytes().ToArray());
        
        return await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandRegister,
            BitConverter.GetBytes((uint)2).SwapBytes().ToArray());
    }

    public async Task<float?> ReadMeasureTimeAsync() => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.MeasureTimeRegister);
}