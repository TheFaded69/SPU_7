using SPU_7.Domain.Modbus;
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


    public async Task<bool> StartMeasurePulseCountAsync() =>
        await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandRegister,
            BitConverter.GetBytes((uint)1).Reverse().ToArray());

    public async Task<bool> StartMeasurePulseCountAsync(ChannelNumber channelNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> StopMeasurePulseCountAsync() =>
        await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandRegister,
            BitConverter.GetBytes((uint)0).Reverse().ToArray());

    public async Task<bool> StopMeasurePulseCountAsync(ChannelNumber channelNumber)
    {
        throw new NotImplementedException();
    }

    public async Task<CommonCommandStatus?> GetCommonCommandStatusAsync()
    {
        var status = (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandStatusRegister);

        return status != null ? (CommonCommandStatus)status : null;
    }

    public async Task<uint?> ReadPulsePeriodAsync(ChannelNumber channelNumber) => channelNumber switch
    {
        ChannelNumber.First => (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel1),
        ChannelNumber.Second => (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel2),
        ChannelNumber.Third => (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel3),
        ChannelNumber.Fourth => (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulsePeriodRegisterChannel4),
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

    public async Task<bool> SetPulseCountMeterModuleChannelSettingsAsync(ChannelNumber channelNumber) => channelNumber switch
    {
        ChannelNumber.First => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister, BitConverter.GetBytes((uint)1).Reverse().ToArray()),
        ChannelNumber.Second => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister, BitConverter.GetBytes((uint)2).Reverse().ToArray()),
        ChannelNumber.Third => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister, BitConverter.GetBytes((uint)3).Reverse().ToArray()),
        ChannelNumber.Fourth => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister, BitConverter.GetBytes((uint)4).Reverse().ToArray()),
        _ => throw new ArgumentOutOfRangeException(nameof(channelNumber), channelNumber, "Не поддерживаемый номер канала у МПКИ")
    };

    public async Task<float?> ReadPulseCountAsync() => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulseCountRegister);
}