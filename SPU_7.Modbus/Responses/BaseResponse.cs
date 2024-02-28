using SPU_7.Modbus.Attributes;

namespace SPU_7.Modbus.Responses;

/// <summary>
/// <para>Базовый класс ответов.</para>
/// <para>Атрибут <see cref="SerializationOrderAttribute"/> обязателен, определяет порядок сериализации свойств.</para>
/// <para>Атрибут <see cref="ByteOrderAttribute"/> необязателен, определяет порядок байт в исходных данных.</para>
/// </summary>
public abstract class BaseResponse
{
    /// <summary>
    /// адрес устройства
    /// </summary>
    [SerializationOrder(0)]
    public byte DeviceAddress { get; set; }

    /// <summary>
    /// команда или ошибка, если старший бит в 1
    /// </summary>
    [SerializationOrder(1)]
    public byte FunctiomalCode { get; set; }

    /// <summary>
    /// Проверяет, является ли ответ ответом-ошибкой
    /// </summary>
    /// <returns>true - если ответ является ошибкой</returns>
    public bool IsResponseError() => (FunctiomalCode & 0x80) > 0;

    /// <summary>
    /// Возвращает функциональный код, если ответ является ошибкой
    /// (обнуляет старший бит)
    /// </summary>
    /// <returns></returns>
    public byte ExtractFunctiomalCode() => (byte)(FunctiomalCode & 0x7F);
}