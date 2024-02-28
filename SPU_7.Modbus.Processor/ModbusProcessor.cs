using System.Collections.Concurrent;
using SPU_7.Common.Modbus;
using SPU_7.Modbus.Processor.Communicators;
using SPU_7.Modbus.Requests;
using SPU_7.Modbus.Responses;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Processor;

/// <summary>
/// Основной класс, который осуществляет все действия по протоколу Modbus.
/// Устанавливает связь с устройством. Отправляет команды и обрабатывает ответы.
/// Обрабатывает очереди команд.
/// </summary>
public class ModbusProcessor : IModbusProcessor
{
    /// <param name="requestSerializer">бинарный сериализатор запросов</param>
    /// <param name="responseDeserializer">бинарный десериализатор ответа</param>
    public ModbusProcessor(IRequestSerializer requestSerializer, IResponseDeserializer responseDeserializer)
    {
        _requestSerializer = requestSerializer;
        _responseDeserializer = responseDeserializer;
        _requestQueue = new BlockingCollection<ModbusRequestResponse>(10);
    }

    /// <summary>
    /// очередь запросов
    /// </summary>
    private readonly BlockingCollection<ModbusRequestResponse> _requestQueue;

    /// <summary>
    /// Поток, обрабатывающий очереди запросов
    /// </summary>
    private Thread _requestDispatcherThread;
    private CancellationTokenSource _requestDispatcherCancellation;

    /// <summary>
    /// Таймер для авто-запросов
    /// </summary>
    private Timer _autoRequestsTimer;

    private readonly IRequestSerializer _requestSerializer;
    private readonly IResponseDeserializer _responseDeserializer;

    #region Properties

    /// <summary>
    /// Адрес устройства
    /// </summary>
    public int DeviceAddress { get; set; }

    /// <summary>
    /// Адрес ПП
    /// </summary>
    public int PrimaryConverterAddress { get; set; }

    /// <summary>
    /// канал связи
    /// </summary>
    public ICommunicator Communicator { get; set; }

    /// <summary>
    /// список запросов, которые выполняются циклически
    /// </summary>
    public BlockingCollection<ModbusRequestResponse> AutoRequestResponses { get; } = new();

    /// <summary>
    /// Настройки протокола
    /// </summary>
    public ProtocolSettings ProtocolSettings { get; set; }

    #endregion

    #region Methods

    /// <summary>
    /// завершает все процессы и закрывает коммуникатор
    /// </summary>
    public void ShutDown()
    {
        Communicator?.Close();
        StopAutoRequests();
        StopRequestDispatcher();
    }

    #region Методы для установки соединения

    /// <summary>
    /// сменить коммуникатор
    /// </summary>
    /// <param name="communicator">Класс, реализующий соединение</param>
    public async Task SetCommunicatorAsync(ICommunicator communicator)
    {
        if (Communicator is { IsReady: true })
        {
            await Communicator.CloseAsync();
        }
        Communicator = communicator;
    }

    /// <summary>
    /// сменить коммуникатор
    /// </summary>
    /// <param name="communicator">Класс, реализующий соединение</param>
    public void SetCommunicator(ICommunicator communicator)
    {
        if (Communicator is { IsReady: true })
        {
            Communicator.Close();
        }
        Communicator = communicator;
    }

    /// <summary>
    /// Установить связь с устройством
    /// </summary>
    public bool Start()
    {
        /*StartRequestDispatcher();
        Communicator.Open();
        //if (Communicator.IsReady)
        // {
        //     StartRequestDispatcher();
        // }
        return Communicator.IsReady && _requestDispatcherThread is { IsAlive: true };*/

        Communicator.Open();
        if (Communicator.IsReady)
        {
            StartRequestDispatcher();
        }
        return Communicator.IsReady && _requestDispatcherThread is { IsAlive: true };
    }

    /// <summary>
    /// Разорвать связь с устройством (асинхронный)
    /// </summary>
    public async Task CloseAsync()
    {
        if (Communicator != null)
            await Communicator.CloseAsync();
    }

    /// <summary>
    /// Разорвать связь с устройством
    /// </summary>
    public void Stop() => Communicator?.Close();

    /// <summary>
    /// Готовность коммуникатора (открыт ли порт, например)
    /// </summary>
    public bool IsReady() => Communicator is { IsReady: true };

    #endregion

    #region Приватные методы для отправки и получения данных

    /// <summary>
    /// отправка запроса и получение ответа
    /// </summary>
    /// <param name="request">запрос</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private (ModbusRequestResult result, byte[] data) SendRequest(BaseRequest request)
    {
        if (!request.IsNoRequest)
        {
            var sendData = _requestSerializer.Serialize(request);
            // добавляем необходимое
            switch (Communicator.ProtocolType)
            {
                case ModbusProtocolType.ModbusRtu:
                    sendData = _requestSerializer.AddCrc(sendData);
                    break;
                case ModbusProtocolType.ModbusTcp:
                    sendData = _requestSerializer.AddMbapHeader(sendData);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // отсылаем пакет
            if (!Communicator.Send(sendData, ProtocolSettings.WriteTimeout))
                return (ModbusRequestResult.FailedDataTransmission, null);
        }
        var attemts = 30;
        while (attemts-- > 0)
        {
            // получаем ответ
            var receiveData = Communicator.Receive(request.GetResponseSize() + (Communicator.ProtocolType == ModbusProtocolType.ModbusRtu ? 2 : 6),
                ProtocolSettings.ReadTimeout);
            if (receiveData == null)
                return (ModbusRequestResult.FailedDataReception, null);
            // проверяем адрес
            if (!IsAddressValid(receiveData, Communicator.ProtocolType == ModbusProtocolType.ModbusRtu ? 0 : 6, request.DeviceAddress))
                continue;
            // проверяем CRC, если ModbusRtu
            if (Communicator.ProtocolType == ModbusProtocolType.ModbusRtu && !_responseDeserializer.CheckCrc(receiveData))
            {
                return (ModbusRequestResult.WrongCRC, null);
            }
            return (ModbusRequestResult.Success, receiveData);
        }
        return (ModbusRequestResult.FailedDataReception, null);
    }

    private bool IsAddressValid(byte[] data, int offset, byte validAddress) => data[0 + offset] == validAddress;

    /// <summary>
    /// Транзакция команды: запрос и получение ответа.
    /// </summary>
    /// <param name="request">запрос</param>
    /// <param name="parameters">массив типов параметров конструктора класса <see cref="TBaseResponse"/>, если он имеет параметры</param>
    /// <param name="parameterValues">массив значений параметров конструктора класса <see cref="TBaseResponse"/>, если он имеет параметры</param>
    /// <returns><see cref="BaseResponse"/></returns>
    private (ModbusRequestResult result, BaseResponse response) ModbusTransaction<TBaseResponse>(BaseRequest request, Type[] parameters = null,
        object[] parameterValues = null) where TBaseResponse : BaseResponse
    {
        // преамбула
        if (ProtocolSettings is { IsPreambleNeed: true, Preamble: { Length: > 0 } })
        {
            if (!Communicator.Send(ProtocolSettings.Preamble, ProtocolSettings.WriteTimeout))
                return (ModbusRequestResult.FailedDataTransmission, null);
            Thread.Sleep(ProtocolSettings.DelayAfterPreamble);
        }

        // отсылаем и получаем результат
        (var result, var receiveData) = SendRequest(request);
        if (result != ModbusRequestResult.Success)
            return (result, null);

        // если протокол ModbusTCP, то смещение 6 байт на величину MBAP
        var offset = Communicator.ProtocolType == ModbusProtocolType.ModbusTcp ? 6 : 0;
        // проверяем не является ли ответ ошибкой
        BaseResponse response;
        if (_responseDeserializer.IsResponseError(receiveData, offset + 1))
        {
            // если ошибка, то создаем экземпляр соответствующего класса
            response = new ErrorResponse();
        }
        else
        {
            // если не ошибка, то создаем экземляр класса TBaseResponse

            // если parameters не указаны (конструктор класса TBaseResponse без параметров), то
            // создаем пустые массивы, т.к. null передавать нельзя, конструктор найден не будет
            if (parameters == null)
            {
                parameters = Array.Empty<Type>();
                parameterValues = Array.Empty<object>();
            }
            // находим конструктор класса TBaseResponse,
            // если он с параметрами, то передаем ему типы parameters, по которым ищется соответствующий конструктор и значения parameterValues
            // если без параметров, то передаем пустые массивы
            // и вызываем конструктор, создавая экземпляр класса response
            response = (BaseResponse)typeof(TBaseResponse).GetConstructor(parameters)?.Invoke(parameterValues);
        }
        if (response != null)
        {
            // десериализуем и возвращаем результат
            _responseDeserializer.Deserialize(receiveData, response, offset);
        }
        return (result, response);
    }

    #endregion

    #region Потоковые методы для работы с очередями запросов

    /// <summary>
    /// Добавляет запрос в очередь
    /// </summary>
    /// <param name="requestResponse">запрос</param>
    public void EnqueueRequest(ModbusRequestResponse requestResponse) => _requestQueue.Add(requestResponse);

    /// <summary>
    /// Добавляет группу запросов в очередь
    /// </summary>
    /// <param name="requestResponses">список запросов</param>
    public void EnqueueRequests(IEnumerable<ModbusRequestResponse> requestResponses)
    {
        foreach (var requestResponse in requestResponses)
        {
            _requestQueue.Add(requestResponse);
        }
    }

    /// <summary>
    /// выполняет транзакцию в зависимости от запроса
    /// </summary>
    /// <param name="requestResponse">запрос</param>
    private void DoTransaction(ModbusRequestResponse requestResponse)
    {
        (var requestResult, var response) = requestResponse.Request switch
        {
            // запрос на чтение идентификатора устройства 0x11
            DeviceIdentificatorRequest deviceIdentificatorRequest => ModbusTransaction<DeviceIdentificatorResponse>(deviceIdentificatorRequest),
            // запрос чтения регистров 0x03
            ReadHoldingRegistersRequest readHoldingRegistersRequest => ModbusTransaction<ReadHoldingRegistersResponse>(readHoldingRegistersRequest, new[] { typeof(int) },
                new object[] { readHoldingRegistersRequest.QuantityOfRegisters }),
            // запрос чтения регистров 0x04
            ReadInputRegistersRequest readInputRegistersRequest => ModbusTransaction<ReadInputRegistersResponse>(readInputRegistersRequest, new[] { typeof(int) },
                new object[] { readInputRegistersRequest.QuantityOfRegisters }),
            // запрос записи регистра 0x06
            WriteSingleRegisterRequest writeSingleRegisterRequest => ModbusTransaction<WriteSingleRegisterResponse>(writeSingleRegisterRequest),
            // запрос записи регистров 0x10
            WriteMultipleRegistersRequest writeMultipleRegistersRequest => ModbusTransaction<WriteMultipleRegistersResponse>(writeMultipleRegistersRequest),
            // запрос записи регистров с последующим чтением регистров 0x17
            ReadWriteMultipleRegistersRequest readWriteMultipleRegistersRequest => ModbusTransaction<ReadWriteMultipleRegistersResponse>(readWriteMultipleRegistersRequest,
                new[] { typeof(int) }, new object[] { readWriteMultipleRegistersRequest.QuantityToRead }),
            _ => throw new ArgumentOutOfRangeException(nameof(requestResponse))
        };
        requestResponse.Response = response;
        requestResponse.RequestResult = requestResult;
        requestResponse.Event.Set();
    }

    /// <summary>
    /// Забирает запросы из очереди и выполняет их
    /// </summary>
    /// <param name="arg"><see cref="CancellationToken"/></param>
    private void RequestDispatcher(object arg)
    {
        var cancellation = (CancellationToken)arg;
        while (!cancellation.IsCancellationRequested)
        {
            try
            {
                DoTransaction(_requestQueue.Take(cancellation));
            }
            catch (Exception)
            {
                // ignored
            }
            Thread.Sleep(50);
        }
    }

    /// <summary>
    /// добавляет автозапросы в очередь
    /// </summary>
    private void EnqueueAutoRequests(object arg)
    {
        // если очередь заполнена, пропускаем
        if (_requestQueue.Count >= AutoRequestResponses.Count) return;
        EnqueueRequests(AutoRequestResponses);
    }

    /// <summary>
    /// запуск авто-запросов
    /// </summary>
    public void StartAutoRequests()
    {
        _autoRequestsTimer?.Dispose();
        _autoRequestsTimer = ProtocolSettings is { PoolingPeriod: > 0 }
            ? new Timer(EnqueueAutoRequests, null, 0, ProtocolSettings.PoolingPeriod)
            : new Timer(EnqueueAutoRequests, null, 0, 5000);
    }

    /// <summary>
    /// остановка авто-запросов
    /// </summary>
    public void StopAutoRequests()
    {
        _autoRequestsTimer?.Dispose();
    }

    /// <summary>
    /// очистить список авто-запросов
    /// </summary>
    public void ClearAutoRequestList()
    {
        if (AutoRequestResponses == null) return;
        while (AutoRequestResponses.Count != 0)
        {
            AutoRequestResponses.Take();
        }
    }

    /// <summary>
    /// Добавить запросы в список автозапросов
    /// </summary>
    public void AddRequestsToAutoRequestList(List<ModbusRequestResponse> requestResponses)
    {
        if (requestResponses == null || requestResponses.Count == 0) return;
        foreach (var request in requestResponses)
        {
            AutoRequestResponses.Add(request);
        }
    }

    /// <summary>
    /// Стартует поток обработки очереди запросов
    /// </summary>
    public void StartRequestDispatcher()
    {
        if (_requestDispatcherThread is { IsAlive: true }) return;
        _requestDispatcherCancellation = new CancellationTokenSource();
        _requestDispatcherThread = new Thread(RequestDispatcher);
        _requestDispatcherThread.Start(_requestDispatcherCancellation.Token);
    }

    /// <summary>
    /// Останавливает поток обработки очереди запросов
    /// </summary>
    public void StopRequestDispatcher()
    {
        _requestDispatcherCancellation?.Cancel();
        _requestDispatcherThread?.Join();
    }

    #endregion

    #endregion
}