namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsFanModel
{
    public StandSettingsValveModel ValveViewModel { get; set; }
    
    public bool IsNeedleValveEnable { get; set; }
    
    public StandSettingsNeedleValveModel NeedleValveViewModel { get; set; }
}