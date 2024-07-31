using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.ElmetroResponses;

public abstract class DigitalElmetroResponse
{
    /// <summary>
    /// Базовый ответ от DigitalElmetro устройства
    /// </summary>
    /// <param name="responseSize">Ожидаемый или базовый размер ответа</param>
    public DigitalElmetroResponse(int responseSize)
    {
        if (responseSize > MaxResponseSize || responseSize < MinResponseSize) throw new ArgumentOutOfRangeException(nameof(responseSize));
        _responseMaxSize = responseSize;
        _responseSize = 0;
        ResponseData = new byte[_responseMaxSize];
    }

    protected const int BaseResponseSize = 6; // Базовый размер ответа
    protected const int StartByteIndex = 0; // Индекс стартового байта
    protected const int DeviceAddressIndex = 1; // Индекс адреса устройства
    protected const int CommandIndex = 2; // Индекс команды
    protected const int DataSizeIndex = 3; // Индекс для чтения принимаемого кол-ва данных
    protected const int DeviceStateIndex = 4; // Индекс состояния устройства
    private int _responseMaxSize; // Полный/Максимальный размер ответа
    private int _responseSize; // Действительный размер ответа

    /// <summary>
    /// Вычисление CRC-8 ответа
    /// </summary>
    /// <exception cref="Exception"></exception>
    protected void CheckCrc8()
    {
        var crc8 = new ArraySegment<byte>(ResponseData, 0, ResponseData.Length - 1).Crc8();
        if (crc8 != ResponseData[^1]) throw new ElmetroProtocolCrc8Exception(crc8, ResponseData[^1]);
    }

    /// <summary>
    /// Вычисление CRC-8 ответа без выброса исключений
    /// </summary>
    protected bool CheckCrc8_NoThrow()
    {
        try {
            var crc8 = new ArraySegment<byte>(ResponseData, 0, ResponseData.Length - 1).Crc8();
            return crc8 == ResponseData[^1];
        }
        catch (Exception) {
            return false;
        }
    }

    /// <summary>
    /// Минимальный размер ответа в байтах
    /// </summary>
    public static int MinResponseSize => 7;

    /// <summary>
    /// Максимальный размер ответа в байтах
    /// </summary>
    public static int MaxResponseSize => 64;

    /// <summary>
    /// Получить размер данных в ответе
    /// </summary>
    public int DataSize => _responseSize;

    /// <summary>
    /// Получить данные ответа
    /// </summary>
    /// <param name="offset">Смещение в массиве ответа</param>
    /// <param name="count">Количество байт для сегмента данных</param>
    /// <returns>Сегмент данных</returns>
    public ArraySegment<byte> GetData(int offset, int count = 1) => new(ResponseData, MinResponseSize - 1 + offset, count);

    /// <summary>
    /// Данные полученные транзакцией
    /// </summary>
    protected byte[] ResponseData { get; set; }

    /// <summary>
    /// Прочитать ответ от устройства
    /// </summary>
    /// <param name="communicationChannel">Канал связи для чтения</param>
    public virtual DigitalElmetroResponse Read(ICommunicationChannel? communicationChannel)
    {
        if (communicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для устройства Elmetro!");
        communicationChannel.Read(ResponseData, 0, BaseResponseSize);
        _responseSize = Math.Max(0, Math.Min(ResponseData[DataSizeIndex] - 1, _responseMaxSize - BaseResponseSize));
        communicationChannel.Read(ResponseData, BaseResponseSize, _responseSize);
        CheckCrc8();
        return this;
    }

    /// <summary>
    /// Прочитать ответ от устройства асинхронно
    /// </summary>
    /// <param name="communicationChannel">Канал связи для чтения</param>
    /// <returns>Задача ожидания ответа от устройства</returns>
    public virtual async Task<DigitalElmetroResponse> ReadAsync(ICommunicationChannel? communicationChannel, CancellationToken cancellationToken = default)
    {
        if (communicationChannel is null) throw new InvalidOperationException("Не назначен канал связи для устройства Elmetro!");
        await communicationChannel.ReadAsync(new ArraySegment<byte>(ResponseData, 0, BaseResponseSize), cancellationToken).ConfigureAwait(false);
        _responseSize = Math.Max(0, Math.Min(ResponseData[DataSizeIndex] - 1, _responseMaxSize - BaseResponseSize));
        await communicationChannel.ReadAsync(new ArraySegment<byte>(ResponseData, BaseResponseSize, _responseSize), cancellationToken).ConfigureAwait(false);
        CheckCrc8();
        return this;
    }
}