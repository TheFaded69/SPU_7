namespace SPU_7.DeviceCommunication.ElmetroProtocol;

/// <summary>
/// Исключение протокола связи с устройством Elmetro
/// </summary>
public class ElmetroProtocolException : Exception
{
    public ElmetroProtocolException(string? message) : base(message)
    {

    }
}