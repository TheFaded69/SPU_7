using SPU_7.DeviceCommunication.Base;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus;
public class RegisterConfiguration : BindableBase
{
    public RegisterConfiguration(ushort address, ushort numOfRegisters, RegisterDataType registerDataType,
        ModbusFunction readFunction = ModbusFunction.ReadInputRegisters,
        ModbusFunction writeFunction = ModbusFunction.WriteMultipleRegisters,
        string name = "")
    {
        if (numOfRegisters == 0)
            throw new ArgumentException("Нет регистров в конфигурации!", nameof(numOfRegisters));
        Address = address;
        ValueDataType = registerDataType;
        ReadFunction = readFunction;
        WriteFunction = writeFunction;
        NumberOfRegisters = numOfRegisters;
        Name = name;
    }

    private ushort _address;
    private ModbusFunction _readFunction;
    private ModbusFunction _writeFunction;
    private ushort _numberOfRegisters;
    private RegisterDataType _valueDataType;
    private string _name = string.Empty;

    /// <summary>
    /// Адрес регистра
    /// </summary>
    public ushort Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    /// <summary>
    /// Функция для чтения данных
    /// </summary>
    public ModbusFunction ReadFunction
    {
        get => _readFunction;
        set => SetProperty(ref _readFunction, value);
    }

    /// <summary>
    /// Функция для записи данных
    /// </summary>
    public ModbusFunction WriteFunction
    {
        get => _writeFunction;
        set => SetProperty(ref _writeFunction, value);
    }

    /// <summary>
    /// Количество регистров занимаемых значением
    /// </summary>
    public ushort NumberOfRegisters
    {
        get => _numberOfRegisters;
        set => SetProperty(ref _numberOfRegisters, value);
    }

    /// <summary>
    /// Тип данных регистра
    /// </summary>
    public RegisterDataType ValueDataType
    {
        get => _valueDataType;
        set => SetProperty(ref _valueDataType, value);
    }

    /// <summary>
    /// Название регистра
    /// </summary>
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    /// <summary>
    /// Скопировать текущие настройки регистра
    /// </summary>
    public RegisterConfiguration GetCopy() => new(Address, NumberOfRegisters, ValueDataType, ReadFunction, WriteFunction, Name);
}