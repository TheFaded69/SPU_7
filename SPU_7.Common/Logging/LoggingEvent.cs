namespace SPU_7.Common.Logging;

public enum LoggingEvent
{
    None,

    /// <summary>
    /// Какая то ошибка Modbus
    /// </summary>
    ModbusError,

    /// <summary>
    /// какая то ошибка протокола ElmetroPascal
    /// </summary>
    ElmetroPascalError,

    /// <summary>
    /// Старт приложения
    /// </summary>
    ApplicationStart,

    /// <summary>
    /// Завершение приложения
    /// </summary>
    ApplicationStop,

    /// <summary>
    /// Ручное управление
    /// </summary>
    Manual,

    /// <summary>
    /// Старт инициализации оборудования
    /// </summary>
    EquipmentInitializationStart,

    /// <summary>
    /// Инициализация оборудования
    /// </summary>
    EquipmentInitialization,

    /// <summary>
    /// Окончание инициализации оборудования
    /// </summary>
    EquipmentInitializationEnd,

    /// <summary>
    /// Выключение оборудования
    /// </summary>
    EquipmentDisable,

    /// <summary>
    /// оборудование
    /// </summary>
    Equipment,

    /// <summary>
    /// Событие класса <see cref="BaseElmetroPascal"/>
    /// </summary>
    BaseElmetroPascal,

    /// <summary>
    /// Событие БД
    /// </summary>
    DataBase,

    /// <summary>
    /// Событие диспетчера сценариев
    /// </summary>
    ScriptDispatcher,

    /// <summary>
    /// Событие диспетчера сценариев автозадатчика
    /// </summary>
    ScriptDispatcherAutoPressurer,

    /// <summary>
    /// Событие диспетчера сценариев ручного задатчика
    /// </summary>
    ScriptDispatcherManualPressurer,

    /// <summary>
    /// Начало сканирование устройств
    /// </summary>
    DeviceScanningStart,

    /// <summary>
    /// Сканирование устройств
    /// </summary>
    DeviceScanning,

    /// <summary>
    /// Скканирование устройств прервано
    /// </summary>
    DeviceScanningInterrupted,

    /// <summary>
    /// Скканирования устройств завершенр
    /// </summary>
    DeviceScanningEnd,

    /// <summary>
    /// Действия с платой коммутатором
    /// </summary>
    SwitchBoardAction,

    /// <summary>
    /// Подготовка к калибровке начата
    /// </summary>
    PrepareCalibrationStart,

    /// <summary>
    /// Подготовка к калибровке
    /// </summary>
    PrepareCalibration,

    /// <summary>
    /// Подготовка к калибровке окончена
    /// </summary>
    PrepareCalibrationEnd,

    /// <summary>
    /// Калибровка начата
    /// </summary>
    CalibrationStart,

    /// <summary>
    /// Калибровка
    /// </summary>
    Calibration,

    /// <summary>
    /// Калибровка окончена
    /// </summary>
    CalibrationEnd,

    /// <summary>
    /// Обрабокта результатов калибровки начата
    /// </summary>
    PostCalibrationStart,

    /// <summary>
    /// Обрабокта результатов калибровки 
    /// </summary>
    PostCalibration,

    /// <summary>
    /// Обрабокта результатов калибровки окончена
    /// </summary>
    PostCalibrationEnd,

    /// <summary>
    /// Подготовка к докалибровке начата
    /// </summary>
    PrepareReCalibrationStart,

    /// <summary>
    /// Подготовка к докалибровке
    /// </summary>
    PrepareReCalibration,

    /// <summary>
    /// Подготовка к докалибровке окончена
    /// </summary>
    PrepareReCalibrationEnd,

    /// <summary>
    /// Докалибровка начата
    /// </summary>
    ReCalibrationStart,

    /// <summary>
    /// Докалибровка
    /// </summary>
    ReCalibration,

    /// <summary>
    /// Докалибровка окончена
    /// </summary>
    ReCalibrationEnd,

    /// <summary>
    /// Обрабокта результатов докалибровки начата
    /// </summary>
    PostReCalibrationStart,

    /// <summary>
    /// Обрабокта результатов докалибровки 
    /// </summary>
    PostReCalibration,

    /// <summary>
    /// Обрабокта результатов докалибровки окончена
    /// </summary>
    PostReCalibrationEnd,

    /// <summary>
    /// Подготовка к термокомпенсации начата
    /// </summary>
    PrepareTermoCompensationStart,

    /// <summary>
    /// Подготовка к термокомпенсации
    /// </summary>
    PrepareTermoCompensation,

    /// <summary>
    /// Подготовка к термокомпенсации окончена
    /// </summary>
    PrepareTermoCompensationEnd,

    /// <summary>
    /// Термокомпенсация начата
    /// </summary>
    TermoCompensationStart,

    /// <summary>
    /// Термокомпенсация
    /// </summary>
    TermoCompensation,

    /// <summary>
    /// Термокомпенсация окончена
    /// </summary>
    TermoCompensationEnd,

    /// <summary>
    /// Обрабокта результатов термокомпенсации начата
    /// </summary>
    PostTermoCompensationStart,

    /// <summary>
    /// Обрабокта результатов термокомпенсации 
    /// </summary>
    PostTermoCompensation,

    /// <summary>
    /// Обрабокта результатов термокомпенсации окончена
    /// </summary>
    PostTermoCompensationEnd,

    /// <summary>
    /// Подготовка к поверке начата
    /// </summary>
    PrepareValidationStart,

    /// <summary>
    /// Подготовка к поверке
    /// </summary>
    PrepareValidation,

    /// <summary>
    /// Подготовка к поверке окончена
    /// </summary>
    PrepareValidationEnd,

    /// <summary>
    /// Поверка начата
    /// </summary>
    ValidationStart,

    /// <summary>
    /// Поверка 
    /// </summary>
    Validation,

    /// <summary>
    /// Поверка окончена
    /// </summary>
    ValidationEnd,

    /// <summary>
    /// Обрабокта результатов поверки начата
    /// </summary>
    PostValidationStart,

    /// <summary>
    /// Обрабокта результатов поверки 
    /// </summary>
    PostValidation,

    /// <summary>
    /// Обрабокта результатов поверки окончена
    /// </summary>
    PostValidationEnd,
}