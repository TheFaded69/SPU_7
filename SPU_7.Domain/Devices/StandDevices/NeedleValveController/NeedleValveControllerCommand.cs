namespace SPU_7.Domain.Devices.StandDevices.NeedleValveController;

public enum NeedleValveControllerCommand : uint
{
    None = 0,
    
    ReturnToZero = 1,
    
    RunToParameter = 2,
}