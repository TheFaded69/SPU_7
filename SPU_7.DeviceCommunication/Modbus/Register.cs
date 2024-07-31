using SPU_7.DeviceCommunication.Base;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Modbus;

public class Register : BindableBase
{
    /// <summary>
    /// Представление Регистра для устройства
    /// </summary>
    /// <param name="address">Адрес регистра (Начальный)</param>
    /// <param name="numOfRegisters">Количество регистров данных в Регистре представления</param>
    /// <param name="registerDataType">Тип данных регистра представления</param>
    /// <param name="readFunction">Функция для чтения значения</param>
    /// <param name="writeFunction">Функция для записи значения</param>
    public Register(ushort address, ushort numOfRegisters, RegisterDataType registerDataType,
                    ModbusFunction readFunction = ModbusFunction.ReadInputRegisters, ModbusFunction writeFunction = ModbusFunction.WriteMultipleRegisters)
    {
        Configuration = new RegisterConfiguration(address, numOfRegisters, registerDataType, readFunction, writeFunction);
    }

    /// <summary>
    /// Представление Регистра для устройства
    /// </summary>
    /// <param name="registerConfiguration">Конфигурация регистра</param>
    public Register(RegisterConfiguration registerConfiguration)
    {
        Configuration = registerConfiguration;
    }

    private object? _value;
    private DateTime? _lastWrite;
    private DateTime? _lastRead;

    //public ModbusDevice Device { get; }

    /// <summary>
    /// Конфигурация регистра
    /// </summary>
    public RegisterConfiguration Configuration { get; }

    /// <summary>
    /// Значение регистра
    /// </summary>
    public object? Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    /// <summary>
    /// Данные для записи/чтения
    /// </summary>
    /*public byte[]? ValueData
    {
        get => Value?.ConvertRegisterData(Configuration.ValueDataType, Configuration.);
        set => Value = value?.ConvertRegisterData(Configuration.ValueDataType);
    }*/

    /// <summary>
    /// Время последнего чтения значения регистра
    /// </summary>
    public DateTime? LastRead
    {
        get => _lastRead;
        set => SetProperty(ref _lastRead, value);
    }

    /// <summary>
    /// Время последней записи значения регистра
    /// </summary>
    public DateTime? LastWrite
    {
        get => _lastWrite;
        set => SetProperty(ref _lastWrite, value);
    }
}