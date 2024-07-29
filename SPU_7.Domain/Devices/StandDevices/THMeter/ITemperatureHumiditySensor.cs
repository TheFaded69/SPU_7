namespace SPU_7.Domain.Devices.StandDevices.THMeter
{
    public interface ITemperatureHumiditySensor
    {
        Task<short?> ReadTemperatureAsync();

        Task<ushort?> ReadHumidityAsync();

    }
}
