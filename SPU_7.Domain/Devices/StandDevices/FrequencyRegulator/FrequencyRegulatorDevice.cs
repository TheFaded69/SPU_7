using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.FrequencyRegulator;

public class FrequencyRegulatorDevice : ModbusUnitProcessor<FrequencyRegulatorRegisterMap>, IFrequencyRegulatorDevice
{
    public FrequencyRegulatorDevice(IModbusProcessor modbusProcessor, 
        IRegisterMapEnum<FrequencyRegulatorRegisterMap> registerMap, 
        int moduleAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)moduleAddress;
    }

    private bool _currentState;
    private PidController _pid;
    
    public void SetPidParameters(double pG, double iG, double dG, double pMax, double pMin, double oMax, double oMin)
    {
        //_pid = new PidController(pG, iG, dG, pMax, pMin,  oMax,  oMin, ReadPV, ReadSP, WriteOV);
    }
    
    
    public async Task<bool> WriteOutputValueAsync(double value)
        => await WriteRegisterAsync(FrequencyRegulatorRegisterMap.FrequencyValueRegister, BitConverter.GetBytes((ushort)(value / 0.1)).Reverse().ToArray());

    public async Task<ushort?> ReadOutputValueAsync()
    {
        return (ushort?)await ReadRegisterAsync(FrequencyRegulatorRegisterMap.FrequencyValueRegister);
    }

    public async Task<ushort?> GetCurrentFrequencyValueAsync()
        => (ushort?)await ReadRegisterAsync(FrequencyRegulatorRegisterMap.PA02);

    public async Task<bool> StartFrequencyWorkAsync()
    {
        return await WriteRegisterAsync(FrequencyRegulatorRegisterMap.CommandRegister, BitConverter.GetBytes((ushort)2).Reverse().ToArray());
    }

    public async Task<bool> StopFrequencyWorkAsync()
    {
        return await WriteRegisterAsync(FrequencyRegulatorRegisterMap.CommandRegister, BitConverter.GetBytes((ushort)1).Reverse().ToArray());
    }

    public void PidEnable()
    {
        _pid.Enable();
    }

    public void PidDisable() 
    {
        _pid.Disable();
    }
}