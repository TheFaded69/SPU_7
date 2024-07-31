using System.Runtime.CompilerServices;

namespace SPU_7.DeviceCommunication.Base;

public readonly struct TaskCompletionSourceAwaiter<TResult> : ICriticalNotifyCompletion
{
    private readonly TaskCompletionSource<TResult> _taskCompletionSource;

    public TaskCompletionSourceAwaiter(TaskCompletionSource<TResult> taskCompletionSource)
    {
        _taskCompletionSource = taskCompletionSource;
    }

    public void OnCompleted(Action continuation)
    {
        ArgumentNullException.ThrowIfNull(continuation);
            
    }

    public void UnsafeOnCompleted(Action continuation)
    {
        throw new NotImplementedException();
    }
}