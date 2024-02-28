using SPU_7.Common.Stand;

namespace SPU_7.Common.Attributes;

public class StandTypeAttribute : Attribute
{
    public StandTypeAttribute(StandType standType)
    {
        StandType = standType;
    }
    
    public StandType StandType { get; }
}