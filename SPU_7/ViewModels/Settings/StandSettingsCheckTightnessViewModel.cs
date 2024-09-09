namespace SPU_7.ViewModels.Settings;

public class StandSettingsCheckTightnessViewModel : ViewModelBase
{
    private float? _targetPressureDifference;
    private int? _stabilizationTime;
    private int? _testTime;
    private float? _minimumFlow;
    private float? _maximumPressureDifference;
    private float? _insideVolume;
    private float? _goodRange;
    private float? _targetFrequency;
    private float? _minimumPressureDischarge;

    public float? TargetPressureDifference
    {
        get => _targetPressureDifference;
        set => SetProperty(ref _targetPressureDifference, value);
    }

    public int? StabilizationTime
    {
        get => _stabilizationTime;
        set => SetProperty(ref _stabilizationTime, value);
    }

    public int? TestTime
    {
        get => _testTime;
        set => SetProperty(ref _testTime, value);
    }

    public float? MinimumFlow
    {
        get => _minimumFlow;
        set => SetProperty(ref _minimumFlow, value);
    }

    public float? MaximumPressureDifference
    {
        get => _maximumPressureDifference;
        set => SetProperty(ref _maximumPressureDifference, value);
    }

    public float? InsideVolume
    {
        get => _insideVolume;
        set => SetProperty(ref _insideVolume, value);
    }

    public float? GoodRange
    {
        get => _goodRange;
        set => SetProperty(ref _goodRange, value);
    }

    public float? TargetFrequency
    {
        get => _targetFrequency;
        set => SetProperty(ref _targetFrequency, value);
    }

    public float? MinimumPressureDischarge
    {
        get => _minimumPressureDischarge;
        set => SetProperty(ref _minimumPressureDischarge, value);
    }

    public int Number { get; set; }
}