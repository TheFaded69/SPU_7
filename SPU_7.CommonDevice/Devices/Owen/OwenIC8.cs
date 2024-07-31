using NLog;
using SPU_7.DeviceCommunication.Base;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;
using SPU_7.DeviceCommunication.OwenProtocol;

namespace SPU_7.CommonDevice.Devices.Owen;

/// <summary>
/// Счётчик импульсов Овен-СИ8
/// </summary>
public class OwenIC8 : BindableBase, IOwenDevice, IConnectionCheck // Owen Impulse Counter 8
{
    /// <summary>
    /// Создание устройства Овен СИ8
    /// </summary>
    /// <param name="communicationChannel">Интерфейс связи с устройством (Последовательный порт)</param>
    public OwenIC8(ICommunicationChannel? communicationChannel)
    {
        Logger = LogManager.GetCurrentClassLogger();
        OwenProtocol = new OwenDeviceProtocol(communicationChannel);
    }

    #region Свойства

    public DeviceCommunicationProtocol CurrentProtocol
    {
        get => DeviceCommunicationProtocol.Owen;
        set { if (value != DeviceCommunicationProtocol.Owen) throw new NotSupportedException("Устройство не поддерживает другие протоколы связи!"); }
    }
    public IOwenDeviceProtocol OwenProtocol { get; }
    public ILogger Logger { get; set; }

    //
    public bool IsOnlineCheck { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public bool IsOnline => throw new NotImplementedException();

    public int IsOnlineInterval { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //
    public Guid Id { get; set; }

    #endregion

    #region Получение параметров из устройства

    public T? GetParameterValue<T>(Enum parameter) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Owen => OwenProtocol.GetParameterData(parameter) is OwenResponse response
        ? (T?)GetValue(response.Data, (OwenIC8_Parameters)parameter)
        : default,
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    public async Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default) => CurrentProtocol switch
    {
        DeviceCommunicationProtocol.Owen => await OwenProtocol.GetParameterDataAsync(parameter, cancellationToken).ConfigureAwait(false) is OwenResponse response
        ? (T?)GetValue(response.Data, (OwenIC8_Parameters)parameter)
        : default,
        _ => throw new NotSupportedException($"{CurrentProtocol.GetDescription()}. Данный тип протокола связи не поддерживается устройством!")
    };

    #endregion

    #region Задание параметров устройства

    public bool SetParameterValue<T>(Enum parameter, T value) where T : notnull => throw new NotSupportedException();

    public Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default) where T : notnull => throw new NotSupportedException();

    #endregion

    #region Текущие значения устройства

    /// <summary>
    /// Получить значение счётчика
    /// </summary>
    public int? GetImpulseCount() => GetParameterValue<int?>(OwenIC8_Parameters.ImpulseCount);

    /// <summary>
    /// Получить значение счётчика асинхронно
    /// </summary>
    public Task<int?> GetImpulseCountAsync() => GetParameterValueAsync<int?>(OwenIC8_Parameters.ImpulseCount);

    /// <summary>
    /// Получить значение счётчика времени
    /// </summary>
    public TimeSpan? GetCountTime() => GetParameterValue<TimeSpan?>(OwenIC8_Parameters.TimerValue);

    /// <summary>
    /// Получить значение счётчика времени асинхронно
    /// </summary>
    public Task<TimeSpan?> GetCountTimeAsync() => GetParameterValueAsync<TimeSpan?>(OwenIC8_Parameters.TimerValue);

    /// <summary>
    /// Считать показания расходомера
    /// </summary>
    public uint? GetFlowValue() => GetParameterValue<uint?>(OwenIC8_Parameters.FlowValue);

    /// <summary>
    /// Считать показания расходомера асинхронно
    /// </summary>
    public Task<uint?> GetFlowValueAsync() => GetParameterValueAsync<uint?>(OwenIC8_Parameters.FlowValue);

    /// <summary>
    /// Получить значение параметра
    /// </summary>
    /// <param name="data">Поле данных после параметра (от 0 до 15 байт)</param>
    /// <param name="parameter">Параметр полученный в ответе</param>
    /// <returns>Значение параметра</returns>
    private static object? GetValue(IEnumerable<byte> data, OwenIC8_Parameters? parameter) => parameter switch
    {
        OwenIC8_Parameters.ImpulseCount => ConvertSInt(data),
        OwenIC8_Parameters.FlowValue => ConvertUInt(data),
        OwenIC8_Parameters.TimerValue => GetTimerValue(data.Take(6)),
        _ => null
    };

    #endregion

    #region Конвертеры для СИ8

    /// <summary>
    /// Получить время из данных Овен СИ8
    /// </summary>
    /// <param name="timeBytes">Байты данных о времени</param>
    /// <returns>Время подсчёта импульсов</returns>
    private static TimeSpan? GetTimerValue(IEnumerable<byte> timeBytes) =>
        new(0, ConvertInt(timeBytes, 3), ConvertInt(timeBytes.Skip(3), 1), ConvertInt(timeBytes.Skip(4), 1), ConvertInt(timeBytes.Skip(5), 1) * 10);

    /// <summary>
    /// Преобразовать данные из Овен СИ8 в число
    /// </summary>
    /// <param name="intBytes">Байты содержащие значение</param>
    /// <param name="numBytes">Кол-во байт для считывания</param>
    /// <returns>Знаковое число</returns>
    private static int? ConvertSInt(IEnumerable<byte> intBytes) => intBytes.First() >= 0x0A
        ? -int.Parse(intBytes.Skip(1).Aggregate(string.Empty, (ostr, bt) => $"{ostr}{bt:X2}"))
        : int.Parse(intBytes.Aggregate(string.Empty, (ostr, bt) => $"{ostr}{bt:X2}"));

    /// <summary>
    /// Преобразовать данные из Овен СИ8 в число
    /// </summary>
    /// <param name="intBytes">Байты содержащие значение</param>
    /// <param name="numBytes">Кол-во байт для считывания</param>
    /// <returns>Знаковое число</returns>
    private static int ConvertInt(IEnumerable<byte> intBytes, int numBytes) =>
        int.Parse(intBytes.Take(numBytes).Aggregate(string.Empty, (ostr, bt) => $"{ostr}{bt:X2}"));

    /// <summary>
    /// Преобразовать данные из Овен СИ8 в беззнаковое число
    /// </summary>
    /// <param name="intBytes">Байты содержащие значение</param>
    /// <param name="numBytes">Кол-во байт для считывания</param>
    /// <returns>Беззнаковое число</returns>
    private static uint? ConvertUInt(IEnumerable<byte> intBytes) =>
        uint.Parse(intBytes.Aggregate(string.Empty, (ostr, bt) => $"{ostr}{bt:X2}"));

    #endregion
}