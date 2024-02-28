using SPU_7.Modbus.Attributes;

namespace SPU_7.Modbus.Requests;

/// <summary>
/// Заголовок, который добавляется к пакету данных в протоколе <b>ModbusTCP</b>
/// </summary>
public class MbapHeader
{
    /// <summary>
    /// Идентификатор транзакции (TCP)
    /// </summary>
    [SerializationOrder(0)]
    public ushort TransactionId { get; set; }

    /// <summary>
    /// Идентификатор протокола (TCP)
    /// </summary>
    [SerializationOrder(1)]
    public ushort ProtocolId { get; set; }

    /// <summary>
    /// Количество последующих байт
    /// </summary>
    [SerializationOrder(2)]
    public ushort Length { get; set; }
}