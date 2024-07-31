namespace SPU_7.DeviceCommunication.Extensions;

public class TaskStream
{
    public TaskStream(Stream stream,TaskCompletionSource<int> tcs)
    {
        Stream = stream;
        CompletionSource = tcs;
    }

    public Stream Stream { get; }
    public TaskCompletionSource<int> CompletionSource { get; }
}

public static class AsyncExtensions
{
    /// <summary>
    /// Обработать результат асинхронной операции
    /// </summary>
    /// <param name="asyncResult">Результат операции</param>
    private static void AsyncResultCallback(IAsyncResult asyncResult)
    {
        var tcs = asyncResult.AsyncState as TaskCompletionSource<IAsyncResult>;
        try {
            if (asyncResult.IsCompleted && tcs?.Task is { IsCompleted: false }) tcs?.TrySetResult(asyncResult);
        }
        catch (Exception ex) {
            tcs?.TrySetException(ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="asyncResult"></param>
    private static void AsyncReadResultCallback(IAsyncResult asyncResult)
    {
        if (asyncResult.AsyncState is not TaskStream ts) return;
        try {
            var readCount = ts.Stream.EndRead(asyncResult);
            ts.CompletionSource.TrySetResult(readCount);
        }
        catch (Exception ex) {
            ts.CompletionSource.TrySetException(ex);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="asyncResult"></param>
    private static void AsyncWriteResultCallback(IAsyncResult asyncResult)
    {
        var ts = asyncResult.AsyncState as TaskStream;
        try {
            ts?.Stream.EndWrite(asyncResult);
            ts?.CompletionSource.TrySetResult(0);
        }
        catch (Exception ex) {
            ts?.CompletionSource.TrySetException(ex);
        }
    }

    /// <summary>
    /// Сделать чтение из SerialStream через асинхронный интерфейс
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="buffer">Массив для чтения данных</param>
    /// <param name="offset">Смещение в буффере чтения данных</param>
    /// <param name="count">Количество байт для чтения</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    /// <returns>Количество прочитанных байт</returns>
    public static Task<int> MakeStreamReadAsync(this Stream stream, ArraySegment<byte> buffer, int offset, int count, CancellationToken cancellationToken = default)
    {
        if (buffer.Array is null) throw new InvalidOperationException("InvalidOperation_NullArray");
        var taskCompletionSource = new TaskCompletionSource<int>();
        var ctr = cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(cancellationToken));
        var ar = stream.BeginRead(buffer.Array, buffer.Offset + offset, count, AsyncReadResultCallback, new TaskStream(stream, taskCompletionSource));
        taskCompletionSource.Task.ContinueWith(t => ctr.DisposeAsync());
        return taskCompletionSource.Task;
    }

    /// <summary>
    /// Сделать запись в SerialStream через асинхронный интерфейс
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="buffer">Массив для записи данных</param>
    /// <param name="cancellationToken">Токен для отмены операции</param>
    public static Task MakeStreamWriteAsync(this Stream stream, ArraySegment<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (buffer.Array is null) throw new InvalidOperationException("InvalidOperation_NullArray");
        var taskCompletionSource = new TaskCompletionSource<int>();
        var ctr = cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(cancellationToken));
        stream.BeginWrite(buffer.Array, buffer.Offset, buffer.Count, AsyncWriteResultCallback, new TaskStream(stream, taskCompletionSource));
        return taskCompletionSource.Task.ContinueWith(t => ctr.DisposeAsync());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="timedOut"></param>
    private static void WaitCompletionCallback(object? state, bool timedOut) => (state as TaskCompletionSource<bool>)?.TrySetResult(!timedOut);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    private static void CancelledCallback(object? state) => (state as TaskCompletionSource<bool>)?.TrySetCanceled();

    /// <summary>
    /// Подождать асинхронно WaitHandle
    /// </summary>
    /// <param name="waitHandle"></param>
    /// <param name="timeout">Таймаут ожидания</param>
    /// <param name="cancellationToken">Токен для отмены ожидания</param>
    /// <returns>Результат ожидания</returns>
    public static Task<bool> WaitOneAsync(this WaitHandle waitHandle, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        RegisteredWaitHandle? registeredWaitHandle = null;
        CancellationTokenRegistration ctr = default;
        var tcs = new TaskCompletionSource<bool>();
        registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(waitHandle, WaitCompletionCallback, tcs, timeout, true);
        if (cancellationToken.CanBeCanceled) {
            ctr = cancellationToken.Register(CancelledCallback, tcs, useSynchronizationContext: false);
        }
        var task = tcs.Task;
        task.ContinueWith(async t =>
        {
            registeredWaitHandle?.Unregister(null);
            await ctr.DisposeAsync().ConfigureAwait(false);
        }, cancellationToken);
        return task;
    }

    /// <summary>
    /// Подождать асинхронно WaitHandle
    /// </summary>
    /// <param name="waitHandle"></param>
    /// <param name="cancellationToken">Токен для отмены ожидания</param>
    /// <returns></returns>
    public static Task<bool> WaitOneAsync(this WaitHandle waitHandle, CancellationToken cancellationToken = default) =>
        waitHandle.WaitOneAsync(Timeout.InfiniteTimeSpan, cancellationToken);
}