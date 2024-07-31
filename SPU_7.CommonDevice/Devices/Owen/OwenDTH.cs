using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;

public class OwenDTH : ModbusDevice, IModbusDevice, IOwenDevice, ITemperatureSensor
{
    public OwenDTH(ICommunicationChannel? communicationChannel = null) : base(communicationChannel, ModbusExtensions.CreateRegisterMap<OwenDTH_RegisterMap>(), DeviceEndianess.ABCD)
    {
        CurrentProtocol = DeviceCommunicationProtocol.Modbus;
        Logger = LogManager.GetLogger(nameof(OwenDTH));
        OwenProtocol = new OwenDeviceProtocol(communicationChannel);
    }

    private DeviceCommunicationProtocol _currentProtocol;

    public new DeviceCommunicationProtocol CurrentProtocol
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

    #region Получение данных о температуре

    /// <summary>
    /// Получить измеренную температуру с датчика в °C
    /// </summary>
    public float? GetTemperature() => GetParameterValue<float?>(OwenDTH_RegisterMap.Temperature);

    /// <summary>
    /// Получить измеренную температуру с датчика в °C, асинхронно
    /// </summary>
    public Task<float?> GetTemperatureAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(OwenDTH_RegisterMap.Temperature, cancellationToken);

    /// <summary>
    /// Получить нижнюю границу измерения температуры в °C
    /// </summary>
    public float? GetLowTemperatureBound() => GetParameterValue<float?>(OwenDTH_RegisterMap.LowMeasureBound);

    /// <summary>
    /// Получить нижнюю границу измерения температуры в °C асинхронно
    /// </summary>
    public Task<float?> GetLowTemperatureBoundAsync() => GetParameterValueAsync<float?>(OwenDTH_RegisterMap.LowMeasureBound);

    /// <summary>
    /// Получить верхнюю границу измерения температуры в °C
    /// </summary>
    public float? GetHighTemperatureBound() => GetParameterValue<float?>(OwenDTH_RegisterMap.HighMeasureBound);

    /// <summary>
    /// Получить верхнюю границу измерения температуры в °C асинхронно
    /// </summary>
    public Task<float?> GetHighTemperatureBoundAsync() => GetParameterValueAsync<float?>(OwenDTH_RegisterMap.HighMeasureBound);

    /// <summary>
    /// Получить нижний предел температуры в °C
    /// </summary>
    public float? GetMinSensorTemperature() => GetParameterValue<float?>(OwenDTH_RegisterMap.MinSensorTemperature);

    /// <summary>
    /// Получить нижний предел температуры в °C асинхронно
    /// </summary>
    public Task<float?> GetMinSensorTemperatureAsync() => GetParameterValueAsync<float?>(OwenDTH_RegisterMap.MinSensorTemperature);

    /// <summary>
    /// Получить верхний предел температуры в °C
    /// </summary>
    public float? GetMaxSensorTemperature() => GetParameterValue<float?>(OwenDTH_RegisterMap.MaxSensorTemperature);

    /// <summary>
    /// Получить верхний предел температуры в °C асинхронно
    /// </summary>
    public Task<float?> GetMaxSensorTemperatureAsync() => GetParameterValueAsync<float?>(OwenDTH_RegisterMap.MaxSensorTemperature);

    #endregion

    #region Запись данных о границах температуры

    /// <summary>
    /// Задать нижний предел температуры в °C
    /// </summary>
    /// <param name="value">Температура в пределах -213..+1310 °C</param>
    public bool SetMinSensorTemperature(float value) => SetParameterValue(OwenDTH_RegisterMap.MinSensorTemperature, value);

    /// <summary>
    /// Задать нижний предел температуры в °C асинхронно
    /// </summary>
    /// <param name="value">Температура в пределах -213..+1310 °C</param>
    public Task<bool> SetMinSensorTemperatureAsync(float value) => SetParameterValueAsync(OwenDTH_RegisterMap.MinSensorTemperature, value);

    /// <summary>
    /// Задать верхний предел температуры в °C
    /// </summary>
    /// <param name="value">Температура в пределах -213..+1310 °C</param>
    public bool SetMaxSensorTemperature(float value) => SetParameterValue(OwenDTH_RegisterMap.MaxSensorTemperature, value);

    /// <summary>
    /// Задать верхний предел температуры в °C асинхронно
    /// </summary>
    /// <param name="value">Температура в пределах -213..+1310 °C</param>
    public Task<bool> SetMaxSensorTemperatureAsync(float value) => SetParameterValueAsync(OwenDTH_RegisterMap.MaxSensorTemperature, value);

    #endregion

    #region Настройки обмена данными в сети

    #region Адрес устройства

    /// <summary>
    /// Получить адрес устройства
    /// </summary>
    public ushort? GetUnitID() => GetParameterValue<ushort?>(OwenDTH_RegisterMap.UnitId);

    /// <summary>
    /// Получить адрес устройства асинхронно
    /// </summary>
    public Task<float?> GetUnitIDAsync() => GetParameterValueAsync<float?>(OwenDTH_RegisterMap.UnitId);

    /// <summary>
    /// Задать адрес устройства
    /// </summary>
    /// <param name="value">Адрес в пределах от 1 до 247</param>
    public bool SetUnitID(byte value) => SetParameterValue(OwenDTH_RegisterMap.UnitId, value);

    /// <summary>
    /// Задать адрес устройства асинхронно
    /// </summary>
    /// <param name="value">Адрес в пределах от 1 до 247</param>
    public Task<bool> SetUnitIDAsync(byte value) => SetParameterValueAsync(OwenDTH_RegisterMap.UnitId, value);

    #endregion

    #region Скорость обмена



    #endregion

    #region Контроль чётности



    #endregion

    #region Количество стоп-бит



    #endregion



    #endregion
}