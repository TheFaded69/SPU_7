using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;
public abstract class Owen110_Base : IModbusDevice, IOwenDevice
{
    protected Owen110_Base(ICommunicationChannel? communicationChannel, IReadOnlyDictionary<Enum, Register> registerMap)
    {
        CurrentProtocol = DeviceCommunicationProtocol.Modbus;
        Logger = LogManager.GetCurrentClassLogger();
        RegisterMap = registerMap;
        ModbusProtocol = new ModbusDeviceProtocol(communicationChannel, DeviceEndianess.ABCD);
        OwenProtocol = new OwenDeviceProtocol(communicationChannel);
    }

    private DeviceCommunicationProtocol _currentProtocol;

    public DeviceCommunicationProtocol CurrentProtocol
    {
        get => _currentProtocol;
        set => _currentProtocol = value switch
        {
            DeviceCommunicationProtocol.Modbus => DeviceCommunicationProtocol.Modbus,
            DeviceCommunicationProtocol.Owen => DeviceCommunicationProtocol.Owen,
            _ => throw new NotSupportedException($"Устройство не поддерживает данный протокол связи! {value.GetDescription()}")
        };
    }

    public Guid Id { get; set; }
    public IModbusDeviceProtocol ModbusProtocol { get; }
    public IOwenDeviceProtocol OwenProtocol { get; }
    public IReadOnlyDictionary<Enum, Register> RegisterMap { get; }
    public ILogger Logger { get; set; }

    public T? GetParameterValue<T>(Enum parameter) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T>(RegisterMap[parameter].Configuration, Logger),
        //DeviceCommunicationProtocol.Owen => OwenProtocol.GetParameterData(parameter),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };
    
    public T? GetParameterValue<T>(RegisterConfiguration registerConfiguration) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T?>(registerConfiguration, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public bool SetParameterValue<T>(Enum parameter, T value) where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValue(RegisterMap[parameter].Configuration, value, Logger),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public bool SetParameterValue<T>(RegisterConfiguration registerConfiguration, T value) where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValue(registerConfiguration, value, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };

    public Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValueAsync<T>(RegisterMap[parameter].Configuration, Logger, cancellationToken),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<T?> GetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, CancellationToken cancellationToken = default) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValueAsync<T?>(registerConfiguration, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default)
        where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValueAsync(RegisterMap[parameter].Configuration, value, Logger, cancellationToken),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<bool> SetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, T value, CancellationToken cancellationToken = default)
        where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValueAsync(registerConfiguration, value, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };
}