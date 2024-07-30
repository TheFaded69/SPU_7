using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

public class TemperatureSensor : ModbusUnitProcessor<TemperatureSensorRegisterMap>, ITemperatureSensor
{

    public TemperatureSensor(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<TemperatureSensorRegisterMap> registerMap
        , int temperatureSensorAddress, int channelNumber) : base(modbusProcessor, registerMap)
    {
        _channelNumber = channelNumber;
        ModuleAddress = (byte)temperatureSensorAddress;
    }

    private readonly int _channelNumber;

    public async Task<float?> ReadTemperatureAsync() => (float?)await ReadRegisterAsync(TemperatureSensorRegisterMap.TemperatureRegister);

    public async Task<float?> ReadTemperatureAsync(bool useChannel) =>
        _channelNumber switch
        {
            1 => (float?)await ReadRegisterAsync(TemperatureSensorRegisterMap.Channel1Register),
            2 => (float?)await ReadRegisterAsync(TemperatureSensorRegisterMap.Channel2Register),
            3 => (float?)await ReadRegisterAsync(TemperatureSensorRegisterMap.Channel3Register),
            4 => (float?)await ReadRegisterAsync(TemperatureSensorRegisterMap.Channel4Register),
            5 => (float?)await ReadRegisterAsync(TemperatureSensorRegisterMap.Channel5Register),
            _ => null
        };
}