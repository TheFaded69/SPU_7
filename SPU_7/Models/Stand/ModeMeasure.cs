using System.ComponentModel;

namespace SPU_7.Models.Stand;

public enum ModeMeasure
{
    None = 0,
    
    [Description("Режим измерения частоты")]
    Frequency = 1,
        
    [Description("Режим измерения периода")]
    Period = 2,
}