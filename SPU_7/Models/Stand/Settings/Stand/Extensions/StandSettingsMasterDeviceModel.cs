using System.IO.Ports;
using SPU_7.Common.Device;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsMasterDeviceModel
{
    public string SelectedMasterDeviceTypeString { get; set; }
    public MasterDeviceType SelectedMasterDeviceType { get; set; }
    public string SelectedComPort { get; set; }
    
    public int MasterDeviceAddress { get; set; }
    public string VendorNumber { get; set; }
    public StandSettingsValveModel PressureSensorValveViewModel { get; set; }
    
    public StandSettingsValveModel MasterDeviceValveViewModel { get; set; }
    
    public int PressureSensorAddress { get; set; }
    
    public int TemperatureSensorAddress { get; set; }
    
    public string SelectedPressureSensorComPort { get; set; }

    public string SelectedTemperatureSensorComPort { get; set; }
    
    public int? TemperatureChannelNumber { get; set; }
    
    public int? PulseCountMeterModuleNumber { get; set; }

    public int? PulseCountMeterModuleChannelNumber { get; set; }

    public string MasterDeviceName { get; set; }

}