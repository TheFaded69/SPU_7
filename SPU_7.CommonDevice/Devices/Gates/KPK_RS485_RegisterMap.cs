using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Gates;

public enum KPK_RS485_RegisterMap
{
    #region Параметры

    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x1002, 2, RegisterDataType.UInt32)]
    UnitID,

    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x1006, 10, RegisterDataType.CharArray)]
    DeviceNumber,

    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x1010, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteSingleRegister)]
    UsePreambule,

    /// <summary>
    /// 
    /// </summary>
    [RegisterConfiguration(0x1011, 1, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteSingleRegister)]
    Preambule,

    #endregion

    #region Параметры USB

    /// <summary>
    /// Протокол передачи данных
    /// </summary>
    [RegisterConfiguration(0x2002, 2, RegisterDataType.UInt32)]
    UsbPortProtocol,

    /// <summary>
    /// Тип подключённого оборудования
    /// </summary>
    [RegisterConfiguration(0x2004, 2, RegisterDataType.UInt32)]
    UsbPortHardwareType,

    /// <summary>
    /// Адрес на шине
    /// </summary>
    [RegisterConfiguration(0x2006, 2, RegisterDataType.UInt32)]
    UsbPortBusAddress,

    /// <summary>
    /// Скорость порта
    /// </summary>
    [RegisterConfiguration(0x2008, 2, RegisterDataType.UInt32)]
    UsbPortSpeed,

    /// <summary>
    /// Кол-во стоп-бит
    /// </summary>
    [RegisterConfiguration(0x200A, 2, RegisterDataType.UInt32)]
    UsbPortStopBits,

    /// <summary>
    /// Проверка чётности
    /// </summary>
    [RegisterConfiguration(0x200C, 2, RegisterDataType.UInt32)]
    UsbPortParity,

    /// <summary>
    /// Таймаут между символами
    /// </summary>
    [RegisterConfiguration(0x200E, 2, RegisterDataType.UInt32)]
    UsbPortSymbolsTimeout,

    /// <summary>
    /// Таймаут ожидания ответа
    /// </summary>
    [RegisterConfiguration(0x2010, 2, RegisterDataType.UInt32)]
    UsbPortResponseTimeout,

    /// <summary>
    /// Пауза между запросами
    /// </summary>
    [RegisterConfiguration(0x2012, 2, RegisterDataType.UInt32)]
    UsbPortRequestPause,

    #endregion

    #region Настройки портов

    /// <summary>
    /// Режим работы внешнего устройства
    /// </summary>
    [RegisterConfiguration(0x2100, 2, RegisterDataType.UInt32)]
    FirstPortWorkingMode,

    /// <summary>
    /// Протокол передачи данных
    /// </summary>
    [RegisterConfiguration(0x2102, 2, RegisterDataType.UInt32)]
    FirstPortProtocol,

    /// <summary>
    /// Тип подключённого оборудования
    /// </summary>
    [RegisterConfiguration(0x2104, 2, RegisterDataType.UInt32)]
    FirstPortHardwareType,

    /// <summary>
    /// Адрес на шине
    /// </summary>
    [RegisterConfiguration(0x2106, 2, RegisterDataType.UInt32)]
    FirstPortBusAddress,

    /// <summary>
    /// Скорость порта
    /// </summary>
    [RegisterConfiguration(0x2108, 2, RegisterDataType.UInt32)]
    FirstPortSpeed,

    /// <summary>
    /// Кол-во стоп-бит
    /// </summary>
    [RegisterConfiguration(0x210A, 2, RegisterDataType.UInt32)]
    FirstPortStopBits,

    /// <summary>
    /// Проверка чётности
    /// </summary>
    [RegisterConfiguration(0x210C, 2, RegisterDataType.UInt32)]
    FirstPortParity,

    /// <summary>
    /// Таймаут между символами
    /// </summary>
    [RegisterConfiguration(0x210E, 2, RegisterDataType.UInt32)]
    FirstPortSymbolsTimeout,

    /// <summary>
    /// Таймаут ожидания ответа
    /// </summary>
    [RegisterConfiguration(0x2110, 2, RegisterDataType.UInt32)]
    FirstPortResponseTimeout,

    /// <summary>
    /// Пауза между запросами
    /// </summary>
    [RegisterConfiguration(0x2112, 2, RegisterDataType.UInt32)]
    FirstPortRequestPause,

    /// <summary>
    /// Преамбула
    /// </summary>
    [RegisterConfiguration(0x2114, 2, RegisterDataType.UInt16, ModbusFunction.ReadInputRegisters, ModbusFunction.WriteSingleRegister)]
    FirstPortPreambule,

    #endregion
}