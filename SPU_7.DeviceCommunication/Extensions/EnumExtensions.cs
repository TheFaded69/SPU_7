using System.ComponentModel;
using SPU_7.DeviceCommunication.Modbus.Attributes;

namespace SPU_7.DeviceCommunication.Extensions;
public static class EnumExtensions
{
    static readonly Dictionary<Enum, string> _descriptionCache = new();
    static readonly Dictionary<Enum, string> _measureUnitsCache = new();

    /// <summary>
    /// Получить описание из значения перечисления
    /// </summary>
    /// <param name="enumValue">Значение перечисления в котором содержится атрибут Description</param>
    /// <returns>Строка с описанием</returns>
    public static string GetDescription(this Enum enumValue)
    {
        if (_descriptionCache.TryGetValue(enumValue, out var value)) return value;
        return _descriptionCache[enumValue] = enumValue.GetType()
            .GetMember(enumValue.ToString())
            .FirstOrDefault()?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)?
            .FirstOrDefault() is DescriptionAttribute description ? description.Description : string.Empty;
    }

    /// <summary>
    /// Получить единицы измерения из перечисления
    /// </summary>
    /// <param name="enumValue">Значение перечисления в котором содержится атрибут MeasureUnits</param>
    /// <returns></returns>
    public static string GetMeasureUnits(this Enum enumValue)
    {
        return _measureUnitsCache.TryGetValue(enumValue, out var value)
            ? value
            : _measureUnitsCache[enumValue] = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()?
                .GetCustomAttributes(typeof(MeasureUnitsAttribute), false)?
                .FirstOrDefault() is MeasureUnitsAttribute mu ? mu.Units : string.Empty;
    }
}