namespace SPU_7.DeviceCommunication.Modbus.Enums;

public enum ModbusFunction
{
    None = 0x00,
    //ReadCoils = 0x01,
    //ReadDiscreteInputs = 0x02,
    ReadHoldingRegisters = 0x03,
    ReadInputRegisters = 0x04,
    //WriteSingleCoil = 0x05,
    WriteSingleRegister = 0x06,
    //ReadExceptionStatus = 0x07,
    //Diagnostics = 0x08,
    //GetCommEventCounter = 0x0B,
    //GetCommEventLog = 0x0C,
    //WriteMultipleCoils = 0x0F,
    WriteMultipleRegisters = 0x10,
    ReportSlaveId = 0x11,
    //ReadFileRecord = 0x14,
    //WriteFileRecord = 0x15,
    //MaskWriteRegister = 0x16,
    //ReadWriteMultipleRegisters = 0x17,
    //ReadFIFOQueue = 0x18,
}