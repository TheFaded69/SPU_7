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
}