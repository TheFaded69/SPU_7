using System;
using Avalonia.Threading;

namespace SPU_7.Models.Services.ContentServices;

public class TimerService : ITimerService
{
    public TimerService()
    {
        
    }

    private Action _timerEnableAction;
    private Action _timerDisableAction;

    
    public int TimeSeconds { get; set; }
    public string Message { get; set; }
    public string OperationName { get; set; }

    private object _locker = new();
    
    public void InfoTimerEnable()
    {
        lock (_locker)
        {
            Dispatcher.UIThread.Invoke(_timerEnableAction);
        }
    }

    public void InfoTimerDisable()
    {
        lock (_locker)
        {
            Dispatcher.UIThread.Invoke(_timerDisableAction);
        }
    }

    public void SetInfoTimerEnableAction(Action timerAction)
    {
        _timerEnableAction = timerAction;
    }

    public void SetInfoTimerDisableAction(Action timerAction)
    {
        _timerDisableAction = timerAction;
    }
}