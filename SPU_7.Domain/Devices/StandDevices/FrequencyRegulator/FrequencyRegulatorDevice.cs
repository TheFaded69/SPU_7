using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.FrequencyRegulator;

public class FrequencyRegulatorDevice : ModbusUnitProcessor<FrequencyRegulatorRegisterMap>, IFrequencyRegulatorDevice
{
    public FrequencyRegulatorDevice(IModbusProcessor modbusProcessor, 
        IRegisterMapEnum<FrequencyRegulatorRegisterMap> registerMap, 
        Func<float?> readCurrentPressure, 
        float readTargetPressure,
        int moduleAddress) : base(modbusProcessor, registerMap)
    {
        _readCurrentPressure = readCurrentPressure;
        _readTargetPressure = readTargetPressure;
        
        ModuleAddress = (byte)moduleAddress;
    }

    private PidController _pid;
    private readonly Func<float?> _readCurrentPressure;
    private readonly float _readTargetPressure;
    
    public void SetPidParameters(double pG, double iG, double dG, double pMax, double pMin, double oMax, double oMin)
    {
        _pid = new PidController(pG, iG, dG, pMax, pMin,  oMax,  oMin, ReadPV, ReadSP, WriteOV);
    }

    /// <summary>
    /// Записать текущее значение в ПИД регулятор (давление)
    /// </summary>
    /// <returns></returns>
    private double? ReadPV() => _readCurrentPressure();

    /// <summary>
    /// Записать требуемое значение в ПИД регулятор (давление)
    /// </summary>
    /// <returns></returns>
    private  double ReadSP() =>  -_readTargetPressure;

    
    /// <summary>
    /// Записать полученное значение из ПИД регулятора (делегат который вызывается ПИД регулятором после расчетов)
    /// В методе логика записи рассчитаного значения в частотный регулятор для установки нужного питания двигателя
    /// </summary>
    /// <param name="value">Выходное значение из ПИД регулятора </param>
    private async Task WriteOV(double value) => await SetOutputValueAsync(value);

    public async Task<bool> SetOutputValueAsync(double value)
        => await WriteRegisterAsync(FrequencyRegulatorRegisterMap.FrequencyValueRegister, BitConverter.GetBytes((ushort)value).Reverse().ToArray());
    
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