using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

namespace SPU_7.Domain.Devices.MasterDevice.RGT;

public class RGTDevice : IRGTDevice
{
    public RGTDevice()
    {
        
    }
    
    public IPressureSensor PressureSensor { get; set; }
    public ITemperatureSensor TemperatureSensor { get; set; }
}