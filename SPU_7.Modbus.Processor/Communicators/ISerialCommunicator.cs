using System.Collections.ObjectModel;
using System.IO.Ports;
using SPU_7.Common.Stand;

namespace SPU_7.Modbus.Processor.Communicators;

/// <summary>
/// последовательный порт
/// </summary>
public interface ISerialCommunicator : ICommunicator
{
    void SetSerialPort(string portName);
    void SetSerialPort(string portName, int baudRate);
    void SetSerialPort(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits, Handshake handshake, bool dtrEnable, bool rtsEnable);

    /// <summary>
    /// Значение, представляющее конец строки 
    /// </summary>
    /// <param name="text"></param>
    void SetNewLine(string text);

    /// <summary>
    /// Очистка буферов
    /// </summary>
    void ClearBuffers();

    /// <summary>
    /// Прочитать имя порта коммуникатора
    /// </summary>
    string GetPortName();
    
    public void AddCollectionToLogger(ObservableCollection<LogMessage> observableCollection);
}