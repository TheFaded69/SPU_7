using System.ComponentModel;
using SPU_7.Common.Attributes;

namespace SPU_7.Common.Line;

public enum SensorPurpose
{
    [Description("Датчик температуры")]
    [Sensor(SensorType.TemperatureSensor)]
    TemperatureSensor = 1,

    [Description("Датчик абсолютного давления")]
    [Sensor(SensorType.TurboFlowPS, SensorType.Metran)]
    PressureSensor = 2,
    
    [Description("Датчик относительного давления")]
    [Sensor(SensorType.TurboFlowPS)]
    PressureOffsetSensor = 3,
    
    [Description("Датчик перепада давления")]
    [Sensor(SensorType.TurboFlowPS)]
    PressureDifferenceSensor = 4,
    
    [Description("Датчик разряжения давления")]
    [Sensor(SensorType.Pascal04)]
    PressureDischargeSensor = 5,
    
    [Description("Датчик влажности")]
    [Sensor()]
    HumiditySensor = 6,
    
    [Description("Датчик влажности и температуры")]
    [Sensor(SensorType.IVA, SensorType.IVTM7)]
    TemperatureHumiditySensor = 7,
}