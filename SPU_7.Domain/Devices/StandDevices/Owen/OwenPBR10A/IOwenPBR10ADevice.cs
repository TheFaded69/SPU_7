namespace SPU_7.Domain.Devices.StandDevices.Owen.OwenPBR10A;

public interface IOwenPBR10ADevice : IOwen
{

    
    Task<bool> StopMovingAsync();
    
    Task<bool> MovingDownAsync();
    
    Task<bool> MovingUpAsync();
    
    Task<float?> GetPositionAsync();

    Task<bool> SetPosition(float position);
}