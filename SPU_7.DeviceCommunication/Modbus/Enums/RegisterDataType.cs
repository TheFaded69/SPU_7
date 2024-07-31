using SPU_7.DeviceCommunication.Modbus.Attributes;

namespace SPU_7.DeviceCommunication.Modbus.Enums;

public enum RegisterDataType
{
    //Int8,
    //UInt8,
    [DataTypeDescription(typeof(short), sizeof(short))]
    Int16  = 0x0000,
    [DataTypeDescription(typeof(ushort), sizeof(ushort))]
    UInt16 = 0x0001,
    [DataTypeDescription(typeof(int), sizeof(int))]
    Int32  = 0x0002,
    [DataTypeDescription(typeof(uint), sizeof(uint))]
    UInt32 = 0x0003,
    [DataTypeDescription(typeof(long), sizeof(long))]
    Int64  = 0x0004,
    [DataTypeDescription(typeof(ulong), sizeof(ulong))]
    UInt64 = 0x0005,
    [DataTypeDescription(typeof(float), sizeof(float))]
    Float  = 0x0006,
    [DataTypeDescription(typeof(double), sizeof(double))]
    Double = 0x0007,
    [DataTypeDescription(typeof(uint), sizeof(uint))]
    UnixTime,
    [DataTypeDescription(typeof(long), sizeof(long))]
    TDateTime,
    ByteArray = 0x2000,
    CharArray = 0x2001,
    ArrayValuesFlag = 0x4000,
    Int16Array  = ArrayValuesFlag | Int16,
    UInt16Array = ArrayValuesFlag | UInt16,
    Int32Array  = ArrayValuesFlag | Int32,
    UInt32Array = ArrayValuesFlag | UInt32,
    Int64Array  = ArrayValuesFlag | Int64,
    UInt64Array = ArrayValuesFlag | UInt64,
    FloatArray  = ArrayValuesFlag | Float,
    DoubleArray = ArrayValuesFlag | Double,
    ValueMask = 0x00FF,
}