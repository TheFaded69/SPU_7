using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.THMeter
{
    public class THMeter : ModbusUnitProcessor<THMeterRegisterMap>, ITHMeter
    {
        public THMeter(IModbusProcessor modbusProcessor, 
            IRegisterMapEnum<THMeterRegisterMap> registerMap,
            int moduleAddress) :
            base(modbusProcessor, registerMap)
        {
            ModuleAddress = (byte)moduleAddress;
        }

        public async Task<ushort?> ReadHumidityAsync() => (ushort?)await ReadRegisterAsync(THMeterRegisterMap.HumidityRegister);

        public async Task<short?> ReadTemperatureAsync() => (short?)await ReadRegisterAsync(THMeterRegisterMap.TemperatureRegister);
    }
}
