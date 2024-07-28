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


    public Task<bool> StartMeasurePulseCountAsync(ChannelNumber channelNumber)
    {
        throw new NotImplementedException();
    }

    public Task<bool> StopMeasurePulseCountAsync(ChannelNumber channelNumber)
    {
        throw new NotImplementedException();
    }
}