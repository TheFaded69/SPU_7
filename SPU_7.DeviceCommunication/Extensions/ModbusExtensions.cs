using SPU_7.DeviceCommunication.Modbus;
using SPU_7.DeviceCommunication.Modbus.Attributes;

namespace SPU_7.DeviceCommunication.Extensions;
public static class ModbusExtensions
{
    /// <summary>
    /// Создание карты регистров для устройства из перечисления
    /// </summary>
    /// <typeparam name="T">Перечисление с картой регистров</typeparam>
    /// <returns>Адрес регистра и новый объект регистра</returns>
#pragma warning disable CS8604 // Possible null reference argument.
    public static Dictionary<Enum, Register> CreateRegisterMap<T>() where T : Enum =>
        Enum.GetValues(typeof(T)).Cast<T>()
        .Select(e => new
        {
            val = e,
            registerConfiguration = e.GetType().GetMember(e.ToString()).FirstOrDefault()?.GetCustomAttributes(false)?.OfType<RegisterConfigurationAttribute>()?.FirstOrDefault()?
            .RegisterConfiguration
        })
        .Where(erc => erc.registerConfiguration is not null)
        .ToDictionary(erc => erc.val as Enum, erc => new Register(erc.registerConfiguration));
#pragma warning restore CS8604 // Possible null reference argument.

    /// <summary>
    /// Добавить в подсчёт CRC-16 новое значение
    /// </summary>
    /// <param name="crc">Текущее значение CRC</param>
    /// <param name="value">Байт данных</param>
    /// <returns>Новый CRC с учётом значения данных</returns>
    public static ushort AddCrc16(ushort crc, byte value)
    {
        crc ^= value;
        for (int i = 0; i < 8; i++) crc = (ushort)((crc & 0x0001) != 0 ? (crc >> 1) ^ 0xA001 : crc >> 1);
        return crc;
    }

    /// <summary>
    /// Расчёт CRC-16
    /// </summary>
    /// <param name="data">Данные для расчёта CRC-16</param>
    /// <returns>CRC-16</returns>
    //public static ushort Crc16(this IEnumerable<byte> data) => data.Aggregate<byte, ushort>(0xFFFF, AddCrc16);
    public static ushort Crc16(this ArraySegment<byte> data) => data.Aggregate<byte, ushort>(0xFFFF, AddCrc16);

    /// <summary>
    /// Расчёт CRC-16
    /// </summary>
    /// <param name="data">Данные для расчёта CRC-16</param>
    /// <returns>CRC-16</returns>
    public static ushort Crc16(this ReadOnlySpan<byte> data) => data.Aggregate<byte, ushort>(0xFFFF, AddCrc16);

    /// <summary>
    /// Расчёт CRC-16
    /// </summary>
    /// <param name="data">Данные для расчёта CRC-16</param>
    /// <returns>CRC-16</returns>
    public static ushort Crc16(this Span<byte> data) => data.Aggregate<byte, ushort>(0xFFFF, AddCrc16);

    /// <summary>
    /// Определить является ли содержимое ответом Modbus RTU
    /// </summary>
    /// <param name="frame">Данные для распознования</param>
    /// <param name="unitId">Идентификатор устройства в сети</param>
    /// <returns>Является ли содержимое ответом Modbus</returns>
    /// <exception cref="ModbusException"></exception>
    public static bool DetectResponseFrame(this ArraySegment<byte> frame, byte unitId)
    {
        if (frame.Count < 5) return false;
        if (frame[0] != unitId || frame[1] >= 0x80) return false;
        switch(frame[1]) { // Проверка размера данных для стандартных функций
            case 0x01:
            case 0x02:
            case 0x03:
            case 0x04:
            case 0x017:
                if (frame.Count < frame[2] + 5) return false;
                break;
            case 0x05:
            case 0x06:
            case 0x0F:
            case 0x10:
                if (frame.Count < 8) return false;
                break;
        }
        var crcInput = (ushort)(frame[^1] << 8 | frame[^2]);
        var actualCrc = frame[..^2].Crc16();
        return crcInput == actualCrc;
    }
}