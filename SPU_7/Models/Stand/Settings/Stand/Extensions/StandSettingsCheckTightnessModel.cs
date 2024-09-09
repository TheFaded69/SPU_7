namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsCheckTightnessModel
{
    public float? TargetPressureDifference { get; set; }   
    public int? StabilizationTime { get; set; }
    public int? TestTime { get; set; }
    public float? MinimumFlow { get; set; }
    public float? MaximumPressureDifference { get; set; }
    public float? InsideVolume { get; set; }
    public float? GoodRange { get; set; }
    public float? TargetFrequency { get; set; }
    public float? MinimumPressureDischarge { get; set; }
}