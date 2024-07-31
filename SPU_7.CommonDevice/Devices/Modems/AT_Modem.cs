using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using SPU_7.DeviceCommunication.Base;
using SPU_7.DeviceCommunication.Communication;

namespace SPU_7.CommonDevice.Devices.Modems;

public class AT_Modem : BindableBase, ICommunicationChannel
{
    public AT_Modem(ISerialPortCommunication? serialPort)
    {
        CommunicationChannel = serialPort;
    }

    private void CommunicationChannel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        // Перенаправление события изменения свойств последовательного порта на класс модема
        if (IsOpen) RaisePropertyChanged(e.PropertyName);
    }

    private ISerialPortCommunication? _serialPortCommunication;
    private string _newLine = "\r\n";
    private string _oldNewLine = "\n";
    private bool _isOpen;
    private int _requestTimeout;
    private int _requestDelay;
    private string _connection = string.Empty;
    private string _okResponse = "OK";
    private string _errorResponse = "ERROR";

    private static Regex ValueRegex => new(@"[+-]?(\d+([.,]\d*)?(\d+)?|[.,]\d+)");

    public ISerialPortCommunication? CommunicationChannel
    {
        get => _serialPortCommunication;
        set
        {
            if (EqualityComparer<ISerialPortCommunication>.Default.Equals(_serialPortCommunication, value) is false) {
                if (_serialPortCommunication is not null) {
                    _serialPortCommunication.PropertyChanged -= CommunicationChannel_PropertyChanged;
                }
                if (value?.IsOpen is true && _isOpen) {
                    value.PropertyChanged += CommunicationChannel_PropertyChanged;
                }
                _serialPortCommunication = value;
                RaisePropertyChanged();
            }
        }
    }

    public string OkResponse
    {
        get => _okResponse;
        set => SetProperty(ref _okResponse, value);
    }

    public string ErrorResponse
    {
        get => _errorResponse;
        set => SetProperty(ref _errorResponse, value);
    }

    public Guid Id { get; set; }

    public string Connection
    {
        get => _connection;
        set => SetProperty(ref _connection, value);
    }

    public bool IsOpen
    {
        get => CommunicationChannel?.IsOpen is true && _isOpen;
        protected set
        {
            var isChanged = SetProperty(ref _isOpen, value);
            if (_serialPortCommunication is null) return;
            if (isChanged && value is true) {
                _serialPortCommunication.PropertyChanged += CommunicationChannel_PropertyChanged;
            }
            else if (isChanged && value is false) {
                _serialPortCommunication.PropertyChanged -= CommunicationChannel_PropertyChanged;
            }
        }
    }

    public bool IsBusy => CommunicationChannel?.IsBusy is true && IsOpen;

    public bool IsReceive => CommunicationChannel?.IsReceive is true && IsOpen;

    public bool IsTransmit => CommunicationChannel?.IsTransmit is true && IsOpen;

    public string NewLine
    {
        get => CommunicationChannel?.NewLine ?? _newLine;
        set
        {
            if (SetProperty(ref _newLine, value) && CommunicationChannel is not null && CommunicationChannel.NewLine != _newLine) {
                _oldNewLine = CommunicationChannel.NewLine;
                CommunicationChannel.NewLine = _newLine;
            }
        }
    }

    public int RequestTimeout
    {
        get => _requestTimeout;
        set => _requestTimeout = value;
    }

    public int RequestDelay
    {
        get => _requestDelay;
        set => _requestDelay = value;
    }

    public int BytesToRead => CommunicationChannel?.BytesToRead ?? 0;
    public int BytesToWrite => CommunicationChannel?.BytesToWrite ?? 0;

    public void DiscardInBuffer()
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        CommunicationChannel.DiscardInBuffer();
    }

    public void DiscardOutBuffer()
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        CommunicationChannel.DiscardOutBuffer();
    }

    /// <summary>
    /// Послать команду модему
    /// </summary>
    /// <param name="command">Команда</param>
    /// <returns>Ответ модема</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Task<string?> SendCommandAsync(string command, CancellationToken cancellationToken = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException();
        return CommunicationChannel.TryProcessRequestAndResponseAsync<string?>(async () =>
        {
            if (CommunicationChannel.BytesToWrite > 0) await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
            if (CommunicationChannel.BytesToRead > 0) await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
            // SendLineAsync
            await CommunicationChannel.WriteLineAsync(command, cancellationToken).ConfigureAwait(false);
            // ReadLineAsync
            string commandResponse = string.Empty;
            string currentResponse = string.Empty;
            do {
                currentResponse = await CommunicationChannel.ReadLineAsync(cancellationToken).ConfigureAwait(false);
                commandResponse += $"{currentResponse}{Environment.NewLine}";
            }
            while (currentResponse.Contains(OkResponse) is false
                && currentResponse.Contains(ErrorResponse) is false);
            return commandResponse;
        });
    }

    public async Task<bool> TryWriteCommandAsync(string command, CancellationToken cancellationToken = default) =>
        (await SendCommandAsync(command, cancellationToken).ConfigureAwait(false))?.Contains(OkResponse) is true;

    public Task<string?> ReceiveCommandAsync(string command, CancellationToken cancellationToken = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException();
        return CommunicationChannel.TryProcessRequestAndResponseAsync<string?>(async () =>
        {
            if (CommunicationChannel.BytesToWrite > 0) await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
            if (CommunicationChannel.BytesToRead > 0) await CommunicationChannel.FlushAsync(cancellationToken).ConfigureAwait(false);
            string currentResponse = string.Empty;
            string commandResponse = string.Empty;
            do {
                currentResponse = await CommunicationChannel.ReadLineAsync(cancellationToken).ConfigureAwait(false);
                commandResponse += $"{currentResponse}{Environment.NewLine}";
            }
            while (currentResponse.Contains(command) is false
                && currentResponse.Contains(ErrorResponse) is false
                && cancellationToken.IsCancellationRequested is false);
            return commandResponse;
        });
    }

    public async Task<bool> TryReceiveCommandAsync(string command, CancellationToken cancellationToken = default) =>
        (await ReceiveCommandAsync(command, cancellationToken).ConfigureAwait(false))?.Contains(command) is true;

    /// <summary>
    /// Получить текущий уровень сигнала RSSI
    /// </summary>
    public async Task<double?> GetSignalLevelAsync(CancellationToken cancellationToken = default)
    {
        var response = await SendCommandAsync("AT+CSQ", cancellationToken: cancellationToken).ConfigureAwait(false);
        return response is string rm && double.TryParse(ValueRegex.Match(rm).Value, out var value) ? value : null;
    }

    public virtual Task<bool> ConnectAsync(string connectionName, CancellationToken token = default)
    {
        //IsOpen = await ReceiveCommandAsync(_connection, token);
        return ConnectAsync(token);
    }

    public virtual async Task<bool> ConnectAsync(CancellationToken token = default)
    {
        IsOpen = await TryReceiveCommandAsync("RING", token);
              //&& await ReceiveCommandAsync("ATA", token);
        return IsOpen;
    }

    public virtual async Task CloseAsync(CancellationToken token = default)
    {
        await Task.Delay(1000, token);
        await WriteAsync(Encoding.ASCII.GetBytes("+++"), token);
        await Task.Delay(1000, token);
        //await SendCommandAsync("ATH", cancellationToken: token);
        IsOpen = false;
    }

    public virtual Task<ArraySegment<byte>> ReadAsync(int dataSize, CancellationToken token = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        return CommunicationChannel.ReadAsync(dataSize, token);
    }

    public virtual Task<int> ReadAsync(ArraySegment<byte> data, CancellationToken token = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        return CommunicationChannel.ReadAsync(data, token);
    }

    public virtual Task<string> ReadLineAsync(CancellationToken token = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        return CommunicationChannel.ReadLineAsync(token);
    }

    public virtual Task WriteAsync(ArraySegment<byte> data, CancellationToken token = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        return CommunicationChannel.WriteAsync(data, token);
    }

    public virtual Task WriteLineAsync(string data, CancellationToken token = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        return CommunicationChannel.WriteLineAsync(data, token);
    }

    public virtual Task<T> TryProcessRequestAndResponseAsync<T>(Func<Task<T>> func)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        return CommunicationChannel.TryProcessRequestAndResponseAsync(func);
    }

    public virtual Task FlushAsync(CancellationToken cancellationToken = default)
    {
        if (CommunicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для модема!");
        return CommunicationChannel.FlushAsync(cancellationToken);
    }

    #region Нереализованные функции интерфейса (Синхронное выполнение)
    public virtual bool Connect(string connectionName) => throw new NotImplementedException();

    public virtual bool Connect() => throw new NotImplementedException();
    public virtual void Close() => throw new NotImplementedException();

    public ArraySegment<byte> Read(int dataSize) => throw new NotImplementedException();

    public void Read(ArraySegment<byte> data, int offset, int dataSize) => throw new NotImplementedException();

    public string ReadLine() => throw new NotImplementedException();

    public void Write(ArraySegment<byte> data) => throw new NotImplementedException();

    public void WriteLine(string data) => throw new NotImplementedException();

    public T TryProcessRequestAndResponse<T>(Func<T> func) => throw new NotImplementedException();
    #endregion
}
