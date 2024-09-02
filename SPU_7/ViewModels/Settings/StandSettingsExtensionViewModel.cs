namespace SPU_7.ViewModels.Settings;

public class StandSettingsExtensionViewModel : ViewModelBase
{
    private bool _isCheckAverageQuadraticDifferenceEnable;
    private bool _isCheckPulseCountMeterModuleEnable;
    private bool _isCheckTightnessEnable;
    private bool _isPulseCountMeterModuleEnable;
    private bool _isPulseMeterCoefficientSettingsEnable;
    private bool _isVacuumCreatorEnable;

    public StandSettingsExtensionViewModel()
    {
        
    }

    public bool IsCheckAverageQuadraticDifferenceEnable
    {
        get => _isCheckAverageQuadraticDifferenceEnable;
        set => SetProperty(ref _isCheckAverageQuadraticDifferenceEnable, value);
    }

    public bool IsCheckPulseCountMeterModuleEnable
    {
        get => _isCheckPulseCountMeterModuleEnable;
        set => SetProperty(ref _isCheckPulseCountMeterModuleEnable, value);
    }

    public bool IsCheckTightnessEnable
    {
        get => _isCheckTightnessEnable;
        set => SetProperty(ref _isCheckTightnessEnable, value);
    }

    public bool IsPulseCountMeterModuleEnable
    {
        get => _isPulseCountMeterModuleEnable;
        set => SetProperty(ref _isPulseCountMeterModuleEnable, value);
    }

    public bool IsPulseMeterCoefficientSettingsEnable
    {
        get => _isPulseMeterCoefficientSettingsEnable;
        set => SetProperty(ref _isPulseMeterCoefficientSettingsEnable, value);
    }

    public bool IsVacuumCreatorEnable
    {
        get => _isVacuumCreatorEnable;
        set => SetProperty(ref _isVacuumCreatorEnable, value);
    }
}