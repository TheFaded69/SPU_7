using System.ComponentModel;

namespace SPU_7.Common.Device;

public enum MasterDeviceType
{
    None = 0,
    
    [Description("GFG")]
    GFG = 1,
    
    [Description("Rabo")]
    Rabo = 2,
    
    [Description("RGT")]
    RGT = 3,
}