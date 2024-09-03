using System;
using Prism.Commands;
using SPU_7.Common.Line;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class FanItemViewModel : ViewModelBase
{
    private readonly IStandController _standController;
    private readonly IStandSettingsService _settingsService;
    private readonly int _lineIndex;
    private readonly int _fanIndex;
    private readonly int _regulatorIndex;
    private readonly int _bitIndex;

    public FanItemViewModel(IStandController standController,
        IStandSettingsService settingsService,
        StandSettingsValveModel standSettingsValveModel,
        StandSettingsNeedleValveModel settingsNeedleValveModel,
        int lineIndex,
        int fanIndex)
    {
        _standController = standController;
        _settingsService = settingsService;
        _lineIndex = lineIndex;
        _regulatorIndex = fanIndex;
        _bitIndex = fanIndex;
        _fanIndex = fanIndex;

        if (lineIndex > 0)
        {
            for (var i = settingsService.StandSettingsModel.LineViewModels.Count - lineIndex - 1; i >= 0; i--)
            {
                foreach (var fanViewModel in settingsService.StandSettingsModel.LineViewModels[i].FanViewModels)
                {
                    if (fanViewModel.SelectedFanType == FanType.FrequencyControlFan)
                        _regulatorIndex++;
                }
            }
        }

        if (standSettingsValveModel != null)
            ValveItemViewModel = new ValveItemViewModel(standSettingsValveModel, standController,
                standSettingsValveModel.IsReverseValve ? StateType.Close : StateType.Open);

        if (settingsNeedleValveModel != null)
            NeedleValveItemViewModel =
                new NeedleValveItemViewModel(settingsNeedleValveModel, standController, StateType.Close);

        EnableFanCommand = new DelegateCommand(EnableFanCommandHandler);
        DisableFanCommand = new DelegateCommand(DisableFanCommandHandler);

        FanHeightValue = settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[fanIndex]
            .IsNeedleValveEnable
            ? 100
            : 20;

        _isValveEnable = settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[fanIndex]
            .IsValveEnable;
        _isFrequencyEnable =
            settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[fanIndex]
                .SelectedFanType == FanType.FrequencyControlFan;
    }

    public int FanHeightValue { get; set; }

    private ValveItemViewModel _valveItemViewModel;
    private bool _isFanWorking;
    private bool _isNeedleValveEnable;
    private int _selectedNeedleValue;
    private bool _isValveEnable;
    private float _fanFrequencyValue;
    private NeedleValveItemViewModel _needleValveItemViewModel;
    private bool _isFrequencyEnable;

    public bool IsValveEnable
    {
        get => _isValveEnable;
        set => SetProperty(ref _isValveEnable, value);
    }

    public ValveItemViewModel ValveItemViewModel
    {
        get => _valveItemViewModel;
        set => SetProperty(ref _valveItemViewModel, value);
    }

    public NeedleValveItemViewModel NeedleValveItemViewModel
    {
        get => _needleValveItemViewModel;
        set => SetProperty(ref _needleValveItemViewModel, value);
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

    public float FanFrequencyValue
    {
        get => _fanFrequencyValue;
        set => SetProperty(ref _fanFrequencyValue, value);
    }

    public DelegateCommand EnableFanCommand { get; set; }

    private async void EnableFanCommandHandler()
    {
        switch (_settingsService.StandSettingsModel.LineViewModels[_lineIndex]
                    .FanViewModels[_fanIndex]
                    .SelectedFanType)
        {
            case FanType.FrequencyControlFan:
                await _standController.SetRegulatorFrequencyAsync(_regulatorIndex, FanFrequencyValue);
                await _standController.EnableFrequencyRegulatorAsync(_regulatorIndex);
                IsFanWorking = true;
                break;
            case FanType.ControlModuleControlFan:
                await _standController.EnableLineFanWorkAsync(_lineIndex, _bitIndex);
                IsFanWorking = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public DelegateCommand DisableFanCommand { get; set; }

    public int SelectedNeedleValue
    {
        get => _selectedNeedleValue;
        set => SetProperty(ref _selectedNeedleValue, value);
    }

    private async void DisableFanCommandHandler()
    {
        switch (_settingsService.StandSettingsModel.LineViewModels[_lineIndex]
                    .FanViewModels[_fanIndex]
                    .SelectedFanType)
        {
            case FanType.FrequencyControlFan:
                await _standController.DisableFrequencyRegulatorAsync(_regulatorIndex);
                IsFanWorking = false;
                break;
            case FanType.ControlModuleControlFan:
                await _standController.DisableLineFanWorkAsync(_lineIndex, _bitIndex);
                IsFanWorking = false;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool IsFrequencyEnable
    {
        get => _isFrequencyEnable;
        set => SetProperty(ref _isFrequencyEnable, value);
    }
}