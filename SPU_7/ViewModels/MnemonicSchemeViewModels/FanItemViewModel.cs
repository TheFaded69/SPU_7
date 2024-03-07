using Prism.Commands;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class FanItemViewModel : ViewModelBase
{
    private readonly IStandController _standController;

    public FanItemViewModel(IStandController standController, StandSettingsValveModel standSettingsValveModel)
    {
        _standController = standController;

        ValveItemViewModel = new ValveItemViewModel(standSettingsValveModel, standController);

        EnableFanCommand = new DelegateCommand(EnableFanCommandHandler);
        DisableFanCommand = new DelegateCommand(DisableFanCommandHandler);
    }
    private ValveItemViewModel _valveItemViewModel;
    private bool _isFanWorking;

    public ValveItemViewModel ValveItemViewModel
    {
        get => _valveItemViewModel;
        set => SetProperty(ref _valveItemViewModel, value);
    }

    public bool IsFanWorking
    {
        get => _isFanWorking;
        set => SetProperty(ref _isFanWorking, value);
    }
    
    public DelegateCommand EnableFanCommand { get; set; }

    private void EnableFanCommandHandler()
    {
        IsFanWorking = true;
    }
    
    public DelegateCommand DisableFanCommand { get; set; }

    private void DisableFanCommandHandler()
    {
        IsFanWorking = false;
    }
}