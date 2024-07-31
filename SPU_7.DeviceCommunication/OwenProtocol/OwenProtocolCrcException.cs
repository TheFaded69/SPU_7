namespace SPU_7.DeviceCommunication.OwenProtocol;

public class OwenProtocolCrcException : OwenProtocolException
{
    /// <summary>
    /// Ошибка после сравнения CRC-16 Протокола ОВЕН
    /// </summary>
    public OwenProtocolCrcException() : base("Получен неверный CRC-16 для протокола ОВЕН")
    {
    }

    /// <summary>
    /// Ошибка после сравнения CRC-16 Протокола ОВЕН
    /// </summary>
    /// <param name="message">Сообщение об ошибке</param>
    public OwenProtocolCrcException(string message) : base(message)
    {
    }

    /// <summary>
    /// Ошибка после сравнения CRC-16 Протокола ОВЕН
    /// </summary>
    /// <param name="crcInput">CRC-16 из данных</param>
    /// <param name="crcCalculated">CRC-16 расчитанный</param>
    public OwenProtocolCrcException(ushort crcInput, ushort crcCalculated) :
        base($"Получен неверный CRC-16 для протокола ОВЕН (На входе: 0x{crcInput:X4}, Расчитанный: 0x{crcCalculated:X4})")
    {
    }
}
