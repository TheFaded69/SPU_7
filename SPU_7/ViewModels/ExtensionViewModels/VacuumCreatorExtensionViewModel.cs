using Prism.Commands;
using SPU_7.Models.Stand;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class VacuumCreatorExtensionViewModel : ViewModelBase
{
    public VacuumCreatorExtensionViewModel(IStandController standController)
    {
        _standController = standController;

        EnableVacuumCreatorCommand = new DelegateCommand(EnableVacuumCreatorCommandHandler);
        DisableVacuumCreatorCommand = new DelegateCommand(DisableVacuumCreatorCommandHandler);
    }

    private readonly IStandController _standController;

    private double? _frequencyRegulatorValue;
    private bool _isFrequencyRegulatorWork;

    public DelegateCommand EnableVacuumCreatorCommand { get; }

    private async void EnableVacuumCreatorCommandHandler()
    {
#if !DEBUGGUI
            _standController.PidEnable();
            await _standController.EnableVacuumCreator();
#endif
    }

    public DelegateCommand DisableVacuumCreatorCommand { get; }

    private async void DisableVacuumCreatorCommandHandler()
    {
#if !DEBUGGUI
            _standController.PidDisable();
            await _standController.DisableVacuumCreator();
#endif
    }

    public double? FrequencyRegulatorValue
    {
        get => _frequencyRegulatorValue;
        set => SetProperty(ref _frequencyRegulatorValue, value);
    }

    public bool IsFrequencyRegulatorWork
    {
        get => _isFrequencyRegulatorWork;
        set => SetProperty(ref _isFrequencyRegulatorWork, value);
    }
}