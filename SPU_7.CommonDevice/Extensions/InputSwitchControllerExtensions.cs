namespace SPU_7.CommonDevice.Extensions;

public static class InputSwitchControllerExtensions
{
    /// <summary>
    /// Получить состояние по индексу
    /// </summary>
    /// <param name="value">Значение состояния всех дискретных входов/выходов</param>
    /// <param name="pinIndex">Индекс для дискретного входа/выхода</param>
    public static bool GetPinState(this ushort value, int pinIndex) => ((value >> pinIndex) & 0x0001) > 0;

    /// <summary>
    /// Получить состояние по индексу
    /// </summary>
    /// <param name="value">Значение состояния всех дискретных входов/выходов</param>
    /// <param name="pinIndex">Индекс для дискретного входа/выхода</param>
    public static bool GetPinState(this uint value, int pinIndex) => ((value >> pinIndex) & 0x0001) > 0;

    /// <summary>
    /// Получить состояние по индексу
    /// </summary>
    /// <param name="value">Значение состояния всех дискретных входов/выходов</param>
    /// <param name="pinIndex">Индекс для дискретного входа/выхода</param>
    public static bool GetPinState(this ulong value, int pinIndex) => ((value >> pinIndex) & 0x0001) > 0;
}