namespace SPU_7.DeviceCommunication.ElmetroProtocol;

/// <summary>
/// Получен неправильный CRC-8 код от устройства элметро
/// </summary>
public class ElmetroProtocolCrc8Exception : ElmetroProtocolException
{
    public ElmetroProtocolCrc8Exception(string? message) : base(message)
    {

    }

    public ElmetroProtocolCrc8Exception(byte calculated, byte answer) : base($"CRC-8 в ответе неправильный! Расчитанный: 0x{calculated:X2}, Полученный: 0x{answer:X2}.")
    {

    }
}