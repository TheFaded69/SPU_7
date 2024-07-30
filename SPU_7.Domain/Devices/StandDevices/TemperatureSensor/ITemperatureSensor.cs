namespace SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

public interface ITemperatureSensor
{
    Task<float?> ReadTemperatureAsync();
    
    Task<float?> ReadTemperatureAsync(bool useChannel);
}