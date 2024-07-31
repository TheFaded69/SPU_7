using SPU_7.DeviceCommunication.Modbus.Attributes;
using SPU_7.DeviceCommunication.Modbus.Enums;

namespace SPU_7.CommonDevice.Devices.Owen;

public enum OwenDTH_RegisterMap
{
    /// <summary>
    /// Название датчика
    /// </summary>
    //[RegisterConfiguration(0x03E8, 3, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    //Name,

    /// <summary>
    /// Версия ПО
    /// </summary>
    //[RegisterConfiguration(0x03EE, 3, RegisterDataType.CharArray, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    //SoftwareVersion,

    /// <summary>
    /// Состояние прибора
    /// </summary>
    [RegisterConfiguration(0x0514, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    DeviceState,

    /// <summary>
    /// Восстановить заводские сетевые настройки
    /// </summary>
    [RegisterConfiguration(0x0578, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    RestoreFactoryNetSettings,

    /// <summary>
    /// Записать параметры во flash память
    /// </summary>
    [RegisterConfiguration(0x057A, 1, RegisterDataType.UInt16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    WriteParametersToFlash,

    /// <summary>
    /// Перезагрузить прибор
    /// </summary>
    [RegisterConfiguration(0x057B, 1, RegisterDataType.UInt16, ModbusFunction.None, ModbusFunction.WriteSingleRegister)]
    Reboot,

    /// <summary>
    /// Значение температуры, °C
    /// </summary>
    [RegisterConfiguration(0x0898, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    Temperature,

    /// <summary>
    /// Верхний предел измерений, °C
    /// </summary>
    [RegisterConfiguration(0x14B6, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    HighMeasureBound,

    /// <summary>
    /// Нижний предел измерений, °C
    /// </summary>
    [RegisterConfiguration(0x14B8, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    LowMeasureBound,

    /// <summary>
    /// Верхний предел, °C
    /// </summary>
    [RegisterConfiguration(0x14BA, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MaxSensorTemperature,

    /// <summary>
    /// Нижний предел, °C
    /// </summary>
    [RegisterConfiguration(0x14BC, 2, RegisterDataType.Float, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteMultipleRegisters)]
    MinSensorTemperature,

    /// <summary>
    /// Постоянная фильтра [0 - отключен, или диапазон от 1 до 10]
    /// </summary>
    [RegisterConfiguration(0x14BE, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    FilterConst,

    /// <summary>
    /// Тип датчика [0 - 50М; 1 - 100М; 2 - 100П; 3 - Pt 100; 4 - Pt 1000; 5 - TXK (L); 6 - THH (N); 7 - TXA (K)]
    /// </summary>
    [RegisterConfiguration(0x14C1, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    SensorType,

    /// <summary>
    /// Тип протокола обмена [1 - Modbus RTU]
    /// </summary>
    [RegisterConfiguration(0x15E1, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    ProtocolType,

    /// <summary>
    /// Адрес устройства [1..16..247]
    /// </summary>
    [RegisterConfiguration(0x15E2, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    UnitId,

    /// <summary>
    /// Скорость обмена [0 - 9600; 1 - 14400; 2 - 19200; 3 - 38400; 4 - 57600; 5 - 115200]
    /// </summary>
    [RegisterConfiguration(0x15E3, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    BaudRate,

    /// <summary>
    /// Бит данных [8]
    /// </summary>
    [RegisterConfiguration(0x15E4, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.None)]
    DataBits,

    /// <summary>
    /// Контроль чётности [0 - None; 1 - Even; 2 - Odd]
    /// </summary>
    [RegisterConfiguration(0x15E5, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    Parity,

    /// <summary>
    /// Кол-во стоп бит [0 - 1; 1 - 1,5; 2 - 2]
    /// </summary>
    [RegisterConfiguration(0x15E6, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    StopBits,

    /// <summary>
    /// Применить новые сетевые параметры, [0;1]
    /// </summary>
    [RegisterConfiguration(0x15EB, 1, RegisterDataType.Int16, ModbusFunction.ReadHoldingRegisters, ModbusFunction.WriteSingleRegister)]
    ApplyNetworkSettings,
}