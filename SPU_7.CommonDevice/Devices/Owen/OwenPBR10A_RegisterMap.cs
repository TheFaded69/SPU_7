using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;

public enum OwenPBR10A_RegisterMap
{
    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x0025, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ControllerTemperature,

    /// <summary>
    /// Состояния DI1-DI5
    /// </summary>
    [RegisterConfiguration(0x0033, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    InputsState,

    #region Настройки связи

    /// <summary>
    /// Скорость передачи [5 - 9600, 6 - 14400, 7 - 19200, 8 - 38400, 9 - 57600, 10 - 115200]
    /// </summary>
    [RegisterConfiguration(0x0209, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    BaudRate,

    /// <summary>
    /// Бит данных
    /// </summary>
    [RegisterConfiguration(0x020A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DataBits,

    /// <summary>
    /// Кол-во стоп-бит [0 - 1; 1 - 2]
    /// </summary>
    [RegisterConfiguration(0x020B, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StopBits,

    /// <summary>
    /// Проверка чётности [0 - None; 1 - Even, 2 - Odd]
    /// </summary>
    [RegisterConfiguration(0x020C, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    Parity,

    /// <summary>
    /// Адрес в сети Modbus
    /// </summary>
    [RegisterConfiguration(0x020D, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    UnitId,

    #endregion

    // ...

    /// <summary>
    /// Уровень сигнала AI1
    /// </summary>
    [RegisterConfiguration(0x03EE, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput1,

    /// <summary>
    /// Уровень сигнала AI2
    /// </summary>
    [RegisterConfiguration(0x03F6, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    AnalogInput2,

    /// <summary>
    /// Чередование фаз [0 - Прямое, 1 - Обратное]
    /// </summary>
    [RegisterConfiguration(0x0453, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PhaseRotation,

    #region Фаза №1 (L1)

    /// <summary>
    /// RMS фазного напряжения L1, В
    /// </summary>
    [RegisterConfiguration(0x04CF, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    RMSVoltageL1,

    /// <summary>
    /// RMS фазного тока L1, А
    /// </summary>
    [RegisterConfiguration(0x04D1, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    RMSCurrentL1,

    /// <summary>
    /// Активная мощность L1, Вт
    /// </summary>
    [RegisterConfiguration(0x04D3, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ActivePowerL1,

    /// <summary>
    /// Реактивная мощность L1, Вар
    /// </summary>
    [RegisterConfiguration(0x04D5, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ReactivePowerL1,

    /// <summary>
    /// Полная мощность L1
    /// </summary>
    [RegisterConfiguration(0x04D7, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FullPowerL1,

    /// <summary>
    /// Коэффицент мощности L1
    /// </summary>
    [RegisterConfiguration(0x04D9, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PowerCoefficientL1,

    /// <summary>
    /// Фазовый угол L1
    /// </summary>
    [RegisterConfiguration(0x04DB, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PhaseAngleL1,

    #endregion

    #region Фаза №2 (L2)

    /// <summary>
    /// RMS фазного напряжения L2, В
    /// </summary>
    [RegisterConfiguration(0x04E7, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    RMSVoltageL2,

    /// <summary>
    /// RMS фазного тока L2, А
    /// </summary>
    [RegisterConfiguration(0x04E9, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    RMSCurrentL2,

    /// <summary>
    /// Активная мощность L2, Вт
    /// </summary>
    [RegisterConfiguration(0x04EB, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ActivePowerL2,

    /// <summary>
    /// Реактивная мощность L2, Вар
    /// </summary>
    [RegisterConfiguration(0x04ED, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ReactivePowerL2,

    /// <summary>
    /// Полная мощность L2
    /// </summary>
    [RegisterConfiguration(0x04EF, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FullPowerL2,

    /// <summary>
    /// Коэффицент мощности L2
    /// </summary>
    [RegisterConfiguration(0x04F1, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PowerCoefficientL2,

    /// <summary>
    /// Фазовый угол L2
    /// </summary>
    [RegisterConfiguration(0x04F3, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PhaseAngleL2,

    #endregion

    #region Фаза №3 (L3)

    /// <summary>
    /// RMS фазного напряжения L3, В
    /// </summary>
    [RegisterConfiguration(0x04FF, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    RMSVoltageL3,

    /// <summary>
    /// RMS фазного тока L3, А
    /// </summary>
    [RegisterConfiguration(0x0501, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    RMSCurrentL3,

    /// <summary>
    /// Активная мощность L3, Вт
    /// </summary>
    [RegisterConfiguration(0x0503, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ActivePowerL3,

    /// <summary>
    /// Реактивная мощность L3, Вар
    /// </summary>
    [RegisterConfiguration(0x0505, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ReactivePowerL3,

    /// <summary>
    /// Полная мощность L3
    /// </summary>
    [RegisterConfiguration(0x0507, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    FullPowerL3,

    /// <summary>
    /// Коэффицент мощности L3
    /// </summary>
    [RegisterConfiguration(0x0509, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PowerCoefficientL3,

    /// <summary>
    /// Фазовый угол L3
    /// </summary>
    [RegisterConfiguration(0x050B, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PhaseAngleL3,

    #endregion

    /// <summary>
    /// Частота напряжения в сети, Гц
    /// </summary>
    [RegisterConfiguration(0x0517, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    VoltageFrequency,

    /// <summary>
    /// Конфигурация Аналогового выхода [0 - Откл.; 1 - 0..1В; 2 - 0..10В; 3 - 0..20мА; 4 - 4..20мА]
    /// </summary>
    [RegisterConfiguration(0x0628, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    AnalogOutputConfiguration,

    /// <summary>
    /// Функция DI1 [0 - КВЗ; 1 - КВО; 2 - МВ; 3 - Блок. защит; 4 - Сброс аварий; 5 - Дожим; 6 - Аварийный стоп; 7 - Безопасное положение]
    /// </summary>
    [RegisterConfiguration(0x2711, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FunctionDigitalInput1,

    /// <summary>
    /// Функция DI2 [0 - КВЗ; 1 - КВО; 2 - МВ; 3 - Блок. защит; 4 - Сброс аварий; 5 - Дожим; 6 - Аварийный стоп; 7 - Безопасное положение]
    /// </summary>
    [RegisterConfiguration(0x2712, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FunctionDigitalInput2,

    /// <summary>
    /// Функция DI3 [0 - КВЗ; 1 - КВО; 2 - МВ; 3 - Блок. защит; 4 - Сброс аварий; 5 - Дожим; 6 - Аварийный стоп; 7 - Безопасное положение]
    /// </summary>
    [RegisterConfiguration(0x2713, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FunctionDigitalInput3,

    /// <summary>
    /// Калибровка хода
    /// </summary>
    [RegisterConfiguration(0x2714, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    StepCalibration,

    /// <summary>
    /// Режим работы [0 - Авто, 1 - Ручной]
    /// </summary>
    [RegisterConfiguration(0x2715, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    WorkingMode,

    /// <summary>
    /// Время полного хода арматуры, в сек.
    /// </summary>
    [RegisterConfiguration(0x2716, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    FullStepTime,

    /// <summary>
    /// Флаги аварии [0 - МТЗ; 1 - Аварийный стоп; 2 - Обрыв фазы L1; 3 - Обрыв фазы L2; 4 - Обрыв фазы L3; 5 - Umax; 6 - Umin; 7 - Частота сети;
    /// 8 - Несимметрия напряжений; 9 - Несимметрия токов; 11 - Температура пускателя; 12 - Температура двигателя; 13 - КЗ PTC;
    /// 14 - Обрыв аналогового входа №1; 15 - - Обрыв аналогового входа №2; 16 - Нет нагрузки; 17 - Неисправность силовой схемы; 18 - Положение]
    /// </summary>
    [RegisterConfiguration(0x2717, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    Emergencies,

    /// <summary>
    /// Управление арматурой [0 - Стоп; 1 - Вниз; 2 - Вверх]
    /// </summary>
    [RegisterConfiguration(0x2719, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    ValveControl,

    /// <summary>
    /// Безопасное положение арматуры [0 - Открыто; 1 - Закрыто]
    /// </summary>
    [RegisterConfiguration(0x271A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SafePosition,

    /// <summary>
    /// Тип сигнала Аналогового входа №1 [0 - Откл.; 1 - 0..1В; 2 - 0..10В; 3 - 0..20мА; 4 - 4..20мА]
    /// </summary>
    [RegisterConfiguration(0x271B, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SignalTypeAnalogInput1,

    /// <summary>
    /// Тип сигнала Аналогового входа №2 [0 - Откл.; 1 - 0..1В; 2 - 0..10В; 3 - 0..20мА; 4 - 4..20мА]
    /// </summary>
    [RegisterConfiguration(0x271C, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    SignalTypeAnalogInput2,

    /// <summary>
    /// Номинальное значение напряжения и тип сети [0 - 230В однофазный; 1 - 400В 3-х фазный; 2 - 230В 3-х фазный]
    /// </summary>
    [RegisterConfiguration(0x271D, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    OperationVoltageType,

    /// <summary>
    /// Допустимое отрицательное отклонение напряжения сети, в % [0-100]
    /// </summary>
    [RegisterConfiguration(0x271E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    NegativeVoltageOffset,

    /// <summary>
    /// Допустимое положительное отклонение напряжения сети, в % [0-100]
    /// </summary>
    [RegisterConfiguration(0x271F, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PositiveVoltageOffset,

    /// <summary>
    /// Допустимое отклонение частоты напряжения сети, в Гц
    /// </summary>
    [RegisterConfiguration(0x2720, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VoltageFrequencyOffset,

    /// <summary>
    /// Тип определения перегрева двигателя [0 - По току; 1 - PTC]
    /// </summary>
    [RegisterConfiguration(0x2722, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    EngineOverheatDetectionType,

    /// <summary>
    /// Номинальный ток двигателя, А
    /// </summary>
    [RegisterConfiguration(0x2723, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    EngineCurrent,

    /// <summary>
    /// Уставка тока дожима, кратность тока дожима относительно номинального
    /// </summary>
    [RegisterConfiguration(0x2725, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    OverstepCurrent,

    /// <summary>
    /// Токовая отсечка, кратность максимального тока в работе относительно номинального
    /// </summary>
    [RegisterConfiguration(0x272A, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaxCurrent,

    /// <summary>
    /// Тип датчика положения
    /// </summary>
    [RegisterConfiguration(0x272C, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    PositionSensorType,

    /// <summary>
    /// Вход управления [0 - DI4/DI5; 1 - AI1; 2 - RS/Eth]
    /// </summary>
    [RegisterConfiguration(0x272D, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    InputType,

    /// <summary>
    /// Допустимая несимметрия токов нагрузки, в % [0-100]
    /// </summary>
    [RegisterConfiguration(0x272E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    CurrentAssymetry,

    /// <summary>
    /// Допустимая несимметрия напряжений питающей сети, в % [0-100]
    /// </summary>
    [RegisterConfiguration(0x272E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    VoltageAssymetry,

    /// <summary>
    /// Дожим [0 - Включено по DI; 1 - Включено всегда]
    /// </summary>
    [RegisterConfiguration(0x2730, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    Overstep,

    /// <summary>
    /// Критерий дожима [0 - По току; 1 - По МВ]
    /// </summary>
    [RegisterConfiguration(0x2731, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    OverstepType,

    /// <summary>
    /// Уровень логической единицы DI [0 - Высокий; 1 - Низкий]
    /// </summary>
    [RegisterConfiguration(0x2732, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    DigitalInputLogicValue,

    /// <summary>
    /// Состояние PTC [0 - Норма; 1 - Перегрев; 2 - КЗ]
    /// </summary>
    [RegisterConfiguration(0x2733, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    StatePTC,

    /// <summary>
    /// Положение/Величина закрытия арматуры, в % [0-100]
    /// </summary>
    [RegisterConfiguration(0x2745, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    PositionPercent,

    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x07D1, 8, RegisterDataType.ByteArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    EventName,

    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x07D9, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    EventsCode, // Код событий
}