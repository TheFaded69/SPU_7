using System.ComponentModel;
using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.UFG;

/// <summary>
/// Карта регистров UFG
/// </summary>
public enum UFG_RegisterMap
{
    /// <summary>
    /// Рабочий расход
    /// </summary>
    [Description("Рабочий расход"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x0002, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    VolumeFlowRegister,

    /// <summary>
    /// Температура, °С
    /// </summary>
    [Description("Температура"), MeasureUnits("°С")]
    [RegisterConfiguration(0x0004, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    Temperature,

    /// <summary>
    /// Давление абсолютное, МПа
    /// </summary>
    [Description("Абсолютное давление"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x0006, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AbsolutePressure,

    /// <summary>
    /// Давление избыточное, МПа
    /// </summary>
    [Description("Избыточное давление"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x000C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ExcessPressure,

    #region Специальные регистры (технологические)

    /// <summary>
    /// Маска нештатных ситуаций
    /// </summary>
    [Description("Маска нештатных ситуаций")]
    [RegisterConfiguration(0x0014, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AlarmBitsMask,

    /// <summary>
    /// Внутренний (заводской) номер
    /// </summary>
    [Description("Заводской номер")]
    [RegisterConfiguration(0x105F, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SerialNumber,

    /// <summary>
    /// Дата/Время прибора
    /// </summary>
    [Description("Дата/Время")]
    [RegisterConfiguration(0x100A, 4, RegisterDataType.TDateTime, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SystemTime,

    /// <summary>
    /// Регистр управления
    /// </summary>
    [Description("Регистр управления")]
    [RegisterConfiguration(0x1014, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ControlRegister,

    /// <summary>
    /// Режим "моста" с ПП
    /// </summary>
    [Description("Режим моста")]
    [RegisterConfiguration(0x105E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BridgeMode,

    /// <summary>
    /// Сетевой адрес ПП
    /// </summary>
    [Description("Сетевой адрес ПП")]
    [RegisterConfiguration(0x1013, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PrimaryConverterAddress,

    /// <summary>
    /// Мин. частота выхода 1, Гц
    /// </summary>
    [Description("Минимальная частота выхода №1"), MeasureUnits("Гц")]
    [RegisterConfiguration(0x1019, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyOut1Min,

    /// <summary>
    /// Макс. частота выхода 1, Гц
    /// </summary>
    [Description("Максимальная частота выхода №1"), MeasureUnits("Гц")]
    [RegisterConfiguration(0x101B, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyOut1Max,

    /// <summary>
    /// Макс. рабочий расход 1, м3/ч
    /// </summary>
    [Description("Максимальный рабочий расход №1"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x101D, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VolumeFlowOut1Max,

    /// <summary>
    /// Мин. частота выхода 2, Гц
    /// </summary>
    [Description("Минимальная частота выхода №2"), MeasureUnits("Гц")]
    [RegisterConfiguration(0x1029, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyOut2Min,

    /// <summary>
    /// Макс. частота выхода 2, Гц
    /// </summary>
    [Description("Максимальная частота выхода №2"), MeasureUnits("Гц")]
    [RegisterConfiguration(0x102B, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FrequencyOut2Max,

    /// <summary>
    /// Макс. рабочий расход 2, м3/ч
    /// </summary>
    [Description("Максимальный рабочий расход №2"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x102D, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VolumeFlowOut2Max,

    /// <summary>
    /// Значение расхода для тока 4 мА
    /// </summary>
    [Description("Значение расхода для тока 4 мА"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x1052, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    CurrentOutMin,

    /// <summary>
    /// Значение расхода для тока 20 мА
    /// </summary>
    [Description("Значение расхода для тока 20 мА"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x1054, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    CurrentOutMax,

    /// <summary>
    /// Технологический регистр управления
    /// </summary>
    [Description("Технологический регистр управления")]
    [RegisterConfiguration(0x1075, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TechnicalControlRegister,

    #endregion

    #region Настройки диапазонов и прочее

    /// <summary>
    /// Мин. диапазон рабочего расхода, м3/ч
    /// </summary>
    [Description("Минимальный диапазон рабочего расхода"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x1800, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VolumeFlowMinRange,

    /// <summary>
    /// Макс. диапазон рабочего расхода, м3/ч
    /// </summary>
    [Description("Максимальный диапазон рабочего расхода"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x1802, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VolumeFlowMaxRange,

    /// <summary>
    /// Мин. рег. температура °C
    /// </summary>
    [Description("Минимальная регистрируемая температура"), MeasureUnits("°C")]
    [RegisterConfiguration(0x180A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MinTemperature,

    /// <summary>
    /// Макс. рег. температура °C
    /// </summary>
    [Description("Максимальная регистрируемая температура"), MeasureUnits("°C")]
    [RegisterConfiguration(0x180C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaxTemperature,

    /// <summary>
    /// Договорная температура °C
    /// </summary>
    [Description("Договорная температура"), MeasureUnits("°C")]
    [RegisterConfiguration(0x180E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AgreedTemperature,

    /// <summary>
    /// Минимальное давление, МПа
    /// </summary>
    [Description("Минимальное давление"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x1810, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MinPressure,

    /// <summary>
    /// Максимальное давление, МПа
    /// </summary>
    [Description("Максимальное давление"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x1812, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaxPressure,

    /// <summary>
    /// Договорное давление, МПа
    /// </summary>
    [Description("Договорное давление"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x1814, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AgreedPressure,

    /// <summary>
    /// Вес импульса для импульсного выхода
    /// </summary>
    [Description("Вес импульса для импульсного выхода")]
    [RegisterConfiguration(0x1818, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PulseWeight,

    #endregion

    #region Регистры специального назначения

    /// <summary>
    /// Флаг подтверждения приема данных
    /// </summary>
    [RegisterConfiguration(0x1200, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    GprsDataAcceptanceConfirmation,
    
    /// <summary>
    /// Флаг подтверждения приема данных
    /// </summary>
    [RegisterConfiguration(0x1201, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    GprsSessionTermination,

    #endregion

    #region Управление паролями

    /// <summary>
    /// Текущий пароль для доступа
    /// </summary>
    [Description("Текущий пароль доступа")]
    [RegisterConfiguration(0xF000, 2, RegisterDataType.UInt32, ModbusFunction.None, ModbusFunction.WriteMultipleRegisters)]
    PasswordAccess,

    [RegisterConfiguration(0xF002, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PasswordAccessLevel,

    /// <summary>
    /// Номер для запроса одноразового пароля
    /// </summary>
    [Description("Номер для запроса одноразового пароля")]
    [RegisterConfiguration(0xF01C, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AuthorizationKey,

    /// <summary>
    /// Уровень доступа для текущего пароля
    /// </summary>
    [Description("Уровень доступа для текущего пароля")]
    [RegisterConfiguration(0xF01E, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    CurrentAccessLevel,

    #endregion

    #region Отладка

    /// <summary>
    /// Пароль для разрешения регистров теста
    /// </summary>
    [RegisterConfiguration(0x5000, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DebugAccessPassword,

    /// <summary>
    /// Тест клавиатуры
    /// </summary>
    [RegisterConfiguration(0x5002, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    KeyboardTest,

    /// <summary>
    /// Код последней нажатой кнопки(сбрасывается в ноль при вычитке)
    /// </summary>
    [RegisterConfiguration(0x5004, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    LastPressedKey,

    /// <summary>
    /// Управление блютуз
    /// </summary>
    [RegisterConfiguration(0x5006, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BluetoothControl,

    /// <summary>
    /// Статус блютуз
    /// </summary>
    [RegisterConfiguration(0x5008, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BluetoothStatus,

    /// <summary>
    /// Управление модемом
    /// </summary>
    [RegisterConfiguration(0x500A, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ModemControl,

    /// <summary>
    /// Текущий статус модема
    /// </summary>
    [RegisterConfiguration(0x500C, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ModemStatus,

    /// <summary>
    /// Управление LCD
    /// </summary>
    [RegisterConfiguration(0x500E, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    LcdControl,

    /// <summary>
    /// Нажатые кнопки на момент чтения (только при включенной отладке)
    /// </summary>
    [RegisterConfiguration(0x5012, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    KeysStatus,

    /// <summary>
    /// Маска управления режимом отладки
    /// </summary>
    [RegisterConfiguration(0x5100, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DebugMode,

    /// <summary>
    /// Отладочное значение рабочего расхода, м3/час
    /// </summary>
    [Description("Отладочное значение рабочего расхода"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x5102, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DebugFlowValue,

    /// <summary>
    /// Отладочное значение текущей температуры, 0С
    /// </summary>
    [Description("Отладочное значение текущей температуры"), MeasureUnits("°C")]
    [RegisterConfiguration(0x5104, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DebugTemperatureValue,

    /// <summary>
    /// Отладочное значение текущего абсолютного давления, МПа
    /// </summary>
    [Description("Отладочное значение текущего абсолютного давления"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x5106, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DebugAbsolutePressureValue,

    #endregion

    #region Диагностические параметры из ПП

    [Description("Расширенные статусы каналов")]
    [RegisterConfiguration(0x002C, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ExtendedChannelStatuses,

    [Description("Проценты ошибок подлучей 0 и 1"), MeasureUnits("%")]
    [RegisterConfiguration(0x002E, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamErrorPercentages01,

    [Description("Проценты ошибок подлучей 2 и 3"), MeasureUnits("%")]
    [RegisterConfiguration(0x0030, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamErrorPercentages23,

    [Description("Проценты ошибок подлучей 4 и 5"), MeasureUnits("%")]
    [RegisterConfiguration(0x0032, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamErrorPercentages45,

    [Description("Проценты ошибок подлучей 6 и 7"), MeasureUnits("%")]
    [RegisterConfiguration(0x0034, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamErrorPercentages67,

    [Description("Скорость потока фильтрованная луча 1"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0036, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam1,

    [Description("Скорость потока фильтрованная луча 2"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0038, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam2,

    [Description("Скорость потока фильтрованная луча 3"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x003A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam3,

    [Description("Скорость потока фильтрованная луча 4"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x003C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam4,

    [Description("Скорость потока фильтрованная луча 5"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x003E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam5,

    [Description("Скорость потока фильтрованная луча 6"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0040, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam6,

    [Description("Скорость потока фильтрованная луча 7"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0042, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam7,

    [Description("Скорость потока фильтрованная луча 8"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0044, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FlowSpeedBeam8,

    [Description("Блок с фильтрованной скоростью звука лучей"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0046, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FilteredSoundSpeedBeamsBlock,

    [Description("Скорость звука фильтрованная луча 1"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0046, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam1,

    [Description("Скорость звука фильтрованная луча 2"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0048, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam2,

    [Description("Скорость звука фильтрованная луча 3"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x004A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam3,

    [Description("Скорость звука фильтрованная луча 4"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x004C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam4,

    [Description("Скорость звука фильтрованная луча 5"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x004E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam5,

    [Description("Скорость звука фильтрованная луча 6"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0050, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam6,

    [Description("Скорость звука фильтрованная луча 7"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0052, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam7,

    [Description("Скорость звука фильтрованная луча 8"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0054, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeam8,

    [Description("Кол-во лучей")]
    [RegisterConfiguration(0x0056, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamsCount,

    [Description("Смещение лучей")]
    [RegisterConfiguration(0x0057, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamOffset,

    [Description("Блок с индексами АРУ лучей")]
    [RegisterConfiguration(0x0058, 8, RegisterDataType.UInt16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndexBlock,

    [Description("Индекс АРУ луча 1")]
    [RegisterConfiguration(0x0058, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    // Индекс Автоматической регулировки усиления луча
    BeamGainIndex1,

    [Description("Индекс АРУ луча 2")]
    [RegisterConfiguration(0x0059, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndex2,

    [Description("Индекс АРУ луча 3")]
    [RegisterConfiguration(0x005A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndex3,

    [Description("Индекс АРУ луча 4")]
    [RegisterConfiguration(0x005B, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndex4,

    [Description("Индекс АРУ луча 5")]
    [RegisterConfiguration(0x005C, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndex5,

    [Description("Индекс АРУ луча 6")]
    [RegisterConfiguration(0x005D, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndex6,

    [Description("Индекс АРУ луча 7")]
    [RegisterConfiguration(0x005E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndex7,

    [Description("Индекс АРУ луча 8")]
    [RegisterConfiguration(0x005F, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamGainIndex8,

    [Description("Статусы лучей по скорости звука")]
    [RegisterConfiguration(0x0060, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedBeamStatuses,

    [Description("Значение резистора Rg")]
    [RegisterConfiguration(0x0061, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    RgResistance,

    [Description("Критерий тревоги по скорости звука"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0062, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedAlarmCriteria,

    [Description("Критерий предупреждения по скорости звука"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x0064, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SoundSpeedWarningCriteria,

    [Description("Блок с соотношением Сигнал/Шум лучей")]
    [RegisterConfiguration(0x0066, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeamsBlock,

    [Description("Сигнал/Шум луча 1")]
    [RegisterConfiguration(0x0066, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam1,

    [Description("Сигнал/Шум луча 2")]
    [RegisterConfiguration(0x0068, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam2,

    [Description("Сигнал/Шум луча 3")]
    [RegisterConfiguration(0x006A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam3,

    [Description("Сигнал/Шум луча 4")]
    [RegisterConfiguration(0x006C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam4,

    [Description("Сигнал/Шум луча 5")]
    [RegisterConfiguration(0x006E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam5,

    [Description("Сигнал/Шум луча 6")]
    [RegisterConfiguration(0x0070, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam6,

    [Description("Сигнал/Шум луча 7")]
    [RegisterConfiguration(0x0072, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam7,

    [Description("Сигнал/Шум луча 8")]
    [RegisterConfiguration(0x0074, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioBeam8,

    #endregion

    #region Bluetooth

    /// <summary>
    /// Имя блютуз
    /// </summary>
    [Description("Имя блютуз")]
    [RegisterConfiguration(0x1039, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BluetoothName,

    /// <summary>
    /// Пин код блютуз
    /// </summary>
    [Description("Пин код блютуз")]
    [RegisterConfiguration(0x1065, 4, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BluetoothPinCode,

    /// <summary>
    /// Тип установленного модуля bluetooth
    /// </summary>
    [Description("Тип установленного модуля блютуз")]
    [RegisterConfiguration(0x1069, 8, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BluetoothModuleName,

    /// <summary>
    /// Время работы блютуз модуля от батареи, сек.
    /// </summary>
    [Description("Время работы блютуз модуля от батареи"), MeasureUnits("сек")]
    [RegisterConfiguration(0x00AD, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BluetoothBatteryWorkTime,

    /// <summary>
    /// Адрес блютуз модуля
    /// </summary>
    [Description("Адрес блютуз модуля")]
    [RegisterConfiguration(0x00AF, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BluetoothAddress,

    #endregion

    #region Модем

    /// <summary>
    /// Состояние SIM карты
    /// </summary>
    [Description("Состояние SIM карты")]
    [RegisterConfiguration(0x008C, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SimStatus,
    
    /// <summary>
    /// Версия модема
    /// </summary>
    [Description("Версия модема")]
    [RegisterConfiguration(0x008B, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ModemVersion,
    

    #endregion

    #region Архивы данных

    /// <summary>
    /// Флаг архива
    /// </summary>
    [Description("Флаг архива")]
    [RegisterConfiguration(0x203A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ArchiveFlag,

    #endregion

    #region Плотномер

    /// <summary>
    /// Режим работы
    /// </summary>
    [Description("Режим работы")]
    [RegisterConfiguration(0x1120, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    WorkMode,

    /// <summary>
    /// Логический адрес плотномера
    /// </summary>
    [Description("Логический адрес плотномера")]
    [RegisterConfiguration(0x1121, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AddressUDM,

    /// <summary>
    /// Период опроса плотномера
    /// </summary>
    [Description("Период опроса плотномера"), MeasureUnits("сек")]
    [RegisterConfiguration(0x1122, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DensityMeterPollingPeriod,

    /// <summary>
    /// Расчет стандартного/массового расхода
    /// </summary>
    [Description("Расчет стандартного/массового расхода")]
    [RegisterConfiguration(0x1123, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FlowCalculation,

    /// <summary>
    /// Максимальное отклонение плотности
    /// </summary>
    [Description("Максимальное отклонение плотности"), MeasureUnits("%")]
    [RegisterConfiguration(0x1124, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaximumDensityDeviation,

    #endregion

    #region Выходы на связь

    /// <summary>
    /// Точка доступа для GPRS
    /// </summary>
    [Description("Точка доступа для GPRS")]
    [RegisterConfiguration(0x4000, 20, RegisterDataType.CharArray)]
    AccessPointGPRS,

    /// <summary>
    /// Логин для точки доступа GPRS
    /// </summary>
    [Description("Логин для точки доступа GPRS")]
    [RegisterConfiguration(0x4014, 10, RegisterDataType.CharArray)]
    AccessPointLogin,

    /// <summary>
    /// Пароль для точки доступа GPRS
    /// </summary>
    [Description("Пароль для точки доступа GPRS")]
    [RegisterConfiguration(0x401E, 10, RegisterDataType.CharArray)]
    AccessPointPassword,

    /// <summary>
    /// IP адрес сервера основной
    /// </summary>
    [Description("IP адрес сервера основной")]
    [RegisterConfiguration(0x4028, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ServerIpAddress,

    /// <summary>
    /// IP порт основной
    /// </summary>
    [Description("IP порт основной")]
    [RegisterConfiguration(0x402A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ServerPort,

    /// <summary>
    /// IP адрес резервного сервера
    /// </summary>
    [Description("IP адрес резервного сервера")]
    [RegisterConfiguration(0x4028, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ServerIpAddressBackup,

    /// <summary>
    /// IP порт резервный
    /// </summary>
    [Description("IP порт резервный")]
    [RegisterConfiguration(0x402A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ServerPortBackup,

    /// <summary>
    /// Телефон для CSD основной
    /// </summary>
    [Description("Телефон для CSD основной")]
    [RegisterConfiguration(0x402E, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    CsdPhone,

    /// <summary>
    /// Телефон для CSD резервный
    /// </summary>
    [Description("Телефон для CSD резервный")]
    [RegisterConfiguration(0x4038, 10, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    CsdPhoneBackup,

    /// <summary>
    /// Таймаут соединения, мин (от 1 до 10)
    /// </summary>
    [Description("Таймаут соединения"), MeasureUnits("мин")]
    [RegisterConfiguration(0x4043, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ConnectionTimeout,

    /// <summary>
    /// Кол-во повторов выхода на связь в случае ошибки (от 1 до 10)
    /// </summary>
    [Description("Кол-во повторов выхода на связь в случае ошибки")]
    [RegisterConfiguration(0x4044, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteSingleRegister)]
    ConnectionRetries,

    /// <summary>
    /// Повтор выхода на связь 1
    /// </summary>
    //[RegisterConfiguration(0x4070, 2, RegisterDataType.TConnection, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    //Connection1Setup,

    /// <summary>
    /// Способ выхода на связь 1
    /// </summary>
    [Description("Способ выхода на связь 1")]
    [RegisterConfiguration(0x4072, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    Connection1Mode,

    /// <summary>
    /// Данные, которые передаются при выходе на связь 1 по событиям (битовая маска)
    /// </summary>
    [Description("Данные при выходе на связь 1 по событиям")]
    [RegisterConfiguration(0x4073, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    Connection1Data,

    #endregion
}