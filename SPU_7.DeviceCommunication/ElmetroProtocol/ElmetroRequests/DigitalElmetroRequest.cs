using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroRequests;

public abstract class DigitalElmetroRequest
{
    public DigitalElmetroRequest(int requestSize)
    {
        if (requestSize > MaxRequestSize || requestSize < MinRequestSize) throw new ArgumentOutOfRangeException(nameof(requestSize));
        RequestData = new byte[requestSize];
        RequestData[StartByteIndex] = 2; // Стартовый байт (должен быть всегда 2 при запросе)
    }

    protected const int StartByteIndex = 0;
    protected const int DeviceAddressIndex = 1;
    protected const int CommandIndex = 2;
    protected const int DataSizeIndex = 3;

    /// <summary>
    /// Миниимальный размер запроса в байтах
    /// </summary>
    protected static int MinRequestSize => 5;

    /// <summary>
    /// Максимальный размер запроса в байтах
    /// </summary>
    protected static int MaxRequestSize => 24;

    /// <summary>
    /// Данные для отправления
    /// </summary>
    protected byte[] RequestData { get; set; }

    /// <summary>
    /// Расчёт CRC-8 для запроса
    /// </summary>
    /// <returns>CRC-8 байт</returns>
    public byte CalculateCrc8() => new ArraySegment<byte>(RequestData, 0, RequestData.Length - 1).Crc8();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="communicationChannel"></param>
    public void Send(ICommunicationChannel? communicationChannel)
    {
        if (communicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для устройства Elmetro!");
        RequestData[^1] = CalculateCrc8();
        communicationChannel.Write(RequestData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="communicationChannel"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task SendAsync(ICommunicationChannel? communicationChannel, CancellationToken cancellationToken = default)
    {
        if (communicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для устройства Elmetro!");
        RequestData[^1] = CalculateCrc8();
        await communicationChannel.WriteAsync(RequestData, cancellationToken).ConfigureAwait(false);
    }
}