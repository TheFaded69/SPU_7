using System.ComponentModel;

namespace SPU_7.Common.Stand;

public enum ProgramType
{
    None = 0,
    
    [Description("СПУ-5")]
    UniversalStand = 1,
    
    [Description("СПП")]
    CheckBoardStand = 2,
}