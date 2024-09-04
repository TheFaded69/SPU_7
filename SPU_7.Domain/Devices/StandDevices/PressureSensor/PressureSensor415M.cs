using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.PressureSensor;

public class PressureSensor415M : ModbusUnitProcessor<PressureSensor415MRegisterMap>, IPressureSensor
{
    public PressureSensor415M(IModbusProcessor modbusProcessor, 
        IRegisterMapEnum<PressureSensor415MRegisterMap> registerMap,
        int pressureSensorAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)pressureSensorAddress;
    }

    public float? Pressure { get; set; }

    /// <summary>
    /// Считать давление в мм рт ст
    /// </summary>
    /// <returns></returns>
    public async Task<float?> ReadPressureAsync()
    {
        var currentPressure = (float?)await ReadRegisterAsync(PressureSensor415MRegisterMap.CurrentWaterStickPressure);

        if (currentPressure == null) return null;

        return Pressure = currentPressure;
    }

    public async Task<bool> ResetToZeroAsync() => true;
}