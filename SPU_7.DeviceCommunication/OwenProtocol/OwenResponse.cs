using SPU_7.DeviceCommunication.Communication;
using SPU_7.DeviceCommunication.Extensions;

namespace SPU_7.DeviceCommunication.OwenProtocol;

public class OwenResponse
{
    public OwenResponse()
    {
        _decodedData = Array.Empty<byte>();
    }

    private byte[] _decodedData;

    /// <summary>
    /// Адрес прибора в сети
    /// </summary>
    public int Address { get => ((_decodedData[1] & 0xE0) << 8) | _decodedData[0]; }

    /// <summary>
    /// Признак удалённого запроса
    /// </summary>
    public bool RemoteRequest { get => (_decodedData[1] & 0x10) > 0; }

    /// <summary>
    /// Код параметра на который пришёл ответ
    /// </summary>
    public ushort Parameter { get => BitConverter.ToUInt16(new byte[] { _decodedData[5], _decodedData[4] }); }

    /// <summary>
    /// Размер данных ответа
    /// </summary>
    public int DataSize { get => _decodedData.Length > 1 ? _decodedData [1] & 0x0F : 0; } // До 15 байт поле данных

    /// <summary>
    /// Данные декодированные из канального уровня протокола ОВЕН
    /// </summary>
    public ArraySegment<byte> Data { get => new (_decodedData, 6, DataSize); }

    /// <summary>
    /// Проверка ответа
    /// </summary>
    /// <param name="serialData">Строка с данными</param>
    /// <exception cref="OwenProtocolCrcException">CRC-16 вычисленный и прочитанный не совпадают</exception>
    private void CheckResponse(string serialData)
    {
        var eData = serialData.Where(b => (b != '#') && (b != '\r')).ToArray();
        _decodedData = new byte[eData.Length / 2];
        for (int idx = 0; idx < eData.Length; idx += 2)
            _decodedData[idx / 2] = (byte)(((eData[idx] - 0x47) << 4) | (eData[idx + 1] - 0x47));
        var crcCalc = _decodedData.Take(_decodedData.Length - sizeof(ushort)).OwenCrc16();
        var crc = BitConverter.ToUInt16(_decodedData.TakeLast(sizeof(ushort)).Reverse().ToArray());
        if (crc != crcCalc) throw new OwenProtocolCrcException();
    }

    /// <summary>
    /// Попытаться получить ответ
    /// </summary>
    /// <param name="communicator">Средство для чтения</param>
    public void Get(ICommunicationChannel communicator) => CheckResponse(communicator.ReadLine());

    /// <summary>
    /// Попытаться получить ответ асинхронно
    /// </summary>
    /// <param name="communicator">Средство для чтения</param>
    /// <returns>Задача по ожиданию ответа</returns>
    public async Task GetAsync(ICommunicationChannel communicator) => CheckResponse(await communicator.ReadLineAsync().ConfigureAwait(false));
}