using System.ComponentModel;

namespace SPU_7.Common.Line;

public enum SensorType
{
    [Description("Turbo Flow PS")]
    TurboFlowPS = 1,
    
    [Description("Паскаль-04")]
    Pascal04 = 2,
    
    [Description("ИВТМ-7")]
    IVTM7 = 3,
    
    [Description("ИВА")]
    IVA = 4,
    
    [Description("Метран")]
    Metran = 5,
    
    [Description("Датчик температуры")]
    TemperatureSensor = 6,
}