namespace SPU_7.CommonDevice.Utils;

/// <summary>
/// Генератор мастер пароля
/// </summary>
public static class MasterPasswordGenerator
{
    /// <summary>
    /// Подсчет хэша
    /// </summary>
    /// <param name="dataString">Строка пароль</param>
    /// <returns>Хэш</returns>
    private static uint CalculateHashCrc32(string dataString) => dataString.Aggregate<char, uint>(0, (current, ch) => AddByteToCrc32(current, (byte)ch));

    /// <summary>
    /// Подсчет хэша
    /// </summary>
    /// <param name="data">Пароль</param>
    /// <returns>Хэш</returns>
    private static uint CalculateHashCrc32(byte[] data) => data.Aggregate<byte, uint>(0, AddByteToCrc32);

    private static uint HashFaq6(string dataString)
    {
        uint hash = 0;
        foreach (var symbol in dataString)
        {
            hash += (byte)symbol;
            hash += hash << 10;
            hash ^= hash >> 6;
        }
        hash += hash << 3;
        hash ^= hash >> 11;
        hash += hash << 15;
        return hash;
    }

    private static uint AddByteToCrc32(uint oldCrc32, byte b)
    {
        const uint crcPoly = 0xEDB88320;
        const uint crcMask = 0xD202EF8D;
        var T = (oldCrc32 & 0xFF) ^ b;
        for (var j = 0; j < 8; j++)
        {
            T = (T & 1) != 0 ? T >> 1 ^ crcPoly : T >> 1;
        }
        return T ^ oldCrc32 >> 8 ^ crcMask;
    }

    /// <summary>
    /// Расчёт HASH при вводе пароля
    /// </summary>
    /// <param name="password">Пароль в текстовом виде</param>
    /// <returns>HASH</returns>
    public static uint CalculateHash(string password) =>
        password.Aggregate(0u, (crc32, ch) => AddByteToCrc32(crc32, (byte)ch));

    /// <summary>
    /// Получить хэш мастер-пароля
    /// </summary>
    /// <param name="serialNumber">Внутренний (заводской) номер</param>
    /// <param name="authorizationKey">Номер для запроса одноразового пароля</param>
    /// <returns>Хэш мастер-пароля</returns>
    public static uint Get(uint serialNumber, uint authorizationKey)
    {
        var factoryNumberHex = serialNumber.ToString("X8");
        var hashFactoryNumberHex = CalculateHashCrc32(factoryNumberHex).ToString("X8");
        var sessionIdHex = authorizationKey.ToString("X8");
        var hashSessionIdHex = CalculateHashCrc32(sessionIdHex).ToString("X8");
        var resultingString = string.Empty;
        for (var i = 0; i < factoryNumberHex.Length; i++)
        {
            resultingString += $"{factoryNumberHex[i]}{sessionIdHex[i]}{hashFactoryNumberHex[i]}{hashSessionIdHex[i]}";
        }
        return CalculateHash(HashFaq6(resultingString).ToString());
    }
}