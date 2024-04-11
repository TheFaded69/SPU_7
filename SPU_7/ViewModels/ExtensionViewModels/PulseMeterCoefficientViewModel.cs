namespace SPU_7.ViewModels.ExtensionViewModels;

public class PulseMeterCoefficientViewModel : ViewModelBase
{
    private int _number;
    private float? _firstCoefficientRead;
    private float? _secondCoefficientRead;
    private float? _firstCoefficientWrite;
    private float? _secondCoefficientWrite;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public float? FirstCoefficientRead
    {
        get => _firstCoefficientRead;
        set => SetProperty(ref _firstCoefficientRead, value);
    }

    public float? SecondCoefficientRead
    {
        get => _secondCoefficientRead;
        set => SetProperty(ref _secondCoefficientRead, value);
    }

    public float? FirstCoefficientWrite
    {
        get => _firstCoefficientWrite;
        set => SetProperty(ref _firstCoefficientWrite, value);
    }

    public float? SecondCoefficientWrite
    {
        get => _secondCoefficientWrite;
        set => SetProperty(ref _secondCoefficientWrite, value);
    }
}