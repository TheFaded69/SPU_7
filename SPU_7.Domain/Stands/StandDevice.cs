using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Stands
{
    public class StandDevice : ModbusUnitProcessor<StandDeviceRegisterMap>
    {
        public StandDevice(IModbusProcessor modbusProcessor, IRegisterMapEnum<StandDeviceRegisterMap> registerMap, int moduleAddressInt) : base(modbusProcessor, registerMap)
        {
            StandComponentDevices = new List<StandComponentDevice>
            {
               new() { BitNumber = 0 },
               new() { BitNumber = 1 },
               new() { BitNumber = 2 },
               new() { BitNumber = 3 },
               new() { BitNumber = 4 },
               new() { BitNumber = 5 },
               new() { BitNumber = 6 },
               new() { BitNumber = 7 },
               new() { BitNumber = 8 },
               new() { BitNumber = 9 }
            };

            ModuleAddressInt = moduleAddressInt;
            ModuleAddress = (byte)moduleAddressInt;
        }

        /// <summary>
        /// Все подключенные устройства (клапаны, закрывашки, лампочки и т.п. на текущем адресе)
        /// </summary>
        public List<StandComponentDevice> StandComponentDevices { get; set; }

        public int ModuleAddressInt;

        public bool NeedUpdateState { get; set; } = false;

        /// <summary>
        /// Открыть коммуникатор
        /// </summary>
        public void CommunicatorOpen()
        {
            ModbusProcessor.Communicator.Open();
        }

        /// <summary>
        /// Закрыть коммуникатор
        /// </summary>
        public void CommunicatorClose()
        {
            ModbusProcessor.Communicator.Close();
        }

        /// <summary>
        /// Устанавливает состояние бита
        /// </summary>
        /// <param name="bitNumber">Номер бита</param>
        /// <param name="state">Состояние бита</param>
        /// <returns>Результат запроса</returns>
        public async Task<bool> SetBitState(int? bitNumber, bool state, bool withoutWrite = false)
        {
            if (bitNumber == null) return false;
            
            StandComponentDevices[(int)bitNumber].Statement = state;
            NeedUpdateState = true;

            if (withoutWrite) return true;

            return await SetWorkRegisterAsync();

        }
        
        /// <summary>
        /// Считать состояние битов 
        /// </summary>
        /// <returns>Состояние битов (ushort)</returns>
        public async Task<ushort?> GetInfoRegisterAsync()
        {
            return (ushort?)await ReadRegisterAsync(StandDeviceRegisterMap.InfoRegister);
        }
        
        /// <summary>
        /// Считать состояния бита
        /// </summary>
        /// <param name="bitNumber">Номер бита</param>
        /// <returns>Состояние бита</returns>
        /// <exception cref="ArgumentOutOfRangeException">Не удалось перевести ответ в двоичный вид</exception>
        public async Task<bool> GetInfoRegisterBitAsync(int bitNumber)
        {
            var data = await GetInfoRegisterAsync();

            return (data >> bitNumber & 0x0001) > 0;
        }
        
        public async Task<bool> SetWorkRegisterAsync(ushort? valueWrite = null)
        {
            if (valueWrite != null)
                return await WriteRegisterAsync(StandDeviceRegisterMap.ControlRegister, BitConverter.GetBytes((ushort)valueWrite).Reverse().ToArray());

            var value = StandComponentDevices
                .Where(baseDevice => baseDevice.Statement)
                .Aggregate((ushort)0, (current, baseDevice) => (ushort)(current + (ushort)Math.Pow(2, baseDevice.BitNumber)));

            var result = await WriteRegisterAsync(StandDeviceRegisterMap.ControlRegister, BitConverter.GetBytes(value).Reverse().ToArray());

            NeedUpdateState = !result;

            return result;
        }

        public async Task<bool> EnableAllAsync() 
            => await WriteRegisterAsync(StandDeviceRegisterMap.ControlRegister, BitConverter.GetBytes((ushort)0xFFFF).Reverse().ToArray());
        
        public async Task<bool> DisableAllAsync()
            => await WriteRegisterAsync(StandDeviceRegisterMap.ControlRegister, BitConverter.GetBytes((ushort)0x0000).Reverse().ToArray());
        
        /// <summary>
        /// Перевод числа в двоичную битовую маску
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Битовая маска длинной 10 символов</returns>
        public string ConvertToBinaryString(ushort? data)
        {
            if (data == null) return null;

            var strData = new string(Convert.ToString((ushort)data, 2).Reverse().ToArray());

            while (strData.Length != 10)
            {
                strData += "0";
            }
            return strData;
        }
    }
}
