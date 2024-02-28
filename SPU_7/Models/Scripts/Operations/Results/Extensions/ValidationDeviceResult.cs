namespace SPU_7.Models.Scripts.Operations.Results.Extensions;

public class ValidationDeviceResult
{
    public int PointNumber{ get; set; }
    public int MeasureNumber{ get; set; }
    ///////////////////
    public double ValidationVolumeTime{ get; set; }
    public double TargetVolume{ get; set; }
    public double? TargetVolumeDifference { get; set; }
    public double? VolumeDifference{ get; set; }
    ///////////////////
    public double? StartVolumeValue { get; set; }
    public double? EndVolumeValue { get; set; }
    public double? TargetFlow { get; set; }
    public double? CalculateFlow { get; set; }
    public double? ValidationFlowTime { get; set; }
    public double? FlowDifference { get; set; }
    ////////
    public string VendorNumber { get; set; }
    public int ProtocolNumber { get; set; }
    ////////
    public float? PressureDifference { get; set; }
    
}