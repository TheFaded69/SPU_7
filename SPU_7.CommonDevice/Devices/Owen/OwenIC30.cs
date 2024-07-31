using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus;

namespace SPU_7.CommonDevice.Devices.Owen;

/// <summary>
/// Счётчик импульсов Овен-СИ30
/// </summary>
public class OwenIC30 : IModbusDevice, IOwenDevice
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="communicationChannel">Интерфейс связи с устройством (Последовательный порт)</param>
    public OwenIC30(ICommunicationChannel? communicationChannel = null)
    {
        CurrentProtocol = DeviceCommunicationProtocol.Modbus;
        Logger = LogManager.GetCurrentClassLogger();
        RegisterMap = ModbusExtensions.CreateRegisterMap<OwenIC30_RegisterMap>();
        ModbusProtocol = new ModbusDeviceProtocol(communicationChannel);
        OwenProtocol = new OwenDeviceProtocol(communicationChannel);
    }

    private DeviceCommunicationProtocol _currentProtocol;

    public Guid Id { get; set; }
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
    public IOwenDeviceProtocol OwenProtocol { get; }
    public IModbusDeviceProtocol ModbusProtocol { get; }
    public IReadOnlyDictionary<Enum, Register> RegisterMap { get; }
    public ILogger Logger { get; set; }

    #region Получение параметров из устройства

    public T? GetParameterValue<T>(Enum parameter) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T?>(RegisterMap[parameter].Configuration, Logger),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        //OwenProtocol.GetParameterData(parameter) is OwenResponse response ? default : default,
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public async Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => await ModbusProtocol.ReadRegisterValueAsync<T?>(RegisterMap[parameter].Configuration, Logger),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        //await OwenProtocol.GetParameterDataAsync(parameter) is OwenResponse response ? default : default,
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public T? GetParameterValue<T>(RegisterConfiguration registerConfiguration) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T?>(registerConfiguration, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };

    public Task<T?> GetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, CancellationToken cancellationToken = default) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValueAsync<T?>(registerConfiguration, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };

    #endregion

    #region Задание параметров устройства

    public bool SetParameterValue<T>(Enum parameter, T value) where T : notnull => throw new NotSupportedException();
    public Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default) where T : notnull => throw new NotSupportedException();
    public bool SetParameterValue<T>(RegisterConfiguration registerConfiguration, T value) where T : notnull => throw new NotSupportedException();
    public Task<bool> SetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, T value, CancellationToken cancellationToken = default) where T : notnull => throw new NotSupportedException();

    #endregion

    #region Текущие значения устройства

    /// <summary>
    /// Получить текущее значение счётчика
    /// </summary>
    public int? GetImpulseCount() => GetParameterValue<int?>(OwenIC30_RegisterMap.ImpulseCount);

    /// <summary>
    /// Получить текущее значение счётчика асинхронно
    /// </summary>
    public Task<int?> GetImpulseCountAsync() => GetParameterValueAsync<int?>(OwenIC30_RegisterMap.ImpulseCount);

    #endregion
}