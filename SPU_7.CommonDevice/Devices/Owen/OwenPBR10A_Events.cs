using System.ComponentModel;

namespace SPU_7.CommonDevice.Devices.Owen;

public enum OwenPBR10A_Events
{
    /// <summary>
    /// Включение контроллера
    /// </summary>
    [Description("Включение")]
    PowerOn = 0,
    /// <summary>
    /// Изменение настроек контроллера
    /// </summary>
    [Description("Изменение настроек")]
    SettingsChange = 1,
    /// <summary>
    /// Калибровка хода
    /// </summary>
    [Description("Калибровка хода")]
    Calibration = 2,
    /// <summary>
    /// Безопасное положение арматуры
    /// </summary>
    [Description("Безопасное положение")]
    SafePosition = 5,
    /// <summary>
    /// Достижение концевых положений
    /// </summary>
    [Description("Концевое положение")]
    EndPosition = 6,
    /// <summary>
    /// Запуск запорной арматуры
    /// </summary>
    [Description("Пуск")]
    Run = 7,
    /// <summary>
    /// Останов запорной арматуры
    /// </summary>
    [Description("Стоп")]
    Stop = 8,
    /// <summary>
    /// Дожим запорной арматуры
    /// </summary>
    [Description("Дожим")]
    RunOver = 9,
    /// <summary>
    /// Сброс аварий
    /// </summary>
    [Description("Сброс аварии")]
    EmergencyClear = 10,
    /// <summary>
    /// Блокировка защит - кроме Аварийного стопа и Безопасного положения
    /// </summary>
    [Description("Блокировка защит")]
    ProtectionBlock = 11,
    /// <summary>
    /// Отклонение частоты питающей сети
    /// </summary>
    [Description("Отклонение частоты питающей сети")]
    FrequencyOutOfRange = 67,
    /// <summary>
    /// Отклонение напряжения питающей сети
    /// </summary>
    [Description("Отклонение напряжения питающей сети")]
    VoltageOutOfRange = 68,
    /// <summary>
    /// Несимметрия напряжений питающей сети
    /// </summary>
    [Description("Несимметрия напряжений питающей сети")]
    VoltageSymmetryOutOfRange = 69,
    /// <summary>
    /// Несимметрия токов двигателя
    /// </summary>
    [Description("Несимметрия токов двигателя")]
    CurrentSymmetryOutOfRange = 70,
    /// <summary>
    /// Перегрев двигателя
    /// </summary>
    [Description("Перегрев двигателя")]
    EngineTemperatureOutOfRange = 71,
    /// <summary>
    /// Ошибка позиционирования запорной арматуры
    /// </summary>
    [Description("Ошибка позиционирования запорной арматуры")]
    Position = 72,
    /// <summary>
    /// Обрыв фаз питающей сети
    /// </summary>
    [Description("Обрыв фаз питающей сети")]
    PhaseDisconnect = 74,
    /// <summary>
    /// Максимальная токовая защита
    /// </summary>
    [Description("Максимальная токовая защита")]
    MaxCurrentProtection = 75,
    /// <summary>
    /// Перегрев пускателя
    /// </summary>
    [Description("Перегрев пускателя")]
    RunnerTemperatureOutOfRange = 76,
    /// <summary>
    /// Нагрузка отсутствует
    /// </summary>
    [Description("Нагрузка отсутствует")]
    NoLoad = 78,
    /// <summary>
    /// Неисправность силовой схемы
    /// </summary>
    [Description("Неисправность силовой схемы")]
    LoadSchemeDamage = 80,
    /// <summary>
    /// КЗ PTC-датчика двигателя
    /// </summary>
    [Description("КЗ PTC-датчика двигателя")]
    TermoSensorShortCurrent = 83,
    /// <summary>
    /// Аварийная остановка
    /// </summary>
    [Description("Аварийный стоп")]
    EmergencyStop = 84,
    /// <summary>
    /// Обрыв аналогового сигнала по входу №1
    /// </summary>
    [Description("Обрыв аналогового сигнала по входу №1")]
    AnalogInput1Cutoff = 86,
    /// <summary>
    /// Обрыв аналогового сигнала по входу №2
    /// </summary>
    [Description("Обрыв аналогового сигнала по входу №2")]
    AnalogInput2Cutoff = 87,
}