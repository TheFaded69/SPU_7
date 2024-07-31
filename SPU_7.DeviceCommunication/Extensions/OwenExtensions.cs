namespace SPU_7.DeviceCommunication.Extensions;

public static class OwenExtensions
{
    /// <summary>
    /// Рассчитать CRC-16 для протокола ОВЕН
    /// </summary>
    /// <param name="data">Входные данные</param>
    /// <returns>CRC-16 код</returns>
    public static ushort OwenCrc16(this IEnumerable<byte> data) => data.Aggregate<byte, ushort>(0x0000, 
        (crc16, dataByte) =>
        {
            for (var i = 0; i < 8; i++, dataByte <<= 1) crc16 = (ushort)(((dataByte ^ (crc16 >> 8)) & 0x80) > 0 ? (crc16 << 1) ^ 0x8F57 : crc16 << 1);
            return crc16;
        });

    /// <summary>
    /// Закодировать данные в протокол ОВЕН
    /// </summary>
    /// <param name="data">Входные данные</param>
    /// <param name="addNewLine">Добавлять ли возврат каретки и конец строки</param>
    /// <returns>Преобразованные данные</returns>
    public static IEnumerable<byte> EncodeToOwenProtocol(this IEnumerable<byte> data) => 
        data.SelectMany(currentByte => new byte[] { (byte)((currentByte >> 4) + 0x47), (byte)((currentByte & 0x0F) + 0x47) }).Prepend((byte)'#').Append((byte)'\r');

    /// <summary>
    /// Декодировать данные из протокол ОВЕН
    /// </summary>
    /// <param name="data">Входные данные</param>
    /// <returns>Преобразованные данные</returns>
    public static IEnumerable<byte> DecodeFromOwenProtocol(this IList<byte> data)
    {
        var sf = string.Empty;
        sf = sf.TrimStart('#').TrimEnd('\r');

        /*if (data.Count <= 2 || data[0] != '#' || data[^1] != '\r') return Array.Empty<byte>();

        for () {
            
        }
        data.Select(currentByte => new byte[] { (byte)((currentByte >> 4) + 0x47), (byte)((currentByte & 0x0F) + 0x47) });*/
        return Array.Empty<byte>();
    }
}