using System.Reflection;
using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Extensions;

public static class PropertyInfoExtensions
{
    /// <summary>
    /// Порядок сериализации свойств в классе
    /// </summary>
    /// <param name="propInfo">Атрибуты свойства</param>
    /// <returns>индекс</returns>
    public static int PropertyOrder(this PropertyInfo propInfo)
    {
        var orderAttr = (SerializationOrderAttribute)propInfo.GetCustomAttributes(typeof(SerializationOrderAttribute), true).SingleOrDefault();
        return orderAttr?.Order ?? int.MaxValue;
    }

    /// <summary>
    /// Желаемый порядок байт в результате сериализции
    /// </summary>
    /// <param name="propInfo">Атрибуты свойства</param>
    /// <returns>порядок байт</returns>
    public static ByteOrderType ByteOrder(this PropertyInfo propInfo)
    {
        var orderAttr = (ByteOrderAttribute)propInfo.GetCustomAttributes(typeof(ByteOrderAttribute), true).SingleOrDefault();
        return orderAttr?.Order ?? ByteOrderType.BigEndian_ABCD;
    }

    /// <summary>
    /// Игнорировать
    /// </summary>
    /// <param name="propInfo">Атрибуты свойства</param>
    public static bool IsSerializationIgnored(this PropertyInfo propInfo)
    {
        var serAttr = (SerializationIgnoredAttribute)propInfo.GetCustomAttributes(typeof(SerializationIgnoredAttribute), true).SingleOrDefault();
        return serAttr?.IsIgnored ?? false;
    }

    /// <summary>
    /// Формат даты времени
    /// </summary>
    /// <param name="propInfo">Атрибуты свойства</param>
    /// <returns>формат</returns>
    public static DateTimeFormat GetDateTimeFormat(this PropertyInfo propInfo)
    {
        var dateTimeFormatAttr = (DateTimeFormatAttribute)propInfo.GetCustomAttributes(typeof(DateTimeFormatAttribute), true).SingleOrDefault();
        return dateTimeFormatAttr?.DateTimeFormat ?? DateTimeFormat.TDateTime;
    }
}