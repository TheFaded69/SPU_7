using SPU_7.Common.Modbus;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Modbus;

/// <summary>
/// Параметры регистра
/// </summary>
public class RegisterConfiguration
{
    public RegisterConfiguration(ushort address, ModbusFunction readFunction, ModbusFunction writeFunction,
        ushort registerQuantity, RegisterDataType dataType, ByteOrderType byteOrderType)
    {
        Address = address;
        ReadFunction = readFunction;
        WriteFunction = writeFunction;
        RegisterQuantity = registerQuantity;
        DataType = dataType;
        ByteOrderType = byteOrderType;
    }

    /// <summary>
    /// Стартовый адрес
    /// </summary>
    public ushort Address { get; }

    /// <summary>
    /// Функция чтения
    /// </summary>
    public ModbusFunction ReadFunction { get; }

    /// <summary>
    /// Функция записи
    /// </summary>
    public ModbusFunction WriteFunction { get; }

    /// <summary>
    /// Тип данных
    /// </summary>
    public RegisterDataType DataType { get; }

    /// <summary>
    /// Количество регистров, занимаемых данными
    /// </summary>
    public ushort RegisterQuantity { get; }

    /// <summary>
    /// Порядок следования байтов
    /// </summary>
    public ByteOrderType ByteOrderType { get; }
}