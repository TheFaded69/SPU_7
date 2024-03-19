using SPU_7.Common.Modbus;
using SPU_7.Domain.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Domain.Devices.MasterDevice.GFG;

public enum GFGRegisterMap
{
    /// <summary>
    /// Расход стандартный
    /// </summary>
    [RegisterSetup(0x0000, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    StandardFlowRegister,

    /// <summary>
    /// Расход рабочий
    /// </summary>
    [RegisterSetup(0x0002, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    VolumeFlowRegister,

    #region Специальные регистры (технологические)

    /// <summary>
    /// НС
    /// </summary>
    [RegisterSetup(0x0014, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    EmergencyRegister,

    /// <summary>
    /// Внутренний (заводской) номер
    /// </summary>
    [RegisterSetup(0x105F, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    VendorNumber,

    /// <summary>
    /// Дата/Время прибора
    /// </summary>
    [RegisterSetup(0x100A, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 4, RegisterDataType.TDateTime, ByteOrderType.BigEndian_ABCD)]
    SystemTime,

    /// <summary>
    /// Регистр управления
    /// </summary>
    [RegisterSetup(0x1014, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    ControlRegister,

    /// <summary>
    /// Режим "моста" с ПП (Управление связью с ПП)
    /// </summary>
    [RegisterSetup(0x105E, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    BridgeMode,

    /// <summary>
    /// Сетевой адрес ПП
    /// </summary>
    [RegisterSetup(0x1013, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    PrimaryConverterAddress,

    /// <summary>
    /// Мин. частота выхода 1, Гц
    /// </summary>
    [RegisterSetup(0x1019, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    FrequencyOut1Min,

    /// <summary>
    /// Макс. частота выхода 1, Гц
    /// </summary>
    [RegisterSetup(0x101B, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    FrequencyOut1Max,

    /// <summary>
    /// Макс. рабочий расход 1, м3/ч ( Максимальное значение переменной частотного выхода 1, м3/ч)
    /// </summary>
    [RegisterSetup(0x101D, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    VolumeFlowOut1Max,

    /// <summary>
    /// Мин. частота выхода 2, Гц
    /// </summary>
    [RegisterSetup(0x1029, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    FrequencyOut2Min,

    /// <summary>
    /// Макс. частота выхода 2, Гц
    /// </summary>
    [RegisterSetup(0x102B, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    FrequencyOut2Max,

    /// <summary>
    /// Макс. рабочий расход 2, м3/ч (Максимальное значение переменной частотного выхода 2, м3/ч)
    /// </summary>
    [RegisterSetup(0x102D, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    VolumeFlowOut2Max,

    /// <summary>
    /// Мин. диапазон рабочего расхода, м3/ч
    /// </summary>
    [RegisterSetup(0x1800, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    VolumeFlowMinRange,

    /// <summary>
    /// Макс. диапазон рабочего расхода, м3/ч
    /// </summary>
    [RegisterSetup(0x1802, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    VolumeFlowMaxRange,

    /// <summary>
    /// Вес импульса для импульсного выхода
    /// </summary>
    [RegisterSetup(0x1818, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    PulseWeight,

    /// <summary>
    /// Значение расхода для тока 4 мА (Минимальное значение переменной, м3\ч)
    /// </summary>
    [RegisterSetup(0x1052, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    CurrentOutMin,

    /// <summary>
    /// Значение расхода для тока 20 мА (Максимальное значение переменной, м3/ч)
    /// </summary>
    [RegisterSetup(0x1054, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    CurrentOutMax,
    
    /// <summary>
    /// Технологический регистр управления
    /// </summary>
    [RegisterSetup(0x1075, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    TechnicalControlRegister,

    #endregion

    #region Регистры специального назначения

    /// <summary>
    /// Флаг подтверждения приема данных
    /// </summary>
    [RegisterSetup(0x1200, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    GprsDataAcceptanceConfirmation,

    /// <summary>
    /// Регистр управления завершением сеанса
    /// </summary>
    [RegisterSetup(0x1201, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    GprsSessionTermination,

    #endregion

    #region Управление паролями

    /// <summary>
    /// Текущий пароль для доступа
    /// </summary>
    [RegisterSetup(0xF000, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    PasswordAccess,

    /// <summary>
    /// Номер для запроса одноразового пароля
    /// </summary>
    [RegisterSetup(0xF01C, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    OneShotPasswordNumber,

    #endregion

    #region Отладка

    /// <summary>
    /// Пароль для разрешения регистров теста
    /// </summary>
    [RegisterSetup(0x5000, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    DebugPasswordAccess,

    /// <summary>
    /// Тест клавиатуры
    /// </summary>
    [RegisterSetup(0x5002, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    KeyboardTest,

    /// <summary>
    /// Код последней нажатой кнопки(сбрасывается в ноль при вычитке)
    /// </summary>
    [RegisterSetup(0x5004, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    LastPressedKey,

    /// <summary>
    /// Управление блютуз
    /// </summary>
    [RegisterSetup(0x5006, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    BluetoothControl,

    /// <summary>
    /// Статус блютуз
    /// </summary>
    [RegisterSetup(0x5008, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    BluetoothStatus,

    /// <summary>
    /// Управление модемом
    /// </summary>
    [RegisterSetup(0x500A, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    ModemControl,

    /// <summary>
    /// Текущий статус модема
    /// </summary>
    [RegisterSetup(0x500C, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    ModemStatus,

    /// <summary>
    /// Управление LCD
    /// </summary>
    [RegisterSetup(0x500E, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    LcdControl,

    /// <summary>
    /// Нажатые кнопки на момент чтения (только при включенной отладке)
    /// </summary>
    [RegisterSetup(0x5012, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    KeysStatus,

    /// <summary>
    /// Маска управления режимом отладки
    /// </summary>
    [RegisterSetup(0x5100, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    DebugMode,

    /// <summary>
    /// Отладочное значение рабочего расхода, м3/час
    /// </summary>
    [RegisterSetup(0x5102, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    DebugFlowValue,

    /// <summary>
    /// Отладочное значение текущей температуры, 0С
    /// </summary>
    [RegisterSetup(0x5104, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    DebugTemperatureValue,

    /// <summary>
    /// Отладочное значение текущего абсолютного давления, МПа
    /// </summary>
    [RegisterSetup(0x5106, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.Float, ByteOrderType.BigEndian_ABCD)]
    DebugAbsolutePressureValue,

    #endregion

    #region Bluetooth

    /// <summary>
    /// Имя блютуз модуля
    /// </summary>
    [RegisterSetup(0x1039, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.BigEndian_ABCD)]
    BluetoothName,
    
    /// <summary>
    /// Адрес блютуз модуля
    /// </summary>
    [RegisterSetup(0x00AF, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.BigEndian_ABCD)]
    BluetoothAddress,

    #endregion

    #region Модем

    /// <summary>
    /// Состояние SIM карты
    /// </summary>
    [RegisterSetup(0x008C, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    SimStatus,
    
    /// <summary>
    /// Версия модема
    /// </summary>
    [RegisterSetup(0x008B, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    ModemVersion,
    
    /// <summary>
    /// IP адрес сервера основной
    /// </summary>
    [RegisterSetup(0x4028, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.IpV4, ByteOrderType.BigEndian_ABCD)]
    ServerIpAddress,
    
    /// <summary>
    /// IP порт основной
    /// </summary>
    [RegisterSetup(0x402A, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    ServerPort,
    
    /// <summary>
    /// Телефон для CSD основной
    /// </summary>
    [RegisterSetup(0x402E, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 10, RegisterDataType.CharArray, ByteOrderType.BigEndian_ABCD)]
    CsdPhone,
    
    /// <summary>
    /// Таймаут соединения, мин
    /// </summary>
    [RegisterSetup(0x4043, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    ConnectionTimeout,

    /// <summary>
    /// Повтор выхода на связь 1
    /// </summary>
    [RegisterSetup(0x4070, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.TConnection, ByteOrderType.BigEndian_ABCD)]
    Connection1Setup,
    
    /// <summary>
    /// Способ выхода на связь 1
    /// </summary>
    [RegisterSetup(0x4072, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    Connection1Mode,
    
    /// <summary>
    /// Данные, которые передаются при выходе на связь 1 по событиям (битовая маска)
    /// </summary>
    [RegisterSetup(0x4073, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 2, RegisterDataType.UInt32, ByteOrderType.BigEndian_ABCD)]
    Connection1Data,
    
    #endregion

    #region Архивы данных

    /// <summary>
    /// Флаг архива
    /// </summary>
    [RegisterSetup(0x203A, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters, 1, RegisterDataType.UInt16, ByteOrderType.BigEndian_ABCD)]
    ArchiveFlag,

    #endregion
}