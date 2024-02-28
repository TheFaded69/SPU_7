using System.ComponentModel;

namespace SPU_7.Modbus.Types;

/// <summary>
/// Код ошибки
/// </summary>
public enum ExceptionCode : byte
{
    /// <summary>
    /// The function code received in the query is not an allowable action for the server (or slave).
    /// This may be because the function code is only applicable to newer devices, and was not
    /// implemented in the unit selected. It could also indicate that the server (or slave) is in the wrong
    /// state to process a request of this type, for example because it is unconfigured and is being
    /// asked to return register values.
    /// </summary>
    [Description("Illegal function")]
    IllegalFunction = 0x01,

    /// <summary>
    /// The data address received in the query is not an allowable address for the server (or slave).
    /// More specifically, the combination of reference number and transfer length is invalid. For a controller with
    /// 100 registers, the PDU addresses the first register as 0, and the last one as 99. If a request
    /// is submitted with a starting register address of 96 and a quantity of registers of 4, then this request
    /// will successfully operate (address-wise at least) on registers 96, 97, 98, 99. If a request is
    /// submitted with a starting register address of 96 and a quantity of registers of 5, then this request
    /// will fail with Exception Code 0x02 “Illegal Data Address” since it attempts to operate on registers
    /// 96, 97, 98, 99 and 100, and there is no register with address 100.
    /// </summary>
    [Description("Illegal data address")]
    IllegalDataAddress = 0x02,

    /// <summary>
    /// A value contained in the query data field is not an allowable value for server (or slave).
    /// This indicates a fault in the structure of the remainder of a complex request, such as that the implied
    /// length is incorrect. It specifically does NOT mean that a data item submitted for storage in a register
    /// has a value outside the expectation of the application program, since the MODBUS protocol
    /// is unaware of the significance of any particular value of any particular register.
    /// </summary>
    [Description("Illegal data value")]
    IllegalDataValue = 0x03,

    /// <summary>
    /// An unrecoverable error occurred while the server (or slave) was attempting to perform the requested action.
    /// </summary>
    [Description("Slave device failure")]
    SlaveDeviceFailure = 0x04,

    /// <summary>
    /// Specialized use in conjunction with programming commands.
    /// /// The server (or slave) has accepted the request and is processing it, but a long duration of time
    /// will be required to do so. This response is returned to prevent a timeout error from occurring
    /// in the client (or master). The client (or master) can next issue a Poll Program Complete message
    /// to determine if processing is completed.
    /// </summary>
    [Description("Acknowledge")]
    Acknowledge = 0x05,

    /// <summary>
    /// Specialized use in conjunction with programming commands.
    /// The server (or slave) is engaged in processing a long–duration program command.
    /// </summary>
    [Description("Slave device busy")]
    SlaveDeviceBusy = 0x06,

    /// <summary>
    /// Specialized use in conjunction with function codes 20 and 21 and reference type 6, to indicate that
    /// the extended file area failed to pass a consistency check.
    /// </summary>
    [Description("Memory parity error")]
    MemoryParityError = 0x08,

    [Description("No record")]
    NoRecord = 0x11,

    [Description("One time password expired")]
    OneTimePasswordExpired = 0x12,

    [Description("Crc error")]
    CrcError = 0x15,

    [Description("Illegal slave address")]
    IllegalSlaveAddress = 0x16,

    [Description("Illegal data size")]
    IllegalDataSize = 0x17,

    [Description("Write protected")]
    WriteProtected = 0x18,

    [Description("Device in boot mode")]
    DeviceInBootMode = 0x55,

    [Description("Invalid protocol")]
    InvalidProtocol = 0x77
}