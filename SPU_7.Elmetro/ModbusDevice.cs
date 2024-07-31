using DeviceCommunication.Base;
using DeviceCommunication.Communication;
using DeviceCommunication.Extensions;
using DeviceCommunication.Modbus;
using DeviceCommunication.Modbus.Enums;
using NLog;

namespace DeviceCommunication.Devices;
public abstract class ModbusDevice : BindableBase, IModbusDevice
{

    public ModbusDevice(ICommunicationChannel? deviceCommunication, IReadOnlyDictionary<Enum, Register> registerMap, DeviceEndianess endianess = DeviceEndianess.CDAB)
    {
        ModbusProtocol = new ModbusDeviceProtocol(deviceCommunication, endianess);
        RegisterMap = registerMap;
    }

    private Guid _id;
    private ILogger _logger = LogManager.GetLogger(nameof(ModbusDevice));

    public Guid Id
    {
        get => _id;
        set => SetProperty(ref _id, value);
    }
    public IModbusDeviceProtocol ModbusProtocol { get; }
    public IReadOnlyDictionary<Enum, Register> RegisterMap { get; }
    public ILogger Logger
    {
        get => _logger;
        set => SetProperty(ref _logger, value);
    }

    public DeviceCommunicationProtocol CurrentProtocol
    {
        get => DeviceCommunicationProtocol.Modbus;
        set { if (value != DeviceCommunicationProtocol.Modbus) throw new NotSupportedException("Устройство не поддерживает другие протоколы связи!"); }
    }

    #region Получение и задание параметров в зависимости от протокола

    #region Получение параметров

    public T? GetParameterValue<T>(Enum parameter) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T?>(RegisterMap[parameter].Configuration, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default)
        => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValueAsync<T?>(RegisterMap[parameter].Configuration, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public T? GetParameterValue<T>(RegisterConfiguration registerConfiguration) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T?>(registerConfiguration, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<T?> GetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, CancellationToken cancellationToken = default)
        => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValueAsync<T?>(registerConfiguration, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    #endregion

    #region Задание параметров

    public bool SetParameterValue<T>(Enum parameter, T value)
        where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValue(RegisterMap[parameter].Configuration, value, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default)
        where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValueAsync(RegisterMap[parameter].Configuration, value, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public bool SetParameterValue<T>(RegisterConfiguration registerConfiguration, T value) where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValue(registerConfiguration, value, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<bool> SetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, T value, CancellationToken cancellationToken = default)
        where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValueAsync(registerConfiguration, value, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    #endregion

    #endregion
}