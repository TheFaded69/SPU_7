using SPU_7.Common.Line;

namespace SPU_7.Common.Attributes;

public class SensorAttribute : Attribute
{
    public SensorType[] SensorTypes { get; }

    public SensorAttribute(params SensorType[] sensorTypes)
    {
        SensorTypes = sensorTypes;
    }
}