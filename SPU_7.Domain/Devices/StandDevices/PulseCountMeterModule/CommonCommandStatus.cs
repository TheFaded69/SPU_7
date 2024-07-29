namespace SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;

public enum CommonCommandStatus : uint
{
    Unknown = 0,
    
    Ready = 1,
    
    Start = 2,
    
    Run = 3,
    
    Done = 4,
    
    Timeout = 5,
    
    Error = 6
}