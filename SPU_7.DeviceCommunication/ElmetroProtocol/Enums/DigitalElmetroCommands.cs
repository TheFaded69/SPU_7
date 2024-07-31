namespace SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

public enum DigitalElmetroCommands : byte
{
    /// <summary>
    /// Чтение информации о модуле давления
    /// </summary>
    PressureSensorInfo = 0x00,

    /// <summary>
    /// Чтение текущего значения давления
    /// </summary>
    CurrentPressure = 0x01,

    /// <summary>
    /// Чтение параметров поддиапазона модуля
    /// </summary>
    SubRangeParameters = 0x0E,

    /// <summary>
    /// Установка времени усреднения давления модуля
    /// </summary>
    PressureFilterPeriod = 0x22,

    /// <summary>
    /// Установка поддиапазона модуля
    /// </summary>
    SetSubRange = 0x23,

    /// <summary>
    /// Перезапуск модуля давления
    /// </summary>
    RestartModule = 0x2A,

    /// <summary>
    /// Подстройка смещения нуля
    /// </summary>
    ZeroOffsetCalibration = 0x2B,

    /// <summary>
    /// Чтение расширенного статуса модуля
    /// </summary>
    ExtendedModuleInfo = 0x30,
}