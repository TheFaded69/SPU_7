namespace SPU_7.Domain.Devices.StandDevices.NeedleValveController;

public enum NeedleValveControllerStatus : uint
{
    Unknown = 0,
    
    Zero = 1,
    
    RunToZero = 2,
    
    RunUp = 3,
    
    RunDown = 4,
    
    Done = 5,
}