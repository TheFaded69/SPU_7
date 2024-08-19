namespace SPU_7.Domain.Extensions;

public interface IFlowObservable
{
    void RegisterFlowObserver(IFlowObserver observer);

    void RemoveFlowObserver(IFlowObserver observer);

    void NotifyFlowObservers(object? obj);
}