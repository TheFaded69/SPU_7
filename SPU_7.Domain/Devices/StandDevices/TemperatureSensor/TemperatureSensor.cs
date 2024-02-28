using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

public class TemperatureSensor : ModbusUnitProcessor<TemperatureSensorRegisterMap>, ITemperatureSensor
{
    public TemperatureSensor(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<TemperatureSensorRegisterMap> registerMap
        , int temperatureSensorAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)temperatureSensorAddress;
    }

    public async Task<float?> ReadTemperatureAsync() => (float?)await ReadRegisterAsync(TemperatureSensorRegisterMap.TemperatureRegister);
}