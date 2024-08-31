using System.ComponentModel;

namespace SPU_7.Common.Line;

public enum FanType
{
    [Description("Управление ПЧВ")]
    FrequencyControlFan = 1,
    
    [Description("Управление через КПК")]
    ControlModuleControlFan = 2,
}