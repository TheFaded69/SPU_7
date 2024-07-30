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
using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
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
            var pressureSensor = string.IsNullOrEmpty(deviceViewModel.SelectedPressureSensorComPort)
                ? null
                : new PressureSensor(modbusProcessors.FirstOrDefault(mb => mb.PortName == deviceViewModel.SelectedPressureSensorComPort), 
                    new RegisterMapEnum<PressureSensorRegisterMap>(),
                    deviceViewModel.PressureSensorAddress);
                    
            var temperatureSensor = string.IsNullOrEmpty(deviceViewModel.SelectedTemperatureSensorComPort)
                ? null
                : new TemperatureSensor(modbusProcessors.FirstOrDefault(mb => mb.PortName == deviceViewModel.SelectedTemperatureSensorComPort), 
                    new RegisterMapEnum<TemperatureSensorRegisterMap>(),
                    deviceViewModel.TemperatureSensorAddress,
                    (int)deviceViewModel.TemperatureChannelNumber);
            
            Devices.Add(new UniversalDevice(null,
                new RegisterMapEnum<UniversalDeviceRegisterMap>(), pressureSensor, temperatureSensor));
        }

        switch (settingsService.StandSettingsModel.LineViewModels[i].SelectedLineType)
        {
            case LineType.MasterDeviceLineType:
                foreach (var masterDeviceModel in settingsService.StandSettingsModel.LineViewModels[i].MasterDeviceViewModels)
                {
                    var pressureSensor = string.IsNullOrEmpty(masterDeviceModel.SelectedPressureSensorComPort)
                        ? null
                        : new PressureSensor(modbusProcessors.FirstOrDefault(mb => mb.PortName == masterDeviceModel.SelectedPressureSensorComPort), 
                            new RegisterMapEnum<PressureSensorRegisterMap>(),
                            masterDeviceModel.PressureSensorAddress);
                    
                    var temperatureSensor = string.IsNullOrEmpty(masterDeviceModel.SelectedTemperatureSensorComPort)
                        ? null
                        : new TemperatureSensor(modbusProcessors.FirstOrDefault(mb => mb.PortName == masterDeviceModel.SelectedTemperatureSensorComPort), 
                            new RegisterMapEnum<TemperatureSensorRegisterMap>(),
                            masterDeviceModel.TemperatureSensorAddress,
                            (int)masterDeviceModel.TemperatureChannelNumber);
                    
                    MasterDevices.Add(masterDeviceModel.SelectedMasterDeviceType switch
                    {
                        MasterDeviceType.GFG => new GfgDevice(null, new RegisterMapEnum<GFGRegisterMap>(), pressureSensor, temperatureSensor),
                        MasterDeviceType.Rabo => new RaboDevice( pressureSensor, temperatureSensor),
                        MasterDeviceType.RGT => new RGTDevice( pressureSensor, temperatureSensor),
                    });
                }
                break;
            case LineType.NozzleLineType:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        
    }
    
    public readonly List<IDevice> Devices = [];

    public readonly List<IMasterDevice> MasterDevices = [];
}