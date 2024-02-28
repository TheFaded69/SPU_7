namespace SPU_7.ViewModels.WorkReportViewModels.ResultViewModels.Validation;

public class ValidationDeviceResultViewModel : ViewModelBase
{
    private int _pointNumber;
    private int _measureNumber;
    private double _validationVolumeTime;
    private double _targetVolume;
    private double? _targetVolumeDifference;
    private double? _volumeDifference;
    private double? _startVolumeValue;
    private double? _endVolumeValue;
    private double? _targetFlow;
    private double? _calculateFlow;
    private double? _validationFlowTime;
    private double? _flowDifference;

    public int PointNumber
    {
        get => _pointNumber;
        set => SetProperty(ref _pointNumber, value);
    }

    public int MeasureNumber
    {
        get => _measureNumber;
        set => SetProperty(ref _measureNumber, value);
    }

    public double ValidationVolumeTime
    {
        get => _validationVolumeTime;
        set => SetProperty(ref _validationVolumeTime, value);
    }

    public double TargetVolume
    {
        get => _targetVolume;
        set => SetProperty(ref _targetVolume, value);
    }

    public double? TargetVolumeDifference
    {
        get => _targetVolumeDifference;
        set => SetProperty(ref _targetVolumeDifference, value);
    }

    public double? VolumeDifference
    {
        get => _volumeDifference;
        set => SetProperty(ref _volumeDifference, value);
    }

    public double? StartVolumeValue
    {
        get => _startVolumeValue;
        set => SetProperty(ref _startVolumeValue, value);
    }

    public double? EndVolumeValue
    {
        get => _endVolumeValue;
        set => SetProperty(ref _endVolumeValue, value);
    }

    public double? TargetFlow
    {
        get => _targetFlow;
        set => SetProperty(ref _targetFlow, value);
    }

    public double? CalculateFlow
    {
        get => _calculateFlow;
        set => SetProperty(ref _calculateFlow, value);
    }

    public double? ValidationFlowTime
    {
        get => _validationFlowTime;
        set => SetProperty(ref _validationFlowTime, value);
    }

    public double? FlowDifference
    {
        get => _flowDifference;
        set => SetProperty(ref _flowDifference, value);
    }
}