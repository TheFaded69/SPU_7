namespace SPU_7.Common;

public class Unsubscriber<TObserver> : IDisposable where TObserver : class
{
    private readonly List<TObserver> _observers;
    private readonly TObserver _observer;

    public Unsubscriber(List<TObserver> observers, TObserver observer)
    {
        _observers = observers;
        _observer = observer;
    }

    public void Dispose()
    {
        if (_observer != null && _observers.Contains(_observer))
            _observers.Remove(_observer);
    }
}