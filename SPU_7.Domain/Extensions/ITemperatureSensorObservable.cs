namespace SPU_7.Domain.Extensions;

public interface ITemperatureSensorObservable
{
    void RegisterTemperatureSensorObserver(ITemperatureSensorObserver observer);

    void RemoveTemperatureSensorObserver(ITemperatureSensorObserver observer);

    void NotifyTemperatureSensorObservers(object? obj);
}