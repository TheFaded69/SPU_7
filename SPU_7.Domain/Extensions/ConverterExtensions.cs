using System.Text;
using SPU_7.Modbus.Extensions;

namespace SPU_7.Domain.Extensions;

public static class ConverterExtensions
{
    /// <summary>
    /// Перекодирует массив символов в кодировке Win1251 с завершающим нулем из устройства в строку Unicode
    /// </summary>
    /// <param name="data">массив символов в кодировке Win1251 с завершающим нулем</param>
    /// <returns>строку</returns>
    public static string ToUnicodeString(this byte[] data)
    {
        // ищем 0 в данных (ноль терминированная строка) и если есть, делаем ресайз
        var nullIndex = Array.IndexOf(data, (byte)0);
        if (nullIndex >= 0) Array.Resize(ref data, nullIndex);
        // перекодируем
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);
        var unicodeBytes = Encoding.Convert(enc1251, Encoding.Unicode, data);
        return Encoding.Unicode.GetString(unicodeBytes);
    }

    /// <summary>
    /// Конвертирует массив символов с завершающим нулем (null-terminated string) в string
    /// </summary>
    /// <param name="chars"></param>
    /// <returns></returns>
    public static string NullTerminatedToString(this char[] chars)
    {
        if (chars == null || chars.Length == 0)
            return "";
        for (var i = 0; i < chars.Length; i++)
        {
            if (chars[i] != '\0') continue;
            var str = new string(chars, 0, i);
            return str;
        }
        return "";
    }

    /// <summary>
    /// Конвертирует строку Unicode в массив символов в кодировке Win1251 с завершающим нулем для передачи в устройство.
    /// Размер выходных данных будет скорректирован в соответствии с <see cref="maxOutDataLength"/> и выровнен кратно 2.
    /// </summary>
    /// <param name="unicodeString">строка Unicode</param>
    /// <param name="maxOutDataLength">максимальная длина выходных данных (с учетом завершающего нуля) - должна быть кратна 2</param>
    /// <returns></returns>
    public static byte[] UnicodeStringToWin1251Data(this string unicodeString, int maxOutDataLength)
    {
        if (string.IsNullOrEmpty(unicodeString)) return null;
        // максимальная длина выходных данных должна быть кратна 2
        if (maxOutDataLength % 2 > 0) maxOutDataLength--;
        if (maxOutDataLength == 0) return null;
        // перекодируем
        var unicodeBytes = Encoding.Unicode.GetBytes(unicodeString);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);
        var win1251Bytes = Encoding.Convert(Encoding.Unicode, enc1251, unicodeBytes);
        if (win1251Bytes.Length % 2 > 0)
        {
            Array.Resize(ref win1251Bytes, win1251Bytes.Length + 1);
        }
        else
        {
            Array.Resize(ref win1251Bytes, win1251Bytes.Length + 2);
        }
        // если длина данных превышает максимальную с учетом завершающего нуля - корректируем
        if (win1251Bytes.Length <= maxOutDataLength - 1)
        {
            win1251Bytes.SwapBytes();
            return win1251Bytes;
        }
        Array.Resize(ref win1251Bytes, maxOutDataLength);
        win1251Bytes[maxOutDataLength - 1] = 0;
        // попарно переворачиваем байты, т.к. строки в устройствах хранятся как 1, 0, 3, 2, 5, 4, 7, 6, 9...
        win1251Bytes.SwapBytes();
        return win1251Bytes;
    }

    /// <summary>
    /// Конвертирует массив байтов в формате TDateTime в DateTime
    /// </summary>
    /// <param name="data">массив байтов</param>
    /// <returns>дата/время</returns>
    // ReSharper disable once InconsistentNaming
    public static DateTime TDateTimeToDateTime(this byte[] data)
    {
        var year = new byte[2];
        Array.Copy(data, 4, year, 0, 2);
        return new DateTime(BitConverter.ToUInt16(year), data[6], data[7], data[3], data[2], data[1], data[0] * 4);
    }

    /// <summary>
    /// Конвертирует дату/время в формате DateTime в массив байтов в формате TDateTime
    /// </summary>
    /// <param name="dateTime">дата/время</param>
    /// <returns>массив байтов</returns>
    public static byte[] DateTimeToTDateTime(this DateTime dateTime)
    {
        var data = new byte[8];
        data[1] = (byte)dateTime.Second;
        data[2] = (byte)dateTime.Minute;
        data[3] = (byte)dateTime.Hour;
        var year = BitConverter.GetBytes((ushort)dateTime.Year);
        data[4] = year[0];
        data[5] = year[1];
        data[6] = (byte)dateTime.Month;
        data[7] = (byte)dateTime.Day;
        return data;
    }

    public static byte[] DateTimeToUnixTime(this DateTime dateTime) =>
        BitConverter.GetBytes((uint)((DateTimeOffset)dateTime).ToUnixTimeSeconds());
}