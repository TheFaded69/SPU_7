using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.DeviceCommunication.OwenProtocol;

public class OwenRequest
{
    public OwenRequest(int address, ushort parameter)
    {
        var requestData = new List<byte> { (byte)(address & 0xFF), (byte)((address >> 8) & 0x07) };
        requestData.AddRange(BitConverter.GetBytes(parameter).Reverse());
        requestData.AddRange(BitConverter.GetBytes(requestData.OwenCrc16()).Reverse());
        Data = requestData.EncodeToOwenProtocol().ToArray();
    }

    /// <summary>
    /// Данные для запроса
    /// </summary>
    public byte[] Data { get; }
}