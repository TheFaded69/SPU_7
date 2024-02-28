using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.PressureSensor415M;

public enum PressureSensor415MRegisterMap
{
    /// <summary>
    /// Калибровка нуля
    /// </summary>
    [RegisterSetup(0x0030, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    ZeroCalibration,

    /// <summary>
    /// Калибровка диапазона
    /// </summary>
    [RegisterSetup(0x0044, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    RangeCalibration,

    /// <summary>
    /// Восстановление заводских настроек диапазона
    /// </summary>
    [RegisterSetup(0x0050, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    RestoreRangeCalibration,

    /// <summary>
    /// Текущая температура в °C
    /// </summary>
    [RegisterSetup(0x0050, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    CurrentTemperature,
    
    /// <summary>
    /// Давление в мм.вод.ст.
    /// </summary>
    [RegisterSetup(0x0054, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    CurrentWaterStickPressure,
    
    /// <summary>
    /// Давление в кПа
    /// </summary>
    [RegisterSetup(0x0058, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    CurrentPressure,
    
    /// <summary>
    /// Заводской номер
    /// </summary>
    [RegisterSetup(0x01F8, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.Int16, ByteOrderType.MidLittleEndian_CDAB)]
    VendorNumber,

    /// <summary>
    /// Номер модели
    /// </summary>
    [RegisterSetup(0x01FA, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.Int16, ByteOrderType.MidLittleEndian_CDAB)]
    ModelNumber,
}