using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;

public enum OwenIC30_RegisterMap
{
    #region Настраиваемые параметры

    /// <summary>
    /// Уставка №1
    /// </summary>
    [RegisterConfiguration(0x000C, 2, RegisterDataType.Int32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    U1 = 0xFA1E,

    /// <summary>
    /// Уставка №2
    /// </summary>
    [RegisterConfiguration(0x000E, 2, RegisterDataType.Int32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    U2 = 0x9707,

    /// <summary>
    /// Режим счётча, 0 - Прямой, 1 - Обратный, 2 - Командный, 3 - Индивидуальный, 4 - Реверсивный, 5 - Квадратурный
    /// </summary>
    [RegisterConfiguration(0x0008, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    CounterMode = 0x38D9,

    /// <summary>
    /// Режим вывода, 0 - Включено после уставки, 1 - Включено до уставки, 2 - Включено на время после уставки, 3 - Включено на время при кратных уставке значениях
    /// </summary>
    [RegisterConfiguration(0x0009, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    OutputMode = 0xD11F,

    /// <summary>
    /// Временной отрезок для ВУ1, от 0 до 999990 мс, default(1000)
    /// </summary>
    [RegisterConfiguration(0x0010, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    T1 = 0x2E75,

    /// <summary>
    /// Временной отрезок для ВУ2, от 0 до 999990 мс, default(1000)
    /// </summary>
    [RegisterConfiguration(0x0012, 2, RegisterDataType.UInt32, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    T2 = 0x436C,

    /// <summary>
    /// Положение десятичной точки множителя FdP, default(0)
    /// </summary>
    [RegisterConfiguration(0x0014, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    DotPositionFdP = 0x6ABF,

    // ...

    /// <summary>
    /// Блокировка кнопок, 0 - Кнопки разблокированы, 1 - Заблокирован срос счётчика, 2 - Заблокировано изменение уставок, 3 - Заблокированы сброс и изменение уставок
    /// </summary>
    [RegisterConfiguration(0x001A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    PanelLock = 0xE954,

    /// <summary>
    /// Пароль (Только для блокировки ручного ввода), от 0000 до 9999, default(0000) - нет пароля
    /// Примечание: Если забыли свой пароль, войти в режим настроек можно с помощью пароля 1098! При вводе этого
    /// значения пароль будет сброшен в 0000. Поэтому не рекомендуется устанавливать значение пароля,
    /// равным 1098, так как в данном случае при каждом вводе пароля его значение будет обнуляться
    /// </summary>
    [RegisterConfiguration(0x001E, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    Password = 0x2728,

    // ...

    #endregion

    #region Настройки RS-485

    /// <summary>
    /// Скорость, 0 – 2400, 1 – 4800, 2 – 9600, 3 – 14400, 4 – 19200, 5 – 28800, 6 – 38400, 7 – 57600, 8 – 115200, default(9600)
    /// </summary>
    [RegisterConfiguration(0x0000, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    BaudRate = 0xB760,

    /// <summary>
    /// Бит данных в байте, 0 – 7 бит (7 bit), 1 – 8 бит (8 bit), default(8 бит)
    /// </summary>
    [RegisterConfiguration(0x0001, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    DataBits = 0x523F,

    /// <summary>
    /// Проверка чётности, 0 – Без паритета (NO), 1 – Четный паритет Без паритета (EVEN), 2 – Нечетный паритет (Odd), default(NO)
    /// </summary>
    [RegisterConfiguration(0x0002, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    Parity = 0xE8C4,

    /// <summary>
    /// Кол-во Стоп бит, 0 – 1 стоп-бит, 1 – 2 стоп-бита, default(1 стоп-бит)
    /// </summary>
    [RegisterConfiguration(0x0003, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    StopBits = 0xB72E,

    /// <summary>
    /// Длина сетевого адреса, 0 – 8 бит, 1 – 11 бит, default(0) 
    /// </summary>
    [RegisterConfiguration(0x0004, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    AddressLength = 0x1ED2,

    /// <summary>
    /// Базовый адрес прибора, default(16)
    /// </summary>
    [RegisterConfiguration(0x0005, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    BaseAddress = 0x9F62,

    /// <summary>
    /// Задержка ответа, от 0 до 45, default(0)
    /// </summary>
    [RegisterConfiguration(0x0006, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ResponseDelay = 0xCBF5,

    #endregion

    #region Считываемые параметры

    /// <summary>
    /// Текущее значение счётчика импульсов Ctr
    /// </summary>
    [RegisterConfiguration(0x0000, 2, RegisterDataType.Int32, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    ImpulseCount = 0xA158,

    /// <summary>
    /// Текущее значение счётчика в физических единицах CEU
    /// </summary>
    [RegisterConfiguration(0x0002, 2, RegisterDataType.Int32, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    CounterUnits = 0xB8BC,

    /// <summary>
    /// Текущее состояние, 0 - Старт, 1 - Стоп
    /// </summary>
    [RegisterConfiguration(0x0004, 1, RegisterDataType.Int16, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    CurrentState = 0x6577,

    /// <summary>
    /// Текущий режим, 0 – счет (пароль не требуется), 1 – настройка с клавиатуры, 2 – настройка с ПК, 3 – счет (требуется пароль)
    /// </summary>
    [RegisterConfiguration(0x0005, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    CurrentMode = 0xCC41,

    /// <summary>
    /// Код сетевой ошибки при последнем обращении, От 0 до 255. После включения прибора – 0
    /// </summary>
    [RegisterConfiguration(0x0006, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    LastNetError = 0x0233,

    /// <summary>
    /// Название прибора, Строка ASCII (4 байта)
    /// </summary>
    [RegisterConfiguration(0x0007, 2, RegisterDataType.ByteArray, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    DeviceName = 0xD681,

    /// <summary>
    /// Версия ПО, Строка ASCII (4 байта)
    /// </summary>
    [RegisterConfiguration(0x0009, 2, RegisterDataType.ByteArray, ModbusFunction.ReadInputRegisters, ModbusFunction.None)]
    SoftwareVersion = 0x2D5B,

    #endregion

}
