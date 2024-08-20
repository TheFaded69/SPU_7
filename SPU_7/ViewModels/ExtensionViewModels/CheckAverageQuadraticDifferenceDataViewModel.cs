namespace SPU_7.ViewModels.ExtensionViewModels;

public class CheckAverageQuadraticDifferenceDataViewModel : ViewModelBase
{
    private int _number;
    private float _flow;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public float Flow
    {
        get => _flow;
        set => SetProperty(ref _flow, value);
    }
}