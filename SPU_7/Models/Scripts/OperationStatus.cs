using System.ComponentModel;

namespace SPU_7.Models.Scripts;

public enum OperationStatus
{
    /// <summary>
    /// Ожидание запуска
    /// </summary>
    [Description("Ожидание запуска")]
    ExecuteWaiting,

    /// <summary>
    /// Выполнение
    /// </summary>
    [Description("Выполнение")]
    Executing,

    /// <summary>
    /// Окончен успешно
    /// </summary>
    [Description("Окончена успешно")]
    CompletedPassed,

    /// <summary>
    /// Окончен неудачно
    /// </summary>
    [Description("Окончена неудачно")]
    CompletedFailed,

    /// <summary>
    /// Завершено по причине ошибки
    /// </summary>
    [Description("Завершена по причине ошибки")]
    CompletedError,

    /// <summary>
    /// Остановлена
    /// </summary>
    [Description("Операция прервана пользователем")]
    Stop,
}