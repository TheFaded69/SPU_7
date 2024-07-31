using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.PS_UNI;

public enum PS_UNI_RegisterMap
{
    #region Основные параметры

    /// <summary>
    /// Давление выходное, Па
    /// </summary>
    [RegisterConfiguration(0x0000, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    OutputPressure,

    /// <summary>
    /// Температура корректирующая, ° С
    /// </summary>
    [RegisterConfiguration(0x0002, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    CorrectionTemperature,

    /// <summary>
    /// Давление линеаризированное, Па
    /// </summary>
    [RegisterConfiguration(0x0004, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    LinearizationPressure,

    /// <summary>
    /// Термокоррекция давления: текущий множитель
    /// </summary>
    [RegisterConfiguration(0x0006, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    ThermalCorrectionCoefficient,

    /// <summary>
    /// Термокоррекция давления: текущее смещение нуля, Па
    /// </summary>
    [RegisterConfiguration(0x0008, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    ThermalCorrectionOffset,

    /// <summary>
    /// Текущий код ЦАП до термокоррекции
    /// </summary>
    [RegisterConfiguration(0x000A, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    DacBeforeThermalCorrection,

    /// <summary>
    /// Текущая величина аналогового выхода, В/мА
    /// </summary>
    [RegisterConfiguration(0x000C, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputValue,

    /// <summary>
    /// Код АЦП канала давления фильтрованный
    /// </summary>
    [RegisterConfiguration(0x000E, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    PressureChannelAdc,

    /// <summary>
    /// Код АЦП канала температуры корректирующий
    /// </summary>
    [RegisterConfiguration(0x0010, 2, RegisterDataType.Int32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    TemperatureChannelAdc,

    /// <summary>
    /// Код НС
    /// </summary>
    [RegisterConfiguration(0x0012, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    EmergencyRegister,

    /// <summary>
    /// Заводской номер прибора
    /// </summary>
    [RegisterConfiguration(0x0014, 4, RegisterDataType.UInt64, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    VendorNumber,

    /// <summary>
    /// Давление термокомпенсированное, Па
    /// </summary>
    [RegisterConfiguration(0x0018, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    TermoCompensatedPressure,

    /// <summary>
    /// Код АЦП канала давления до коррекции по напряжению
    /// </summary>
    [RegisterConfiguration(0x0020, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AdcBeforeThermalCorrection,

    /// <summary>
    /// Коррекция по напряжению АЦП канала давления: текущий множитель
    /// </summary>
    [RegisterConfiguration(0x0022, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    PressureChannelAdcVoltageCorrectionCoefficient,

    /// <summary>
    /// Коррекция по напряжению АЦП канала давления: текущее смещение нуля
    /// </summary>
    [RegisterConfiguration(0x0024, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    PressureChannelAdcVoltageCorrectionOffset,

    /// <summary>
    /// Термокоррекция ЦАП: текущий множитель
    /// </summary>
    [RegisterConfiguration(0x0026, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    DacThermalCorrectionCoefficient,

    /// <summary>
    /// Термокоррекция ЦАП: текущее смещение
    /// </summary>
    [RegisterConfiguration(0x0028, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    DacThermalCorrectionOffset,

    /// <summary>
    /// Выходной код ЦАП после термокоррекции
    /// </summary>
    [RegisterConfiguration(0x002A, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    DacAfterThermalCorrection,

    /// <summary>
    /// Коррекция по напряжению АЦП канала температуры/ЦАП: текущий множитель
    /// </summary>
    [RegisterConfiguration(0x002C, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    TemperatureChannelAdcVoltageCorrectionCoefficient,

    /// <summary>
    /// Коррекция по напряжению АЦП канала температуры/ЦАП: текущее смещение нуля
    /// </summary>
    [RegisterConfiguration(0x002E, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    TemperatureChannelAdcVoltageCorrectionOffset,

    /// <summary>
    /// Температура контроллера, °С
    /// </summary>
    [RegisterConfiguration(0x0030, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    ControllerTemperature,

    /// <summary>
    /// Температура АЦП, °С
    /// </summary>
    [RegisterConfiguration(0x0032, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    AdcTemperature,

    /// <summary>
    /// Температура внешнего датчичка, °С
    /// </summary>
    [RegisterConfiguration(0x0034, 2, RegisterDataType.Float, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    ExternalSensorTemperature,

    /// <summary>
    /// Внешнее напряжение питания датчика, мВ
    /// </summary>
    [RegisterConfiguration(0x0036, 2, RegisterDataType.UInt32, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteMultipleRegisters)]
    ExternalSensorPowerVoltage,

    #endregion

    #region Настройки

    /// <summary>
    /// Период проведения измерений, сек
    /// </summary>
    [RegisterConfiguration(0x0006, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MeasurePeriod,

    /// <summary>
    /// Коэффициент усиления АЦП канала давления
    /// </summary>
    [RegisterConfiguration(0x000A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PressureAdcGainRatio,

    /// <summary>
    /// Коэффициент весового фильтра канала давления (время демфирования)
    /// </summary>
    [RegisterConfiguration(0x00C3, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PressureChannelWeightRatio,

    #endregion

    #region Линеаризация по давлению

    /// <summary>
    /// Кол-во точек линеаризации
    /// </summary>
    [RegisterConfiguration(0x0082, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_PointAmount,

    /// <summary>
    /// Код АЦП канала давления в точке 1
    /// </summary>
    [RegisterConfiguration(0x0042, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc1,

    /// <summary>
    /// Код АЦП канала давления в точке 2
    /// </summary>
    [RegisterConfiguration(0x0044, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc2,

    /// <summary>
    /// Код АЦП канала давления в точке 3
    /// </summary>
    [RegisterConfiguration(0x0046, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc3,

    /// <summary>
    /// Код АЦП канала давления в точке 4
    /// </summary>
    [RegisterConfiguration(0x0048, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc4,

    /// <summary>
    /// Код АЦП канала давления в точке 5
    /// </summary>
    [RegisterConfiguration(0x004A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc5,

    /// <summary>
    /// Код АЦП канала давления в точке 6
    /// </summary>
    [RegisterConfiguration(0x004C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc6,

    /// <summary>
    /// Код АЦП канала давления в точке 7
    /// </summary>
    [RegisterConfiguration(0x004E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc7,

    /// <summary>
    /// Код АЦП канала давления в точке 8
    /// </summary>
    [RegisterConfiguration(0x0070, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc8,

    /// <summary>
    /// Код АЦП канала давления в точке 9
    /// </summary>
    [RegisterConfiguration(0x0072, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc9,

    /// <summary>
    /// Код АЦП канала давления в точке 10
    /// </summary>
    [RegisterConfiguration(0x0074, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc10,

    /// <summary>
    /// Код АЦП канала давления в точке 11
    /// </summary>
    [RegisterConfiguration(0x0076, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc11,

    /// <summary>
    /// Код АЦП канала давления в точке 12
    /// </summary>
    [RegisterConfiguration(0x0078, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc12,

    /// <summary>
    /// Код АЦП канала давления в точке 13
    /// </summary>
    [RegisterConfiguration(0x007A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc13,

    /// <summary>
    /// Код АЦП канала давления в точке 14
    /// </summary>
    [RegisterConfiguration(0x007C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc14,

    /// <summary>
    /// Код АЦП канала давления в точке 15
    /// </summary>
    [RegisterConfiguration(0x007E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc15,

    /// <summary>
    /// Код АЦП канала давления в точке 16
    /// </summary>
    [RegisterConfiguration(0x0080, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Adc16,

    /// <summary>
    /// Давление эталона в точке 1
    /// </summary>
    [RegisterConfiguration(0x0050, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference1,

    /// <summary>
    /// Давление эталона в точке 2
    /// </summary>
    [RegisterConfiguration(0x0052, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference2,

    /// <summary>
    /// Давление эталона в точке 3
    /// </summary>
    [RegisterConfiguration(0x0054, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference3,

    /// <summary>
    /// Давление эталона в точке 4
    /// </summary>
    [RegisterConfiguration(0x0056, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference4,

    /// <summary>
    /// Давление эталона в точке 5
    /// </summary>
    [RegisterConfiguration(0x0058, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference5,

    /// <summary>
    /// Давление эталона в точке 6
    /// </summary>
    [RegisterConfiguration(0x005A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference6,

    /// <summary>
    /// Давление эталона в точке 7
    /// </summary>
    [RegisterConfiguration(0x005C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference7,

    /// <summary>
    /// Давление эталона в точке 8
    /// </summary>
    [RegisterConfiguration(0x005E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference8,

    /// <summary>
    /// Давление эталона в точке 9
    /// </summary>
    [RegisterConfiguration(0x0060, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference9,

    /// <summary>
    /// Давление эталона в точке 10
    /// </summary>
    [RegisterConfiguration(0x0062, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference10,

    /// <summary>
    /// Давление эталона в точке 11
    /// </summary>
    [RegisterConfiguration(0x0064, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference11,

    /// <summary>
    /// Давление эталона в точке 12
    /// </summary>
    [RegisterConfiguration(0x0066, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference12,

    /// <summary>
    /// Давление эталона в точке 13
    /// </summary>
    [RegisterConfiguration(0x0068, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference13,

    /// <summary>
    /// Давление эталона в точке 14
    /// </summary>
    [RegisterConfiguration(0x006A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference14,

    /// <summary>
    /// Давление эталона в точке 15
    /// </summary>
    [RegisterConfiguration(0x006C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference15,

    /// <summary>
    /// Давление эталона в точке 16
    /// </summary>
    [RegisterConfiguration(0x006E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PL_Reference16,

    #endregion

    #region Термокоррекция по давлению (PTC - Pressure Thermal Correction)

    /// <summary>
    /// Кол-во точек термокоррекции
    /// </summary>
    [RegisterConfiguration(0x0084, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_PointAmount,

    /// <summary>
    /// Код температуры в точке 1
    /// </summary>
    [RegisterConfiguration(0x0012, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_1,

    /// <summary>
    /// Код температуры в точке 2
    /// </summary>
    [RegisterConfiguration(0x0014, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_2,

    /// <summary>
    /// Код температуры в точке 3
    /// </summary>
    [RegisterConfiguration(0x0016, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_3,

    /// <summary>
    /// Код температуры в точке 4
    /// </summary>
    [RegisterConfiguration(0x0018, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_4,

    /// <summary>
    /// Код температуры в точке 5
    /// </summary>
    [RegisterConfiguration(0x0086, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_5,

    /// <summary>
    /// Код температуры в точке 6
    /// </summary>
    [RegisterConfiguration(0x0088, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_6,

    /// <summary>
    /// Код температуры в точке 7
    /// </summary>
    [RegisterConfiguration(0x008A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_7,

    /// <summary>
    /// Код температуры в точке 8
    /// </summary>
    [RegisterConfiguration(0x008C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Temperature_8,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 1
    /// </summary>
    [RegisterConfiguration(0x001A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_1,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 2
    /// </summary>
    [RegisterConfiguration(0x001C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_2,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 3
    /// </summary>
    [RegisterConfiguration(0x001E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_3,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 4
    /// </summary>
    [RegisterConfiguration(0x0020, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_4,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 5
    /// </summary>
    [RegisterConfiguration(0x008E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_5,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 6
    /// </summary>
    [RegisterConfiguration(0x0090, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_6,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 7
    /// </summary>
    [RegisterConfiguration(0x0092, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_7,

    /// <summary>
    /// Коэффициент термокоррекции давления в точке 8
    /// </summary>
    [RegisterConfiguration(0x0094, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Slope_8,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 1
    /// </summary>
    [RegisterConfiguration(0x0022, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_1,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 2
    /// </summary>
    [RegisterConfiguration(0x0024, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_2,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 3
    /// </summary>
    [RegisterConfiguration(0x0026, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_3,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 4
    /// </summary>
    [RegisterConfiguration(0x0028, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_4,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 5
    /// </summary>
    [RegisterConfiguration(0x0096, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_5,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 6
    /// </summary>
    [RegisterConfiguration(0x0098, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_6,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 7
    /// </summary>
    [RegisterConfiguration(0x009A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_7,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 8
    /// </summary>
    [RegisterConfiguration(0x009C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PTC_Offset_8,

    #endregion

    #region Термокоррекция аналогового выхода (ATC - Analog Thermal Correction)

    /// <summary>
    /// Кол-во точек термокоррекции
    /// </summary>
    [RegisterConfiguration(0x009E, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_PointAmount,

    /// <summary>
    /// Код в точке 1
    /// </summary>
    [RegisterConfiguration(0x00A0, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_1,

    /// <summary>
    /// Код в точке 2
    /// </summary>
    [RegisterConfiguration(0x00A2, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_2,

    /// <summary>
    /// Код в точке 3
    /// </summary>
    [RegisterConfiguration(0x00A4, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_3,

    /// <summary>
    /// Код в точке 4
    /// </summary>
    [RegisterConfiguration(0x00A6, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_4,

    /// <summary>
    /// Код в точке 5
    /// </summary>
    [RegisterConfiguration(0x00A8, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_5,

    /// <summary>
    /// Код в точке 6
    /// </summary>
    [RegisterConfiguration(0x00AA, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_6,

    /// <summary>
    /// Код в точке 7
    /// </summary>
    [RegisterConfiguration(0x00AC, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_7,

    /// <summary>
    /// Код в точке 8
    /// </summary>
    [RegisterConfiguration(0x00AE, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Temperature_8,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 1
    /// </summary>
    [RegisterConfiguration(0x002A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_1,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 2
    /// </summary>
    [RegisterConfiguration(0x002C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_2,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 3
    /// </summary>
    [RegisterConfiguration(0x002E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_3,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 4
    /// </summary>
    [RegisterConfiguration(0x0030, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_4,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 5
    /// </summary>
    [RegisterConfiguration(0x00B0, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_5,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 6
    /// </summary>
    [RegisterConfiguration(0x00B2, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_6,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 7
    /// </summary>
    [RegisterConfiguration(0x00B4, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_7,

    /// <summary>
    /// Коэффициент термокоррекции ЦАП в точке 8
    /// </summary>
    [RegisterConfiguration(0x00B6, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Slope_8,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 1
    /// </summary>
    [RegisterConfiguration(0x0032, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_1,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 2
    /// </summary>
    [RegisterConfiguration(0x0034, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_2,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 3
    /// </summary>
    [RegisterConfiguration(0x0036, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_3,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 4
    /// </summary>
    [RegisterConfiguration(0x0038, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_4,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 5
    /// </summary>
    [RegisterConfiguration(0x00B8, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_5,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 6
    /// </summary>
    [RegisterConfiguration(0x00BA, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_6,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 7
    /// </summary>
    [RegisterConfiguration(0x00BC, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_7,

    /// <summary>
    /// Давление термокорректирующего смещения в точке 8
    /// </summary>
    [RegisterConfiguration(0x00BE, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ATC_Offset_8,

    #endregion

    #region Настройки аналогового выхода

    /// <summary>
    /// Нижний диапазон аналогового выхода, Па
    /// </summary>
    [RegisterConfiguration(0x0100, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputLowBound,

    /// <summary>
    /// Верхний диапазон аналогового выхода, Па
    /// </summary>
    [RegisterConfiguration(0x0102, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputHighBound,

    /// <summary>
    /// Смещение нуля переменной аналогового выхода, мА/В
    /// </summary>
    [RegisterConfiguration(0x0104, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputOffset,

    /// <summary>
    /// Множитель переменной аналогового выхода
    /// </summary>
    [RegisterConfiguration(0x0106, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputCoefficient,

    /// <summary>
    /// Тип функции преобразования:
    /// 0 - прямая линейная
    /// 1 - обратная линейная
    /// 2 - прямая корнеизвлекающая
    /// 3 - обратная корнеизвлекающая
    /// </summary>
    [RegisterConfiguration(0x0108, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputConvertFunction,

    /// <summary>
    /// Уровень на выходе при ошибке:
    /// 0 - низкий (3,7мА/0,37В)
    /// 1 - высокий (22,1мА/2,21В)
    /// </summary>
    [RegisterConfiguration(0x0109, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputErrorLevel,

    /// <summary>
    /// Статус аналогового выхода
    /// </summary>
    [RegisterConfiguration(0x010F, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputStatus,

    #endregion

    #region Паспортные данные

    /// <summary>
    /// Наименование
    /// </summary>
    [RegisterConfiguration(0x0300, 16, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    Name,

    /// <summary>
    /// Тип:
    /// 0 - абсолютный
    /// 1 - избыточный
    /// 2 - дифференциальный
    /// </summary>
    [RegisterConfiguration(0x0310, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    Type,

    /// <summary>
    /// Тип погрешности:
    /// 0 - относительная
    /// 1 - приведенная
    /// </summary>
    [RegisterConfiguration(0x0311, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    InaccuracyType,

    /// <summary>
    /// Погрешность, %:
    /// </summary>
    [RegisterConfiguration(0x0312, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    Inaccuracy,

    /// <summary>
    /// Нижнее паспортное давление, Па
    /// </summary>
    [RegisterConfiguration(0x0314, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PressureLowBound,

    /// <summary>
    /// Верхнее паспортное давление, Па
    /// </summary>
    [RegisterConfiguration(0x0316, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PressureHighBound,

    /// <summary>
    /// Заводской номер
    /// </summary>
    [RegisterConfiguration(0x0318, 4, RegisterDataType.UInt64, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PassportVendorNumber,

    /// <summary>
    /// Дата выпуска
    /// </summary>
    [RegisterConfiguration(0x031C, 2, RegisterDataType.UnixTime, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ReleaseDate,

    /// <summary>
    /// Тип выхода:
    /// 0 - RS485
    /// 1 - 0,4 - 2 В
    /// 2 - 4 - 20 мА
    /// </summary>
    [RegisterConfiguration(0x031E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    OutputType,

    #endregion

    #region Управление паролями

    /// <summary>
    /// Номер для запроса одноразового пароля
    /// </summary>
    [RegisterConfiguration(0xF000, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PasswordAccess,

    /// <summary>
    /// Номер для запроса одноразового пароля
    /// </summary>
    [RegisterConfiguration(0xF01C, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    OneShotPasswordNumber,

    #endregion
}