using System.Text;

namespace SPU_7.DeviceCommunication.Extensions;

public static class ElmetroExtensions
{
    /// <summary>
    /// Байт 0x00
    /// </summary>
    private static ReadOnlySpan<byte> ZeroByte => new(new byte[1]);

    /// <summary>
    /// Вычисляет CRC-8 данных
    /// </summary>
    /// <param name="data">Входные данные</param>
    /// <returns>CRC-8 данных</returns>
    public static byte Crc8(this ArraySegment<byte> data) => data.Aggregate((bo, b) => (byte)(bo ^ b));

    /// <summary>
    /// Получить стоку из данных
    /// </summary>
    /// <param name="data">Входные данные</param>
    /// <returns>Строка из данных</returns>
    public static string GetElmetroString(this ReadOnlySpan<byte> data) => Encoding.UTF8.GetString(data.TrimEnd(ZeroByte));

    /// <summary>
    /// Получить стоку из данных
    /// </summary>
    /// <param name="data">Входные данные</param>
    /// <returns>Строка из данных</returns>
    public static string GetElmetroString(this ArraySegment<byte> data) => Encoding.UTF8.GetString(data.Slice(0, data.IndexOf(0)));

    /// <summary>
    /// Получить дату-время из формата ElmetroPascal
    /// </summary>
    /// <param name="data">Байты даты-времени (как минимум 6 байт)</param>
    /// <returns>Дата-время</returns>
    public static DateTime GetElmetroDateTime(this ArraySegment<byte> data) => new(2000 + data[5], data[4], data[3], data[2], data[1], data[0]);
}