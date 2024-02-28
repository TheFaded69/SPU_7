namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public class ValidationPulseMeterConfigurationViewModel : ViewModelBase
{
    private double? _pulseWeight;

    public double? PulseWeight
    {
        get => _pulseWeight;
        set => SetProperty(ref _pulseWeight, value);
    }
}