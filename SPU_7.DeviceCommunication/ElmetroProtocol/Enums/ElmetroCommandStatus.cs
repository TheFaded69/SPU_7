using System.ComponentModel;

namespace SPU_7.DeviceCommunication.ElmetroProtocol.Enums;

public enum ElmetroCommandStatus : byte
{
    /// <summary>
    /// Команда выполнена успешно
    /// </summary>
    [Description("Команда выполнена успешно")]
    Success = 0x00,

    /// <summary>
    /// Ошибочное значение входных параметров
    /// </summary>
    [Description("Ошибочное значение входных параметров")]
    InputError = 0x03,

    /// <summary>
    /// Ошибочное количество входных параметров
    /// </summary>
    [Description("Ошибочное количество входных параметров")]
    InputCountError = 0x05,

    /// <summary>
    /// Ошибка выполнения команды в устройстве
    /// </summary>
    [Description("Ошибка выполнения команды в устройстве")]
    DeviceError = 0x06,

    /// <summary>
    /// Выполнение команды запрещено
    /// </summary>
    [Description("Выполнение команды запрещено")]
    Forbidden = 0x10,

    /// <summary>
    /// Команда не поддерживается
    /// </summary>
    [Description("Команда не поддерживается")]
    NotSupported = 0x40
}
