using System.Collections.Generic;
using SPU_7.Domain.Devices;
using SPU_7.Domain.Devices.UniversalDevices;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;
using SPU_7.Models.Services.StandSetting;

namespace SPU_7.Models.Stand;

public class StandLine
{
    public StandLine(IStandSettingsService settingsService, IModbusProcessor modbusProcessor, int i)
    {
        foreach (var deviceViewModel in settingsService.StandSettingsModel.LineViewModels[i].DeviceViewModels)
        {
            Devices.Add(new UniversalDevice(modbusProcessor,
                new RegisterMapEnum<UniversalDeviceRegisterMap>(),
                deviceViewModel.PressureSensorAddress,
                deviceViewModel.PulseMeterNumber,
                deviceViewModel.PulseMeterChannelNumber));
        }
    }
    
    public List<IDevice> Devices = new();
}