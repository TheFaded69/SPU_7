using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.StandDevices.PulseMeter;

/// <summary>
/// Регистры модуля платы счета импульсов и измерения частоты
/// </summary>
public enum PulseMeter2ChannelRegisterMap
{
    // Input Registers

    /// <summary>
    /// 1-й канал Время начала измерения (ms)
    /// </summary>
    [RegisterSetup(0x0000, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    MeasureTimeBeginChannel1,

    /// <summary>
    /// 1-й канал Время окончания измерения (ms)
    /// </summary>
    [RegisterSetup(0x0002, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    MeasureTimeEndChannel1,

    /// <summary>
    /// 1-й канал Кол-во измерений
    /// </summary>
    [RegisterSetup(0x0004, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    MeasureAmountChannel1,

    /// <summary>
    /// 1-й канал Среднее значение периода (разрешение 4 us)
    /// </summary>
    [RegisterSetup(0x0006, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    AveragePeriodChannel1,

    /// <summary>
    /// 1-й канал Текущее значение периода (разрешение 4 us)
    /// </summary>
    [RegisterSetup(0x0008, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    CurrentPeriodChannel1,

    /// <summary>
    /// 1-й канал Статус
    /// </summary>
    [RegisterSetup(0x000A, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    StatusChannel1,

    /// <summary>
    /// 1-й канал Корректированное среднее значение периода (ms)
    /// </summary>
    [RegisterSetup(0x000C, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    AveragePeriodCorrectedChannel1,

    /// <summary>
    /// 1-й канал Частота (Hz)
    /// </summary>
    [RegisterSetup(0x000E, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    FrequencyChannel1,

    /// <summary>
    /// 1-й канал Частота корректированная (Hz)
    /// </summary>
    [RegisterSetup(0x0010, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    FrequencyCorrectedChannel1,

    /// <summary>
    /// 1-й канал Частота усреднённая (Hz)
    /// </summary>
    [RegisterSetup(0x0012, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    AverageFrequencyChannel1,

    /// <summary>
    /// 2-й канал Время начала измерения (ms)
    /// </summary>
    [RegisterSetup(0x0014, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    MeasureTimeBeginChannel2,

    /// <summary>
    /// 2-й канал Время окончания измерения (ms)
    /// </summary>
    [RegisterSetup(0x0016, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    MeasureTimeEndChannel2,

    /// <summary>
    /// 2-й канал Кол-во измерений
    /// </summary>
    [RegisterSetup(0x0018, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    MeasureAmountChannel2,

    /// <summary>
    /// 2-й канал Среднее значение периода (разрешение 4 us)
    /// </summary>
    [RegisterSetup(0x001A, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    AveragePeriodChannel2,

    /// <summary>
    /// 2-й канал Статус
    /// </summary>
    [RegisterSetup(0x001C, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    CurrentPeriodChannel2,

    /// <summary>
    /// 2-й канал Текущее значение периода (разрешение 4 us)
    /// </summary>
    [RegisterSetup(0x001E, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    StatusChannel2,

    /// <summary>
    /// 2-й канал Корректированное среднее значение периода (ms)
    /// </summary>
    [RegisterSetup(0x0020, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    AveragePeriodCorrectedChannel2,

    /// <summary>
    /// 2-й канал Частота (Hz)
    /// </summary>
    [RegisterSetup(0x0022, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    FrequencyChannel2,

    /// <summary>
    /// 2-й канал Частота корректированная (Hz)
    /// </summary>
    [RegisterSetup(0x0024, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    FrequencyCorrectedChannel2,

    /// <summary>
    /// 2-й канал Частота усреднённая (Hz)
    /// </summary>
    [RegisterSetup(0x0026, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    AverageFrequencyChannel2,

    /// <summary>
    /// 1-й канал свободнобегущий счётчик 16-бит импульсов
    /// </summary>
    [RegisterSetup(0x0058, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    FreeRunningCounterChannel1,

    /// <summary>
    /// 2-й канал свободнобегущий счётчик 16-бит импульсов
    /// </summary>
    [RegisterSetup(0x005A, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    FreeRunningCounterChannel2,

    /// <summary>
    /// 1-й канал запускаемый счётчик 16-бит импульсов
    /// </summary>
    [RegisterSetup(0x0060, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    LaunchCounterChannel1,

    /// <summary>
    /// 2-й канал запускаемый счётчик 16-бит импульсов
    /// </summary>
    [RegisterSetup(0x0062, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    LaunchCounterChannel2,

    /// <summary>
    /// Время измерения кол-ва импульсов, ms
    /// </summary>
    [RegisterSetup(0x0068, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    MeasureCounterTime,

    // Holding Registers

    /// <summary>
    /// 1-й канал Управление подканалами
    /// </summary>
    [RegisterSetup(0x0000, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    SubChannelsControlChannel1,

    /// <summary>
    /// 2-й канал Управление подканалами
    /// </summary>
    [RegisterSetup(0x0002, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    SubChannelsControlChannel2,

    /// <summary>
    /// 1-й канал Режим
    /// </summary>
    [RegisterSetup(0x0008, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    ModeChannel1,

    /// <summary>
    /// 2-й канал Режим
    /// </summary>
    [RegisterSetup(0x000A, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    ModeChannel2,

    /// <summary>
    /// Системное время (ms)
    /// </summary>
    [RegisterSetup(0x0010, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    SystemTime,

    /// <summary>
    /// 1-й канал СТАРТ
    /// </summary>
    [RegisterSetup(0x0012, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    PeriodMeasureStartChannel1,

    /// <summary>
    /// 2-й канал СТАРТ
    /// </summary>
    [RegisterSetup(0x0014, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    PeriodMeasureStartChannel2,

    /// <summary>
    /// Сетевой адрес прибора
    /// </summary>
    [RegisterSetup(0x0020, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    NetAddressRegister,

    /// <summary>
    /// Регистр управления
    /// </summary>
    [RegisterSetup(0x0022, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    ControlRegister,

    /// <summary>
    /// Версия метролог. значимого ПО
    /// </summary>
    [RegisterSetup(0x0024, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    MzpoVersion,

    /// <summary>
    /// Версия метролог. незначимого ПО
    /// </summary>
    [RegisterSetup(0x0026, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    MnzpoVersion,

    /// <summary>
    /// Имя прошивки (платы)
    /// </summary>
    [RegisterSetup(0x0028, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.MidLittleEndian_CDAB)]
    FirmwareName,

    /// <summary>
    /// Дата прошивки
    /// </summary>
    [RegisterSetup(0x0032, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.MidLittleEndian_CDAB)]
    FirmwareDate,

    /// <summary>
    /// Серийный номер
    /// </summary>
    [RegisterSetup(0x003C, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.MidLittleEndian_CDAB)]
    SerialNumber,

    /// <summary>
    /// Наименование прибора
    /// </summary>
    [RegisterSetup(0x0046, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.MidLittleEndian_CDAB)]
    ModuleName,

    /// <summary>
    /// Дата производства
    /// </summary>
    [RegisterSetup(0x0050, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.MidLittleEndian_CDAB)]
    ProductionDate,

    /// <summary>
    /// Производитель
    /// </summary>
    [RegisterSetup(0x005A, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.MidLittleEndian_CDAB)]
    VendorName,

    /// <summary>
    /// Минимальная длительность импульса (защита от дребезга при Debounce_flg=1)
    /// </summary>
    [RegisterSetup(0x0064, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    PulseDurationMin,

    /// <summary>
    /// Таймаут ожидания импульса, с
    /// </summary>
    [RegisterSetup(0x0066, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    PulseWaitingTimeout,

    /// <summary>
    /// Макс. количество импульсов в измерении (мах - 65535)
    /// </summary>
    [RegisterSetup(0x0068, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    PulseAmountMax,

    /// <summary>
    /// Количество диапазонов по частоте (если вкл. Fcorr_flg)
    /// </summary>
    [RegisterSetup(0x006A, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    RangeAmount,

    /// <summary>
    /// № экрана ЖКИ
    /// </summary>
    [RegisterSetup(0x006C, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    ScreenNumber,

    /// <summary>
    /// Яркость подсветки ЖКИ (0 - 65535)
    /// </summary>
    [RegisterSetup(0x006E, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    ScreenBrightness,

    /// <summary>
    /// Контрастность подсветки ЖКИ (0 - 65535)
    /// </summary>
    [RegisterSetup(0x0070, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.MidLittleEndian_CDAB)]
    ScreenContrast,

    /// <summary>
    /// Множитель поправки по длительности импульса (интервалу)
    /// </summary>
    [RegisterSetup(0x0072, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    PulseDurationCorrectionMultiplier,

    /// <summary>
    /// Множитель поправки интервала времени счёта импульсов (запуск по 0x1C)
    /// </summary>
    [RegisterSetup(0x0074, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    PulseCountingIntervalCorrectionMultiplier,

    /// <summary>
    /// Множитель поправки по частоте импульсов (если Fcorr_flg = 0)
    /// </summary>
    [RegisterSetup(0x0076, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.MidLittleEndian_CDAB)]
    PulseFrequencyCorrectionMultiplier,
}