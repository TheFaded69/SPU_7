namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsValveModel
{
    public int Number{ get; set; }
    public int? Address{ get; set; }
    public string RegisterAddress{ get; set; }
    public int? BitNumber{ get; set; }
    public int? StateAddress{ get; set; }
    public string StateRegisterAddress{ get; set; }
    public int? StateBitNumber{ get; set; }
    public bool IsControlState { get; set; }
    public bool IsTubeValve{ get; set; }
    public bool IsPressureDifferenceValve{ get; set; }
    
    public bool IsReverseValve { get; set; }
    public int LineNumber { get; set; } 
}