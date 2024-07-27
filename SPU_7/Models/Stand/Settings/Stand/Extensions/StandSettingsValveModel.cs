namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsValveModel
{
    public int Number{ get; set; }
    public int? Address{ get; set; }
    public string RegisterAddress{ get; set; }
    public int? BitNumber{ get; set; }
    public int? StateOnAddress{ get; set; }
    public string StateOnRegisterAddress{ get; set; }
    public int? StateOnBitNumber{ get; set; }
    public int? StateOffAddress{ get; set; }
    public string StateOffRegisterAddress{ get; set; }
    public int? StateOffBitNumber{ get; set; }
    public bool IsControlState { get; set; }
    //public bool IsTubeValve{ get; set; }
    //public bool IsPressureDifferenceValve{ get; set; }
    
    public bool IsReverseValve { get; set; }
    public int LineNumber { get; set; } 
}