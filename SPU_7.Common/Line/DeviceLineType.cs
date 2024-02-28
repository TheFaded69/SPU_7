using System.ComponentModel;

namespace SPU_7.Common.Line;

public enum DeviceLineType
{
    [Description("Мембранный СГ")]
    MembraneDevice = 1,
        
    [Description("Струйный СГ")]
    JetDevice = 2,
}