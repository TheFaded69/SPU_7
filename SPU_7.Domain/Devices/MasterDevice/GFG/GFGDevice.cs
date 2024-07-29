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

    public GFGDevice(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<GFGRegisterMap> registerMap,
        IModbusProcessor? pressureSensorModbusProcessor,
        int pressureSensorAddress,
        IModbusProcessor? temperatureModbusProcessor,
        int temperatureSensorAddress) : base(modbusProcessor, registerMap)
    {
        if (pressureSensorModbusProcessor != null)
            PressureSensor = new PressureSensor(pressureSensorModbusProcessor,
                new RegisterMapEnum<PressureSensorRegisterMap>(), pressureSensorAddress);

        if (temperatureModbusProcessor != null)
            TemperatureSensor = new TemperatureSensor(temperatureModbusProcessor,
                new RegisterMapEnum<TemperatureSensorRegisterMap>(), temperatureSensorAddress);
    }
    
    public GFGDevice(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<GFGRegisterMap> registerMap, 
        IPressureSensor pressureSensor, 
        ITemperatureSensor temperatureSensor) : base(modbusProcessor, registerMap)
    {
        PressureSensor = pressureSensor;
        TemperatureSensor = temperatureSensor;
    }

    public IPressureSensor PressureSensor { get; set; }
    public ITemperatureSensor TemperatureSensor { get; set; }
    
    
    private float? Pressure { get; set; }
    
    private float? Temperature { get; set; }
}