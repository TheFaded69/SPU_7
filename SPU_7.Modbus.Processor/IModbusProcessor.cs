using System.Collections.Concurrent;
using SPU_7.Common.Modbus;
using SPU_7.Modbus.Processor.Communicators;

namespace SPU_7.Modbus.Processor;

/// <summary>
/// Основной класс, который осуществляет все действия по протоколу Modbus.
/// Устанавливает связь с устройством. Отправляет команды и обрабатывает ответы.
/// Обрабатывает очереди команд.
/// </summary>
public interface IModbusProcessor
{
    ICommunicator Communicator { get; set; }

    /// <summary>
    /// Список авто-запросов
    /// </summary>
    BlockingCollection<ModbusRequestResponse> AutoRequestResponses { get; }

    /// <summary>
    /// Адрес устройства
    /// </summary>
    int DeviceAddress { get; set; }

    /// <summary>
    /// Адрес ПП
    /// </summary>
    int PrimaryConverterAddress { get; set; }

    /// <summary>
    /// Настройки протокола
    /// </summary>
    ProtocolSettings ProtocolSettings { get; set; }

    /// <summary>
    /// Позволяет сменить коммуникатор (асинхронный)
    /// </summary>
    /// <param name="communicator">Класс, реализующий соединение</param>
    Task SetCommunicatorAsync(ICommunicator communicator);

    /// <summary>
    /// Позволяет сменить коммуникатор
    /// </summary>
    /// <param name="communicator">Класс, реализующий соединение</param>
    void SetCommunicator(ICommunicator communicator);

    /// <summary>
    /// Установить связь с устройством
    /// </summary>
    /// <returns>true если установлено</returns>
    bool Start();

    /// <summary>
    /// Разорвать связь с устройством (асинхронный)
    /// </summary>
    Task CloseAsync();

    /// <summary>
    /// Разорвать связь с устройством
    /// </summary>
    void Stop();

    /// <summary>
    /// Готовность коммуникатора (открыт ли порт, например)
    /// </summary>
    /// <returns></returns>
    bool IsReady();

    /// <summary>
    /// Добавляет запрос в очередь
    /// </summary>
    /// <param name="requestResponse">запрос-ответ</param>
    void EnqueueRequest(ModbusRequestResponse requestResponse);

    /// <summary>
    /// Добавляет группу запросов в очередь
    /// </summary>
    /// <param name="requestResponses">список запросов-ответов</param>
    void EnqueueRequests(IEnumerable<ModbusRequestResponse> requestResponses);

    /// <summary>
    /// запуск авто-запросов
    /// </summary>
    void StartAutoRequests();

    /// <summary>
    /// остановка авто-запросов
    /// </summary>
    void StopAutoRequests();

    /// <summary>
    /// очистить список авто-запросов
    /// </summary>
    void ClearAutoRequestList();

    /// <summary>
    /// Добавить запросы в список автозапросов
    /// </summary>
    void AddRequestsToAutoRequestList(List<ModbusRequestResponse> requestResponses);

    /// <summary>
    /// Стартует поток обработки очереди запросов
    /// </summary>
    void StartRequestDispatcher();

    /// <summary>
    /// Останавливает поток обработки очереди запросов
    /// </summary>
    void StopRequestDispatcher();

    /// <summary>
    /// завершает все процессы и акзывает коммуникатор
    /// </summary>
    void ShutDown();
}