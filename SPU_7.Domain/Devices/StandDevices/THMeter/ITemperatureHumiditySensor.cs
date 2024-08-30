namespace SPU_7.Domain.Devices.StandDevices.THMeter
{
    public interface ITemperatureHumiditySensor
    {
        Task<float?> ReadTemperatureAsync();

        Task<float?> ReadHumidityAsync();

    }
}
