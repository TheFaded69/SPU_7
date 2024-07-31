using System.Collections;
using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.TDIOModules;
public class KPK_AIN : ModbusDevice, IModbusDevice, IAnalogInputController
{
    public KPK_AIN(ICommunicationChannel? communicationChannel = null)
        : base(communicationChannel, ModbusExtensions.CreateRegisterMap<KPK_AIN_RegisterMap>(), DeviceEndianess.ABCD)
    {
        Logger = LogManager.GetLogger(nameof(KPK_AIN));
    }

    public int AnalogInputsCount => 5;

    /// <summary>
    /// Получить блок мгновенных значений аналогового входа
    /// </summary>
    public IList<float>? GetInputValues() => GetParameterValue<IList>(KPK_AIN_RegisterMap.AnalogInputBlock)?.Cast<float>().ToArray();

    /// <summary>
    /// Получить блок мгновенных значений аналогового входа асинхронно
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task<IList<float>?> GetInputValuesAsync(CancellationToken cancellationToken = default) =>
        (await GetParameterValueAsync<IList>(KPK_AIN_RegisterMap.AnalogInputBlock, cancellationToken).ConfigureAwait(false))?.Cast<float>().ToArray();

    /// <summary>
    /// Получить значения для входа под индексом
    /// </summary>
    /// <param name="inputIndex">Индекс входа</param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public float? GetInputValue(int inputIndex) => inputIndex switch
    {
        0 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput1),
        1 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput2),
        2 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput3),
        3 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput4),
        4 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput5),
        _ => throw new IndexOutOfRangeException(nameof(inputIndex))
    };

    /// <summary>
    /// Получить значения для входа под индексом асинхронно
    /// </summary>
    /// <param name="inputIndex">Индекс входа</param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public Task<float?> GetInputValueAsync(int inputIndex, CancellationToken cancellationToken = default) => inputIndex switch
    {
        0 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput1, cancellationToken),
        1 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput2, cancellationToken),
        2 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput3, cancellationToken),
        3 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput4, cancellationToken),
        4 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput5, cancellationToken),
        _ => throw new IndexOutOfRangeException(nameof(inputIndex))
    };

    /// <summary>
    /// Получить блок средних значений аналогового входа
    /// </summary>
    public IList<float>? GetAvgInputValues() => GetParameterValue<IList>(KPK_AIN_RegisterMap.AnalogInputAvgBlock)?.Cast<float>().ToArray();

    /// <summary>
    /// Получить блок средних значений аналогового входа асинхронно
    /// </summary>
    /// <param name="cancellationToken"></param>
    public async Task<IList<float>?> GetAvgInputValuesAsync(CancellationToken cancellationToken = default) =>
        (await GetParameterValueAsync<IList>(KPK_AIN_RegisterMap.AnalogInputAvgBlock, cancellationToken))?.Cast<float>().ToArray();

    /// <summary>
    /// Получить среднее значение для входа под индексом
    /// </summary>
    /// <param name="inputIndex">Индекс входа</param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public float? GetInputAverageValue(int inputIndex) => inputIndex switch
    {
        0 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput1Avg),
        1 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput2Avg),
        2 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput3Avg),
        3 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput4Avg),
        4 => GetParameterValue<float?>(KPK_AIN_RegisterMap.AnalogInput5Avg),
        _ => throw new IndexOutOfRangeException(nameof(inputIndex))
    };

    /// <summary>
    /// Получить среднее значение для входа под индексом асинхронно
    /// </summary>
    /// <param name="inputIndex">Индекс входа</param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public Task<float?> GetInputAverageValueAsync(int inputIndex, CancellationToken cancellationToken = default) => inputIndex switch
    {
        0 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput1Avg, cancellationToken),
        1 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput2Avg, cancellationToken),
        2 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput3Avg, cancellationToken),
        3 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput4Avg, cancellationToken),
        4 => GetParameterValueAsync<float?>(KPK_AIN_RegisterMap.AnalogInput5Avg, cancellationToken),
        _ => throw new IndexOutOfRangeException(nameof(inputIndex))
    };
}