namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsFrequencyRegulatorModel
{
    public string PortName { get; set; }
    public int ModuleAddress { get; set; }
    public double kP { get; set; }
    public double kI { get; set; }
    public double kD { get; set; }
    public double pvMax { get; set; }
    public double pvMin { get; set; }
    public double outMax { get; set; }
    public double outMin { get; set; }
}