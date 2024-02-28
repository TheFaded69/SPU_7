using System.Runtime.Serialization;
using SPU_7.Modbus.Extensions;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Requests;

public interface IRequestSerializer
{
    /// <summary>
    /// Сериализует запрос в массив байт
    /// </summary>
    /// <param name="request">запрос</param>
    /// <returns>массив байт</returns>
    /// <exception cref="SerializationException">В случае, если тип данных, использованный в классе запроса, не реализован в сериализаторе.</exception>
    byte[] Serialize(BaseRequest request);

    /// <summary>
    /// Добавляет CRC
    /// </summary>
    /// <param name="data">данные</param>
    /// <param name="crcByteOrder">порядок следования байт в CRC</param>
    /// <returns>массив байт</returns>
    byte[] AddCrc(byte[] data, ByteOrderType crcByteOrder = ByteOrderType.LittleEndian_DCBA);

    /// <summary>
    /// Добавляет MBAP
    /// </summary>
    /// <param name="data">данные</param>
    /// <param name="transactionId">номер транзакции</param>
    /// <returns>массив байт</returns>
    byte[] AddMbapHeader(byte[] data, ushort? transactionId = null);

    /// <summary>
    /// расчет CRC
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <returns>CRC</returns>
    byte[] CalculateCrc(byte[] data);

    /// <summary>
    /// генератор случайного ushort Id
    /// </summary>
    /// <returns></returns>
    ushort GetRandomId();
}

/// <summary>
/// Сериализатор запроса
/// </summary>
public class RequestSerializer : IRequestSerializer
{
    /// <summary>
    /// Сериализует запрос в массив байт
    /// </summary>
    /// <param name="request">запрос</param>
    /// <returns>массив байт</returns>
    /// <exception cref="SerializationException">В случае, если тип данных, использованный в классе запроса, не реализован в сериализаторе.</exception>
    public byte[] Serialize(BaseRequest request)
    {
        byte[] serialized = null;
        // получаем список свойств класса в порядке, указанном атрибутами ParameterOrder
        // var props = request.GetType().GetProperties().Where(p => p.CanWrite).OrderBy(p => p.PropertyOrder());
        var props = request.GetType().GetProperties().OrderBy(p => p.PropertyOrder());
        foreach (var prop in props)
        {
            // если сериализации не требуется - пропускаем
            if (prop.IsSerializationIgnored()) continue;
            // сериализуем очередное свойство в нужном порядке следования байт
            // порядок байт определяется атрибутом ByteOrder, если отсутствует, то BigEndian
            var arr = Convert(prop.GetValue(request), prop.ByteOrder());
            if (arr == null)
            {
                // если сериализация данного типа не реализована
                throw new SerializationException($"Serialization of {prop.GetValue(request)?.GetType()} is not implemented!");
            }
            if (arr.Length == 0) continue;
            // добавляем результат
            serialized ??= Array.Empty<byte>();
            Array.Resize(ref serialized, serialized.Length + arr.Length);
            Array.Copy(arr, 0, serialized, serialized.Length - arr.Length, arr.Length);
        }
        return serialized;
    }

    /// <summary>
    /// Сериализует запрос в массив байт
    /// </summary>
    /// <param name="obj">экземпляр класса</param>
    /// <returns>массив байт</returns>
    /// <exception cref="SerializationException">В случае, если тип данных, использованный в классе запроса, не реализован в сериализаторе.</exception>
    private byte[] Serialize<T>(T obj) where T : class
    {
        byte[] serialized = null;
        // получаем список свойств класса в порядке, указанном атрибутами ParameterOrder
        // var props = obj.GetType().GetProperties().Where(p => p.CanWrite).OrderBy(p => p.PropertyOrder());
        var props = obj.GetType().GetProperties().OrderBy(p => p.PropertyOrder());
        foreach (var prop in props)
        {
            // сериализуем очередное свойство в нужном порядке следования байт
            // порядок байт определяется атрибутом ByteOrder, если отсутствует, то BigEndian
            var arr = Convert(prop.GetValue(obj), prop.ByteOrder());
            if (arr == null)
            {
                // если сериализация данного типа не реализована
                throw new SerializationException($"Serialization of {prop.GetValue(obj)?.GetType()} is not implemented!");
            }
            if (arr.Length == 0) continue;
            // добавляем результат
            serialized ??= Array.Empty<byte>();
            Array.Resize(ref serialized, serialized.Length + arr.Length);
            Array.Copy(arr, 0, serialized, serialized.Length - arr.Length, arr.Length);
        }
        return serialized;
    }

    /// <summary>
    /// Конвертирует массив символов в массив байт.
    /// Символы должны быть ASCII, т.к. кастуются до байта.
    /// </summary>
    /// <param name="arr">массив символов</param>
    /// <returns>массив байт</returns>
    private byte[] CharArrayToByteArray(char[] arr)
    {
        var serialized = new byte[arr.Length];
        for (var i = 0; i < arr.Length; i++)
        {
            serialized[i] = (byte)arr[i];
        }
        return serialized;
    }

    /// <summary>
    /// список ushort в массив байт
    /// </summary>
    /// <param name="list">список</param>
    /// <param name="byteOrder">какой порядок байт желаем получить</param>
    /// <returns>массив байт</returns>
    private byte[] UshortListToByteArray(List<ushort> list, ByteOrderType byteOrder = ByteOrderType.BigEndian_ABCD)
    {
        if (list == null) return Array.Empty<byte>();
        byte[] serialized = null;
        foreach (var arr in list.Select(item => BitConverter.IsLittleEndian
                         ? byteOrder == ByteOrderType.BigEndian_ABCD ? BitConverter.GetBytes(item).Reverse().ToArray() : BitConverter.GetBytes(item)
                         : byteOrder == ByteOrderType.BigEndian_ABCD ? BitConverter.GetBytes(item) : BitConverter.GetBytes(item).Reverse().ToArray())
                     .Where(arr => arr.Length != 0))
        {
            // добавляем результат
            serialized ??= Array.Empty<byte>();
            Array.Resize(ref serialized, serialized.Length + arr.Length);
            Array.Copy(arr, 0, serialized, serialized.Length - arr.Length, arr.Length);
        }
        return serialized;
    }

    /// <summary>
    /// Универсальный конвертер. Сериализует определенный тип данных
    /// в массив байт в порядке следования <see cref="byteOrder"/>. Также учитывается
    /// тип хранения байт на текущей архитектуре процессора.
    /// </summary>
    /// <param name="value">конвертируемое значение</param>
    /// <param name="byteOrder">какой порядок байт желаем получить</param>
    /// <typeparam name="T">тип данных</typeparam>
    /// <returns>массив байт</returns>
    private byte[] Convert<T>(T value, ByteOrderType byteOrder = ByteOrderType.BigEndian_ABCD)
    {
        var arr = value switch
        {
            float floatVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(floatVal) : BitConverter.GetBytes(floatVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(floatVal).SwapBytes() : BitConverter.GetBytes(floatVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(floatVal).Reverse().ToArray() : BitConverter.GetBytes(floatVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(floatVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(floatVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            double doubleVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(doubleVal) : BitConverter.GetBytes(doubleVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(doubleVal).SwapBytes() : BitConverter.GetBytes(doubleVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(doubleVal).Reverse().ToArray() : BitConverter.GetBytes(doubleVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(doubleVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(doubleVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            long longVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(longVal) : BitConverter.GetBytes(longVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(longVal).SwapBytes() : BitConverter.GetBytes(longVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(longVal).Reverse().ToArray() : BitConverter.GetBytes(longVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(longVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(longVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            ulong ulongVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ulongVal) : BitConverter.GetBytes(ulongVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ulongVal).SwapBytes() : BitConverter.GetBytes(ulongVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ulongVal).Reverse().ToArray() : BitConverter.GetBytes(ulongVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ulongVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(ulongVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            int intVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(intVal) : BitConverter.GetBytes(intVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(intVal).SwapBytes() : BitConverter.GetBytes(intVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(intVal).Reverse().ToArray() : BitConverter.GetBytes(intVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(intVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(intVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            uint uintVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(uintVal) : BitConverter.GetBytes(uintVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(uintVal).SwapBytes() : BitConverter.GetBytes(uintVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(uintVal).Reverse().ToArray() : BitConverter.GetBytes(uintVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(uintVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(uintVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            short shortVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(shortVal) : BitConverter.GetBytes(shortVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(shortVal).SwapBytes() : BitConverter.GetBytes(shortVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(shortVal).Reverse().ToArray() : BitConverter.GetBytes(shortVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(shortVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(shortVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            ushort ushortVal => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ushortVal) : BitConverter.GetBytes(ushortVal).Reverse().ToArray(),
                ByteOrderType.MidLittleEndian_CDAB => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ushortVal).SwapBytes() : BitConverter.GetBytes(ushortVal).Reverse().ToArray().SwapBytes(),
                ByteOrderType.BigEndian_ABCD => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ushortVal).Reverse().ToArray() : BitConverter.GetBytes(ushortVal),
                ByteOrderType.MidBigEndian_BADC => BitConverter.IsLittleEndian ? BitConverter.GetBytes(ushortVal).Reverse().ToArray().SwapBytes() : BitConverter.GetBytes(ushortVal).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            sbyte sbytetVal => new[] { (byte)sbytetVal },
            byte byteVal => new[] { byteVal },
            char charVal => new[] { (byte)charVal },
            char[] charArray => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => CharArrayToByteArray(charArray),
                ByteOrderType.MidLittleEndian_CDAB => CharArrayToByteArray(charArray).SwapBytes(),
                ByteOrderType.BigEndian_ABCD => CharArrayToByteArray(charArray),
                ByteOrderType.MidBigEndian_BADC => CharArrayToByteArray(charArray).SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            byte[] byteArray => byteOrder switch
            {
                ByteOrderType.LittleEndian_DCBA => byteArray,
                ByteOrderType.MidLittleEndian_CDAB => byteArray.SwapBytes(),
                ByteOrderType.BigEndian_ABCD => byteArray,
                ByteOrderType.MidBigEndian_BADC => byteArray.SwapBytes(),
                _ => throw new ArgumentOutOfRangeException(nameof(byteOrder), byteOrder, null)
            },
            List<ushort> ushortList => UshortListToByteArray(ushortList, byteOrder),
            _ => null
        };
        return arr;
    }

    /// <summary>
    /// Добавляет CRC
    /// </summary>
    /// <param name="data">данные</param>
    /// <param name="crcByteOrder">порядок следования байт в CRC</param>
    /// <returns>массив байт</returns>
    public byte[] AddCrc(byte[] data, ByteOrderType crcByteOrder = ByteOrderType.LittleEndian_DCBA)
    {
        var crc = CalculateCrc(data);
        var result = new byte[data.Length + crc.Length];
        Array.Copy(data, result, data.Length);
        Array.Copy(crc, 0, result, data.Length, crc.Length);
        return result;
    }

    /// <summary>
    /// Добавляет MBAP
    /// </summary>
    /// <param name="data">данные</param>
    /// <param name="transactionId">номер транзакции</param>
    /// <returns>массив байт</returns>
    public byte[] AddMbapHeader(byte[] data, ushort? transactionId = null)
    {
        transactionId ??= GetRandomId();
        var mbapObject = new MbapHeader
        {
            TransactionId = (ushort)transactionId,
            ProtocolId = 0,
            Length = (ushort)data.Length
        };
        var mbap = Serialize(mbapObject);
        var result = new byte[data.Length + mbap.Length];
        Array.Copy(mbap, result, mbap.Length);
        Array.Copy(data, 0, result, mbap.Length, data.Length);
        return result;
    }

    /// <summary>
    /// расчет CRC
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <returns>CRC</returns>
    public byte[] CalculateCrc(byte[] data)
    {
        if (data == null) return Array.Empty<byte>();
        ushort value = 0xFFFF;
        foreach (var b in data)
        {
            value ^= b;
            for (byte i = 0; i < 8; i++)
            {
                var carry = (byte)(value & 1);
                value >>= 1;
                if (carry != 0)
                {
                    value ^= 0xA001;
                }
            }
        }
        return BitConverter.GetBytes(value);
    }

    /// <summary>
    /// генератор случайного ushort Id
    /// </summary>
    /// <returns></returns>
    public ushort GetRandomId()
    {
        var rand = new Random((int)DateTime.Now.Ticks);
        return (ushort)rand.Next(0x10000);
    }
}