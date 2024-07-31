namespace SPU_7.DeviceCommunication.Communication;

public interface IOwenDevice : IDevice
{
    /// <summary>
    /// Реализация протокола ОВЕН
    /// </summary>
    IOwenDeviceProtocol OwenProtocol { get; }
}