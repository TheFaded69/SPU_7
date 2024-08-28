namespace SPU_7.ViewModels;

public class LineInformationViewModel : ViewModelBase
{
    public LineInformationViewModel()
    {
        
    }
    private float? _pressureDifference;
    private float? _pressure;
    private float? _pressureDischarged;
    private float? _currentFlow;


    public float? PressureDifference
    {
        get => _pressureDifference;
        set => SetProperty(ref _pressureDifference, value);
    }

    public float? Pressure
    {
        get => _pressure;
        set => SetProperty(ref _pressure, value);
    }

    public float? PressureDischarged
    {
        get => _pressureDischarged;
        set => SetProperty(ref _pressureDischarged, value);
    }

    public float? CurrentFlow
    {
        get => _currentFlow;
        set => SetProperty(ref _currentFlow, value);
    }
}