using System.Collections;

namespace SPU_7.DeviceCommunication.TurboDevices;
public abstract class TDBlockEnumerator
{
    protected TDBlockEnumerator(IEnumerator enumerator) => InputDataEnumerator = enumerator;

    /// <summary>
    /// Перечислитель для оптимизации прохода по блокам данных
    /// </summary>
    public IEnumerator InputDataEnumerator { get; }
}