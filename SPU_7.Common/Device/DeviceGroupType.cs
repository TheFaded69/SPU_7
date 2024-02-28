using System.ComponentModel;

namespace SPU_7.Common.Device;

public enum DeviceGroupType
{
    None = 0,
    
    [Description("NBIoT")]
    NBIoT = 1,
        
    [Description("SPI-ГРАНД")]
    SPIGRAND = 3,
    
    [Description("ГРАНД")]
    Grand = 4,
    
    [Description("Универсальное устройство")]
    Universal = 5,
}