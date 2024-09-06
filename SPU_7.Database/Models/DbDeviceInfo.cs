using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models;

public class DbDeviceInfo : DbEntityGuid
{
    public string DeviceTypeInformation { get; set; }
}