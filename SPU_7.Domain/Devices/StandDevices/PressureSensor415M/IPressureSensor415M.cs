namespace SPU_7.Domain.Devices.StandDevices.PressureSensor415M;

public interface IPressureSensor415M
{
    /// <summary>
    /// Считать давление в мм.рт.ст
    /// </summary>
    /// <returns></returns>
    Task<float?> ReadPressureAsync();
}