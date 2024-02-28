using SPU_7.Common.Modbus;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Modbus;

// ReSharper disable once InconsistentNaming
/// <summary>
/// Регистр тестируемых устройств
/// </summary>
public class Register : RegisterConfiguration
{
    public Register(ushort address, string name, string measureUnit, RegisterDataType dataType, ushort registerQuantity, object value) 
        : base(address, ModbusFunction.ReadHoldingRegisters,
        ModbusFunction.WriteMultipleRegisters, registerQuantity, dataType, ByteOrderType.BigEndian_ABCD)
    {
        Value = value;
        Name = name;
        MeasureUnit = measureUnit;
    }

    /// <summary>
    /// Наименование регистра
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Единицы измерения
    /// </summary>
    public string MeasureUnit { get; }

    /// <summary>
    /// Значение
    /// </summary>
    public object Value { get; set; }
}