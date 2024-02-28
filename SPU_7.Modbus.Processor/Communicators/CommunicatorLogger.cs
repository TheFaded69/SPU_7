using System.Collections.ObjectModel;
using SPU_7.Common.Stand;

namespace SPU_7.Modbus.Processor.Communicators;

public class CommunicatorLogger : IDisposable
{
    public CommunicatorLogger(string name, string logPath = null)
    {
        var path = logPath != null
            ? $"{logPath}/{LogDirectory}/{name}_{DateTime.Now:yyyy-MM-dd}.log"
            : $"{Directory.GetCurrentDirectory()}/{LogDirectory}/{name}_{DateTime.Now:yyyy-MM-dd}.log";
        try
        {
            var dir = Path.GetDirectoryName(path);
            if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
            _streamWriter = new StreamWriter(path, true);
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private const string LogDirectory = "./SerialPortLogs";
    private readonly StreamWriter _streamWriter;

    private string GetHexString(byte[] data) =>
        data == null ? "" : data.Select(b => $"{b:X2}").Aggregate((str, hex) => str + "-" + hex);

    private void Write(string message, byte[] data)
    {
        if (_streamWriter != null && data != null)
        {
            _streamWriter.WriteLine($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}\t{data.Length}b\t{GetHexString(data)}");
            _streamWriter.Flush();
            
            foreach (var collection in _collections)
            {
                collection?.Add(new LogMessage($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}\t{data.Length}b\t{GetHexString(data)}", LogLevel.Info));
            }
        }
    }

    private async Task WriteAsync(string message)
    {
        if (_streamWriter != null)
        {
            await _streamWriter.WriteLineAsync($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}");
            await _streamWriter.FlushAsync();
            
            foreach (var collection in _collections)
            {
                collection?.Add(new LogMessage($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}", LogLevel.Info));
            }
        }
    }

    private void Write(string message)
    {
        _streamWriter?.WriteLine($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}");
        _streamWriter?.Flush();

        foreach (var collection in _collections)
        {
            collection?.Add(new LogMessage($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}", LogLevel.Info));
        }
    }

    private async Task WriteAsync(string message, byte[] data)
    {
        if (_streamWriter != null && data != null)
        {
            await _streamWriter.WriteLineAsync($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}\t{data.Length}b\t{GetHexString(data)}");
            await _streamWriter.FlushAsync();
            
            foreach (var collection in _collections)
            {
                collection?.Add(new LogMessage($"{DateTime.Now:dd.MM.yyyy HH:mm:ss.ffff}\t{message}\t{data.Length}b\t{GetHexString(data)}", LogLevel.Info));
            }
        }
    }

    public void WriteStartServer(string address) => Write($"START {address}");

    public async Task WriteStartServerAsync(string address) => await WriteAsync($"START {address}");

    public void WriteOpenPort(string port) => Write($"OPEN {port}");

    public async Task WriteOpenPortAsync(string port) => await WriteAsync($"OPEN {port}");

    public void WriteClosePort(string port) => Write($"CLOSE {port}");

    public async Task WriteClosePortAsync(string port) => await WriteAsync($"CLOSE {port}");

    public void WriteStopServer(string address) => Write($"STOP {address}");

    public async Task WriteStopServerAsync(string address) => await WriteAsync($"STOP {address}");

    public void WriteSendData(byte[] data) => Write("SEND:", data);

    public async Task WriteSendDataAsync(byte[] data) => await WriteAsync("SEND:", data);

    public void WriteSendText(string text) => Write($"SEND: {text}");

    public async Task WriteSendTextAsync(string text) => await WriteAsync($"SEND: {text}");

    public void WriteSendTimeout(int timeout) => Write($"SEND TIMEOUT ({timeout})");

    public async Task WriteSendTimeoutAsync(int timeout) => await WriteAsync($"SEND TIMEOUT ({timeout})");

    public void WriteSendException(string e) => Write($"SEND EXCEPTION: {e}");

    public async Task WriteSendExceptionAsync(string e) => await WriteAsync($"SEND EXCEPTION: {e}");

    public void WriteReceiveData(byte[] data) => Write("RECV:", data);

    public async Task WriteReceiveDataAsync(byte[] data) => await WriteAsync("RECV:", data);

    public void WriteReceiveText(string text) => Write($"RECV: {text}");

    public async Task WriteReceiveTextAsync(string text) => await WriteAsync($"RECV: {text}");

    public void WriteReceiveTimeout(int timeout) => Write($"RECV TIMEOUT ({timeout})");

    public async Task WriteReceiveTimeoutAsync(int timeout) => await WriteAsync("RECV TIMEOUT ({timeout})");

    public void WriteReceiveException(string e) => Write($"RECV EXCEPTION: {e}");

    public async Task WriteReceiveExceptionAsync(string e) => await WriteAsync($"RECV EXCEPTION: {e}");

    public void WriteMessage(string message) => Write($"{message}");

    public async Task WriteMessageAsync(string message) => await WriteAsync($"{message}");

    public void Dispose()
    {
        try
        {
            _streamWriter?.Dispose();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private List<ObservableCollection<LogMessage>> _collections = new();
    public void AddCollectionToLogger(ObservableCollection<LogMessage> messages)
    {
        _collections.Add(messages);
    }
}