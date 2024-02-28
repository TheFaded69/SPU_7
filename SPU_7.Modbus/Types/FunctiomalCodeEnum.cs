using System.ComponentModel;

namespace SPU_7.Modbus.Types;

/// <summary>
/// Команда
/// </summary>
public enum FunctiomalCodeEnum : byte
{
    [Description("Read Coils (0x01)")]
    ReadCoils = 0x01,

    [Description("Read Discrete Inputs (0x02)")]
    ReadDiscreteInputs = 0x02,

    [Description("Read Holding Registers (0x03)")]
    ReadHoldingRegisters = 0x03,

    [Description("Read Input Register (0x04)")]
    ReadInputRegisters = 0x04,

    [Description("Write Single Coil (0x05)")]
    WriteSingleCoil = 0x05,

    [Description("Write Single Register (0x06)")]
    WriteSingleRegister = 0x06,

    [Description("Write Multiple Coils (0x0F)")]
    WriteMultipleCoils = 0x0F,

    [Description("Write Multiple Registers (0x10)")]
    WriteMultipleRegisters = 0x10,

    [Description("Report SlaveId (0x11)")]
    ReportSlaveId = 0x11,

    [Description("Read Write Multiple Registers (0x17)")]
    ReadWriteMultipleRegisters = 0x17,
}