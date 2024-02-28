namespace SPU_7.Domain.Extensions;

public interface IDeviceObservable
{
    void RegisterDeviceObserver(IDeviceObserver observer);

    void RemoveDeviceObserver(IDeviceObserver observer);

    void NotifyDeviceObservers(object? obj);
}