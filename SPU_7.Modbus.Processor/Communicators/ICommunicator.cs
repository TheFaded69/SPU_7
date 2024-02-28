using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Processor.Communicators;

/// <summary>
/// последовательный порт
/// </summary>
public interface ICommunicator
{
    /// <summary>
    /// тип протокола
    /// </summary>
    ModbusProtocolType ProtocolType { get; }

    /// <summary>
    /// Установка протокола
    /// </summary>
    /// <param name="protocolType">протокол</param>
    void SetProtocol(ModbusProtocolType protocolType);

    /// <summary>
    /// готовность к связи
    /// </summary>
    bool IsReady { get; }

    /// <summary>
    /// флаг занятости
    /// </summary>
    bool IsBusy { get; }

    /// <summary>
    /// флаг активности передачи
    /// </summary>
    bool Tx { get; }

    /// <summary>
    /// флаг активности приема
    /// </summary>
    bool Rx { get; }

    /// <summary>
    /// Нужно ли писать лог
    /// </summary>
    bool IsLogging { get; set; }

    /// <summary>
    /// Открыть/стартовать коммуникатор
    /// </summary>
    void Open();

    /// <summary>
    /// Закрыть/остановить коммуникатор
    /// </summary>
    Task CloseAsync();

    /// <summary>
    /// Закрыть/остановить коммуникатор
    /// </summary>
    void Close();

    /// <summary>
    /// Получить данные
    /// </summary>
    /// <param name="count">количество ожидаемых байт</param>
    /// <param name="timeout">таймаут ожидания в мс</param>
    /// <returns>массив байт прочитанных данных</returns>
    Task<byte[]> ReceiveAsync(int count, int timeout = 2000);

    /// <summary>
    /// Получить данные
    /// </summary>
    /// <param name="count">количество ожидаемых байт</param>
    /// <param name="timeout">таймаут ожидания в мс</param>
    /// <returns>массив байт прочитанных данных</returns>
    byte[] Receive(int count, int timeout = 1000);

    /// <summary>
    /// Прочесть строку из порта
    /// </summary>
    /// <param name="timeout">таймаут в мс</param>
    string ReceiveLine(int timeout = 3000);

    /// <summary>
    /// Прочесть строку из порта
    /// </summary>
    /// <param name="timeout">таймаут в мс</param>
    public Task<string> ReceiveLineAsync(int timeout = 3000);

    /// <summary>
    /// Отослать данные
    /// </summary>
    /// <param name="data">отсылаемые данные</param>
    /// <param name="timeout">таймаут завершения посылки</param>
    /// <returns>true - удачная посылка</returns>
    Task<bool> SendAsync(byte[] data, int timeout = 2000);

    /// <summary>
    /// Отослать данные
    /// </summary>
    /// <param name="data">отсылаемые данные</param>
    /// <param name="timeout">таймаут завершения посылки</param>
    /// <returns>true - удачная посылка</returns>
    bool Send(byte[] data, int timeout = 2000);

    /// <summary>
    /// отправить строку без завершающего NewLine
    /// </summary>
    /// <param name="text">строка</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    bool Send(string text, int timeout = 2000);

    /// <summary>
    /// отправить строку
    /// </summary>
    /// <param name="text">строка</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    bool SendLine(string text, int timeout = 2000);

    /// <summary>
    /// отправить строку с завершающим NewLine
    /// </summary>
    /// <param name="text">строка</param>
    /// <param name="timeout">мс</param>
    /// <returns>true - если удачно</returns>
    Task<bool> SendLineAsync(string text, int timeout = 2000);
}