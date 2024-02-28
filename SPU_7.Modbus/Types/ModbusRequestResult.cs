namespace SPU_7.Modbus.Types;

public enum ModbusRequestResult
{
    /// <summary>
    /// Все OK
    /// </summary>
    Success,

    /// <summary>
    /// Ответ получен, но ошибка на уровне протокола Modbus (Exception Code)
    /// </summary>
    SuccessError,

    /// <summary>
    /// Не удалось отправить запрос (например, неправильно указан порт)
    /// </summary>
    FailedDataTransmission,

    /// <summary>
    /// Не далось получить ответ
    /// </summary>
    FailedDataReception,

    /// <summary>
    /// неверный CRC
    /// </summary>
    // ReSharper disable once InconsistentNaming
    WrongCRC,
}