namespace SPU_7.DeviceCommunication.OwenProtocol;

public class OwenProtocolException : Exception
{
    /// <summary>
    /// Исключение протокола связи ОВЕН
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public OwenProtocolException(string message) : base(message) { }
}