using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.PressureSensor415M;

public class PressureSensor415M : ModbusUnitProcessor<PressureSensor415MRegisterMap>, IPressureSensor415M
{
    public PressureSensor415M(IModbusProcessor modbusProcessor, IRegisterMapEnum<PressureSensor415MRegisterMap> registerMap, int pressureSensorAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)pressureSensorAddress;
    }

    /// <summary>
    /// Считать давление в  
    /// </summary>
    /// <returns></returns>
    public async Task<float?> ReadPressureAsync()
    {
        var pressure = (float?)await ReadRegisterAsync(PressureSensor415MRegisterMap.CurrentWaterStickPressure);
        return -pressure;
    }
}