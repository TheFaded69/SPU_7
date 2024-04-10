using System;
using System.Collections.Generic;
using System.Linq;
using SPU_7.Common.Device;
using SPU_7.Common.Line;
using SPU_7.Domain.Devices;
using SPU_7.Domain.Devices.Device.UniversalDevice;
using SPU_7.Domain.Devices.MasterDevice.GFG;
using SPU_7.Domain.Devices.MasterDevice.Rabo;
using SPU_7.Domain.Devices.MasterDevice.RGT;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;
using SPU_7.Models.Services.StandSetting;

namespace SPU_7.Models.Stand;

public class StandLine
{
    public StandLine(IStandSettingsService settingsService, List<IModbusProcessor> modbusProcessors, int i)
    {
        foreach (var deviceViewModel in settingsService.StandSettingsModel.LineViewModels[i].DeviceViewModels)
        {
            Devices.Add(new UniversalDevice(null,
                new RegisterMapEnum<UniversalDeviceRegisterMap>()));
        }

        switch (settingsService.StandSettingsModel.LineViewModels[i].SelectedLineType)
        {
            case LineType.MasterDeviceLineType:
                foreach (var masterDeviceModel in settingsService.StandSettingsModel.LineViewModels[i].MasterDeviceViewModels)
                {
                    MasterDevices.Add(masterDeviceModel.SelectedMasterDeviceType switch
                    {
                        MasterDeviceType.GFG => new GFGDevice(modbusProcessors.First(mp => 
                            mp.PortName == masterDeviceModel.SelectedComPort), new RegisterMapEnum<GFGRegisterMap>()),
                        MasterDeviceType.Rabo => new RaboDevice(),
                        MasterDeviceType.RGT => new RGTDevice(),
                        _ => throw new ArgumentOutOfRangeException()
                    });
                }
                break;
            case LineType.NozzleLineType:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
    }
    
    public List<IDevice> Devices = new();

    public List<IMasterDevice> MasterDevices = new();
}