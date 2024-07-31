using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus;

namespace SPU_7.CommonDevice.Devices.Owen;
public class OwenPBR10A : IModbusDevice, IOwenDevice
{
    public OwenPBR10A(ICommunicationChannel? communicationChannel = null) //: base(deviceCommunication, , DeviceEndianess.CDAB)
    {
        CurrentProtocol = DeviceCommunicationProtocol.Modbus;
        Logger = LogManager.GetCurrentClassLogger();
        ModbusProtocol = new ModbusDeviceProtocol(communicationChannel);
        OwenProtocol = new OwenDeviceProtocol(communicationChannel);
        RegisterMap = ModbusExtensions.CreateRegisterMap<OwenPBR10A_RegisterMap>();
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
    public ILogger Logger { get; set; }
    public IModbusDeviceProtocol ModbusProtocol { get; }
    public IReadOnlyDictionary<Enum, Register> RegisterMap { get; }
    public IOwenDeviceProtocol OwenProtocol { get; }

    #region Получение параметров из устройства

    public T? GetParameterValue<T>(Enum parameter) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T>(RegisterMap[parameter].Configuration, Logger),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValueAsync<T>(RegisterMap[parameter].Configuration, Logger, cancellationToken),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public T? GetParameterValue<T>(RegisterConfiguration registerConfiguration) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValue<T?>(registerConfiguration, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };

    public Task<T?> GetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, CancellationToken cancellationToken = default) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.ReadRegisterValueAsync<T?>(registerConfiguration, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };

    #endregion

    #region Задание параметров устройства

    public bool SetParameterValue<T>(Enum parameter, T value) where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValue(RegisterMap[parameter].Configuration, value, Logger),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default)
        where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValueAsync(RegisterMap[parameter].Configuration, value, Logger, cancellationToken),
        DeviceCommunicationProtocol.Owen => throw new NotImplementedException(),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public bool SetParameterValue<T>(RegisterConfiguration registerConfiguration, T value) where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValue(registerConfiguration, value, Logger),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };

    public Task<bool> SetParameterValueAsync<T>(RegisterConfiguration registerConfiguration, T value, CancellationToken cancellationToken = default)
        where T : notnull => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Modbus => ModbusProtocol.WriteRegisterValueAsync(registerConfiguration, value, Logger, cancellationToken),
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается функцией!")
    };

    #endregion

    #region Общая информация о приборе
    /*
    /// <summary>
    /// Получить температуру контроллера Овен
    /// </summary>
    public short? ControllerTemperature() => ReadRegisterValue<short?>(RegisterMap[OwenPBR10A_RegisterMap.ControllerTemperature].Configuration, Logger);

    /// <summary>
    /// Получить температуру контроллера Овен асинхронно
    /// </summary>
    public Task<short?> ControllerTemperatureAsync() => ReadRegisterValueAsync<short?>(RegisterMap[OwenPBR10A_RegisterMap.ControllerTemperature].Configuration, Logger);

    /// <summary>
    /// Получить код события Овен ПБР
    /// </summary>
    public OwenPBR10A_Events? GetEventCode() => (OwenPBR10A_Events?)ReadRegisterValue<ushort?>(RegisterMap[OwenPBR10A_RegisterMap.EventsCode].Configuration, Logger);

    /// <summary>
    /// Получить код события Овен ПБР асинхронно
    /// </summary>
    public async Task<OwenPBR10A_Events?> GetEventCodeAsync() => (OwenPBR10A_Events?)await ReadRegisterValueAsync<ushort?>(RegisterMap[OwenPBR10A_RegisterMap.EventsCode].Configuration, Logger);
    */
    #endregion

    #region Информация о питающей сети

    #region Синхронные методы

    /// <summary>
    /// Получить частоту напряжения в сети, Гц
    /// </summary>
    public float? GetVoltageFrequency() => GetParameterValue<float?>(OwenPBR10A_RegisterMap.VoltageFrequency);

    /// <summary>
    /// Получить среднее квадратическое напряжение на L1
    /// </summary>
    public float? GetRMSVoltageL1() => GetParameterValue<float?>(OwenPBR10A_RegisterMap.RMSVoltageL1);

    /// <summary>
    /// Получить среднее квадратическое напряжение на L2
    /// </summary>
    public float? GetRMSVoltageL2() => GetParameterValue<float?>(OwenPBR10A_RegisterMap.RMSVoltageL2);

    /// <summary>
    /// Получить среднее квадратическое напряжение на L3
    /// </summary>
    public float? GetRMSVoltageL3() => GetParameterValue<float?>(OwenPBR10A_RegisterMap.RMSVoltageL3);

    #endregion

    #region Асинхронные методы

    /// <summary>
    /// Получить частоту напряжения в сети, Гц
    /// </summary>
    public Task<float?> GetVoltageFrequencyAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(OwenPBR10A_RegisterMap.VoltageFrequency, cancellationToken);

    /// <summary>
    /// Получить среднее квадратическое напряжение на L1
    /// </summary>
    public Task<float?> GetRMSVoltageL1Async(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(OwenPBR10A_RegisterMap.RMSVoltageL1, cancellationToken);

    /// <summary>
    /// Получить среднее квадратическое напряжение на L2
    /// </summary>
    public Task<float?> GetRMSVoltageL2Async(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(OwenPBR10A_RegisterMap.RMSVoltageL2, cancellationToken);

    /// <summary>
    /// Получить среднее квадратическое напряжение на L3
    /// </summary>
    public Task<float?> GetRMSVoltageL3Async(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(OwenPBR10A_RegisterMap.RMSVoltageL3, cancellationToken);

    #endregion
    #endregion

    #region Управление арматурой

    #region Синхронные методы

    /// <summary>
    /// Остановить арматуру
    /// </summary>
    public bool StopMoving() => SetParameterValue(OwenPBR10A_RegisterMap.ValveControl, OwenPBR10A_MovingType.Stop);

    /// <summary>
    /// Перемещение вниз
    /// </summary>
    public bool MovingDown() => SetParameterValue(OwenPBR10A_RegisterMap.ValveControl, OwenPBR10A_MovingType.Down);

    /// <summary>
    /// Перемещение вверх
    /// </summary>
    public bool MovingUp() => SetParameterValue(OwenPBR10A_RegisterMap.ValveControl, OwenPBR10A_MovingType.Up);

    /// <summary>
    /// Задать позицию в процентах
    /// </summary>
    /// <param name="percent">Процент позиции</param>
    public bool SetPositionPercentage(float percent) {
        throw new NotImplementedException();
        while (true) {
            var currentPosition = GetParameterValue<ushort?>(OwenPBR10A_RegisterMap.PositionPercent);

            /*if (percent > currentPosition) {

            }*/
            break;
        }

        return false;
    }

    /// <summary>
    /// Получить положение задвижки в %
    /// </summary>
    public float? GetPosition() => GetParameterValue<ushort?>(OwenPBR10A_RegisterMap.PositionPercent);

    #endregion

    #region Асинхронные методы

    /// <summary>
    /// Остановить арматуру
    /// </summary>
    public Task<bool> StopMovingAsync(CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(OwenPBR10A_RegisterMap.ValveControl, OwenPBR10A_MovingType.Stop, cancellationToken);

    /// <summary>
    /// Перемещение вниз
    /// </summary>
    public Task<bool> MovingDownAsync(CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(OwenPBR10A_RegisterMap.ValveControl, OwenPBR10A_MovingType.Down, cancellationToken);

    /// <summary>
    /// Перемещение вверх
    /// </summary>
    public Task<bool> MovingUpAsync(CancellationToken cancellationToken = default) =>
        SetParameterValueAsync(OwenPBR10A_RegisterMap.ValveControl, OwenPBR10A_MovingType.Up, cancellationToken);

    /// <summary>
    /// Получить положение задвижки в % асинхронно
    /// </summary>
    public async Task<float?> GetPositionAsync(CancellationToken cancellationToken = default) =>
        await GetParameterValueAsync<ushort?>(OwenPBR10A_RegisterMap.PositionPercent, cancellationToken);
    //.....................................................

    #endregion

    #endregion
}