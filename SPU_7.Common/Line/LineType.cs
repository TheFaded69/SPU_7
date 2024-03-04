using System.ComponentModel;

namespace SPU_7.Common.Line;

public enum LineType
{
    None = 0,
    
    [Description("Поверка мастер устройствами")]
    MasterDeviceLine = 1,

    [Description("Поверка соплами")]
    NozzleLine = 2,
    
    
}