using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.IVTM;

public enum IVTM7_RegisterMap
{
    /// <summary>
    /// Текущая влажность в %
    /// </summary>
    [RegisterConfiguration(0x0000, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    CurrentHumidity,

    /// <summary>
    /// Точка росы в °Cтр
    /// </summary>
    [RegisterConfiguration(0x0002, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    HumidityPoint,

    /// <summary>
    /// Абсолютная влажность в г/м³
    /// </summary>
    [RegisterConfiguration(0x0004, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    AbsoluteHumidity,

    /// <summary>
    /// Текущая температура в °C
    /// </summary>
    [RegisterConfiguration(0x0006, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    CurrentTemperature,
}