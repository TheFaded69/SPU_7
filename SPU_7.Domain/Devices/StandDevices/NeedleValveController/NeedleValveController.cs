using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Extensions;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.NeedleValveController;

public class NeedleValveController : ModbusUnitProcessor<NeedleValveControllerRegisterMap>, INeedleValveController
{
    public NeedleValveController(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<NeedleValveControllerRegisterMap> registerMap,
        int moduleAddress) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)moduleAddress;
    }


    public async Task<NeedleValveControllerStatus?> ReadStatusAsync() =>
        (NeedleValveControllerStatus?)await ReadRegisterAsync(NeedleValveControllerRegisterMap.StatusRegister);

    public async Task<bool> SetCommandAsync(NeedleValveControllerCommand needleValveControllerCommand) =>
        await WriteRegisterAsync(NeedleValveControllerRegisterMap.CommandRegister,
            BitConverter.GetBytes((uint)needleValveControllerCommand).SwapBytes().ToArray());

    public async Task<bool> SetParameterAsync(uint value) =>
        await WriteRegisterAsync(NeedleValveControllerRegisterMap.ParameterRegister,
            BitConverter.GetBytes(value).SwapBytes().ToArray());

    public async Task<uint?> ReadCurrentParameterAsync() =>
        (uint?)await ReadRegisterAsync(NeedleValveControllerRegisterMap.CurrentParameterRegister);
}