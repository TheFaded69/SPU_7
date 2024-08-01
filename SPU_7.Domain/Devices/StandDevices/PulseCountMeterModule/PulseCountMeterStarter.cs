using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Extensions;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;

public class PulseCountMeterStarter : ModbusUnitProcessor<PulseCountMeterStarterRegisterMap>
{
    public PulseCountMeterStarter(IModbusProcessor modbusProcessor, IRegisterMapEnum<PulseCountMeterStarterRegisterMap> registerMap) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = 150;
    }

    public async Task<bool> SendStartCommand()
    {
        return await WriteRegisterAsync(PulseCountMeterStarterRegisterMap.CommandRegister,
            BitConverter.GetBytes((uint)1).SwapBytes().ToArray());
    }
}