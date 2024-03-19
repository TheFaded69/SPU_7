using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

namespace SPU_7.Domain.Devices.MasterDevice.Rabo;

public class RaboDevice : IRaboDevice
{
    public RaboDevice()
    {
        
    }

    public IPressureSensor PressureSensor { get; set; }
    public ITemperatureSensor TemperatureSensor { get; set; }
}