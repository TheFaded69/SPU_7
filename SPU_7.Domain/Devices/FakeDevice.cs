using SPU_7.Common.Device;
using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices;

/// <summary>
/// Фейковое устройство, необходимо для определения идентификации устройства (его тип, исполнение, номер и т.п.)
/// </summary>
public class FakeDevice : ModbusUnitProcessor<FakeRegisterMap>
{
    public FakeDevice(IModbusProcessor modbusProcessor, IRegisterMapEnum<FakeRegisterMap> registerMap) : base(modbusProcessor, registerMap)
    {
        ModuleAddress = 1;
    }
    
    public DeviceType DeviceType { get; set; }
    public uint InnerVendorNumber { get; set; }
    public string VendorNumberString { get; set; }
    public string RegisterMapVersionString { get; set; }
    public int RegisterMapVersionInt { get; set; }
    
    public async Task<bool> UpdateDeviceIdentificatorAsync()
    {
        try
        {
            var deviceId = await GetID();
            if (deviceId == null)
                return false;
            RegisterMapVersionInt = deviceId.RegisterMapMajorVersion * 100 + deviceId.RegisterMapMinorVersion;
            RegisterMapVersionString = $"{deviceId.RegisterMapMajorVersion}.{deviceId.RegisterMapMinorVersion}";
            if (RegisterMapVersionString.Contains(',')) RegisterMapVersionString = RegisterMapVersionString.Replace(",",".");
                
            VendorNumberString = deviceId.DeviceSerialNumber.NullTerminatedToString();
            DeviceType = (DeviceTypeId) deviceId.DeviceTypeId switch
            {
                _ => throw new ArgumentOutOfRangeException()
            };
            return true;
        }
        catch (Exception)
        {
            return false;;
        }
    }
    
    public void CommunicatorOpen()
    {
        ModbusProcessor.Communicator.Open();
    }

    public void CommunicatorClose()
    {
        ModbusProcessor.Communicator.Close();
    }
}