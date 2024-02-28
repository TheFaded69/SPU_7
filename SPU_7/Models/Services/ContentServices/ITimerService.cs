using System;

namespace SPU_7.Models.Services.ContentServices;

public interface ITimerService
{
     int TimeSeconds { get; set; }
    string Message { get; set; }
    string OperationName { get; set; }

    void InfoTimerEnable();
    void InfoTimerDisable();
    void SetInfoTimerEnableAction(Action timerAction);
    void SetInfoTimerDisableAction(Action timerAction);
}