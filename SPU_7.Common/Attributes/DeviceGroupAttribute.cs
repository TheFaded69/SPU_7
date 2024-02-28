using SPU_7.Common.Device;

namespace SPU_7.Common.Attributes;

public class DeviceGroupAttribute : Attribute
{
    public DeviceGroupAttribute(DeviceGroupType deviceGroupType)
    {
        DeviceGroupType = deviceGroupType;
    }
    
    public DeviceGroupType DeviceGroupType { get; set; }
}