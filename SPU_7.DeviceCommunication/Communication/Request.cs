namespace SPU_7.DeviceCommunication.Communication;

internal abstract class Request : IDisposable
{
    public Request()
    {
        ResetEvent = new ManualResetEvent(false);
    }

    private bool _disposedValue;

    //public bool IsAsync { get; protected set; }
    //public ILogger? Logger { get; }

    public Exception? Exception { get; protected set; }

    /// <summary>
    /// Событие для сигнализации того, что готов результат
    /// </summary>
    public ManualResetEvent ResetEvent { get; }

    /// <summary>
    /// Активировать запрос
    /// </summary>
    public virtual Task ActivateRequest()
    {
        Exception = new InvalidOperationException("Не переопределён метод ActivateRequest!");
        return Task.FromException(Exception);
    }

    #region Освобождение ресурсов для IDisposable

    protected virtual void Dispose(bool disposing)
    {
        if (_disposedValue) return;
        if (disposing) {
            ResetEvent.Close();
        }
        _disposedValue = true;
    }

    ~Request()
    {
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    #endregion
}

internal class RequestWithTask<T> : Request
{
    public RequestWithTask(Func<Task<T>> requestTaskFunc)
    {
        _requestTaskFunc = requestTaskFunc;
    }

    /// <summary>
    /// Функция для вызова задачи
    /// </summary>
    private readonly Func<Task<T>> _requestTaskFunc;

    /// <summary>
    /// Задача по выполнению запроса
    /// </summary>
    public Task<T>? RequestTask { get; private set; }

    public override Task ActivateRequest()
    {
        try {
            return RequestTask = _requestTaskFunc();
        }
        catch (Exception ex) {
            Exception = ex;
            return RequestTask = Task.FromException<T>(Exception);
        }
    }
}

internal class RequestWithFunc<T> : Request
{
    public RequestWithFunc(Func<T> func)
    {
        _requestFunc = func;
    }

    /// <summary>
    /// Функция для запроса
    /// </summary>
    private readonly Func<T> _requestFunc;

    /// <summary>
    /// Задача по выполнению запроса
    /// </summary>
    public Task<T>? RequestTask { get; private set; }

    public override Task ActivateRequest()
    {
        try {
            return RequestTask = Task.FromResult(_requestFunc());
        }
        catch (Exception ex) {
            Exception = ex;
            return RequestTask = Task.FromException<T>(Exception);
        }
    }
}
