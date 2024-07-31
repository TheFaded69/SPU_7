using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.PS415M;
public class PressureSensor415M : ModbusDevice, IModbusDevice, IPressureSensor
{
    public PressureSensor415M(ICommunicationChannel? deviceCommunicator = null)
        : base(deviceCommunicator, ModbusExtensions.CreateRegisterMap<PS415M_RegisterMap>(), DeviceEndianess.CDAB)
    {
        Logger = LogManager.GetLogger(nameof(PressureSensor415M));
    }

    #region Текущие значения устройства

    /// <summary>
    /// Получить текущее давление в Па
    /// </summary>
    public float? GetPressure(PressureType pressureType = PressureType.DefaultPressure) =>
        GetParameterValue<float?>(pressureType switch
        {
            PressureType.DefaultPressure => PS415M_RegisterMap.CurrentPressure,
            PressureType.ExcessPressure => PS415M_RegisterMap.CurrentPressure,
            PressureType.AbsolutePressure => throw new NotSupportedException(),
            _ => throw new NotImplementedException(),
        }) is float value ? value * 1e3f : null;

    /// <summary>
    /// Получить текущее давление в Па асинхронно
    /// </summary>
    public async Task<float?> GetPressureAsync(PressureType pressureType = PressureType.DefaultPressure, CancellationToken cancellationToken = default) =>
        await GetParameterValueAsync<float?>(pressureType switch
    {
        PressureType.DefaultPressure => PS415M_RegisterMap.CurrentPressure,
        PressureType.ExcessPressure => PS415M_RegisterMap.CurrentPressure,
        PressureType.AbsolutePressure => throw new NotSupportedException(),
        _ => throw new NotImplementedException(),
    }, cancellationToken) is float value ? value * 1e3f : null;

    /// <summary>
    /// Получить текущую температуру в °C
    /// </summary>
    public float? GetTemperature() => GetParameterValue<float?>(PS415M_RegisterMap.CurrentTemperature);

    /// <summary>
    /// Получить текущую температуру в °C асинхронно
    /// </summary>
    public Task<float?> GetTemperatureAsync() => GetParameterValueAsync<float?>(PS415M_RegisterMap.CurrentTemperature);

    #endregion
}