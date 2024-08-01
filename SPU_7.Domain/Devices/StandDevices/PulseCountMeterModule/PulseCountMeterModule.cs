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

    public async Task<bool> SetPulseCountMeterModuleChannelSettingsAsync(ChannelNumber channelNumber)
    {
        //Применяем настройки
        await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.CommonCommandRegister,
            BitConverter.GetBytes((uint)2).SwapBytes().ToArray());
        
        return channelNumber switch
        {
            ChannelNumber.First => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                BitConverter.GetBytes((uint)1).SwapBytes().ToArray()),
            ChannelNumber.Second => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                BitConverter.GetBytes((uint)2).SwapBytes().ToArray()),
            ChannelNumber.Third => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                BitConverter.GetBytes((uint)3).SwapBytes().ToArray()),
            ChannelNumber.Fourth => await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.SettingsProfileRegister,
                BitConverter.GetBytes((uint)4).SwapBytes().ToArray()),
            _ => throw new ArgumentOutOfRangeException(nameof(channelNumber), channelNumber,
                "Не поддерживаемый номер канала у МПКИ")
        };
    }

    public async Task<float?> ReadPulseCountAsync() => (float?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.PulseCountRegister);
    public async Task<uint?> ReadControlRegisterAsync() => (uint?)await ReadRegisterAsync(PulseMeterCountModuleRegisterMap.ControlRegister);

    public async Task<bool> TurnOnControlBitControlRegisterAsync()
    {
        var controlRegister = await ReadControlRegisterAsync();

        controlRegister |= 0x0010;

        return await WriteRegisterAsync(PulseMeterCountModuleRegisterMap.ControlRegister,
            BitConverter.GetBytes((uint)controlRegister).SwapBytes().ToArray());
    }
}