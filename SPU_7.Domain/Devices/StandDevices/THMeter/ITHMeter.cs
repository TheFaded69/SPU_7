namespace SPU_7.Domain.Devices.StandDevices.THMeter
{
    public interface ITHMeter
    {
        Task<short?> ReadTemperatureAsync();

        Task<ushort?> ReadHumidityAsync();

    }
}
