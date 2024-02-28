using SPU_7.Common.Device;

namespace SPU_7.Models.Scripts.Operations.Configurations;

public class BaseOperationConfigurationModel
{
    public DeviceType DeviceType { get; set; }
    
    public DeviceGroupType GroupType { get; set; }
}