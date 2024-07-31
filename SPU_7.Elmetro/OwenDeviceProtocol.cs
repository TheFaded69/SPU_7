using DeviceCommunication.Communication;
using DeviceCommunication.Extensions;
using DeviceCommunication.OwenProtocol;
using NLog;

namespace DeviceCommunication.Devices;

public class OwenDeviceProtocol : IOwenDeviceProtocol
{
    public OwenDeviceProtocol(ICommunicationChannel? deviceCommunication)
    {
        CommunicationChannel = deviceCommunication;
        RetryCount = 3;

        _oldNewLineSet = Environment.NewLine;
        _deviceNewLineSet = "\r";
    }

    private string _oldNewLineSet;
    private string _deviceNewLineSet;

    public Guid Id { get; set; }

    public ICommunicationChannel? CommunicationChannel { get; set; }

    public int Address { get; set; }
    public int RetryCount { get; set; }

    /// <summary>
    /// Лог ошибок при работе с устройством
    /// </summary>
    private ILogger Logger => LogManager.GetLogger(GetType().FullName);

    /// <summary>
    /// Получить данные параметра
    /// </summary>
    /// <param name="parameter">Параметр для запроса</param>
    /// <returns>Значение параметра или null, если не удалось прочитать</returns>
    public OwenResponse? GetParameterData(Enum parameter)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для устройства ОВЕН!");
        var parameterValue = Convert.ToUInt16(parameter);
        var request = new OwenRequest(Address, parameterValue);
        var response = new OwenResponse();
        var retries = RetryCount;
        do {
            try {
                _oldNewLineSet = CommunicationChannel.NewLine;
                CommunicationChannel.NewLine = _deviceNewLineSet;
                CommunicationChannel.Write(request.Data);
                response.Get(CommunicationChannel);
                CommunicationChannel.NewLine = _oldNewLineSet;
                //if (Address != response.Address || parameterValue != response.Parameter) continue;
                return response;
            }
            catch (TimeoutException) {
                Logger.Error("Не удалось получить значение парметра по номеру 0x{ParameterNumber:X4}, Параметр: {ParameterDescription}, Осталось попыток: {RetryCount}",
                    parameterValue, parameter.GetDescription(), retries);
            }
            catch (OwenProtocolCrcException owenEx) {
                Logger.Error("Не пройдена проверка целостности для парметра по номеру 0x{ParameterNumber:X4}, Параметр: {ParameterDescription}, Осталось попыток: {RetryCount}, {OwenExceptionMessage}",
                    parameterValue, parameter.GetDescription(), retries, owenEx.Message);
            }
            catch(Exception ex) {
                Logger.Error(ex, "Не удалось получить значение парметра по номеру 0x{ParameterNumber:X4}, Параметр: {ParameterDescription}", parameterValue, parameter.GetDescription());
                throw;
            }
        } while (retries-- > 0);
        return null;
    }

    /// <summary>
    /// Получить данные параметра асинхронно
    /// </summary>
    /// <param name="parameter">Параметр для запроса</param>
    /// <returns>Задача на получение значения параметра или null, если не удалось прочитать</returns>
    public async Task<OwenResponse?> GetParameterDataAsync(Enum parameter, CancellationToken cancellationToken = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для устройства ОВЕН!");
        var parameterValue = Convert.ToUInt16(parameter);
        var request = new OwenRequest(Address, parameterValue);
        var response = new OwenResponse();
        var retries = RetryCount;
        do {
            try {
                return await CommunicationChannel.TryProcessRequestAndResponseAsync(async () =>
                {
                    _oldNewLineSet = CommunicationChannel.NewLine;
                    CommunicationChannel.NewLine = _deviceNewLineSet;
                    if (CommunicationChannel is SerialPortCommunication serialPort) {
                        serialPort.DiscardInBuffer();
                        serialPort.DiscardOutBuffer();
                    }
                    await CommunicationChannel.WriteAsync(request.Data, cancellationToken).ConfigureAwait(false);
                    await response.GetAsync(CommunicationChannel).ConfigureAwait(false);
                    CommunicationChannel.NewLine = _oldNewLineSet;
                    //if (Address != response.Address || parameterValue != response.Parameter) continue;
                    return response;
                }).ConfigureAwait(false);
            }
            catch (TimeoutException) {
                Logger.Error("Не удалось получить значение парметра по номеру 0x{ParameterNumber:X4}, Параметр: {ParameterDescription}, Осталось попыток: {RetryCount}",
                    parameterValue, parameter.GetDescription(), retries);
            }
            catch (OwenProtocolCrcException owenEx) {
                Logger.Error("Не пройдена проверка целостности для парметра по номеру 0x{ParameterNumber:X4}, Параметр: {ParameterDescription}, Осталось попыток: {RetryCount}, {OwenExceptionMessage}",
                    parameterValue, parameter.GetDescription(), retries, owenEx.Message);
            }
            catch (Exception ex) {
                Logger.Error(ex, "Не удалось получить значение парметра по номеру 0x{ParameterNumber:X4}, Параметр: {ParameterDescription}", parameterValue, parameter.GetDescription());
                throw;
            }
        } while (retries-- > 0);
        return null;
    }
}