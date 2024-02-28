using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.PressureSensor;

public class PressureSensor : ModbusUnitProcessor<PressureSensorRegisterMap>, IPressureSensor
{
    public PressureSensor(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<PressureSensorRegisterMap> registerMap,
        int moduleAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)moduleAddress;
    }

    /// <summary>
    /// Считать давление с ДД
    /// </summary>
    /// <returns>Давление</returns>
    public async Task<float?> ReadPressureAsync() =>
        (float?)await ReadRegisterAsync(PressureSensorRegisterMap.PressureRegister);

    /// <summary>
    /// Сброс ДД на ноль
    /// </summary>
    /// <returns></returns>
    public async Task<bool> ResetToZeroAsync()
    {
        if (!await WriteRegisterAsync(PressureSensorRegisterMap.ResetToZeroRegister, BitConverter.GetBytes((float)0).Reverse().ToArray())) return false;

        await Task.Delay(60000);
        
        var pressure = await ReadPressureAsync();
        if (pressure == null) return false;

        return await WriteRegisterAsync(PressureSensorRegisterMap.ResetToZeroRegister, BitConverter.GetBytes((float)-pressure).Reverse().ToArray());
    }
}