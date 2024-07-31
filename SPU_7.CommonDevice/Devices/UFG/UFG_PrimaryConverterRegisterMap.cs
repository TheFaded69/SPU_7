using System.ComponentModel;
using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

/// <summary>
/// Карта регистров первичного преобразователя UFG
/// </summary>
namespace SPU_7.CommonDevice.Devices.UFG;
public enum UFG_PrimaryConverterRegisterMap
{
    /// <summary>
    /// Текущий рабочий расход
    /// </summary>
    [Description("Текущий рабочий расход"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x0002, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    VolumeFlowValue,

    #region Отладка

    /// <summary>
    /// Блок с регистрами отладки
    /// </summary>
    [RegisterConfiguration(0x1000, 24, RegisterDataType.ByteArray, ModbusFunction.ReadHoldingRegisters)]
    DebugRegistersBlock,

    /// <summary>
    /// Отладочный режим рабочего расхода
    /// </summary>
    [RegisterConfiguration(0x1003, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    VolumeFlowDebugMode,

    /// <summary>
    /// Отладочное значение рабочего расхода, м3/час
    /// </summary>
    [RegisterConfiguration(0x100A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VolumeFlowDebugValue,

    #endregion

    #region Лучи

    #region Коэффициент усиления АРУ по подлучам

    /// <summary>
    /// Блок с коэффицентами усиления подлучей
    /// </summary>
    [Description("Коэффицент усиления подлучей")]
    [RegisterConfiguration(0x0100, 16, RegisterDataType.UInt16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamsGainFlowBlock,

    /// <summary>
    /// Коэффициент усиления подлуча 1 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 1 по потоку")]
    [RegisterConfiguration(0x0100, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam1FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 1 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 1 против потока")]
    [RegisterConfiguration(0x0101, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam1FlowAgainst,
    
    /// <summary>
    /// Коэффициент усиления подлуча 2 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 2 по потоку")]
    [RegisterConfiguration(0x0102, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam2FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 2 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 2 против потока")]
    [RegisterConfiguration(0x0103, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam2FlowAgainst,
    
    /// <summary>
    /// Коэффициент усиления подлуча 3 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 3 по потоку")]
    [RegisterConfiguration(0x0104, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam3FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 3 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 3 против потока")]
    [RegisterConfiguration(0x0105, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam3FlowAgainst,
    
    /// <summary>
    /// Коэффициент усиления подлуча 4 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 4 по потоку")]
    [RegisterConfiguration(0x0106, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam4FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 4 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 4 против потока")]
    [RegisterConfiguration(0x0107, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam4FlowAgainst,
    
    /// <summary>
    /// Коэффициент усиления подлуча 5 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 5 по потоку")]
    [RegisterConfiguration(0x0108, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam5FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 5 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 5 против потока")]
    [RegisterConfiguration(0x0109, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam5FlowAgainst,
    
    /// <summary>
    /// Коэффициент усиления подлуча 6 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 6 по потоку")]
    [RegisterConfiguration(0x010A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam6FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 6 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 6 против потока")]
    [RegisterConfiguration(0x010B, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam6FlowAgainst,
    
    /// <summary>
    /// Коэффициент усиления подлуча 7 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 7 по потоку")]
    [RegisterConfiguration(0x010C, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam7FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 7 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 7 против потока")]
    [RegisterConfiguration(0x010D, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam7FlowAgainst,
    
    /// <summary>
    /// Коэффициент усиления подлуча 8 по потоку
    /// </summary>
    [Description("Коэффицент усиления подлуча 8 по потоку")]
    [RegisterConfiguration(0x010E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam8FlowAlong,
    
    /// <summary>
    /// Коэффициент усиления подлуча 8 против потока
    /// </summary>
    [Description("Коэффицент усиления подлуча 8 против потока")]
    [RegisterConfiguration(0x010F, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    GainSubBeam8FlowAgainst,

    #endregion

    #region Скорость звука нефильтрованная

    /// <summary>
    /// Блок с нефильтрованной скоростью звука лучей, м/с
    /// </summary>
    [Description("Нефильтрованная скорость звука лучей"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x1018, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamsNotFilteredSoundSpeedBlock,

    #endregion

    #region Скорость звука фильтрованная

    /// <summary>
    /// Блок с фильтрованной скоростью звука лучей, м/с
    /// </summary>
    [Description("Фильтрованная скорость звука лучей"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x1028, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamsFilteredSoundSpeedBlock,

    /// <summary>
    /// Фильтрованная скорость звука луча 1, м/с
    /// </summary>
    [Description("Фильтрованная скорость звука луча №1"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x1028, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FilteredSoundSpeedBeam1,

    #endregion

    #region Скорость потока нефильтрованная

    /// <summary>
    /// Блок с нефильтрованной скоростью потока лучей, м/с
    /// </summary>
    [Description("Нефильтрованная скорость потока лучей"), MeasureUnits("м/с")]
    [RegisterConfiguration(0x1038, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamsNotFilteredFlowSpeedBlock,

    #endregion

    #region Амплитуда сигнала подлучей

    /// <summary>
    /// Блок с амплитудой сигнала подлучей
    /// </summary>
    [Description("Амплитуда сигнала подлучей")]
    [RegisterConfiguration(0x0140, 16, RegisterDataType.UInt16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamsSignalAmplitudeBlock,

    /// <summary>
    /// Амплитуда сигнала подлуча 1 по потоку
    /// </summary>
    [Description("Амплитуда сигнала подлуча №1 по потоку")]
    [RegisterConfiguration(0x0140, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalAmplitudeSubBeam1FlowAlong,

    /// <summary>
    /// Амплитуда сигнала подлуча 1 против потока
    /// </summary>
    [Description("Амплитуда сигнала подлуча №1 против потока")]
    [RegisterConfiguration(0x0140, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalAmplitudeSubBeam1FlowAgainst,

    #endregion

    #region Текущие разности

    /// <summary>
    /// Блок с текущей разностью лучей
    /// </summary>
    [Description("Текущая разность лучей")]
    [RegisterConfiguration(0x1078, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamsCurrentDifferenceBlock,

    #endregion

    #region Процент ошибок подлучей

    /// <summary>
    /// Блок с процентами ошибок подлучей по потоку и против потока
    /// </summary>
    [Description("Процент ошибок подлучей по потоку и против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10E8, 16, RegisterDataType.UInt16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamsErrorRateFlowBlock,

    /// <summary>
    /// Процент ошибок подлуча 1 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №1 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10E8, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam1FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 1 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №1 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10E9, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam1FlowAgainst,
    
    /// <summary>
    /// Процент ошибок подлуча 2 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №2 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10EA, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam2FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 2 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №2 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10EB, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam2FlowAgainst,
    
    /// <summary>
    /// Процент ошибок подлуча 3 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №3 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10EC, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam3FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 3 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №3 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10ED, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam3FlowAgainst,
    
    /// <summary>
    /// Процент ошибок подлуча 4 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №4 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10EE, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam4FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 4 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №4 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10EF, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam4FlowAgainst,
    
    /// <summary>
    /// Процент ошибок подлуча 5 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №5 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F0, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam5FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 5 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №5 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F1, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam5FlowAgainst,
    
    /// <summary>
    /// Процент ошибок подлуча 6 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №6 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F2, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam6FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 6 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №6 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F3, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam6FlowAgainst,
    
    /// <summary>
    /// Процент ошибок подлуча 7 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №7 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F4, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam7FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 7 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №7 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F5, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam7FlowAgainst,
    
    /// <summary>
    /// Процент ошибок подлуча 8 по потоку
    /// </summary>
    [Description("Процент ошибок подлуча №8 по потоку"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F6, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam8FlowAlong,
    
    /// <summary>
    /// Процент ошибок подлуча 8 против потока
    /// </summary>
    [Description("Процент ошибок подлуча №8 против потока"), MeasureUnits("%")]
    [RegisterConfiguration(0x10F7, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ErrorRateSubBeam8FlowAgainst,

    #endregion

    #region Отношение сигнал/шум по подлучам

    [Description("Отношение сигнал/шум по подлучам")]
    [RegisterConfiguration(0x0160, 32, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamsSignalToNoiseRatioFlowBlock,

    /// <summary>
    /// Отношение сигнал/шум подлуча 1 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №1 по потоку")]
    [RegisterConfiguration(0x0160, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam1FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 1 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №1 против потока")]
    [RegisterConfiguration(0x0162, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam1FlowAgainst,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 2 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №2 по потоку")]
    [RegisterConfiguration(0x0164, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam2FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 2 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №2 против потока")]
    [RegisterConfiguration(0x0166, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam2FlowAgainst,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 3 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №3 по потоку")]
    [RegisterConfiguration(0x0168, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam3FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 3 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №3 против потока")]
    [RegisterConfiguration(0x016A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam3FlowAgainst,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 4 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №4 по потоку")]
    [RegisterConfiguration(0x016C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam4FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 4 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №4 против потока")]
    [RegisterConfiguration(0x016E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam4FlowAgainst,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 5 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №5 по потоку")]
    [RegisterConfiguration(0x0170, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam5FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 5 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №5 против потока")]
    [RegisterConfiguration(0x0172, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam5FlowAgainst,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 6 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №6 по потоку")]
    [RegisterConfiguration(0x0174, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam6FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 6 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №6 против потока")]
    [RegisterConfiguration(0x0176, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam6FlowAgainst,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 7 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №7 по потоку")]
    [RegisterConfiguration(0x0178, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam7FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 7 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №7 против потока")]
    [RegisterConfiguration(0x017A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam7FlowAgainst,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 8 по потоку
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №8 по потоку")]
    [RegisterConfiguration(0x017C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam8FlowAlong,
    
    /// <summary>
    /// Отношение сигнал/шум подлуча 8 против потока
    /// </summary>
    [Description("Отношение сигнал/шум подлуча №8 против потока")]
    [RegisterConfiguration(0x017E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SignalToNoiseRatioSubBeam8FlowAgainst,

    #endregion

    #region Текущее состояние подлучей

    /// <summary>
    /// Блок с состоянием подлучей
    /// </summary>
    [Description("Состояние подлучей")]
    [RegisterConfiguration(0x10D8, 16, RegisterDataType.UInt16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    SubBeamsStatusFlowBlock,

    /// <summary>
    /// Состояние подлуча 1 по потоку
    /// </summary>
    [Description("Состояние подлуча №1 по потоку")]
    [RegisterConfiguration(0x10D8, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam1FlowAlong,

    /// <summary>
    /// Состояние подлуча 1 против потока
    /// </summary>
    [Description("Состояние подлуча №1 против потока")]
    [RegisterConfiguration(0x10D9, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam1FlowAgainst,

    /// <summary>
    /// Состояние подлуча 2 по потоку
    /// </summary>
    [Description("Состояние подлуча №2 по потоку")]
    [RegisterConfiguration(0x10DA, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam2FlowAlong,

    /// <summary>
    /// Состояние подлуча 2 против потока
    /// </summary>
    [Description("Состояние подлуча №2 против потока")]
    [RegisterConfiguration(0x10DB, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam2FlowAgainst,

    /// <summary>
    /// Состояние подлуча 3 по потоку
    /// </summary>
    [Description("Состояние подлуча №3 по потоку")]
    [RegisterConfiguration(0x10DC, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam3FlowAlong,

    /// <summary>
    /// Состояние подлуча 3 против потока
    /// </summary>
    [Description("Состояние подлуча №3 против потока")]
    [RegisterConfiguration(0x10DD, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam3FlowAgainst,

    /// <summary>
    /// Состояние подлуча 4 по потоку
    /// </summary>
    [Description("Состояние подлуча №4 по потоку")]
    [RegisterConfiguration(0x10DE, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam4FlowAlong,

    /// <summary>
    /// Состояние подлуча 4 против потока
    /// </summary>
    [Description("Состояние подлуча №4 против потока")]
    [RegisterConfiguration(0x10DF, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam4FlowAgainst,

    /// <summary>
    /// Состояние подлуча 5 по потоку
    /// </summary>
    [Description("Состояние подлуча №5 по потоку")]
    [RegisterConfiguration(0x10E0, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam5FlowAlong,

    /// <summary>
    /// Состояние подлуча 5 против потока
    /// </summary>
    [Description("Состояние подлуча №5 против потока")]
    [RegisterConfiguration(0x10E1, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam5FlowAgainst,

    /// <summary>
    /// Состояние подлуча 6 по потоку
    /// </summary>
    [Description("Состояние подлуча №6 по потоку")]
    [RegisterConfiguration(0x10E2, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam6FlowAlong,

    /// <summary>
    /// Состояние подлуча 6 против потока
    /// </summary>
    [Description("Состояние подлуча №6 против потока")]
    [RegisterConfiguration(0x10E3, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam6FlowAgainst,

    /// <summary>
    /// Состояние подлуча 7 по потоку
    /// </summary>
    [Description("Состояние подлуча №7 по потоку")]
    [RegisterConfiguration(0x10E4, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam7FlowAlong,

    /// <summary>
    /// Состояние подлуча 7 против потока
    /// </summary>
    [Description("Состояние подлуча №7 против потока")]
    [RegisterConfiguration(0x10E5, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam7FlowAgainst,

    /// <summary>
    /// Состояние подлуча 8 по потоку
    /// </summary>
    [Description("Состояние подлуча №8 по потоку")]
    [RegisterConfiguration(0x10E6, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam8FlowAlong,

    /// <summary>
    /// Состояние подлуча 8 против потока
    /// </summary>
    [Description("Состояние подлуча №8 против потока")]
    [RegisterConfiguration(0x10E7, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StatusSubBeam8FlowAgainst,

    #endregion

    #region Длина пути лучей

    /// <summary>
    /// Блок с длинами пути лучей 
    /// </summary>
    [Description("Длина пути лучей"), MeasureUnits("м")]
    [RegisterConfiguration(0x2030, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamsPathLengthBlock,

    /// <summary>
    /// Длина пути луча 1
    /// </summary>
    [Description("Длина пути луча №1"), MeasureUnits("м")]
    [RegisterConfiguration(0x2030, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam1,
    
    /// <summary>
    /// Длина пути луча 2
    /// </summary>
    [Description("Длина пути луча №2"), MeasureUnits("м")]
    [RegisterConfiguration(0x2032, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam2,
    
    /// <summary>
    /// Длина пути луча 3
    /// </summary>
    [Description("Длина пути луча №3"), MeasureUnits("м")]
    [RegisterConfiguration(0x2034, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam3,
    
    /// <summary>
    /// Длина пути луча 4
    /// </summary>
    [Description("Длина пути луча №4"), MeasureUnits("м")]
    [RegisterConfiguration(0x2036, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam4,
    
    /// <summary>
    /// Длина пути луча 5
    /// </summary>
    [Description("Длина пути луча №5"), MeasureUnits("м")]
    [RegisterConfiguration(0x2038, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam5,
    
    /// <summary>
    /// Длина пути луча 6
    /// </summary>
    [Description("Длина пути луча №6"), MeasureUnits("м")]
    [RegisterConfiguration(0x203A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam6,
    
    /// <summary>
    /// Длина пути луча 7
    /// </summary>
    [Description("Длина пути луча №7"), MeasureUnits("м")]
    [RegisterConfiguration(0x203C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam7,
    
    /// <summary>
    /// Длина пути луча 8
    /// </summary>
    [Description("Длина пути луча №8"), MeasureUnits("м")]
    [RegisterConfiguration(0x203E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PathLengthBeam8,

    #endregion

    #region Косинус угла лучей

    /// <summary>
    /// Блок с косинусами угла лучей
    /// </summary>
    [Description("Косинус угла лучей")]
    [RegisterConfiguration(0x2040, 16, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    BeamsAngleCosineBlock,

    /// <summary>
    /// Косинус угла луча 1
    /// </summary>
    [Description("Косинус угла луча №1")]
    [RegisterConfiguration(0x2040, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam1,
    
    /// <summary>
    /// Косинус угла луча 2
    /// </summary>
    [Description("Косинус угла луча №2")]
    [RegisterConfiguration(0x2042, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam2,
    
    /// <summary>
    /// Косинус угла луча 3
    /// </summary>
    [Description("Косинус угла луча №3")]
    [RegisterConfiguration(0x2044, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam3,
    
    /// <summary>
    /// Косинус угла луча 4
    /// </summary>
    [Description("Косинус угла луча №4")]
    [RegisterConfiguration(0x2046, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam4,
    
    /// <summary>
    /// Косинус угла луча 5
    /// </summary>
    [Description("Косинус угла луча №5")]
    [RegisterConfiguration(0x2048, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam5,
    
    /// <summary>
    /// Косинус угла луча 6
    /// </summary>
    [Description("Косинус угла луча №6")]
    [RegisterConfiguration(0x204A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam6,
    
    /// <summary>
    /// Косинус угла луча 7
    /// </summary>
    [Description("Косинус угла луча №7")]
    [RegisterConfiguration(0x204C, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam7,
    
    /// <summary>
    /// Косинус угла луча 8
    /// </summary>
    [Description("Косинус угла луча №8")]
    [RegisterConfiguration(0x204E, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AngleCosineBeam8,

    #endregion

    #endregion

    #region Осциллограммы

    #region Текущее значение DELVAL

    /// <summary>
    /// Значение Delval луча 1 по потоку
    /// </summary>
    [Description("Значение Delval луча №1 по потоку")]
    [RegisterConfiguration(0x01A0, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam1_AlongFlow,

    /// <summary>
    /// Значение Delval луча 1 против потока
    /// </summary>
    [Description("Значение Delval луча №1 против потока")]
    [RegisterConfiguration(0x01A2, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam1_AgainstFlow,

    /// <summary>
    /// Значение Delval луча 2 по потоку
    /// </summary>
    [Description("Значение Delval луча №2 по потоку")]
    [RegisterConfiguration(0x01A4, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam2_AlongFlow,

    /// <summary>
    /// Значение Delval луча 2 против потока
    /// </summary>
    [Description("Значение Delval луча №2 против потока")]
    [RegisterConfiguration(0x01A6, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam2_AgainstFlow,

    /// <summary>
    /// Значение Delval луча 3 по потоку
    /// </summary>
    [Description("Значение Delval луча №3 по потоку")]
    [RegisterConfiguration(0x01A8, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam3_AlongFlow,

    /// <summary>
    /// Значение Delval луча 3 против потока
    /// </summary>
    [Description("Значение Delval луча №3 против потока")]
    [RegisterConfiguration(0x01AA, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam3_AgainstFlow,

    /// <summary>
    /// Значение Delval луча 4 по потоку
    /// </summary>
    [Description("Значение Delval луча №4 по потоку")]
    [RegisterConfiguration(0x01AC, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam4_AlongFlow,

    /// <summary>
    /// Значение Delval луча 4 против потока
    /// </summary>
    [Description("Значение Delval луча №4 против потока")]
    [RegisterConfiguration(0x01AE, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam4_AgainstFlow,

    /// <summary>
    /// Значение Delval луча 5 по потоку
    /// </summary>
    [Description("Значение Delval луча №5 по потоку")]
    [RegisterConfiguration(0x01B0, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam5_AlongFlow,

    /// <summary>
    /// Значение Delval луча 5 против потока
    /// </summary>
    [Description("Значение Delval луча №5 против потока")]
    [RegisterConfiguration(0x01B2, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam5_AgainstFlow,

    /// <summary>
    /// Значение Delval луча 6 по потоку
    /// </summary>
    [Description("Значение Delval луча №6 по потоку")]
    [RegisterConfiguration(0x01B4, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam6_AlongFlow,

    /// <summary>
    /// Значение Delval луча 6 против потока
    /// </summary>
    [Description("Значение Delval луча №6 против потока")]
    [RegisterConfiguration(0x01B6, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam6_AgainstFlow,

    /// <summary>
    /// Значение Delval луча 7 по потоку
    /// </summary>
    [Description("Значение Delval луча №7 по потоку")]
    [RegisterConfiguration(0x01B8, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam7_AlongFlow,

    /// <summary>
    /// Значение Delval луча 7 против потока
    /// </summary>
    [Description("Значение Delval луча №7 против потока")]
    [RegisterConfiguration(0x01BA, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam7_AgainstFlow,

    /// <summary>
    /// Значение Delval луча 8 по потоку
    /// </summary>
    [Description("Значение Delval луча №8 по потоку")]
    [RegisterConfiguration(0x01BC, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam8_AlongFlow,

    /// <summary>
    /// Значение Delval луча 8 против потока
    /// </summary>
    [Description("Значение Delval луча №8 против потока")]
    [RegisterConfiguration(0x01BE, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalCurrentBeam8_AgainstFlow,

    #endregion

    #region Минимальный Delval

    /// <summary>
    /// Блок с минимальными коэффициентами Delval по потоку и против потока
    /// </summary>
    [Description("Минимальные коэффиценты Delval лучей")]
    [RegisterConfiguration(0x2100, 32, RegisterDataType.UInt32Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalBeamsMinimumBlock,

    /// <summary>
    /// Коэффициент Delval луча 1 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №1 по потоку")]
    [RegisterConfiguration(0x2100, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam1_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 1 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №1 против потока")]
    [RegisterConfiguration(0x2102, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam1_AgainstFlow,

    /// <summary>
    /// Коэффициент Delval луча 2 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №2 по потоку")]
    [RegisterConfiguration(0x2104, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam2_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 2 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №2 против потока")]
    [RegisterConfiguration(0x2106, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam2_AgainstFlow,

    /// <summary>
    /// Коэффициент Delval луча 3 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №3 по потоку")]
    [RegisterConfiguration(0x2108, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam3_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 3 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №3 против потока")]
    [RegisterConfiguration(0x210A, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam3_AgainstFlow,

    /// <summary>
    /// Коэффициент Delval луча 4 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №4 по потоку")]
    [RegisterConfiguration(0x210C, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam4_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 4 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №4 против потока")]
    [RegisterConfiguration(0x210E, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam4_AgainstFlow,

    /// <summary>
    /// Коэффициент Delval луча 5 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №5 по потоку")]
    [RegisterConfiguration(0x2110, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam5_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 5 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №5 против потока")]
    [RegisterConfiguration(0x2112, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam5_AgainstFlow,

    /// <summary>
    /// Коэффициент Delval луча 6 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №6 по потоку")]
    [RegisterConfiguration(0x2114, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam6_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 6 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №6 против потока")]
    [RegisterConfiguration(0x2116, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam6_AgainstFlow,

    /// <summary>
    /// Коэффициент Delval луча 7 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №7 по потоку")]
    [RegisterConfiguration(0x2118, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam7_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 7 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №7 против потока")]
    [RegisterConfiguration(0x211A, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam7_AgainstFlow,

    /// <summary>
    /// Коэффициент Delval луча 8 по потоку
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №8 по потоку")]
    [RegisterConfiguration(0x211C, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam8_AlongFlow,

    /// <summary>
    /// Коэффициент Delval луча 8 против потока
    /// </summary>
    [Description("Минимальный коэффицент Delval луча №8 против потока")]
    [RegisterConfiguration(0x211E, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DelvalMinimumBeam8_AgainstFlow,

    #endregion

    #region Начало окна сканирования АРУ

    /// <summary>
    /// Блок с началом окна сканирования АРУ лучей
    /// </summary>
    [Description("Начало окна сканирования АРУ лучей")]
    [RegisterConfiguration(0x20B0, 8, RegisterDataType.UInt16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BeamsScanWindowStartBlock,

    /// <summary>
    /// Начало окна сканирования АРУ луча 1
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №1")]
    [RegisterConfiguration(0x20B0, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam1,

    /// <summary>
    /// Начало окна сканирования АРУ луча 2
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №2")]
    [RegisterConfiguration(0x20B1, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam2,

    /// <summary>
    /// Начало окна сканирования АРУ луча 3
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №3")]
    [RegisterConfiguration(0x20B2, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam3,

    /// <summary>
    /// Начало окна сканирования АРУ луча 4
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №4")]
    [RegisterConfiguration(0x20B3, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam4,

    /// <summary>
    /// Начало окна сканирования АРУ луча 5
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №5")]
    [RegisterConfiguration(0x20B4, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam5,

    /// <summary>
    /// Начало окна сканирования АРУ луча 6
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №6")]
    [RegisterConfiguration(0x20B5, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam6,

    /// <summary>
    /// Начало окна сканирования АРУ луча 7
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №7")]
    [RegisterConfiguration(0x20B6, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam7,

    /// <summary>
    /// Начало окна сканирования АРУ луча 8
    /// </summary>
    [Description("Начало окна сканирования АРУ луча №8")]
    [RegisterConfiguration(0x20B7, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowStartBeam8,

    #endregion

    #region Размер окна сканирования АРУ

    /// <summary>
    /// Блок с размерами окон сканирования АРУ лучей 
    /// </summary>
    [Description("Размер окна сканирования АРУ лучей")]
    [RegisterConfiguration(0x20B8, 8, RegisterDataType.UInt16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BeamsScanWindowSizeBlock,

    /// <summary>
    /// Размер окна сканирования АРУ луча №1
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №1")]
    [RegisterConfiguration(0x20B8, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam1,

    /// <summary>
    /// Размер окна сканирования АРУ луча №2
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №2")]
    [RegisterConfiguration(0x20B9, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam2,

    /// <summary>
    /// Размер окна сканирования АРУ луча №3
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №3")]
    [RegisterConfiguration(0x20BA, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam3,

    /// <summary>
    /// Размер окна сканирования АРУ луча №4
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №4")]
    [RegisterConfiguration(0x20BB, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam4,

    /// <summary>
    /// Размер окна сканирования АРУ луча №5
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №5")]
    [RegisterConfiguration(0x20BC, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam5,

    /// <summary>
    /// Размер окна сканирования АРУ луча №6
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №6")]
    [RegisterConfiguration(0x20BD, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam6,

    /// <summary>
    /// Размер окна сканирования АРУ луча №7
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №7")]
    [RegisterConfiguration(0x20BE, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam7,

    /// <summary>
    /// Размер окна сканирования АРУ луча №8
    /// </summary>
    [Description("Размер окна сканирования АРУ луча №8")]
    [RegisterConfiguration(0x20BF, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ScanWindowSizeBeam8,

    #endregion

    #region Коэффициенты времени добавляющие

    /// <summary>
    /// Блок с добавляющими коэффициентами времени лучей
    /// </summary>
    [Description("Коэффициенты времени добавляющие")]
    [RegisterConfiguration(0x20C0, 32, RegisterDataType.FloatArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SubBeamsTimeOffsetBlock,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 1 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №1 по потоку")]
    [RegisterConfiguration(0x20C0, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam1_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 1 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №1 против потока")]
    [RegisterConfiguration(0x20C2, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam1_AgainstFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 2 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №2 по потоку")]
    [RegisterConfiguration(0x20C4, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam2_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 2 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №2 против потока")]
    [RegisterConfiguration(0x20C6, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam2_AgainstFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 3 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №3 по потоку")]
    [RegisterConfiguration(0x20C8, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam3_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 3 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №3 против потока")]
    [RegisterConfiguration(0x20CA, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam3_AgainstFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 4 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №4 по потоку")]
    [RegisterConfiguration(0x20CC, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam4_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 4 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №4 против потока")]
    [RegisterConfiguration(0x20CE, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam4_AgainstFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 5 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №5 по потоку")]
    [RegisterConfiguration(0x20D0, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam5_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 5 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №5 против потока")]
    [RegisterConfiguration(0x20D2, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam5_AgainstFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 6 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №6 по потоку")]
    [RegisterConfiguration(0x20D4, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam6_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 6 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №6 против потока")]
    [RegisterConfiguration(0x20D6, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam6_AgainstFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 7 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №7 по потоку")]
    [RegisterConfiguration(0x20D8, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam7_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 7 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №7 против потока")]
    [RegisterConfiguration(0x20DA, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam7_AgainstFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 8 по потоку
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №8 по потоку")]
    [RegisterConfiguration(0x20DC, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam8_AlongFlow,

    /// <summary>
    /// Коэффициент добавляющий времени подлуча 8 против потока
    /// </summary>
    [Description("Коэффициент добавляющий времени подлуча №8 против потока")]
    [RegisterConfiguration(0x20DE, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeOffsetSubBeam8_AgainstFlow,

    #endregion

    /// <summary>
    /// Идеальная амплитуда, В
    /// </summary>
    [Description("Идеальная амплитуда"), MeasureUnits("В")]
    [RegisterConfiguration(0x201D, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PerfectAmplitude,

    /// <summary>
    /// Размер выборки для определения уровня помехи
    /// </summary>
    [Description("Размер выборки для определения уровня помехи")]
    [RegisterConfiguration(0x2022, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    NoiseLevelSampleSize,

    /// <summary>
    /// Коэффициент умножения преобразователя времени
    /// </summary>
    [Description("Коэффициент умножения преобразователя времени")]
    [RegisterConfiguration(0x2018, 4, RegisterDataType.Double, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    TimeConverterMultiplicationFactor,

    /// <summary>
    /// Частота выборки осциллограммы (0 - 1.8, 1 - 3.6)
    /// </summary>
    [Description("Частота выборки осциллограммы"), MeasureUnits("МГц")]
    [RegisterConfiguration(0x3012, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    WaveformSamplingFrequency,

    /// <summary>
    /// Выбор каскада АЦП для снятия осцилограммы
    /// </summary>
    [Description("Каскад АЦП для снятия осцилограммы")]
    [RegisterConfiguration(0x1178, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    OscillogrammCascade,

    /// <summary>
    /// Режим отладки лучей (1 – ВКЛ, 0 - ВЫКЛ)
    /// </summary>
    [Description("Режим отладки лучей")]
    [RegisterConfiguration(0x1000, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BeamDebugMode,

    /// <summary>
    /// Номер активного канала (с 0-го по 15-й)
    /// </summary>
    [Description("Номер активного канала")]
    [RegisterConfiguration(0x1002, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ActiveBeamChannelNumber,

    /// <summary>
    /// Кол-во точек эталонной осциллограммы
    /// </summary>
    [Description("Кол-во точек эталонной осциллограммы")]
    [RegisterConfiguration(0x3100, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    OscPointsCount,

    /// <summary>
    /// Точки эталонной осциллограммы
    /// </summary>
    [Description("Точка эталонной осциллограммы")]
    [RegisterConfiguration(0x3101, 256, RegisterDataType.Int16Array, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    OscPointsBlock,

    /// <summary>
    /// Данные из выборки осцилограммы
    /// </summary>
    [Description("Данные из выборки осцилограммы")]
    [RegisterConfiguration(0x4000, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SampleDataStart,

    #endregion

    #region Дублирование

    /// <summary>
    /// Настройка дублирования
    /// </summary>
    [Description("Режим дублирования")]
    [RegisterConfiguration(0x2406, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DuplicationMode,

    #endregion

    /// <summary>
    /// Тип датчика давления
    /// </summary>
    [Description("Тип датчика давления")]
    [RegisterConfiguration(0x2411, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    PressureSensorType,

    #region Параметры

    /// <summary>
    /// Адрес Modbus
    /// </summary>
    [Description("Адрес в сети Modbus")]
    [RegisterConfiguration(0x2001, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    UnitId,

    /// <summary>
    /// Количество лучей
    /// </summary>
    [Description("Количество лучей")]
    [RegisterConfiguration(0x2002, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    BeamsCount,

    /// <summary>
    /// Смещение лучей
    /// </summary>
    [Description("Смещение лучей")]
    [RegisterConfiguration(0x2003, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    BeamsDisplacement,

    /// <summary>
    /// Диаметр трубы
    /// </summary>
    [Description("Диаметр трубы"), MeasureUnits("м")]
    [RegisterConfiguration(0x2004, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PipeDiameter,

    #endregion

    /// <summary>
    /// Минимальный расход, м³/ч
    /// </summary>
    [Description("Минимальный расход"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x2170, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MinimalFlow,

    /// <summary>
    /// Максимальный расход, м³/ч
    /// </summary>
    [Description("Максимальный расход"), MeasureUnits("м³/ч")]
    [RegisterConfiguration(0x2172, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaximumFlow,

    /// <summary>
    /// Минимальная шкала датчика давления, МПа
    /// </summary>
    [Description("Минимальная шкала датчика давления"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x2194, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MinPressureSensorValue,

    /// <summary>
    /// Максимальная шкала датчика давления, МПа
    /// </summary>
    [Description("Максимальная шкала датчика давления"), MeasureUnits("МПа")]
    [RegisterConfiguration(0x2192, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaxPressureSensorValue,


}