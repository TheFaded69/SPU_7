using System.IO.Ports;
using NLog;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.ElmetroProtocol;
using SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroRequests;
using SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroResponses;

namespace SPU_7.CommonDevice.Devices.ElmetroPascal;

/// <summary>
/// Датчик элметро
/// </summary>
public class ElmetroDigitalDevice : IDevice, IPressureSensor
{
    /// <summary>
    /// Конструктор устройства Элметро
    /// </summary>
    /// <param name="communicationChannel">Последовательный порт для связи с устройством</param>
    /// <exception cref="ArgumentException"></exception>
    public ElmetroDigitalDevice(ICommunicationChannel? communicationChannel = null)
    {
        //if (deviceCommunication is not SerialPortCommunication) throw new ArgumentException("Неподходящий тип коммуникатора для ElmetroDigitalDevice", nameof(deviceCommunication));
        Logger = LogManager.GetLogger(nameof(ElmetroDigitalDevice));
        CommunicationChannel = communicationChannel;
        RetryCount = 3;
        // Стандартные настройки для связи с модулем:
        if (communicationChannel is SerialPortCommunication spc) {
            spc.BaudRate = 19200;
            spc.DataBits = 8;
            spc.Parity = Parity.Odd;
            spc.StopBits = StopBits.One;
            spc.Handshake = Handshake.None;
            spc.WriteTimeout = 1000;
            spc.ReadTimeout = 1000;
        }
    }

    #region Свойства

    /// <summary>
    /// Интерфейс связи с устройством
    /// </summary>
    public ICommunicationChannel? CommunicationChannel { get; set; }

    public DeviceCommunicationProtocol CurrentProtocol
    {
        get => DeviceCommunicationProtocol.Elmetro;
        set { if (value != DeviceCommunicationProtocol.Elmetro) throw new NotSupportedException("Устройство не поддерживает другие протоколы связи!"); }
    }

    public ILogger Logger { get; set; }

    /// <summary>
    /// Количество попыток пересылки команды
    /// </summary>
    public int RetryCount { get; set; }

    public Guid Id { get; set; }

    #endregion

    #region Получение параметров из устройства

    public T? GetParameterValue<T>(Enum parameter) =>
        throw new NotSupportedException("Данный метод получения параметра не поддерживается устройством!");

    public Task<T?> GetParameterValueAsync<T>(Enum parameter, CancellationToken cancellationToken = default) =>
        throw new NotSupportedException("Данный метод получения параметра не поддерживается устройством!");

    #endregion

    #region Задание параметров устройства

    public bool SetParameterValue<T>(Enum parameter, T value) where T : notnull =>
        throw new NotSupportedException("Данный метод не поддерживается устройством!");

    public Task<bool> SetParameterValueAsync<T>(Enum parameter, T value, CancellationToken cancellationToken = default) where T : notnull =>
        throw new NotSupportedException("Данный метод не поддерживается устройством!");

    #endregion

    #region Обработка запросов

    /// <summary>
    /// Выполнить запрос и попытаться получить ответ
    /// </summary>
    /// <param name="request">Запрос</param>
    /// <typeparam name="TResponse">Ответ</typeparam>
    /// <returns>Ответ на запрос или null, если ничего не получили</returns>
    private TResponse? ExecuteRequest<TResponse>(DigitalElmetroRequest request)
        where TResponse : DigitalElmetroResponse, new()
    {
        var retries = RetryCount;
        do {
            try {
                request.Send(CommunicationChannel);
                return (TResponse)new TResponse().Read(CommunicationChannel);
            }
            catch (TimeoutException) {
                Logger.Error("Не удалось выполнить запрос за отведённое время! Оставшееся кол-во попыток: {RetriesLeft}.", retries);
            }
            catch (ElmetroProtocolException ex) {
                Logger.Error(ex, "Ошибка при передаче данных! Оставшееся кол-во попыток: {RetriesLeft}.", retries);
            }
        } while (retries-- > 0);
        return default;
    }

    /// <summary>
    /// Выполнить запрос и попытаться получить ответ асинхронно
    /// </summary>
    /// <typeparam name="TResponse">Ответ</typeparam>
    /// <param name="request">Запрос</param>
    /// <returns>Задача на получение ответа на запрос или null, если ничего не получили</returns>
    private async Task<TResponse?> ExecuteRequestAsync<TResponse>(DigitalElmetroRequest request, CancellationToken cancellationToken = default)
        where TResponse : DigitalElmetroResponse, new()
    {
        var retries = RetryCount;
        do {
            try {
                await request.SendAsync(CommunicationChannel, cancellationToken).ConfigureAwait(false);
                return (TResponse)await new TResponse().ReadAsync(CommunicationChannel, cancellationToken).ConfigureAwait(false);
            }
            catch (TimeoutException) {
                Logger.Error("Не удалось выполнить запрос за отведённое время! Оставшееся кол-во попыток: {RetriesLeft}.", retries);
            }
            catch (ElmetroProtocolException ex) {
                Logger.Error(ex, "Ошибка при передаче данных! Оставшееся кол-во попыток: {RetriesLeft}.", retries);
            }
        } while (retries-- > 0);
        return default;
    }

    #endregion

    #region Команды

    public float? GetPressure(PressureType pressureType = PressureType.DefaultPressure) => pressureType switch
    {
        PressureType.DefaultPressure => GetCurrentPressure(),
        PressureType.AbsolutePressure => GetCurrentPressure(),
        PressureType.ExcessPressure => GetCurrentPressure(),
        _ => throw new IndexOutOfRangeException()
    };

    public Task<float?> ReadPressureAsync(PressureType pressureType = PressureType.DefaultPressure, CancellationToken cancellationToken = default) =>
        pressureType switch
        {
            PressureType.DefaultPressure => GetCurrentPressureAsync(cancellationToken),
            PressureType.AbsolutePressure => GetCurrentPressureAsync(cancellationToken),
            PressureType.ExcessPressure => GetCurrentPressureAsync(cancellationToken),
            _ => throw new IndexOutOfRangeException()
        };


    /// <summary>
    /// Поддиапазон модуля
    /// </summary>
    /// <param name="subRangeIndex">Индекс поддиапазона</param>
    /// <returns>Информация о полученном диапазоне</returns>
    public ElmetroPressureRangeResponse? GetPressureRange(int subRangeIndex = 0) => ExecuteRequest<ElmetroPressureRangeResponse>(new ElmetroPressureRangeRequest(0));

    /// <summary>
    /// Поддиапазон модуля
    /// </summary>
    /// <param name="subRangeIndex">Индекс поддиапазона</param>
    /// <returns>Задача по получению информации о диапазоне</returns>
    public Task<ElmetroPressureRangeResponse?> GetPressureRangeAsync(int subRangeIndex = 0, CancellationToken cancellationToken = default) =>
        ExecuteRequestAsync<ElmetroPressureRangeResponse>(new ElmetroPressureRangeRequest(subRangeIndex), cancellationToken);

    /// <summary>
    /// Получить давление в ед.изм. устройства
    /// </summary>
    /// <returns>Значение давления в кПа</returns>
    public float? GetCurrentPressure() => ExecuteRequest<ElmetroPressureResponse>(new ElmetroPressureRequest())?.Value;

    /// <summary>
    /// Получить давление в ед.изм. устройства асинхронно
    /// </summary>
    /// <returns>Задача на получение значения давления в кПа</returns>
    public async Task<float?> GetCurrentPressureAsync(CancellationToken cancellationToken = default)
        => (await ExecuteRequestAsync<ElmetroPressureResponse>(new ElmetroPressureRequest(), cancellationToken).ConfigureAwait(false))?.Value;

    /// <summary>
    /// Получить информацию о текущем модуле
    /// </summary>
    public ElmetroInfo? GetDeviceInfo() =>
        ExecuteRequest<ElmetroPSInfoResponse>(new ElmetroPSInfoRequest()) is ElmetroPSInfoResponse info ? new ElmetroInfo(info) : null;

    /// <summary>
    /// Получить информацию о текущем модуле асинхронно
    /// </summary>
    /// <returns>Задача на получение информации о текущем модуле</returns>
    public async Task<ElmetroInfo?> GetDeviceInfoAsync(CancellationToken cancellationToken = default) =>
        await ExecuteRequestAsync<ElmetroPSInfoResponse>(new ElmetroPSInfoRequest(), cancellationToken).ConfigureAwait(false) is ElmetroPSInfoResponse info
        ? new ElmetroInfo(info)
        : null;

    #endregion
}