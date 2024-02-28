using SPU_7.Modbus.Attributes;
using SPU_7.Modbus.Types;

namespace SPU_7.Modbus.Requests
{
    /// <summary>
    /// Команда записи регистров (<b>0x10</b>)
    /// </summary>
    public class WriteMultipleRegistersRequest : BaseRequest
    {
        /// <param name="deviceAddress">Адрес устройства</param>
        /// <param name="startingAddress">Начальный адрес регистров</param>
        /// <param name="registerValues">массив значений регистров для записи</param>
        public WriteMultipleRegistersRequest(byte deviceAddress, ushort startingAddress, byte[] registerValues)
        {
            DeviceAddress = deviceAddress;
            FunctiomalCode = (byte)FunctiomalCodeEnum.WriteMultipleRegisters;
            StartingAddress = startingAddress;
            // QuantityOfRegisters = (ushort) registerValues.Count;
            // ByteCount = (byte) (registerValues.Count * 2);
            QuantityOfRegisters = (ushort)(registerValues.Length / 2);
            ByteCount = (byte)registerValues.Length;
            RegisterValues = registerValues;
        }

        /// <summary>
        /// Начальный адрес регистров
        /// </summary>
        [SerializationOrder(2)]
        public ushort StartingAddress { get; set; }

        /// <summary>
        /// Количество регистров
        /// </summary>
        [SerializationOrder(3)]
        public ushort QuantityOfRegisters { get; set; }

        /// <summary>
        /// количество байт (<b>2 * <see cref="QuantityOfRegisters"/></b>)
        /// </summary>
        [SerializationOrder(4)]
        public byte ByteCount { get; set; }

        /// <summary>
        /// массив байт для записи
        /// </summary>
        [SerializationOrder(5)]
        public byte[] RegisterValues { get; set; }

        /// <summary>
        /// размер ответа
        /// </summary>
        /// <returns></returns>
        public override int GetResponseSize()
        {
            return 6;
        }
    }
}