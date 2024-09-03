using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.StandDevices.Owen.OwenPBR10A;

public class OwenPBR10ADevice : ModbusUnitProcessor<OwenPBR10ARegisterMap>, IOwenPBR10ADevice
{
    public OwenPBR10ADevice(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<OwenPBR10ARegisterMap> registerMap, int moduleAddress, int number) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = (byte)moduleAddress;
        ModuleAddressInt = moduleAddress;
        OwenNumber = number;
    }

    public int OwenNumber { get; set; }
    public int ModuleAddressInt { get; set; }

    public async Task<bool> StopMovingAsync()
    {
        return await WriteRegisterAsync(OwenPBR10ARegisterMap.ValveControl,
            BitConverter.GetBytes((ushort)IOwenPBR10AMoveType.Stop));
    }

    public async Task<bool> MovingDownAsync()
    {
        return await WriteRegisterAsync(OwenPBR10ARegisterMap.ValveControl,
            BitConverter.GetBytes((ushort)IOwenPBR10AMoveType.Down));
    }

    public async Task<bool> MovingUpAsync()
    {
        return await WriteRegisterAsync(OwenPBR10ARegisterMap.ValveControl,
            BitConverter.GetBytes((ushort)IOwenPBR10AMoveType.Up));
    }

    public async Task<ushort?> GetPositionAsync()
    {
        return (ushort?)await ReadRegisterAsync(OwenPBR10ARegisterMap.PositionPercent);
    }

    public async Task<bool> SetPosition(int position)
    {
        var result = true;
        var currentPosition = await GetPositionAsync();

        if (currentPosition < position)
        {
            result = await MovingDownAsync();
            
            while (await GetPositionAsync() < position)
            {
                await Task.Delay(100);
            }

            return result = await StopMovingAsync();
        }
        else if (currentPosition > position)
        {
            result = await MovingUpAsync();

            while (await GetPositionAsync() > position)
            {
                await Task.Delay(100);
            }

            return result = await StopMovingAsync();
        }

        return result;
    }
}