namespace SPU_7.DeviceCommunication.Extensions;
public static class StringFormatExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="str"></param>
    /// <param name="num"></param>
    /// <returns></returns>
    public static IEnumerable<string> SplitOn(this string str, int num)
    {
        int offset = 0;
        while (offset < str.Length) {
            var lenLeft = str.Length - offset;
            if (lenLeft < num) num = lenLeft; 
            yield return str.Substring(offset, num);
            offset += num;
        }
    }

    /// <summary>
    /// Преобразовать данные в строку с шестнадцатеричным представлением
    /// </summary>
    /// <param name="data">Данные</param>
    public static string ToHexString(this byte[] data, char separator = ' ') => string.Join(separator, Convert.ToHexString(data).SplitOn(2));

    /// <summary>
    /// Преобразовать данные в строку с шестнадцатеричным представлением
    /// </summary>
    /// <param name="data">Данные</param>
    public static string ToHexString(this ArraySegment<byte> data, char separator = ' ') => string.Join(separator, Convert.ToHexString(data).SplitOn(2));
}
