using System.ComponentModel;

namespace SPU_7.Common.Stand;

public enum NozzleManualType
{
    [Description("Стандартное управление соплами")]
    StandardManual = 1,
    
    [Description("Частотное управление соплами")]
    FrequencyManual = 2,
}