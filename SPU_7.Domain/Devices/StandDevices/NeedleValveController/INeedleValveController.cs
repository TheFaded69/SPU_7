namespace SPU_7.Domain.Devices.StandDevices.NeedleValveController;

public interface INeedleValveController
{
    Task<NeedleValveControllerStatus?> ReadStatusAsync();

    Task<bool> SetCommandAsync(NeedleValveControllerCommand needleValveControllerCommand);

    Task<bool> SetParameterAsync(uint value);

    Task<uint?> ReadCurrentParameterAsync();
}