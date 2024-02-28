namespace SPU_7.ViewModels.Settings;

public class StandSettingsPulseMeterViewModel : ViewModelBase
{
    private int _number;
    private int? _address;
    private double? _firstCalibrateCoefficient;
    private double? _secondCalibrateCoefficient;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public int? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public double? FirstCalibrateCoefficient
    {
        get => _firstCalibrateCoefficient;
        set => SetProperty(ref _firstCalibrateCoefficient, value);
    }

    public double? SecondCalibrateCoefficient
    {
        get => _secondCalibrateCoefficient;
        set => SetProperty(ref _secondCalibrateCoefficient, value);
    }
}