using System.ComponentModel;

namespace SPU_7.Modbus.Types;

/// <summary>
/// порядок байтов
/// </summary>
public enum ByteOrderType
{
    /// <summary>
    /// обратный порядок (от младшего к старшему)
    /// </summary>
    [Description("Обратный DCBA (0123)")]
    // ReSharper disable once InconsistentNaming
    LittleEndian_DCBA = 0,

    /// <summary>
    /// обратный порядок (от младшего к старшему) с перевортом соседних байтов
    /// </summary>
    [Description("Обратный CDAB (1032)")]
    // ReSharper disable once InconsistentNaming
    MidLittleEndian_CDAB = 1,

    /// <summary>
    /// прямой порядок (от старшего к младшему)
    /// </summary>
    [Description("Прямой ABCD (3210)")]
    // ReSharper disable once InconsistentNaming
    BigEndian_ABCD = 2,

    /// <summary>
    /// прямой порядок (от старшего к младшему) с перевортом соседних байтов
    /// </summary>
    [Description("Прямой BADC (2301)")]
    // ReSharper disable once InconsistentNaming
    MidBigEndian_BADC = 3,
}