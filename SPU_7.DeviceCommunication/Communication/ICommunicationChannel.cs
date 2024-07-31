using System.ComponentModel;

namespace SPU_7.DeviceCommunication.Communication;

public interface ICommunicationChannel : INotifyPropertyChanged
{
    /// <summary>
    /// Уникальный идентификатор канала связи
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    /// Строка подключения
    /// </summary>
    string Connection { get; set; }

    /// <summary>
    /// Открыт ли канал связи
    /// </summary>
    bool IsOpen { get; }

    /// <summary>
    /// Занят ли канал связи сейчас какой ни будь задачей
    /// </summary>
    bool IsBusy { get; }

    /// <summary>
    /// Происходит ли сейчас получение данных
    /// </summary>
    bool IsReceive { get; }

    /// <summary>
    /// Происходит ли сейчас отправка данных
    /// </summary>
    bool IsTransmit { get; }

    /// <summary>
    /// Символы новой строки для распознования её конца
    /// </summary>
    string NewLine { get; set; }

    /// <summary>
    /// Таймаут ожидания очереди для обработки запроса в миллисекундах
    /// </summary>
    int RequestTimeout { get; set; }

    /// <summary>
    /// Задержка между запросами в миллисекундах ()
    /// </summary>
    int RequestDelay { get; set; }

    /// <summary>
    /// Кол-во байт доступных для чтения
    /// </summary>
    int BytesToRead { get; }

    /// <summary>
    /// Кол-во байт оставшихся для записи
    /// </summary>
    int BytesToWrite { get; }

    #region Синхронные методы

    /// <summary>
    /// Открыть соединение
    /// </summary>
    /// <param name="connectionName">Полный адрес для соединения/название порта</param>
    bool Connect(string connectionName);

    /// <summary>
    /// Открыть соединение с уже известным источником
    /// </summary>
    bool Connect();

    /// <summary>
    /// Закрыть соединение
    /// </summary>
    void Close();

    /// <summary>
    /// Очищает буфер прёма данных
    /// </summary>
    void DiscardInBuffer();

    /// <summary>
    /// Очищает буфер вывода данных
    /// </summary>
    void DiscardOutBuffer();

    #region Чтение

    /// <summary>
    /// Прочитать определённое количество байт
    /// </summary>
    /// <param name="dataSize">Количество байт для чтения</param>
    /// <returns>Массив байт</returns>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    ArraySegment<byte> Read(int dataSize);

    /// <summary>
    /// Прочитать определённое количество байт
    /// </summary>
    /// <param name="data">Буфер для приёма данных</param>
    /// <param name="offset">Смещение в принимающем буфере</param>
    /// <param name="dataSize">Количество байт для чтения</param>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    void Read(ArraySegment<byte> data, int offset, int dataSize);

    /// <summary>
    /// Чтение строки
    /// </summary>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    string ReadLine();

    #endregion
    #region Запись

    /// <summary>
    /// Записать количество байт
    /// </summary>
    /// <param name="data">Байты для записи</param>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    void Write(ArraySegment<byte> data);

    /// <summary>
    /// Запись строки
    /// </summary>
    /// <param name="data">Строка для записи</param>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    void WriteLine(string data);

    /// <summary>
    /// Попытаться обработать запрос (канал связи будет занят для обработки запроса)
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения функции</typeparam>
    /// <param name="func">Функция для выполнения</param>
    /// <returns>Данные</returns>
    /// <exception cref="TimeoutException"></exception>
    [Obsolete("Операции ввода/вывода должны выполняться асинхронно")]
    T TryProcessRequestAndResponse<T>(Func<T> func);

    #endregion
    #endregion

    #region Асинхронные методы

    /// <summary>
    /// Открыть соединение асинхронно
    /// </summary>
    /// <param name="connectionName"></param>
    Task<bool> ConnectAsync(string connectionName, CancellationToken token = default);

    /// <summary>
    /// Открыть соединение с уже известным источником асинхронно
    /// </summary>
    Task<bool> ConnectAsync(CancellationToken token = default);

    /// <summary>
    /// Закрыть соединение асинхронно
    /// </summary>
    Task CloseAsync(CancellationToken token = default);

    /// <summary>
    /// Попытаться обработать запрос (канал связи будет занят для обработки запроса)
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения асинхронной функции</typeparam>
    /// <param name="func">Асинхронная функция для выполнения</param>
    /// <returns>Задача с ожиданием данных</returns>
    /// <exception cref="TimeoutException"></exception>
    Task<T> TryProcessRequestAndResponseAsync<T>(Func<Task<T>> func);

    /// <summary>
    /// 
    /// </summary>
    Task FlushAsync(CancellationToken cancellationToken = default);

    #region Чтение

    /// <summary>
    /// Прочитать определённое количество байт асинхронно
    /// </summary>
    /// <param name="dataSize">Количество байт для чтения</param>
    /// <param name="token">Отмена задачи чтения</param>
    /// <returns>Задача по ожиданию массива байт</returns>
    Task<ArraySegment<byte>> ReadAsync(int dataSize, CancellationToken token = default);

    /// <summary>
    /// Прочитать определённое количество байт асинхронно
    /// </summary>
    /// <param name="data">Буфер для приёма данных</param>
    /// <param name="token">Отмена задачи чтения</param>
    /// <returns>Задача по ожиданию окончания операции чтения</returns>
    Task<int> ReadAsync(ArraySegment<byte> data, CancellationToken token = default);

    /// <summary>
    /// Чтение строки асинхронно
    /// </summary>
    /// <param name="token">Отмена задачи чтения</param>
    Task<string> ReadLineAsync(CancellationToken token = default);

    #endregion
    #region Запись

    /// <summary>
    /// Записать количество байт асинхронно
    /// </summary>
    /// <param name="data">Байты для записи</param>
    /// <returns>Задача по ожиданию окончания операции записи</returns>
    Task WriteAsync(ArraySegment<byte> data, CancellationToken token = default);

    /// <summary>
    /// Запись строки асинхронно
    /// </summary>
    /// <param name="data">Данные для записи</param>
    /// <returns>Задача по ожиданию записи</returns>
    Task WriteLineAsync(string data, CancellationToken token = default);

    #endregion

    #endregion
}
