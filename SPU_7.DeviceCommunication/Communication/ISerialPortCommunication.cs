using System.IO.Ports;

namespace SPU_7.DeviceCommunication.Communication;

public interface ISerialPortCommunication : ICommunicationChannel, IDisposable
{
    /// <summary>
    /// Таймаут для чтения кол-ва байт данных, в мс
    /// </summary>
    int ReadTimeout { get; set; }

    /// <summary>
    /// Таймаут для записи кол-ва байт данных, в мс
    /// </summary>
    int WriteTimeout { get; set; }

    /// <summary>
    /// Количество бит в одном байте данных
    /// </summary>
    int DataBits { get; set; }

    /// <summary>
    /// Скорость передачи
    /// </summary>
    int BaudRate { get; set; }

    /// <summary>
    /// Проверка чётности битов
    /// </summary>
    Parity Parity { get; set; }

    /// <summary>
    /// Количество стоп-бит на байт
    /// </summary>
    StopBits StopBits { get; set; }

    /// <summary>
    /// Протокол управления портом
    /// </summary>
    Handshake Handshake { get; set; }

    /// <summary>
    /// Размер буфера чтения
    /// </summary>
    int ReadBufferSize { get; set; }

    /// <summary>
    /// Размер буфера записи
    /// </summary>
    int WriteBufferSize { get; set; }

    /// <summary>
    /// Очищает буфер прёма данных
    /// </summary>
    //void DiscardInBuffer();

    /// <summary>
    /// Очищает буфер вывода данных
    /// </summary>
    //void DiscardOutBuffer();
}