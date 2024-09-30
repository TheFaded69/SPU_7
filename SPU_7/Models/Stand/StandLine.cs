using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Avalonia.Controls;
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
using IDevice = SPU_7.Domain.Devices.IDevice;

namespace SPU_7.Models.Stand;

public class StandLine
{
    public StandLine(IStandSettingsService settingsService, List<IModbusProcessor> modbusProcessors, int lineIndex)
    {
        LineNumber = lineIndex + 1;
        CurrentFlow = settingsService.StandSettingsModel.LineViewModels[lineIndex].NozzleViewModels
            .Sum(noz => noz.NozzleFactValue);
        
        CurrentFlow = 0;


        foreach (var deviceViewModel in settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels)
        {
            var pressureSensor = string.IsNullOrEmpty(deviceViewModel.SelectedPressureSensorComPort)
                ? null
                : new PressureSensor(modbusProcessors
                        .FirstOrDefault(mb => mb.PortName == deviceViewModel.SelectedPressureSensorComPort),
                    new RegisterMapEnum<PressureSensorRegisterMap>(),
                    deviceViewModel.PressureSensorAddress);

            var temperatureSensor = string.IsNullOrEmpty(deviceViewModel.SelectedTemperatureSensorComPort)
                ? null
                : new TemperatureSensor(modbusProcessors
                        .FirstOrDefault(mb => mb.PortName == deviceViewModel.SelectedTemperatureSensorComPort),
                    new RegisterMapEnum<TemperatureSensorRegisterMap>(),
                    deviceViewModel.TemperatureSensorAddress,
                    (int)deviceViewModel.TemperatureChannelNumber);

            Devices.Add(new UniversalDevice(null,
                new RegisterMapEnum<UniversalDeviceRegisterMap>(), pressureSensor, temperatureSensor,
                modbusProcessors.FirstOrDefault(mb => mb.PortName == settingsService.StandSettingsModel
                    .PulseMeterViewModels
                    .FirstOrDefault(p => p.Address == deviceViewModel.PulseMeterNumber)
                    ?.SelectedComPort),
                deviceViewModel.PulseMeterNumber, deviceViewModel.PulseMeterChannelNumber));
        }

        switch (settingsService.StandSettingsModel.LineViewModels[lineIndex].SelectedLineType)
        {
            case LineType.MasterDeviceLineType:
                foreach (var masterDeviceModel in settingsService.StandSettingsModel.LineViewModels[lineIndex]
                             .MasterDeviceViewModels)
                {
                    var pressureSensor = string.IsNullOrEmpty(masterDeviceModel.SelectedPressureSensorComPort)
                        ? null
                        : new PressureSensor(modbusProcessors
                                .FirstOrDefault(mb => mb.PortName == masterDeviceModel.SelectedPressureSensorComPort),
                            new RegisterMapEnum<PressureSensorRegisterMap>(),
                            masterDeviceModel.PressureSensorAddress);

                    var temperatureSensor = string.IsNullOrEmpty(masterDeviceModel.SelectedTemperatureSensorComPort)
                        ? null
                        : new TemperatureSensor(modbusProcessors
                                .FirstOrDefault(mb =>
                                    mb.PortName == masterDeviceModel.SelectedTemperatureSensorComPort),
                            new RegisterMapEnum<TemperatureSensorRegisterMap>(),
                            masterDeviceModel.TemperatureSensorAddress,
                            (int)masterDeviceModel.TemperatureChannelNumber);

                    MasterDevices.Add(masterDeviceModel.SelectedMasterDeviceType switch
                    {
                        MasterDeviceType.GFG => new GfgDevice(null,
                            new RegisterMapEnum<GFGRegisterMap>(),
                            pressureSensor,
                            temperatureSensor),
                        MasterDeviceType.Rabo => new RaboDevice(pressureSensor, temperatureSensor),
                        MasterDeviceType.RGT => new RGTDevice(pressureSensor, temperatureSensor),
                    });
                }

                break;
            case LineType.NozzleLineType:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        foreach (var sensorViewModel in settingsService.StandSettingsModel.LineViewModels[lineIndex].SensorViewModels)
        {
            switch (sensorViewModel.SensorPurpose)
            {
                case SensorPurpose.TemperatureSensor:
                    TemperatureSensor = new TemperatureSensor(modbusProcessors
                            .FirstOrDefault(mb => mb.PortName == sensorViewModel.SelectedComPort),
                        new RegisterMapEnum<TemperatureSensorRegisterMap>(),
                        sensorViewModel.Address,
                        sensorViewModel.ChannelNumber);
                    break;
                case SensorPurpose.PressureSensor:
                    PressureSensor = sensorViewModel.SensorType switch
                    {
                        SensorType.TurboFlowPS => new PressureSensor(modbusProcessors
                                .FirstOrDefault(mb => mb.PortName == sensorViewModel.SelectedComPort),
                            new RegisterMapEnum<PressureSensorRegisterMap>(),
                            sensorViewModel.Address),
                        _ => new PressureSensor(modbusProcessors
                                .FirstOrDefault(mb => mb.PortName == sensorViewModel.SelectedComPort),
                            new RegisterMapEnum<PressureSensorRegisterMap>(),
                            sensorViewModel.Address)
                    };
                    break;
                case SensorPurpose.PressureOffsetSensor:
                    PressureSensor = sensorViewModel.SensorType switch
                    {
                        SensorType.TurboFlowPS => new PressureSensor(modbusProcessors
                                .FirstOrDefault(mb => mb.PortName == sensorViewModel.SelectedComPort),
                            new RegisterMapEnum<PressureSensorRegisterMap>(),
                            sensorViewModel.Address),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                case SensorPurpose.PressureDifferenceSensor:
                    PressureDifferenceSensor = sensorViewModel.SensorType switch
                    {
                        SensorType.TurboFlowPS => new PressureSensor(modbusProcessors
                                .FirstOrDefault(mb => mb.PortName == sensorViewModel.SelectedComPort),
                            new RegisterMapEnum<PressureSensorRegisterMap>(),
                            sensorViewModel.Address),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                case SensorPurpose.PressureDischargeSensor:
                    PressureDischargeSensor = sensorViewModel.SensorType switch
                    {
                        SensorType.Metran => new PressureSensor415M(modbusProcessors
                                .FirstOrDefault(mb => mb.PortName == sensorViewModel.SelectedComPort),
                            new RegisterMapEnum<PressureSensor415MRegisterMap>(),
                            sensorViewModel.Address),
                        _ => throw new ArgumentOutOfRangeException()
                    };
                    break;
                case SensorPurpose.HumiditySensor:
                    break;
                case SensorPurpose.TemperatureHumiditySensor:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public readonly List<IDevice> Devices = [];
    public readonly List<IMasterDevice> MasterDevices = [];
    private double? _currentFlow;
    public ITemperatureSensor? TemperatureSensor { get; set; }
    public IPressureSensor? PressureSensor { get; set; }
    public IPressureSensor? PressureDifferenceSensor { get; set; }
    public IPressureSensor? PressureDischargeSensor { get; set; }

    public int LineNumber { get; set; }

    public double? CurrentFlow
    {
        get => _currentFlow;
        set => _currentFlow = value;
    }

    public float? CurrentCalculateFlow { get; set; }
}