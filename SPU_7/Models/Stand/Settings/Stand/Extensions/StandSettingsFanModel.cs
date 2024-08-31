using SPU_7.Common.Line;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsFanModel
{
    public bool IsValveEnable { get; set; }
    public StandSettingsValveModel FanValveViewModel { get; set; }
    public bool IsNeedleValveEnable { get; set; }

    public StandSettingsNeedleValveModel NeedleValveViewModel { get; set; }
    
    public StandSettingsFrequencyRegulatorModel FrequencyRegulatorViewModel { get; set; }
    
    public float? MinimumFlow { get; set; }
    
    public float? MaximumFlow { get; set; }
    
    public string SelectedFanTypeString { get; set; }
    
    public FanType SelectedFanType { get; set; }
    
    public int Address { get; set; }
    
    public int BitIndex { get; set; }
    
    public string SelectedComPort { get; set; }
    
    public bool IsFrequencyRegulatorSettingsEnable { get; set; }

    public bool IsControlModuleSettingsEnable { get; set; }
}