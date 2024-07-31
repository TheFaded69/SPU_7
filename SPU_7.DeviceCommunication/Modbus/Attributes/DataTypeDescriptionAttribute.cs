namespace SPU_7.DeviceCommunication.Modbus.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class DataTypeDescriptionAttribute : Attribute
{
    public DataTypeDescriptionAttribute(Type typeInfo, int size)
    {
        TypeInfo = typeInfo;
        Size = size;
    }

    public Type TypeInfo { get; }

    public int Size { get; }
}