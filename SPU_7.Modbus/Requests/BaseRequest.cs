using SPU_7.Modbus.Attributes;

namespace SPU_7.Modbus.Requests;

/// <summary>
/// <para>Базовый класс запросов.</para>
/// <para>Атрибут <see cref="SerializationOrderAttribute"/> обязателен. Определяет порядок сериализации свойств.</para>
/// <para>Атрибут <see cref="ByteOrderAttribute"/> необязателен. Определяет порядок байтов после сериализации.</para>
/// <para>Атрибут <see cref="SerializationIgnoredAttribute"/> необязателен. Данное свойство будет игнорироваться сериализатором</para>
/// </summary>
public abstract class BaseRequest
{
    /// <summary>
    /// адрес устройства
    /// </summary>
    [SerializationOrder(0)]
    public byte DeviceAddress { get; set; }

    /// <summary>
    /// команда
    /// </summary>
    [SerializationOrder(1)]
    public byte FunctiomalCode { get; protected set; }

    /// <summary>
    /// размер ответа на запрос
    /// </summary>
    /// <returns></returns>
    public abstract int GetResponseSize();

    /// <summary>
    /// Включен ли режим моста
    /// </summary>
    [SerializationIgnored]
    public BridgeMode BridgeMode { get; set; }

    /// <summary>
    /// Запрос без запроса - ожидаем сразу ответ (при установке входящего
    /// соединения по TCP)
    /// </summary>
    [SerializationIgnored]
    public bool IsNoRequest { get; set; }
}