using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.IVTM;

/// <summary>
/// Измеритель влажности и температуры ИВТМ-7/1-Щ
/// </summary>
public class IVTM7 : ModbusDevice, IModbusDevice, ITemperatureSensor
{
    /// <summary>
    /// Измеритель влажности и температуры ИВТМ-7/1-Щ
    /// </summary>
    /// <param name="communicationChannel">Интерфейс связи с устройством (Последовательный порт)</param>
    public IVTM7(ICommunicationChannel? communicationChannel = null)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<IVTM7_RegisterMap>(), DeviceEndianess.CDAB)
    {
        Logger = LogManager.GetLogger(nameof(IVTM7));
    }

    /// <summary>
    /// Получить Влажность в процентах
    /// </summary>
    public float? GetHumidityPercent() => GetParameterValue<float?>(IVTM7_RegisterMap.CurrentHumidity);

    /// <summary>
    /// Получить Влажность в процентах асинхронно
    /// </summary>
    public Task<float?> GetHumidityPercentAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(IVTM7_RegisterMap.CurrentHumidity, cancellationToken);

    /// <summary>
    /// Получить Температуру в °C
    /// </summary>
    public float? GetTemperature() => GetParameterValue<float?>(IVTM7_RegisterMap.CurrentTemperature);

    /// <summary>
    ///  Получить Температуру в °C асинхронно
    /// </summary>
    public Task<float?> GetTemperatureAsync(CancellationToken cancellationToken = default) =>
        GetParameterValueAsync<float?>(IVTM7_RegisterMap.CurrentTemperature, cancellationToken);
}