using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

namespace SPU_7.Domain.Devices;

public interface IMasterDevice
{
    IPressureSensor PressureSensor { get; set; }
    
    ITemperatureSensor TemperatureSensor { get; set; }
}