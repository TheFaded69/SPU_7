namespace SPU_7.Domain.Devices.StandDevices.TemperatureSensor;

public interface ITemperatureSensor
{
    float? Temperature { get; set; }
    Task<float?> ReadTemperatureAsync();
    
    Task<float?> ReadTemperatureAsync(bool useChannel);
}