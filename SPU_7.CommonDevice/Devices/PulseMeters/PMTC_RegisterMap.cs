using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.PulseMeters;

/// <summary>
/// Регистры модуля платы счета импульсов и измерения частоты
/// </summary>
public enum PMTC_RegisterMap
{
    /// <summary>
    /// Время начала измерения канала №1 (ms)
    /// </summary>
    [RegisterConfiguration(0x0000, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    BeginMeasureTimeChannel1,

    /// <summary>
    /// Время окончания измерения канала №1 (ms)
    /// </summary>
    [RegisterConfiguration(0x0002, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    EndMeasureTimeChannel1,

    /// <summary>
    /// Кол-во измерений канала №1
    /// </summary>
    [RegisterConfiguration(0x0004, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    MeasureCountChannel1,

    /// <summary>
    /// Среднее значение периода канала №1 (разрешение 4 us)
    /// </summary>
    [RegisterConfiguration(0x0006, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AveragePeriodChannel1,

    /// <summary>
    /// Текущее значение периода канала №1 (разрешение 4 us)
    /// </summary>
    [RegisterConfiguration(0x0008, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    CurrentPeriodChannel1,

    /// <summary>
    /// Статус канала №1
    /// </summary>
    [RegisterConfiguration(0x000A, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusChannel1,

    /// <summary>
    /// Скорректированное среднее значение периода канала №1 (ms)
    /// </summary>
    [RegisterConfiguration(0x000C, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AverageCorrectedPeriodChannel1,

    /// <summary>
    /// Частота на канале №1 (Hz)
    /// </summary>
    [RegisterConfiguration(0x000E, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyChannel1,

    /// <summary>
    /// Частота на канале №1 скорректированная (Hz)
    /// </summary>
    [RegisterConfiguration(0x0010, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyCorrectedChannel1,

    /// <summary>
    /// Частота на канале №1 усреднённая (Hz)
    /// </summary>
    [RegisterConfiguration(0x0012, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AverageFrequencyChannel1,

    /// <summary>
    /// Время начала измерения канала №2 (ms)
    /// </summary>
    [RegisterConfiguration(0x0014, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    BeginMeasureTimeChannel2,

    /// <summary>
    /// Время окончания измерения канала №2 (ms)
    /// </summary>
    [RegisterConfiguration(0x0016, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    EndMeasureTimeChannel2,

    /// <summary>
    /// Счётчик канала №2
    /// </summary>
    [RegisterConfiguration(0x0018, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    MeasureCountChannel2,

    /// <summary>
    /// Среднее значение периода канала №2 (разрешение 4 us)
    /// </summary>
    [RegisterConfiguration(0x001A, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AveragePeriodChannel2,

    /// <summary>
    /// Текущее значение периода канала №2
    /// </summary>
    [RegisterConfiguration(0x001C, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    CurrentPeriodChannel2,

    /// <summary>
    /// Статус канала №2 (разрешение 4 us)
    /// </summary>
    [RegisterConfiguration(0x001E, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusChannel2,

    /// <summary>
    /// Скорректированное среднее значение периода канала №2 (ms)
    /// </summary>
    [RegisterConfiguration(0x0020, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AverageCorrectedPeriodChannel2,

    /// <summary>
    /// Частота на канале №2 (Hz)
    /// </summary>
    [RegisterConfiguration(0x0022, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyChannel2,

    /// <summary>
    /// Частота на канале №2 скорректированная (Hz)
    /// </summary>
    [RegisterConfiguration(0x0024, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyCorrectedChannel2,

    /// <summary>
    /// Частота на канале №2 усреднённая (Hz)
    /// </summary>
    [RegisterConfiguration(0x0026, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AverageFrequencyChannel2,

    /// <summary>
    /// Свободнобегущий счётчик импульсов на канале №1 разрешением 16-бит
    /// </summary>
    [RegisterConfiguration(0x0058, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    FreeRunningCounterChannel1,

    /// <summary>
    /// Свободнобегущий счётчик импульсов на канале №2 разрешением 16-бит
    /// </summary>
    [RegisterConfiguration(0x005A, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    FreeRunningCounterChannel2,

    /// <summary>
    /// Запускаемый счётчик импульсов на канале №1 разрешением 16-бит
    /// </summary>
    [RegisterConfiguration(0x0060, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    LaunchCounterChannel1,

    /// <summary>
    /// Запускаемый счётчик импульсов на канале №2 разрешением 16-бит
    /// </summary>
    [RegisterConfiguration(0x0062, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    LaunchCounterChannel2,

    /// <summary>
    /// Время измерения кол-ва импульсов, мс
    /// </summary>
    [RegisterConfiguration(0x0068, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    CounterMeasureTime,

    // Holding Registers

    /// <summary>
    /// Управление подканалами канала №1
    /// </summary>
    [RegisterConfiguration(0x0000, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SubChannelsControlChannel1,

    /// <summary>
    /// Управление подканалами канала №2
    /// </summary>
    [RegisterConfiguration(0x0002, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SubChannelsControlChannel2,

    /// <summary>
    /// Режим канала №1
    /// </summary>
    [RegisterConfiguration(0x0008, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ModeChannel1,

    /// <summary>
    /// Режим канала №2
    /// </summary>
    [RegisterConfiguration(0x000A, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ModeChannel2,

    /// <summary>
    /// Системное время (мс)
    /// </summary>
    [RegisterConfiguration(0x0010, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SystemTime,

    /// <summary>
    /// Начать измерение периода на канале №1
    /// </summary>
    [RegisterConfiguration(0x0012, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StartPeriodMeasureChannel1,

    /// <summary>
    /// Начать измерение периода на канале №2
    /// </summary>
    [RegisterConfiguration(0x0014, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StartPeriodMeasureChannel2,

    /// <summary>
    /// Сетевой адрес прибора
    /// </summary>
    [RegisterConfiguration(0x0020, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    NetworkAddress,

    /// <summary>
    /// Регистр управления
    /// </summary>
    [RegisterConfiguration(0x0022, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ControlRegister,

    /// <summary>
    /// Версия метрологически значимого ПО
    /// </summary>
    [RegisterConfiguration(0x0024, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MetrologySoftwareVersion,

    /// <summary>
    /// Версия метрологически незначимого ПО?
    /// </summary>
    [RegisterConfiguration(0x0026, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SoftwareVersion,

    /// <summary>
    /// Имя прошивки (платы)
    /// </summary>
    [RegisterConfiguration(0x0028, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FirmwareString,

    /// <summary>
    /// Дата прошивки
    /// </summary>
    [RegisterConfiguration(0x0032, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FirmwareDate,

    /// <summary>
    /// Серийный номер
    /// </summary>
    [RegisterConfiguration(0x003C, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SerialNumber,

    /// <summary>
    /// Наименование прибора
    /// </summary>
    [RegisterConfiguration(0x0046, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ModuleName,

    /// <summary>
    /// Дата производства
    /// </summary>
    [RegisterConfiguration(0x0050, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ProductionDate,

    /// <summary>
    /// Производитель
    /// </summary>
    [RegisterConfiguration(0x005A, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VendorName,

    /// <summary>
    /// Минимальная длительность импульса (защита от дребезга при Debounce_flg=1)
    /// </summary>
    [RegisterConfiguration(0x0064, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MinPulseDuration,

    /// <summary>
    /// Таймаут ожидания одного импульса, с
    /// </summary>
    [RegisterConfiguration(0x0066, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PulseTimeout,

    /// <summary>
    /// Макс. количество импульсов в измерении (макс. значение - 65535)
    /// </summary>
    [RegisterConfiguration(0x0068, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaxImpulseCount,

    /// <summary>
    /// Количество диапазонов по частоте (если вкл. Fcorr_flg)
    /// </summary>
    [RegisterConfiguration(0x006A, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    RangeAmount,

    /// <summary>
    /// № экрана ЖКИ
    /// </summary>
    [RegisterConfiguration(0x006C, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ScreenNumber,

    /// <summary>
    /// Яркость подсветки ЖКИ (0 - 65535)
    /// </summary>
    [RegisterConfiguration(0x006E, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ScreenBrightness,

    /// <summary>
    /// Контрастность подсветки ЖКИ (0 - 65535)
    /// </summary>
    [RegisterConfiguration(0x0070, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ScreenContrast,

    /// <summary>
    /// Множитель поправки по длительности импульса (интервалу)
    /// </summary>
    [RegisterConfiguration(0x0072, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PulseDurationCorrectionMultiplier,

    /// <summary>
    /// Множитель поправки интервала времени счёта импульсов (запуск по 0x1C)
    /// </summary>
    [RegisterConfiguration(0x0074, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PulseCountingIntervalCorrectionMultiplier,

    /// <summary>
    /// Множитель поправки по частоте импульсов (если Fcorr_flg = 0)
    /// </summary>
    [RegisterConfiguration(0x0076, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PulseFrequencyCorrectionMultiplier,
}