namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsDeviceModel
{
    public int Number;
    public int PressureSensorAddress;
    public int TemperatureSensorAddress;
    public int PulseMeterNumber;
    public int PulseMeterChannelNumber;
    public string RegisterAddress{ get; set; }
    public int? BitNumber{ get; set; }
    public int? StateAddress{ get; set; }
    public string StateRegisterAddress{ get; set; }
    public int? StateBitNumber{ get; set; }
    public bool IsControlState { get; set; }
    public int? Address { get; set; }
    
    public int? PulseCountMeterModuleNumber { get; set; }

    public int? PulseCountMeterModuleChannelNumber { get; set; }
    
    public string SelectedPressureSensorComPort { get; set; }

    public string SelectedTemperatureSensorComPort { get; set; }
    
    public int? TemperatureChannelNumber { get; set; }
}