using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.MasterDevice.GFG;

public class GFGDevice : ModbusUnitProcessor<GFGRegisterMap>, IGFGDevice
{
    public GFGDevice(IModbusProcessor modbusProcessor, IRegisterMapEnum<GFGRegisterMap> registerMap) 
        : base(modbusProcessor, registerMap)
    {
        
    }

    public IPressureSensor PressureSensor { get; set; }
    public ITemperatureSensor TemperatureSensor { get; set; }
}