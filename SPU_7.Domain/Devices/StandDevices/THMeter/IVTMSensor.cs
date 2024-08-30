using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.THMeter;

/// <summary>
/// ИВТМ датчик
/// </summary>
public class IVTMSensor : ModbusUnitProcessor<IVTMSensorRegisterMap>, ITemperatureHumiditySensor
{
    public IVTMSensor(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<IVTMSensorRegisterMap> registerMap,
        int moduleAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)moduleAddress;
    }

    public async Task<float?> ReadHumidityAsync() =>
        (float?)await ReadRegisterAsync(IVTMSensorRegisterMap.HumidityRegister);

    public async Task<float?> ReadTemperatureAsync() =>
        (float?)await ReadRegisterAsync(IVTMSensorRegisterMap.TemperatureRegister);
}