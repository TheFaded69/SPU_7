using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.THMeter
{
    public class TemperatureHumiditySensor : ModbusUnitProcessor<TemperatureHumiditySensorRegisterMap>, ITHMeter
    {
        public TemperatureHumiditySensor(IModbusProcessor modbusProcessor, 
            IRegisterMapEnum<TemperatureHumiditySensorRegisterMap> registerMap,
            int moduleAddress) :
            base(modbusProcessor, registerMap)
        {
            ModuleAddress = (byte)moduleAddress;
        }

        public async Task<ushort?> ReadHumidityAsync() => (ushort?)await ReadRegisterAsync(TemperatureHumiditySensorRegisterMap.HumidityRegister);

        public async Task<short?> ReadTemperatureAsync() => (short?)await ReadRegisterAsync(TemperatureHumiditySensorRegisterMap.TemperatureRegister);
    }
}
