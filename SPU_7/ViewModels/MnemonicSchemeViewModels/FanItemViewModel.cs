using Prism.Commands;
using System.Linq;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class FanItemViewModel : ViewModelBase
{
    private readonly IStandController _standController;

    public FanItemViewModel(IStandController standController,
        IStandSettingsService settingsService,
        StandSettingsValveModel standSettingsValveModel,
        int lineIndex,
        int fanIndex)
    {
        _standController = standController;

        ValveItemViewModel = new ValveItemViewModel(standSettingsValveModel, standController);

        EnableFanCommand = new DelegateCommand(EnableFanCommandHandler);
        DisableFanCommand = new DelegateCommand(DisableFanCommandHandler);

        FanHeightValue = settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[fanIndex].IsNeedleValveEnable ? 95 : 20;

    }

    public int FanHeightValue { get; set; }

    private ValveItemViewModel _valveItemViewModel;
    private bool _isFanWorking;
    private bool _isNeedleValveEnable;
    private int _selectedNeedleValue;

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

    public bool IsNeedleValveEnable
    {
        get => _isNeedleValveEnable;
        set => SetProperty(ref _isNeedleValveEnable, value);
    }

    public DelegateCommand EnableFanCommand { get; set; }

    private void EnableFanCommandHandler()
    {
        IsFanWorking = true;
    }
    
    public DelegateCommand DisableFanCommand { get; set; }

    public int SelectedNeedleValue
    {
        get => _selectedNeedleValue;
        set => SetProperty(ref _selectedNeedleValue, value);
    }

    private void DisableFanCommandHandler()
    {
        IsFanWorking = false;
    }
}