using SPU_7.Common.Line;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsSensorModel
{
    public int Number { get; set; }
    
    public string SelectedComPort { get; set; }

    public int Address { get; set; }
    
    public string SelectedSensorPurposes { get; set; }
    
    public SensorPurpose SensorPurpose { get; set; }

    public string SelectedSensorType { get; set; }
    
    public SensorType SensorType { get; set; }
    
    public int ChannelNumber { get; set; }
}