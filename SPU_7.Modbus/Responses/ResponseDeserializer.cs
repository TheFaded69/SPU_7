using SPU_7.Modbus.Extensions;
using SPU_7.Modbus.Requests;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Responses;

public interface IResponseDeserializer
{
    /// <summary>
    /// Универсальный десериализатор массива байт в класс
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <param name="offset">смещение относительно начала, с которого начинаем читать</param>
    /// <param name="obj">объект, куда десериализуем</param>
    /// <typeparam name="TObject">class</typeparam>
    void Deserialize<TObject>(byte[] data, TObject obj, int offset = 0) where TObject : class;

    /// <summary>
    /// десериализует и возвращает MBAP Header
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    MbapHeader GetMbapHeader(byte[] data);

    /// <summary>
    /// проверка CRC
    /// </summary>
    /// <param name="data">массив байт с CRC в конце</param>
    /// <param name="byteOrder">порядок следования байт</param>
    /// <returns>CRC</returns>
    bool CheckCrc(byte[] data, ByteOrderType byteOrder = ByteOrderType.LittleEndian_DCBA);

    /// <summary>
    /// Проверяет, является ли ответ ответом-ошибкой
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <param name="offset">смещение</param>
    /// <returns>true - если ответ является ошибкой</returns>
    bool IsResponseError(byte[] data, int offset = 0);
}

/// <summary>
/// Десериализатор ответа из массива байт в класс.
/// </summary>
public class ResponseDeserializer : IResponseDeserializer
{
    /// <summary>
    /// Коррекция порядка следования байт
    /// </summary>
    /// <param name="data">корректируемые данные</param>
    /// <param name="order">желаемый порядок следования байт</param>
    private void CorrectByteOrder(byte[] data, ByteOrderType order)
    {
        switch (order)
        {
            case ByteOrderType.LittleEndian_DCBA:
                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(data);
                }
                break;
            case ByteOrderType.MidLittleEndian_CDAB:
                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(data);
                }
                data.SwapBytes();
                break;
            case ByteOrderType.BigEndian_ABCD:
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(data);
                }
                break;
            case ByteOrderType.MidBigEndian_BADC:
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(data);
                }
                data.SwapBytes();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(order), order, null);
        }
    }

    /// <summary>
    /// Универсальный десериализатор массива байт в класс
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <param name="offset">смещение относительно начала, с которого начинаем читать</param>
    /// <param name="obj">объект, куда десериализуем</param>
    /// <typeparam name="TObject">class</typeparam>
    public void Deserialize<TObject>(byte[] data, TObject obj, int offset = 0) where TObject : class
    {
        if (data == null || offset >= data.Length) return;
        var props = obj.GetType().GetProperties().OrderBy(p => p.PropertyOrder());
        foreach (var prop in props)
        {
            if (prop.PropertyType == typeof(float))
            {
                var part = new byte[sizeof(float)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToSingle(part);
                prop.SetValue(obj, val);
                offset += sizeof(float);
            }
            else if (prop.PropertyType == typeof(double))
            {
                var part = new byte[sizeof(double)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToDouble(part);
                prop.SetValue(obj, val);
                offset += sizeof(double);
            }
            else if (prop.PropertyType == typeof(ulong))
            {
                var part = new byte[sizeof(ulong)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToUInt64(part);
                prop.SetValue(obj, val);
                offset += sizeof(ulong);
            }
            else if (prop.PropertyType == typeof(long))
            {
                var part = new byte[sizeof(long)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToInt64(part);
                prop.SetValue(obj, val);
                offset += sizeof(long);
            }
            else if (prop.PropertyType == typeof(uint))
            {
                var part = new byte[sizeof(uint)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToUInt32(part);
                prop.SetValue(obj, val);
                offset += sizeof(uint);
            }
            else if (prop.PropertyType == typeof(int))
            {
                var part = new byte[sizeof(int)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToInt32(part);
                prop.SetValue(obj, val);
                offset += sizeof(int);
            }
            else if (prop.PropertyType == typeof(ushort))
            {
                var part = new byte[sizeof(ushort)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToUInt16(part);
                prop.SetValue(obj, val);
                offset += sizeof(ushort);
            }
            else if (prop.PropertyType == typeof(short))
            {
                var part = new byte[sizeof(short)];
                Array.Copy(data, offset, part, 0, part.Length);
                CorrectByteOrder(part, prop.ByteOrder());
                var val = BitConverter.ToInt16(part);
                prop.SetValue(obj, val);
                offset += sizeof(short);
            }
            else if (prop.PropertyType == typeof(sbyte) || prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(char))
            {
                prop.SetValue(obj, data[offset++]);
            }
            else if (prop.PropertyType == typeof(char[]))
            {
                var array = (char[])prop.GetValue(obj, null);
                if (array == null) continue;
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = (char)data[offset++];
                }
            }
            else if (prop.PropertyType == typeof(byte[]))
            {
                var array = (byte[])prop.GetValue(obj, null);
                if (array == null) continue;
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = data[offset++];
                }
            }
            else if (prop.PropertyType == typeof(List<ushort>))
            {
                var list = (List<ushort>)prop.GetValue(obj, null);
                if (list == null) continue;
                var size = list.Capacity;
                for (var i = 0; i < size; i++)
                {
                    var part = new byte[sizeof(ushort)];
                    Array.Copy(data, offset, part, 0, part.Length);
                    CorrectByteOrder(part, prop.ByteOrder());
                    list.Add(BitConverter.ToUInt16(part));
                    offset += sizeof(ushort);
                }
            }
        }
    }

    /// <summary>
    /// десериализует и возвращает MBAP Header
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public MbapHeader GetMbapHeader(byte[] data)
    {
        var mbapHeader = new MbapHeader();
        Deserialize(data, mbapHeader);
        return mbapHeader;
    }

    /// <summary>
    /// проверка CRC
    /// </summary>
    /// <param name="data">массив байт с CRC в конце</param>
    /// <param name="byteOrder">порядок следования байт</param>
    /// <returns>CRC</returns>
    public bool CheckCrc(byte[] data, ByteOrderType byteOrder = ByteOrderType.LittleEndian_DCBA)
    {
        if (data == null) return false;
        ushort value = 0xFFFF;
        for (var j = 0; j < data.Length - 2; j++)
        {
            var b = data[j];
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

        var crc = new byte[2];
        Array.Copy(data, data.Length - 2, crc, 0, 2);
        CorrectByteOrder(crc, byteOrder);
        return BitConverter.ToUInt16(crc, 0) == value;
    }

    /// <summary>
    /// Проверяет, является ли ответ ответом-ошибкой
    /// </summary>
    /// <param name="data">массив байт</param>
    /// <param name="offset">смещение</param>
    /// <returns>true - если ответ является ошибкой</returns>
    public bool IsResponseError(byte[] data, int offset = 0)
    {
        return (data[offset] & 0x80) > 0;
    }
}