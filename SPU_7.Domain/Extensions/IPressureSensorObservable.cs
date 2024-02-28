namespace SPU_7.Domain.Extensions;

public interface IPressureSensorObservable
{
    void RegisterPressureSensorObserver(IPressureSensorObserver observer);

    void RemovePressureSensorObserver(IPressureSensorObserver observer);

    void NotifyPressureSensorObservers(object? obj);
}