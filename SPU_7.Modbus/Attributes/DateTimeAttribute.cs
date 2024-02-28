// ReSharper disable InconsistentNaming

namespace SPU_7.Modbus.Attributes;

public enum DateTimeFormat
{
    TDateTime,
    UnixTime
}

public class DateTimeFormatAttribute : Attribute
{
    public DateTimeFormatAttribute(DateTimeFormat dateTimeFormat)
    {
        DateTimeFormat = dateTimeFormat;
    }

    public DateTimeFormat DateTimeFormat { get; set; }
}