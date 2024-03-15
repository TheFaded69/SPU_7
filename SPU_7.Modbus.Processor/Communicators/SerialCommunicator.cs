using System.Collections.ObjectModel;
using System.IO.Ports;
using SPU_7.Common.Stand;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Processor.Communicators;

public class SerialCommunicator : ISerialCommunicator, IDisposable
{
    public SerialCommunicator()
    {
        SerialPort = new SerialPort();
        ProtocolType = ModbusProtocolType.ModbusRtu;
    }

    private bool _isLogging;
    private CommunicatorLogger _logger;

    /// <summary>
    /// Нужно ли писать лог
    /// </summary>
    public bool IsLogging
    {
        get => _isLogging;
        set
        {
            if (value && !_isLogging)
                _logger = new CommunicatorLogger(SerialPort.PortName);
            else if (!value && _isLogging)
            {
                _logger?.Dispose();
                _logger = null;
            }
            _isLogging = value;
        }
    }

    /// <summary>
    /// тип протокола
    /// </summary>
    public ModbusProtocolType ProtocolType { get; private set; }

    public void SetProtocol(ModbusProtocolType protocolType) => ProtocolType = protocolType;

    protected SerialPort SerialPort { get; }

    public void SetSerialPort(string portName)
    {
        if (portName != SerialPort.PortName)
        {
            _logger?.Dispose();
            _logger = new CommunicatorLogger(portName);
        }
        if (SerialPort.IsOpen)
        {
            Close();
            SerialPort.PortName = portName;
            Open();
        }
        else
        {
            SerialPort.PortName = portName;
        }
    }

    public void SetSerialPort(string portName, int baudRate)
    {
        SetSerialPort(portName);
        SerialPort.BaudRate = baudRate;
    }

    public void SetSerialPort(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits, Handshake handshake, bool dtrEnable, bool rtsEnable)
    {

        SetSerialPort(portName, baudRate);
        SerialPort.DataBits = dataBits;
        SerialPort.Parity = parity;
        SerialPort.StopBits = stopBits;
        SerialPort.Handshake = handshake;
        SerialPort.DtrEnable = dtrEnable;
        SerialPort.RtsEnable = rtsEnable;
    }

    /// <summary>
    /// Значение, представляющее конец строки
    /// </summary>
    /// <param name="text"></param>
    public void SetNewLine(string text) => SerialPort.NewLine = text;

    /// <summary>
    /// флаг готовности
    /// </summary>
    public bool IsReady
    {
        get => SerialPort.IsOpen;
        private set { }
    }

    /// <summary>
    /// флаг занятости (производится прием или передача)
    /// </summary>
    public bool IsBusy { get; private set; }

    /// <summary>
    /// флаг активности передачи
    /// </summary>
    public bool Tx { get; private set; }

    /// <summary>
    /// флаг активности приема
    /// </summary>
    public bool Rx { get; private set; }

    /// <summary>
    /// открыть
    /// </summary>
    /// <returns>true если удачно</returns>
    public void Open()
    {
        if (SerialPort.IsOpen) return;
        try
        {
            IsBusy = false;
            Tx = false;
            Rx = false;
            _logger?.WriteOpenPort(SerialPort.PortName);
            SerialPort.Open();
            SerialPort.DiscardInBuffer();
            SerialPort.DiscardOutBuffer();
        }
        catch (Exception e)
        {
            _logger?.WriteMessage($"Не удалось открыть {SerialPort.PortName}");
            _logger?.WriteMessage(e.Message);
            IsBusy = false;
            Tx = false;
            Rx = false;
            throw;
        }
    }

    /// <summary>
    /// закрыть
    /// </summary>
    public async Task CloseAsync()
    {
        if (!SerialPort.IsOpen) return;
        await Task.Run(() =>
        {
            while (IsBusy)
            {
                Task.Delay(100);
            }
        });
        SerialPort.Close();
        if (_logger != null) await _logger.WriteClosePortAsync(SerialPort.PortName);
        IsBusy = false;
        Tx = false;
        Rx = false;
    }

    /// <summary>
    /// закрыть
    /// </summary>
    public void Close()
    {
        if (!SerialPort.IsOpen) return;
        SerialPort.Close();
        _logger?.WriteClosePort(SerialPort.PortName);
        IsBusy = false;
        Tx = false;
        Rx = false;
    }

    /// <summary>
    /// получить данные
    /// </summary>
    /// <param name="count">количество ожидаемых байт</param>
    /// <param name="timeout">мс</param>
    /// <returns>массив байт</returns>
    public async Task<byte[]> ReceiveAsync(int count, int timeout = 2000)
    {
        IsBusy = true;
        Rx = true;
        return await Task.Run(() =>
        {
            var data = new byte[count];
            try
            {
                SerialPort.ReadTimeout = timeout;
                var result = SerialPort.Read(data, 0, count);
                IsBusy = false;
                Rx = false;
                if (result > 0)
                {
                    _logger?.WriteReceiveData(data);
                    return data;
                }
                _logger?.WriteReceiveTimeout(timeout);
                return null;
            }
            catch (Exception e)
            {
                _logger?.WriteReceiveException(e.Message);
                IsBusy = false;
                Rx = false;
                Open();
                return null;
            }
        });
    }

    /// <summary>
    /// Прием из последовательного порта байтов
    /// </summary>
    /// <param name="count">ожидаемое количество байтов</param>
    /// <param name="timeout">таймаут в мс</param>
    public byte[] Receive(int count, int timeout = 1000)
    {
        SerialPort.ReadTimeout = timeout;
        IsBusy = true;
        Rx = true;
        // массив по размеру ожидаемого количества байтов
        var data = new byte[count];
        var offset = 0;
        // создаем задачу
        var ct = new CancellationTokenSource(timeout).Token;
        var data1 = data;
        var task = Task.Run(() =>
        {
            while (offset < count)
            {
                var readed = SerialPort.Read(data1, offset, count - offset);
                offset += readed;
                ct.ThrowIfCancellationRequested();
            }
        }, ct);
        try
        {
            // ждем либо приема count байтов, либо таймаут
            task.Wait(ct);
        }
        catch (Exception e)
        {
            _logger?.WriteReceiveException(e.Message);
            Open();
        }

        IsBusy = false;
        Rx = false;
        // если ничего не приняли
        if (offset == 0)
        {
            _logger?.WriteReceiveTimeout(timeout);
            return null;
        }
        // если приняли count байтов
        if (offset == count)
        {
            _logger?.WriteReceiveData(data);
            return data;
        }
        // если приняли меньше, то делаем ресайз массива
        Array.Resize(ref data, offset);
        _logger?.WriteReceiveData(data);
        return data;
    }

    /// <summary>
    /// Прочесть строку из порта
    /// </summary>
    /// <param name="timeout">таймаут в мс</param>
    public string ReceiveLine(int timeout = 3000)
    {
        try
        {
            IsBusy = true;
            Rx = true;
            SerialPort.ReadTimeout = timeout;
            var line = SerialPort.ReadLine();
            _logger?.WriteReceiveText(line);
            return line;
        }
        catch (Exception e)
        {
            _logger?.WriteReceiveException(e.Message);
            Open();
            return null;
        }
        finally
        {
            IsBusy = false;
            Rx = false;
        }
    }

    /// <summary>
    /// Прочесть строку из порта
    /// </summary>
    /// <param name="timeout">таймаут в мс</param>
    public async Task<string> ReceiveLineAsync(int timeout = 3000)
    {
        try
        {
            IsBusy = true;
            Rx = true;
            return await Task.Run(() =>
            {
                SerialPort.ReadTimeout = timeout;
                var line = SerialPort.ReadLine();
                _logger?.WriteReceiveText(line);
                return line;
            });
        }
        catch (Exception e)
        {
            if (_logger != null) await _logger.WriteReceiveExceptionAsync(e.Message);
            Open();
            return null;
        }
        finally
        {
            IsBusy = false;
            Rx = false;
        }
    }

    /// <summary>
    /// отправить данные
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    public async Task<bool> SendAsync(byte[] data, int timeout = 2000)
    {
        IsBusy = true;
        Tx = true;
        return await Task.Run(() =>
        {
            try
            {
                SerialPort.WriteTimeout = timeout;
                SerialPort.Write(data, 0, data.Length);
                _logger?.WriteSendData(data);
                IsBusy = false;
                Tx = false;
                return true;
            }
            catch (Exception e)
            {
                _logger?.WriteSendException(e.Message);
                IsBusy = false;
                Tx = false;
                Open();
                return false;
            }
        });
    }

    /// <summary>
    /// отправить данные
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    public bool Send(byte[] data, int timeout = 2000)
    {
        IsBusy = true;
        Tx = true;
        try
        {
            SerialPort.DiscardInBuffer();
            SerialPort.WriteTimeout = timeout;
            SerialPort.Write(data, 0, data.Length);
            _logger?.WriteSendData(data);
            IsBusy = false;
            Tx = false;
            return true;
        }
        catch (Exception e)
        {
            _logger?.WriteSendException(e.Message);
            IsBusy = false;
            Tx = false;
            Open();
            return false;
        }
    }

    /// <summary>
    /// отправить строку без завершающего NewLine
    /// </summary>
    /// <param name="text">строка</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    public bool Send(string text, int timeout = 2000)
    {
        IsBusy = true;
        Tx = true;
        try
        {
            SerialPort.DiscardInBuffer();
            SerialPort.WriteTimeout = timeout;
            SerialPort.Write(text);
            _logger?.WriteSendText(text);
            IsBusy = false;
            Tx = false;
            return true;
        }
        catch (Exception e)
        {
            _logger?.WriteSendException(e.Message);
            IsBusy = false;
            Tx = false;
            Open();
            return false;
        }
    }

    /// <summary>
    /// отправить строку с завершающим NewLine
    /// </summary>
    /// <param name="text">строка</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    public bool SendLine(string text, int timeout = 2000)
    {
        IsBusy = true;
        Tx = true;
        try
        {
            SerialPort.DiscardInBuffer();
            SerialPort.WriteTimeout = timeout;
            SerialPort.WriteLine(text);
            _logger?.WriteSendText(text);
            IsBusy = false;
            Tx = false;
            return true;
        }
        catch (Exception e)
        {
            _logger?.WriteSendException(e.Message);
            IsBusy = false;
            Tx = false;
            Open();
            return false;
        }
    }

    /// <summary>
    /// отправить строку с завершающим NewLine
    /// </summary>
    /// <param name="text">строка</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    public async Task<bool> SendLineAsync(string text, int timeout = 2000)
    {
        IsBusy = true;
        Tx = true;
        return await Task.Run(() =>
        {
            try
            {
                SerialPort.DiscardInBuffer();
                SerialPort.WriteTimeout = timeout;
                SerialPort.WriteLine(text);
                _logger?.WriteSendText(text);
                IsBusy = false;
                Tx = false;
                return true;
            }
            catch (Exception e)
            {
                _logger?.WriteSendException(e.Message);
                IsBusy = false;
                Tx = false;
                try
                {
                    Open();
                }
                catch (Exception)
                {
                    // ignored
                }
                return false;
            }
        });
    }

    /// <summary>
    /// Очистка буферов
    /// </summary>
    public void ClearBuffers()
    {
        SerialPort.DiscardOutBuffer();
        SerialPort.DiscardInBuffer();
    }

    /// <summary>
    /// Прочитать имя порта коммуникатора
    /// </summary>
    public string GetPortName() => SerialPort.PortName;

    public void AddCollectionToLogger(ObservableCollection<LogMessage> observableCollection)
    {
        _logger?.AddCollectionToLogger(observableCollection);
    }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    private void ReleaseUnmanagedResources()
    {
        _logger?.Dispose();
    }

    ~SerialCommunicator()
    {
        ReleaseUnmanagedResources();
    }
}