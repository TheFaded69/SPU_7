using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Requests;

/// <summary>
/// Команда записи регистров с последующим чтением регистров (<b>0x17</b>)
/// </summary>
public class ReadWriteMultipleRegistersRequest : BaseRequest
{
    /// <param name="deviceAddress">Адрес устройства</param>
    /// <param name="readStartingAddress">Начальный адрес регистров для чтения</param>
    /// <param name="quantityOfRegistersToRead">Количество регистров для чтения</param>
    /// <param name="writeStartingAddress">Начальный адрес регистров для записи</param>
    /// <param name="writeRegisterValues">массив значений регистров для записи</param>
    public ReadWriteMultipleRegistersRequest(byte deviceAddress, ushort readStartingAddress, ushort quantityOfRegistersToRead, ushort writeStartingAddress,
       byte[] writeRegisterValues)
    {
        DeviceAddress = deviceAddress;
        FunctiomalCode = (byte)FunctiomalCodeEnum.ReadWriteMultipleRegisters;
        ReadStartingAddress = readStartingAddress;
        QuantityToRead = quantityOfRegistersToRead;
        WriteStartingAddress = writeStartingAddress;
        WriteRegisterValues = writeRegisterValues;
        QuantityToWrite = (ushort)(writeRegisterValues.Length / 2);
        WriteByteCount = (byte)writeRegisterValues.Length;
    }

    /// <summary>
    /// Начальный адрес регистров для чтения
    /// </summary>
    [SerializationOrder(2)]
    public ushort ReadStartingAddress { get; }

    /// <summary>
    /// Количество регистров для чтения
    /// </summary>
    [SerializationOrder(3)]
    public ushort QuantityToRead { get; set; }

    /// <summary>
    /// Начальный адрес регистров для записи
    /// </summary>
    [SerializationOrder(4)]
    public ushort WriteStartingAddress { get; }

    /// <summary>
    /// Количество регистров для записи
    /// </summary>
    [SerializationOrder(5)]
    public ushort QuantityToWrite { get; set; }

    /// <summary>
    /// Количество байт для записи (<b>2 * <see cref="QuantityToWrite"/></b>)
    /// </summary>
    [SerializationOrder(6)]
    public byte WriteByteCount { get; set; }

    // /// <summary>
    // /// Список значений регистров для записи
    // /// </summary>
    // [SerializationOrder(7)]
    // public List<ushort> WriteRegisterValues { get; }

    /// <summary>
    /// массив байт для записи
    /// </summary>
    [SerializationOrder(7)]
    public byte[] WriteRegisterValues { get; set; }

    public override int GetResponseSize()
    {
        return 3 + QuantityToRead * 2;
    }
}