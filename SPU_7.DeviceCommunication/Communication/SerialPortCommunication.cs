using System.Collections.Concurrent;
using System.IO.Ports;
using NLog;
using SPU_7.DeviceCommunication.Base;
using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.DeviceCommunication.Communication;

public class SerialPortCommunication : BindableBase, ISerialPortCommunication, IDisposable
{
    public SerialPortCommunication(SerialPort serialPort)
    {
        Id = Guid.NewGuid();
        SerialPort = serialPort;
        NewLine = SerialPort.NewLine;
        RequestTimeout = 1000;
        RequestsQueue = new ConcurrentQueue<Request>();
        ProcessRequestsQueueTask = Task.Factory.StartNew(ProcessRequestsQueueTaskAsync, TaskCreationOptions.DenyChildAttach | TaskCreationOptions.LongRunning);
        SerialPort.DataReceived += SerialPort_DataReceived;
    }

    private bool _disposedValue;
    private bool _isBusy;
    private int _requestTimeout;
    private int _requestDelay;
    private bool _isReceive;
    private bool _isTransmit;

    private ILogger? Logger { get; set; }

    /// <summary>
    /// Очередь для запросов
    /// </summary>
    private ConcurrentQueue<Request> RequestsQueue { get; }

    /// <summary>
    /// Задача с очередью запросов
    /// </summary>
    private Task ProcessRequestsQueueTask { get; }

    /// <summary>
    /// Последовательный порт для связи
    /// </summary>
    protected SerialPort SerialPort { get; set; }

    /// <summary>
    /// Идентификатор последовательного порта
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название последовательного порта
    /// </summary>
    public string Connection
    {
        get => SerialPort.PortName;
        set
        {
            if (SerialPort.PortName == value) return;
            SerialPort.PortName = value;
            Logger = LogManager.GetLogger($"SerialPort{SerialPort.PortName}");
            RaisePropertyChanged();
        }
    }

    public bool IsOpen => SerialPort.IsOpen;

    public int ReadTimeout
    {
        get => SerialPort.ReadTimeout;
        set
        {
            if (SerialPort.ReadTimeout == value) return;
            SerialPort.ReadTimeout = value;
            RaisePropertyChanged();
        }
    }
    public int WriteTimeout
    {
        get => SerialPort.WriteTimeout;
        set
        {
            if (SerialPort.WriteTimeout == value) return;
            SerialPort.WriteTimeout = value;
            RaisePropertyChanged();
        }
    }
    public int DataBits
    {
        get => SerialPort.DataBits;
        set
        {
            if (SerialPort.DataBits == value) return;
            SerialPort.DataBits = value;
            RaisePropertyChanged();
        }
    }
    public int BaudRate
    {
        get => SerialPort.BaudRate;
        set
        {
            if (SerialPort.BaudRate == value) return;
            SerialPort.BaudRate = value;
            RaisePropertyChanged();
        }
    }
    public Parity Parity
    {
        get => SerialPort.Parity;
        set
        {
            if (SerialPort.Parity == value) return;
            SerialPort.Parity = value;
            RaisePropertyChanged();
        }
    }
    public StopBits StopBits
    {
        get => SerialPort.StopBits;
        set
        {
            if (SerialPort.StopBits == value) return;
            SerialPort.StopBits = value;
            RaisePropertyChanged();
        }
    }
    public Handshake Handshake
    {
        get => SerialPort.Handshake;
        set
        {
            if (SerialPort.Handshake == value) return;
            SerialPort.Handshake = value;
            RaisePropertyChanged();
        }
    }
    public string NewLine
    {
        get => SerialPort.NewLine;
        set
        {
            if (SerialPort.NewLine == value) return;
            SerialPort.NewLine = value;
            RaisePropertyChanged();
        }
    }
    public bool IsBusy
    {
        get => _isBusy;
        private set => SetProperty(ref _isBusy, value);
    }
    public int RequestTimeout
    {
        get => _requestTimeout;
        set => SetProperty(ref _requestTimeout, value);
    }
    public int RequestDelay
    {
        get => _requestDelay;
        set => SetProperty(ref _requestDelay, value);
    }
    public bool IsReceive
    {
        get => _isReceive;
        private set => SetProperty(ref _isReceive, value);
    }
    public bool IsTransmit
    {
        get => _isTransmit;
        private set => SetProperty(ref _isTransmit, value);
    }
    public int ReadBufferSize
    {
        get => SerialPort.ReadBufferSize;
        set
        {
            if (SerialPort.ReadBufferSize == value) return;
            SerialPort.ReadBufferSize = value;
            RaisePropertyChanged();
        }
    }
    public int WriteBufferSize
    {
        get => SerialPort.WriteBufferSize;
        set
        {
            if (SerialPort.WriteBufferSize == value) return;
            SerialPort.WriteBufferSize = value;
            RaisePropertyChanged();
        }
    }

    public int BytesToRead => SerialPort.BytesToRead;

    public int BytesToWrite => SerialPort.BytesToWrite;

    #region Обработчики событий

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        RaisePropertyChanged(nameof(BytesToRead));
        RaisePropertyChanged(nameof(BytesToWrite));
    }

    #endregion

    #region Синхронные методы
    #region Чтение

    /// <summary>
    /// Чтение из последовательного порта
    /// </summary>
    /// <param name="dataSize">Размер данных</param>
    /// <returns>Считанные данные</returns>
    public ArraySegment<byte> Read(int dataSize)
    {
        if (!SerialPort.IsOpen) Connect();
        var data = new byte[dataSize];
        var readOffset = 0;
        IsBusy = true;
        while (readOffset < data.Length)
            readOffset += SerialPort.Read(data, readOffset, data.Length - readOffset);
        IsBusy = false;
        return data;
    }

    /// <summary>
    /// Чтение из последовательного порта
    /// </summary>
    /// <param name="data">Массив для записи входных данных</param>
    /// <param name="offset">Смещение в принимающем массиве</param>
    /// <param name="dataSize">Кол-во байт для записи</param>
    public void Read(ArraySegment<byte> data, int offset, int dataSize)
    {
        if (data.Array is null) throw new InvalidOperationException("InvalidOperation_NullArray");
        if (!SerialPort.IsOpen) Connect();
        var readOffset = 0;
        IsBusy = true;
        while (readOffset < dataSize)
            readOffset += SerialPort.Read(data.Array, data.Offset + offset + readOffset, dataSize - readOffset);
        IsBusy = false;
    }

    /// <summary>
    /// Чтение строки из последовательного порта
    /// </summary>
    public string ReadLine() => SerialPort.ReadLine();

    #endregion
    #region Запись

    /// <summary>
    /// Запись в последовательный порт
    /// </summary>
    /// <param name="data">Данные для записи</param>
    public void Write(ArraySegment<byte> data)
    {
        if (data.Array is null) throw new InvalidOperationException("InvalidOperation_NullArray");
        if (!SerialPort.IsOpen) Connect();
        IsBusy = true;
        SerialPort.Write(data.Array, 0, data.Count);
        IsBusy = false;
    }

    /// <summary>
    /// Запись строки в последовательный порт
    /// </summary>
    /// <param name="data">Строка для записи</param>
    public void WriteLine(string data)
    {
        if (!SerialPort.IsOpen) Connect();
        IsBusy = true;
        SerialPort.WriteLine(data);
        IsBusy = false;
    }

    #endregion

    public T TryProcessRequestAndResponse<T>(Func<T> func)
    {
        using var request = new RequestWithFunc<T>(func);
        RequestsQueue.Enqueue(request);
        var waitResult = RequestTimeout != 0 ? request.ResetEvent.WaitOne(TimeSpan.FromMilliseconds(RequestTimeout)) : request.ResetEvent.WaitOne();
        if (!waitResult) {
            throw new TimeoutException("Слишком долгое ожидание очереди - превышен заданный лимит времени.");
        }
        if (request.Exception is not null) throw request.Exception;
        return request.RequestTask is not null ? request.RequestTask.Result : throw new InvalidOperationException("Задача не была назначена!");
    }

    #endregion

    #region Асинхронные методы
    #region Чтение

    /// <summary>
    /// Чтение из последовательного порта асинхронно
    /// </summary>
    /// <param name="dataSize">Размер данных</param>
    /// <returns>Задачу по ожиданию данных</returns>
    public async Task<ArraySegment<byte>> ReadAsync(int dataSize, CancellationToken token = default)
    {
        var data = new byte[dataSize];
        await ReadAsync(data, token).ConfigureAwait(false);
        return data;
    }

    /// <summary>
    /// Чтение из последовательного порта асинхронно
    /// </summary>
    /// <param name="data">Массив для записи входных данных</param>
    /// <param name="offset">Смещение в принимающем массиве</param>
    /// <param name="dataSize">Кол-во байт для записи</param>
    /// <returns></returns>
    public async Task<int> ReadAsync(ArraySegment<byte> data, CancellationToken token = default)
    {
        if (!SerialPort.IsOpen) Connect();
        //using var timeoutCancellation = new CancellationTokenSource(ReadTimeout);
        //using var ctr = token.Register(() => timeoutCancellation.Cancel());
        try {
            var readOffset = 0;
            IsBusy = true;
            IsReceive = true;
            if (Logger?.IsDebugEnabled is true) Logger?.Debug("Чтение данных: {DataSize} байт.", data.Count);
            while (readOffset < data.Count) {
                if (SerialPort.BytesToRead == 0)
                    await Task.Delay((int)((data.Count - readOffset) * (9600.0 / SerialPort.BaudRate)), token); // Для некоторых конвертеров (Проблема: одновременная работа WaitCommEvent в классе SerialPort)
                var readCount = await SerialPort.BaseStream
                    .ReadAsync(data.Array!, readOffset + data.Offset, data.Count - readOffset, token)
                    .WaitAsync(TimeSpan.FromMilliseconds(SerialPort.ReadTimeout), token)
                    .ConfigureAwait(false);
                if (Logger?.IsDebugEnabled is true && readCount > 0) Logger?.Debug("Прочитано {DataSize} байт: {DataHex}", readCount, data.Slice(readOffset, readCount).ToHexString());
                readOffset += readCount;
            }
            return readOffset;
        }
        catch (TaskCanceledException ex) when (token.IsCancellationRequested is false) {
            var message = "Таймаут!";
            if (Logger?.IsWarnEnabled is true) Logger.Warn(ex, message);
            throw new TimeoutException(message);
        }
        catch (OperationCanceledException ex) when (token.IsCancellationRequested) {
            if (Logger?.IsWarnEnabled is true) Logger?.Warn(ex, "Был запрос на отмену!");
            throw;
        }
        catch (OperationCanceledException ex) {
            if (Logger?.IsWarnEnabled is true) Logger?.Warn(ex, "Таймаут!");
            throw;
        }
        catch (IOException ex) when (token.IsCancellationRequested is false) {
            Logger?.Error(ex, "Таймаут с IOException");
            throw new TimeoutException();
        }
        finally {
            IsReceive = false;
            IsBusy = false;
        }
    }
    /*
    private static int ReadBufferIntoChars(ArraySegment<byte> inputBuffer, ArraySegment<char> charsBuffer, int bytesLeft, Encoding encoding)
    {
        if (inputBuffer.Array is null)
            throw new ArgumentNullException(nameof(inputBuffer));

        if (charsBuffer.Array is null)
            throw new ArgumentNullException(nameof(charsBuffer));

        int bytesToRead = Math.Min(charsBuffer.Count, bytesLeft);
        var fallback = encoding.DecoderFallback as DecoderReplacementFallback;
        var decoder = encoding.GetDecoder();
        if (encoding.IsSingleByte && encoding.GetMaxCharCount(bytesToRead) == bytesToRead
            && fallback is not null and { MaxCharCount: 1 }) {
            decoder.GetChars(inputBuffer.Array, inputBuffer.Offset, bytesToRead, charsBuffer.Array, charsBuffer.Offset);
            return bytesToRead;
        }
        else {
            int totalBytesExamined = 0;
            int totalCharsFound = 0;
            int currentBytesToExamine;
            int currentCharsFound;
            int lastFullCharPos = inputBuffer.Offset;
            do {
                currentBytesToExamine = Math.Min(charsBuffer.Count - totalCharsFound, inputBuffer.Count - inputBuffer.Offset - totalBytesExamined);
                if (currentBytesToExamine <= 0)
                    break;

                totalBytesExamined += currentBytesToExamine;
                currentBytesToExamine = inputBuffer.Offset + totalBytesExamined - lastFullCharPos;

                currentCharsFound = decoder.GetCharCount(inputBuffer.Array, lastFullCharPos, currentBytesToExamine);

                if (currentCharsFound > 0) {
                    int foundCharsByteLength = currentBytesToExamine;
                    do {
                        foundCharsByteLength--;
                    }
                    while (decoder.GetCharCount(inputBuffer.Array, lastFullCharPos, foundCharsByteLength) == currentCharsFound);
                    decoder.GetChars(inputBuffer.Array, lastFullCharPos, foundCharsByteLength + 1, charsBuffer.Array, charsBuffer.Offset + totalCharsFound);
                    lastFullCharPos += foundCharsByteLength + 1;
                }
                totalCharsFound += currentCharsFound;
            }
            while ((totalCharsFound < charsBuffer.Count) && (totalBytesExamined < (inputBuffer.Count - inputBuffer.Offset)));
            return totalCharsFound;
        }
    }
    
    private async Task<int> ReadSingleChar(ArraySegment<byte> inputBuffer, ArraySegment<char> charsBuffer, CancellationToken cancellationToken = default)
    {
        if (charsBuffer.Count == 0)
            return 0;

        if (inputBuffer.Array is null)
            throw new ArgumentNullException(nameof(inputBuffer));

        long startTicks = Environment.TickCount64;
        int bytesInStream = SerialPort.BytesToRead;
        var readLen = await ReadAsync(new ArraySegment<byte>(inputBuffer.Array, inputBuffer.Offset, bytesInStream), cancellationToken).ConfigureAwait(false);
        var spe = SerialPort.Encoding;
        var spd = SerialPort.Encoding.GetDecoder();
        var cah = spd.GetCharCount(inputBuffer.Array, 0, readLen);
        if (cah > 0) {
            return ReadBufferIntoChars(inputBuffer, charsBuffer, inputBuffer.Count - readLen, spe);
        }

        int justRead;
        int maxReadSize = spe.GetMaxByteCount(charsBuffer.Count);
        do {
            // MaybeResizeBuffer(bytesInStream);
            readLen += await ReadAsync(new ArraySegment<byte>(inputBuffer.Array, inputBuffer.Offset + readLen, maxReadSize), cancellationToken).ConfigureAwait(false);
            justRead = ReadBufferIntoChars(inputBuffer, charsBuffer, inputBuffer.Count - readLen, spe);
            if (justRead > 0) return justRead;
        }
        while (true);

        throw new TimeoutException();
    }

    private async Task<string> ReadToAsync(string value, CancellationToken cancellationToken = default)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));
        if (value.Length == 0)
            throw new ArgumentException("Пустое значение", nameof(value));

        var maxBytesForSingleChar = SerialPort.Encoding.GetMaxByteCount(1);
        var currentLine = new StringBuilder(value.Length);
        var bytesInStream = SerialPort.BytesToRead;
        var lastValueChar = value[^1];
        int numCharsRead = 0;
        long timeUsed = 0;
        long timeNow;

        if (_singleCharBuffer is null || _singleCharBuffer.Length < maxBytesForSingleChar)
            _singleCharBuffer ??= new char[maxBytesForSingleChar];

        var inputBuffer = new byte[SerialPort.ReadBufferSize];
        var readLen = await ReadAsync(new ArraySegment<byte>(inputBuffer, 0, bytesInStream), cancellationToken).ConfigureAwait(false);
        while (cancellationToken.IsCancellationRequested is false) {
            // Вместо readLen нужно использовать смещение в текущем Stream
            var ibo = new ArraySegment<byte>(inputBuffer, readLen, inputBuffer.Length - readLen);
            var scbo = new ArraySegment<char>(_singleCharBuffer, 0, 1);
            if (SerialPort.ReadTimeout == SerialPort.InfiniteTimeout) {
                numCharsRead = await ReadSingleChar(ibo, scbo, cancellationToken).ConfigureAwait(false);
            }
            else if (SerialPort.ReadTimeout - timeUsed >= 0) {
                timeNow = Environment.TickCount64;
                numCharsRead = await ReadSingleChar(ibo, scbo, cancellationToken).ConfigureAwait(false);
                timeUsed += Environment.TickCount64 - timeNow;
            }
            else {
                throw new TimeoutException();
            }

            readLen += numCharsRead;
            currentLine.Append(_singleCharBuffer, 0, numCharsRead);

            if (lastValueChar == _singleCharBuffer[numCharsRead - 1] && currentLine.Length >= value.Length) {
                bool found = true;
                for (int i = 2; i <= value.Length; i++) {
                    if (value[^i] != currentLine[^i]) {
                        found = false;
                        break;
                    }
                }
                if (found) break;
            }
        }
        if (cancellationToken.IsCancellationRequested)
            throw new TaskCanceledException();

        return currentLine.ToString(0, currentLine.Length - value.Length);
    }
    */

    /// <summary>
    /// Чтение строки из последовательного порта асинхронно
    /// </summary>
    public Task<string> ReadLineAsync(CancellationToken cancellationToken = default)
    {
        if (!SerialPort.IsOpen) Connect();
        // return ReadToAsync(NewLine, cancellationToken);
        return Task.Run(() =>
        {
            try {
                IsBusy = true;
                IsReceive = true;
                return SerialPort.ReadLine();
            }
            finally {
                IsReceive = false;
                IsBusy = false;
            }
        }, cancellationToken);
    }

    #endregion
    #region Запись

    /// <summary>
    /// Запись в последовательный порт асинхронно
    /// </summary>
    /// <param name="data">Данные для записи</param>
    /// <returns></returns>
    public async Task WriteAsync(ArraySegment<byte> data, CancellationToken token = default)
    {
        if (!SerialPort.IsOpen) Connect();
        /*using var timeoutCancellation = new CancellationTokenSource(WriteTimeout);
        using var dctr = timeoutCancellation.Token.Register(DiscardOutBuffer);
        using var ctr = token.Register(() => timeoutCancellation.Cancel());*/
        try {
            IsBusy = true;
            IsTransmit = true;
            if (Logger?.IsDebugEnabled is true) Logger?.Debug("Отправка данных размером {PayloadLength} байт: {DataHex}", data.Count, data.ToHexString());
            await SerialPort.BaseStream.MakeStreamWriteAsync(data, token).WaitAsync(TimeSpan.FromMilliseconds(WriteTimeout), token).ConfigureAwait(false);
        }
        catch (TaskCanceledException ex) when (/*timeoutCancellation.IsCancellationRequested && */token.IsCancellationRequested is false) {
            var message = "Таймаут!";
            if (Logger?.IsWarnEnabled is true) Logger.Warn(ex, message);
            throw new TimeoutException(message);
        }
        catch (OperationCanceledException ex)
            when (token.IsCancellationRequested) {
            if (Logger?.IsWarnEnabled is true) Logger?.Warn(ex, "Был запрос на отмену!");
            throw;
        }
        catch (OperationCanceledException ex) /*when (timeoutCancellation.IsCancellationRequested)*/ {
            if (Logger?.IsWarnEnabled is true) Logger?.Warn(ex, "Таймаут");
            throw new TimeoutException("Не удалось выполнить операцию записи за отведённое время.");
        }
        catch (IOException ex) when (/*timeoutCancellation.IsCancellationRequested && */token.IsCancellationRequested is false) {
            Logger?.Error(ex, "Таймаут с IOException");
            throw new TimeoutException("Не удалось выполнить операцию записи за отведённое время.");
        }
        finally {
            IsTransmit = false;
            IsBusy = false;
        }
    }

    /// <summary>
    /// Запись строки в последовательный порт асинхронно
    /// </summary>
    /// <param name="data">Данные для записи</param>
    /// <returns>Задача по ожиданию записи</returns>
    public Task WriteLineAsync(string data, CancellationToken token = default) => WriteAsync(SerialPort.Encoding.GetBytes(data + SerialPort.NewLine), token);

    #endregion

    /// <summary>
    /// Попытаться обработать запрос (последовательный порт будет занят для обработки запроса)
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого значения асинхронной функции</typeparam>
    /// <param name="func">Асинхронная функция для выполнения</param>
    /// <returns>Задача с ожиданием данных</returns>
    /// <exception cref="TimeoutException"></exception>
    public async Task<T> TryProcessRequestAndResponseAsync<T>(Func<Task<T>> func)
    {
        using var request = new RequestWithTask<T>(func);
        RequestsQueue.Enqueue(request);
        var waitResult = RequestTimeout != 0
            ? await request.ResetEvent.WaitOneAsync(TimeSpan.FromMilliseconds(RequestTimeout)).ConfigureAwait(false)
            : await request.ResetEvent.WaitOneAsync().ConfigureAwait(false);
        if (!waitResult) {
            throw new TimeoutException("Слишком долгое ожидание очереди - превышен заданный лимит времени.");
        }
        return request.RequestTask is not null ? await request.RequestTask.ConfigureAwait(false) : throw new InvalidOperationException("Задача не была назначена!");
    }

    /// <summary>
    /// Обработка очереди запросов в последовательный порт
    /// </summary>
    private async Task ProcessRequestsQueueTaskAsync()
    {
        //SpinWait sw = default;
        while (true) {
            if (RequestsQueue.TryDequeue(out var request) is false) {
                //sw.SpinOnce();
                await Task.Delay(1);
                continue; // Продолжить попытки опустошения очереди
            }
            try {
                await Task.Delay(RequestDelay).ConfigureAwait(false);
                await request.ActivateRequest().ConfigureAwait(false); // Синхронно / асинхронно ожидать завершения запроса
            }
            catch (Exception) { } // Поглощать все возникающие исключения для обработчика очереди запросов
            finally {
                if (request.ResetEvent.SafeWaitHandle.IsClosed is false)
                    request.ResetEvent.Set(); // Подать сигнал о готовности результата
            }
        }
    }

    #endregion

    /// <summary>
    /// Очищает буфер прёма данных
    /// </summary>
    public void DiscardInBuffer()
    {
        if (SerialPort.IsOpen) {
            IsBusy = true;
            SerialPort.DiscardInBuffer();
            IsBusy = false;
        }
    }

    /// <summary>
    /// Очищает буфер вывода данных
    /// </summary>
    public void DiscardOutBuffer()
    {
        if (SerialPort.IsOpen) {
            IsBusy = true;
            SerialPort.DiscardOutBuffer();
            IsBusy = false;
        }
    }

    /// <summary>
    /// Очистить буфер ввода и ожидать отправление всех байт
    /// </summary>
    /// <param name="cancellationToken"></param>
    public Task FlushAsync(CancellationToken cancellationToken = default) => SerialPort.BaseStream.FlushAsync(cancellationToken);

    /// <summary>
    /// Соединиться с последовательным портом
    /// </summary>
    /// <param name="portName">Название порта для соединения</param>
    public bool Connect(string portName)
    {
        try {
            SerialPort.PortName = portName;
            SerialPort.Open();
            Logger = LogManager.GetLogger($"SerialPort{SerialPort.PortName}");
            Logger?.Trace("Последовательный порт {PortName} открыт", SerialPort.PortName);
            return true;
        }
        //catch(UnauthorizedAccessException ex) { // Последовательный порт уже занят другим приложением или доступ к нему запрещён
        //}
        //catch (ArgumentOutOfRangeException ex) { // Неправильные настройки порта (Например значение не из оригинального Enum)
        //}
        catch (InvalidOperationException ex) { // Последовательный порт уже открыт
            Logger?.Error(ex, "Порт {PortName} уже открыт", SerialPort.PortName);
            return false;
        }
    }

    public bool Connect()
    {
        try {
            SerialPort.Open();
            Logger?.Trace("Последовательный порт {PortName} открыт", SerialPort.PortName);
            RaisePropertyChanged(nameof(IsOpen));
            return true;
        }
        catch (InvalidOperationException ex) { // Последовательный порт уже открыт
            Logger?.Error(ex, "Порт {PortName} уже открыт", SerialPort.PortName);
            return false;
        }
    }

    public Task<bool> ConnectAsync(string connectionName, CancellationToken token = default)
    {
        try {
            SerialPort.PortName = connectionName;
            SerialPort.Open();
            Logger?.Trace("Последовательный порт {PortName} открыт", SerialPort.PortName);
            RaisePropertyChanged(nameof(IsOpen));
            return Task.FromResult(true);
        }
        catch (InvalidOperationException ex) { // Последовательный порт уже открыт
            Logger?.Error(ex, "Порт {PortName} уже открыт", SerialPort.PortName);
            return Task.FromResult(false);
        }
    }

    public Task<bool> ConnectAsync(CancellationToken token = default)
    {
        try {
            SerialPort.Open();
            Logger?.Trace("Последовательный порт {PortName} открыт", SerialPort.PortName);
            RaisePropertyChanged(nameof(IsOpen));
            return Task.FromResult(true);
        }
        catch (InvalidOperationException ex) { // Последовательный порт уже открыт
            Logger?.Error(ex, "Порт {PortName} уже открыт", SerialPort.PortName);
            return Task.FromResult(false);
        }
    }

    /// <summary>
    /// Закрыть соедиение
    /// </summary>
    public void Close()
    {
        SerialPort.Close();
        Logger?.Trace("Порт {PortName} закрыт", SerialPort.PortName);
        RaisePropertyChanged(nameof(IsOpen));
    }

    public Task CloseAsync(CancellationToken token = default)
    {
        SerialPort.Close();
        return Task.CompletedTask;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;
        if (disposing) {
            SerialPort.DataReceived -= SerialPort_DataReceived;
            SerialPort.Dispose();
            _disposedValue = true;
        }
    }

    ~SerialPortCommunication()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}