using System.ComponentModel;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public enum SolenoidValveType
{
    [Description("Нормально открытый")]
    NormalOpen = 1,
    
    [Description("Нормально закрытый")]
    NormalClose = 2,
}