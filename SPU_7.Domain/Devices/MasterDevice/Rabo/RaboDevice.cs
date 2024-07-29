using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.MasterDevice.Rabo;

public class RaboDevice : IRaboDevice
{
    public RaboDevice()
    {
        
    }

    public RaboDevice(IModbusProcessor? pressureSensorModbusProcessor,
        int pressureSensorAddress,
        IModbusProcessor? temperatureModbusProcessor,
        int temperatureSensorAddress)
    {
        if (pressureSensorModbusProcessor != null)
            PressureSensor = new PressureSensor(pressureSensorModbusProcessor,
                new RegisterMapEnum<PressureSensorRegisterMap>(), pressureSensorAddress);

        if (temperatureModbusProcessor != null)
            TemperatureSensor = new TemperatureSensor(temperatureModbusProcessor,
                new RegisterMapEnum<TemperatureSensorRegisterMap>(), temperatureSensorAddress);
    }
    
    public RaboDevice(IPressureSensor pressureSensor, ITemperatureSensor temperatureSensor)
    {
        PressureSensor = pressureSensor;
        TemperatureSensor = temperatureSensor;
    }

    public IPressureSensor PressureSensor { get; set; }
    public ITemperatureSensor TemperatureSensor { get; set; }
}