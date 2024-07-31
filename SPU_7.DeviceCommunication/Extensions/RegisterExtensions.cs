using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text;
using SPU_7.DeviceCommunication.Modbus;
using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.DeviceCommunication.Extensions;

public static class RegisterExtensions
{
    static readonly Dictionary<Enum, RegisterConfiguration> _cachedRegisterConfiguration = new();
    static readonly Dictionary<Enum, DataTypeDescriptionAttribute> _cachedDataTypeDescription = new();

    /// <summary>
    /// Получить конфигурацию регистра из значения перечисления
    /// </summary>
    /// <param name="registerMapValue">Значение перечисления</param>
    /// <returns>Конфигурация регистра</returns>
    public static RegisterConfiguration? GetRegisterConfiguration(this Enum registerMapValue) =>
        _cachedRegisterConfiguration.TryGetValue(registerMapValue, out RegisterConfiguration? regConfig)
            ? regConfig
            : registerMapValue.GetType()
                .GetMember(registerMapValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute(typeof(RegisterConfigurationAttribute), false) is RegisterConfigurationAttribute rc
                ? _cachedRegisterConfiguration[registerMapValue] = rc.RegisterConfiguration
                : null;

    /// <summary>
    /// Получить описание типа преобразовываемого для взаимодействия с устройством
    /// </summary>
    /// <param name="dataTypeEnum">Тип данных в перечислении RegisterDataType</param>
    /// <returns>Описание типа данных атрибутом</returns>
    public static DataTypeDescriptionAttribute? GetDataTypeDescription(this Enum dataTypeEnum) =>
        _cachedDataTypeDescription.TryGetValue(dataTypeEnum, out DataTypeDescriptionAttribute? regDataTypeDescription)
            ? regDataTypeDescription
            : dataTypeEnum.GetType()
                .GetMember(dataTypeEnum.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DataTypeDescriptionAttribute>(false) is DataTypeDescriptionAttribute dtd
                ? _cachedDataTypeDescription[dataTypeEnum] = dtd
                : null;

    #region Работа со строками

    /// <summary>
    /// Конвертировние массива символов в кодировке Win1251 с завершающим нулем в строку
    /// </summary>
    /// <param name="data">Массив символов в кодировке Win1251 с завершающим нулем</param>
    public static string Win1251ToString(this byte[] data)
    {
        // Поиск завершающего 0 в данных и если есть, изменить размер строки данных до него
        var nullIndex = Array.IndexOf(data, (byte)0);
        if (nullIndex >= 0) Array.Resize(ref data, nullIndex);
        // Изменение кодировки строки:
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);
        var unicodeBytes = Encoding.Convert(enc1251, Encoding.Unicode, data);
        return Encoding.Unicode.GetString(unicodeBytes);
    }

    /// <summary>
    /// Конвертирует строку в массив символов в кодировке Win1251 с завершающим нулем для передачи в устройство.
    /// Размер выходных данных будет скорректирован в соответствии с <see cref="maxOutDataLength"/> и выровнен кратно 2.
    /// </summary>
    /// <param name="unicodeString">Строка</param>
    /// <param name="maxOutDataLength">Максимальная длина выходных данных (с учетом завершающего нуля) - должна быть кратна 2</param>
    /// <returns>Данные для записи</returns>
    public static byte[] UnicodeStringToWin1251Data(this string unicodeString, int maxOutDataLength)
    {
        if (string.IsNullOrEmpty(unicodeString)) return Array.Empty<byte>();
        // Максимальная длина выходных данных должна быть кратна 2
        if (maxOutDataLength % 2 > 0) maxOutDataLength--;
        if (maxOutDataLength == 0) return Array.Empty<byte>();
        // Перекодировка строки
        var unicodeBytes = Encoding.Unicode.GetBytes(unicodeString);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        var enc1251 = Encoding.GetEncoding(1251);
        var win1251Bytes = Encoding.Convert(Encoding.Unicode, enc1251, unicodeBytes);
        var wbSegment = new ArraySegment<byte>(win1251Bytes);
        if (win1251Bytes.Length % 2 > 0) {
            Array.Resize(ref win1251Bytes, win1251Bytes.Length + 1);
        }
        else {
            Array.Resize(ref win1251Bytes, win1251Bytes.Length + 2);
        }
        // Если длина данных превышает максимальную с учетом завершающего нуля - нужно скорректирвать
        if (win1251Bytes.Length <= maxOutDataLength - 1) {
            wbSegment.SwapBytes();
            return win1251Bytes;
        }
        Array.Resize(ref win1251Bytes, maxOutDataLength);
        win1251Bytes[maxOutDataLength - 1] = 0;
        // попарно переворачиваем байты, т.к. строки в устройствах хранятся как 1, 0, 3, 2, 5, 4, 7, 6, 9...
        wbSegment.SwapBytes();
        return win1251Bytes;
    }

    #endregion

    #region Дата и время

    /// <summary>
    /// Преобразовать полученные данные в Дату-время
    /// </summary>
    /// <param name="data">Данные даты-времени</param>
    /// <returns>Дата-время</returns>
    public static DateTime GetDateTimeFromData(this ArraySegment<byte> data)
    {
        try {
            return new DateTime(BitConverter.ToUInt16(data.Slice(4, 2)), data[6], data[7], data[3], data[2], data[1], data[0] * 4);
        }
        catch (Exception) {
            return default;
        }
    }

    /// <summary>
    /// Преобразовать Дату-время в данные
    /// </summary>
    /// <param name="dateTime">Дата-время</param>
    /// <returns>Данные даты-времени</returns>
    public static byte[] GetDataFromDateTime(this DateTime dateTime)
    {
        var outputData = new byte[8];
        Array.Copy(BitConverter.GetBytes((ushort)dateTime.Year), 0, outputData, 4, 2);
        outputData[0] = (byte)(dateTime.Millisecond / 4);
        outputData[1] = (byte)dateTime.Second;
        outputData[2] = (byte)dateTime.Minute;
        outputData[3] = (byte)dateTime.Hour;
        outputData[6] = (byte)dateTime.Day;
        outputData[7] = (byte)dateTime.Month;
        return outputData;
    }

    #endregion

    #region Работа с данными

    /// <summary>
    /// Поменять байты местами в массиве
    /// </summary>
    /// <param name="data">Входные данные</param>
    /// <returns>Выходные данные</returns>
    public static ArraySegment<byte> SwapBytes(this ArraySegment<byte> data)
    {
        if (data.Count % 2 > 0) throw new DataMisalignedException("Данные не выровнены до чётного числа байт!");
        for (var i = 0; i < data.Count; i += 2) {
            (data[i], data[i + 1]) = (data[i + 1], data[i]);
        }
        return data;
    }

    /// <summary>
    /// Преобразовать блок данных в массив
    /// </summary>
    /// <param name="data">Данные для преобразования</param>
    /// <param name="baseRegisterDataType">Базовый тип данных</param>
    /// <returns>Список объектов из полученых данных</returns>
    public static IList ConvertToArray(this ArraySegment<byte> data, RegisterDataType baseRegisterDataType, DeviceEndianess endianess)
    {
        var size = baseRegisterDataType.GetDataTypeDescription()?.Size ?? 2;
        var outArray = new ArrayList();
        for (var i = 0; i < data.Count; i += size) {
            var registerData = data.Slice(i, size).ConvertRegisterData(baseRegisterDataType, endianess);
            outArray.Add(registerData);
        }
        return outArray;
    }

    /// <summary>
    /// Преобразовать данные из массива значений в байты
    /// </summary>
    /// <param name="objectsList">Список объектов</param>
    /// <param name="baseRegisterDataType">Базовый тип данных</param>
    /// <returns>Массив данных</returns>
    public static byte[] ConvertFromArray(this IList objectsList, RegisterDataType baseRegisterDataType, DeviceEndianess endianess)
    {
        var objData = new List<byte>();
        foreach(var obj in objectsList) {
            objData.AddRange(obj.ConvertRegisterData(baseRegisterDataType, endianess));
        }
        return objData.ToArray();
    }

    /// <summary>
    /// Приведение данных к необходимому виду
    /// </summary>
    /// <param name="data">Данные для входа</param>
    /// <param name="endianess">Тип представления памяти в устройстве</param>
    /// <returns>Преобразованные данные</returns>
    /// <exception cref="NotImplementedException">Выбранная модель памяти не поддерживается</exception>
    public static ArraySegment<byte> ConvertRegisterBytes(this ArraySegment<byte> data, DeviceEndianess endianess) => endianess switch
    {
        DeviceEndianess.ABCD => data.ReverseSegment(),
        DeviceEndianess.BADC => data.SwapBytes().ReverseSegment(),
        DeviceEndianess.DCBA => data,
        DeviceEndianess.CDAB => data.SwapBytes(),
        _ => throw new NotImplementedException("Данная модель памяти не поддерживается")
    };

    /// <summary>
    /// Преобразование данных в нужное представление значения
    /// </summary>
    /// <param name="data">Данные для входа</param>
    /// <param name="dataType">Тип данных для регистра</param>
    /// <returns>Преобразованные данные в объект C#</returns>
    /// <exception cref="IndexOutOfRangeException">Выбранный тип данных для регистра не поддерживается</exception>
    public static object ConvertRegisterData(this ArraySegment<byte> data, RegisterDataType dataType, DeviceEndianess endianess)
    {
        if ((dataType & RegisterDataType.ArrayValuesFlag) > 0) {
            return data.ConvertToArray(dataType & RegisterDataType.ValueMask, endianess);
        }
        data = data.ConvertRegisterBytes(endianess);
        return dataType switch
        {
            RegisterDataType.Int16 => BitConverter.ToInt16(data),
            RegisterDataType.Int32 => BitConverter.ToInt32(data),
            RegisterDataType.Int64 => BitConverter.ToInt64(data),
            RegisterDataType.UInt16 => BitConverter.ToUInt16(data),
            RegisterDataType.UInt32 => BitConverter.ToUInt32(data),
            RegisterDataType.UInt64 => BitConverter.ToUInt64(data),
            RegisterDataType.Float => BitConverter.ToSingle(data),
            RegisterDataType.Double => BitConverter.ToDouble(data),
            RegisterDataType.ByteArray => data.ToArray(),
            RegisterDataType.CharArray => data.ToArray(),
            RegisterDataType.UnixTime => BitConverter.ToUInt32(data),
            RegisterDataType.TDateTime => data.GetDateTimeFromData(),
            _ => throw new ArgumentOutOfRangeException(nameof(dataType), "Выбранный тип данных не поддерживается в текущей реализации"),
        };
    }

    /// <summary>
    /// Преобразование из объекта в данные для записи регистра
    /// </summary>
    /// <param name="data">Объект для записи</param>
    /// <param name="dataType">Тип данных регистра для записи</param>
    /// <returns>Данные в виде байтов</returns>
    /// <exception cref="IndexOutOfRangeException">Выбранный тип данных для регистра не поддерживается</exception>
    public static ArraySegment<byte> ConvertRegisterData(this object data, RegisterDataType dataType, DeviceEndianess endianess)
    {
        if ((dataType & RegisterDataType.ArrayValuesFlag) > 0) {
            return (data as IList)?.ConvertFromArray(dataType & RegisterDataType.ValueMask, endianess) ?? Array.Empty<byte>();
        }
        return new ArraySegment<byte>(dataType switch
        {
            RegisterDataType.Int16 => BitConverter.GetBytes(Convert.ToInt16(data)),
            RegisterDataType.Int32 => BitConverter.GetBytes(Convert.ToInt32(data)),
            RegisterDataType.Int64 => BitConverter.GetBytes(Convert.ToInt64(data)),
            RegisterDataType.UInt16 => BitConverter.GetBytes(Convert.ToUInt16(data)),
            RegisterDataType.UInt32 => BitConverter.GetBytes(Convert.ToUInt32(data)),
            RegisterDataType.UInt64 => BitConverter.GetBytes(Convert.ToUInt64(data)),
            RegisterDataType.Float => BitConverter.GetBytes(Convert.ToSingle(data, CultureInfo.InvariantCulture)),
            RegisterDataType.Double => BitConverter.GetBytes(Convert.ToDouble(data, CultureInfo.InvariantCulture)),
            RegisterDataType.ByteArray => (byte[])data,
            RegisterDataType.CharArray => (byte[])data,
            RegisterDataType.UnixTime => BitConverter.GetBytes(Convert.ToUInt32(data)),
            RegisterDataType.TDateTime => Convert.ToDateTime(data).GetDataFromDateTime(),
            _ => throw new ArgumentOutOfRangeException(nameof(dataType), "Выбранный тип данных не поддерживается в текущей реализации"),
        }).ConvertRegisterBytes(endianess);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerator"></param>
    /// <param name="dataType"></param>
    /// <param name="endianess"></param>
    /// <returns></returns>
    public static T ConvertRegisterData<T>(this IEnumerator enumerator, RegisterDataType dataType, DeviceEndianess endianess) =>
        (T)new ArraySegment<byte>(enumerator.TakeToArray<byte>(dataType.GetDataTypeDescription()?.Size ?? 0)).ConvertRegisterData(dataType, endianess);

    /// <summary>
    /// Преобразовать значение в желаемое представление
    /// </summary>
    /// <typeparam name="T">Тип конечного значения</typeparam>
    /// <param name="value">Значение для преобразования</param>
    /// <returns>Преобразованное значение</returns>
    public static T? ConvertObjectTo<T>(this object value) // , Encoding encoding - 
    {
        return Nullable.GetUnderlyingType(typeof(T)) switch
        {
            Type t when t == typeof(bool)   => (T)(object)Convert.ToBoolean(value),
            Type t when t == typeof(sbyte)  => (T)(object)Convert.ToSByte(value),
            Type t when t == typeof(char)   => (T)(object)Convert.ToChar(value),
            Type t when t == typeof(short)  => (T)(object)Convert.ToInt16(value),
            Type t when t == typeof(int)    => (T)(object)Convert.ToInt32(value),
            Type t when t == typeof(long)   => (T)(object)Convert.ToInt64(value),
            Type t when t == typeof(byte)   => (T)(object)Convert.ToByte(value),
            Type t when t == typeof(ushort) => (T)(object)Convert.ToUInt16(value),
            Type t when t == typeof(uint)   => (T)(object)Convert.ToUInt32(value),
            Type t when t == typeof(ulong)  => (T)(object)Convert.ToUInt64(value),
            Type t when t == typeof(float)  => (T)(object)Convert.ToSingle(value),
            Type t when t == typeof(double) => (T)(object)Convert.ToDouble(value),
            Type t when t == typeof(decimal)=> (T)(object)Convert.ToDecimal(value),
            Type t when t == typeof(string) => (T?)(object?)Convert.ToString(value),
            _ => (T?)value,
        };
    }

    #endregion
}