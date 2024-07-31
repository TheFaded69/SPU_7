namespace SPU_7.DeviceCommunication.Modbus.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class MeasureUnitsAttribute : Attribute
{
    public MeasureUnitsAttribute(string units)
    {
        Units = units;
    }

    public string Units { get; set; }
}