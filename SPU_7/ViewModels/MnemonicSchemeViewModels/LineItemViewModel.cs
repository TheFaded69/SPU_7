using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Material.Styles.Converters;
using Prism.Commands;
using SPU_7.Common.Line;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class LineItemViewModel : ViewModelBase
{
    public LineItemViewModel(INotificationService notificationService,
        IStandSettingsService settingsService,
        IStandController standController,
        int lineIndex)
    {
        _notificationService = notificationService;
        _settingsService = settingsService;
        _standController = standController;

        var deviceCount = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels.Count;
        var deviceMaxCount = settingsService.StandSettingsModel.LineViewModels
            .Select(lineViewModel => lineViewModel.DeviceViewModels.Count)
            .Prepend(0)
            .Max();

        AfterDeviceWidthValue = 100 + 160 * (deviceMaxCount - deviceCount);
        switch (settingsService.StandSettingsModel.LineViewModels[lineIndex].SelectedLineType)
        {
            case LineType.None:
                break;
            case LineType.MasterDeviceLineType:
                IsMasterDeviceVisible = true;
                IsFanVisible = true;
                ValidationHeightValue = 20 + (settingsService.StandSettingsModel.LineViewModels[lineIndex].MasterDeviceViewModels.Count - 1) * 90;

            {
                double firstHeight = 0;
                double secondHeight = 0;
                double totalHeight = 0;
                double fanHeight = 0;
                double offsetTop = 0;
                double heightWithoutNeedle = 110;
                double heightWithNeedle = 170;

                if (settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels.Count != 1)
                {
                    for (var i = 0; i < settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels.Count; i++)
                    {
                        if (i == 0 || i == settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels.Count - 1)
                        {
                            if (i == 0)
                            {
                                firstHeight = settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[i].IsNeedleValveEnable ? heightWithNeedle : heightWithoutNeedle;
                            }
                            totalHeight += settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[i].IsNeedleValveEnable ? heightWithNeedle : heightWithoutNeedle;
                            fanHeight += settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[i].IsNeedleValveEnable ? heightWithNeedle / 2 : heightWithoutNeedle / 2;
                        }
                        else
                        {
                            totalHeight += settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[i].IsNeedleValveEnable ? heightWithNeedle : heightWithoutNeedle;
                            fanHeight += settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[i].IsNeedleValveEnable ? heightWithNeedle : heightWithoutNeedle;
                        }
                    }

                    offsetTop = (firstHeight / 2) - (totalHeight - fanHeight) / 2;
                }

                FanHeightValue = fanHeight + 20;
                FanHeightMargin = new Thickness(0, offsetTop * 2, 0, 0);
            }


                break;
            case LineType.NozzleLineType:
                IsNozzleVisible = true;
                IsVacuumValveVisible = true;
                ValidationHeightValue = 20 + (settingsService.StandSettingsModel.LineViewModels[lineIndex].NozzleViewModels.Count - 1) * 80;
                FanHeightValue = 20 + (settingsService.StandSettingsModel.LineViewModels[lineIndex].VacuumValveViewModels.Count - 1) * 90;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        for (var i = 0; i < settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels.Count; i++)
        {
            DeviceItemViewModels.Add(new DeviceItemViewModel(standController, settingsService, i, lineIndex));
        }

        if (settingsService.StandSettingsModel.LineViewModels[lineIndex].IsStartCommonValve)
            StartCommonValveViewModel = new ValveItemViewModel(settingsService.StandSettingsModel.LineViewModels[lineIndex].StartCommonValveViewModel,
                _standController);
        
        if (settingsService.StandSettingsModel.LineViewModels[lineIndex].IsStartValveMasterDevice)
            StartValveViewModel = new ValveItemViewModel(settingsService.StandSettingsModel.LineViewModels[lineIndex].StartValveMasterDeviceViewModel,
                _standController);
        
        for (var i = 0; i < settingsService.StandSettingsModel.LineViewModels[lineIndex].MasterDeviceViewModels.Count; i++)
        {
            MasterDeviceItemViewModels.Add(new MasterDeviceItemViewModel(_standController,
                settingsService.StandSettingsModel.LineViewModels[lineIndex].MasterDeviceViewModels[i].ValveViewModel));
        }

        for (var i = 0; i < settingsService.StandSettingsModel.LineViewModels[lineIndex].NozzleViewModels.Count; i++)
        {
            NozzleItemViewModels.Add(
                new NozzleItemViewModel(settingsService.StandSettingsModel.LineViewModels[lineIndex].NozzleViewModels[i], _standController));
        }
        
        if (settingsService.StandSettingsModel.LineViewModels[lineIndex].IsEndValveMasterDevice)
            EndValveViewModel = new ValveItemViewModel(settingsService.StandSettingsModel.LineViewModels[lineIndex].EndValveMasterDeviceViewModel,
                _standController);
        
        if (settingsService.StandSettingsModel.LineViewModels[lineIndex].IsEndCommonValve)
            EndCommonValveViewModel = new ValveItemViewModel(settingsService.StandSettingsModel.LineViewModels[lineIndex].EndCommonValveViewModel,
                _standController);

        for (var i = 0; i < settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels.Count; i++)
        {
            FanItemViewModels.Add(new FanItemViewModel(_standController,
                _settingsService,
                settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[i].ValveViewModel,
                lineIndex,
                i)
            {
                IsNeedleValveEnable = settingsService.StandSettingsModel.LineViewModels[lineIndex].FanViewModels[i].IsNeedleValveEnable
            });
        }

        for (var i = 0; i < settingsService.StandSettingsModel.LineViewModels[lineIndex].VacuumValveViewModels.Count; i++)
        {
            VacuumItemViewModels.Add(new VacuumItemViewModel());
        }

        LineNumber = lineIndex + 1;

        ReverseFlowCommand = new DelegateCommand(ReverseFlowCommandHandler);
        SetLineStateCommand = new DelegateCommand(SetLineStateCommandHandler);
        CloseAllCommand = new DelegateCommand(CloseAllCommandHandler);
        OpenAllCommand = new DelegateCommand(OpenAllCommandHandler);
    }

    private readonly INotificationService _notificationService;
    private readonly IStandSettingsService _settingsService;
    private readonly IStandController _standController;

    private bool _isReverseLine;

    private DeviceLineType _selectedDeviceLineType;
    private bool _isLineActive;
    private LineDirectionFlowState _lineDirectionFlowState = LineDirectionFlowState.AllOpen;
    private int _lineNumber;
    private LineType _selectedLineType;
    private bool _isMasterDeviceVisible;
    private bool _isNozzleVisible;
    private bool _isFanVisible;
    private bool _isVacuumValveVisible;

    public bool IsMasterDeviceVisible
    {
        get => _isMasterDeviceVisible;
        set => SetProperty(ref _isMasterDeviceVisible, value);
    }

    public bool IsNozzleVisible
    {
        get => _isNozzleVisible;
        set => SetProperty(ref _isNozzleVisible, value);
    }

    public bool IsFanVisible
    {
        get => _isFanVisible;
        set => SetProperty(ref _isFanVisible, value);
    }

    public bool IsVacuumValveVisible
    {
        get => _isVacuumValveVisible;
        set => SetProperty(ref _isVacuumValveVisible, value);
    }

    public int AfterDeviceWidthValue { get; set; }
    public int ValidationHeightValue { get; set; }
    public double FanHeightValue { get; set; }
    public Thickness FanHeightMargin { get; set; }
    public ObservableCollection<DeviceItemViewModel> DeviceItemViewModels { get; set; } = new();
    
    public ValveItemViewModel StartCommonValveViewModel { get; set; }
    public ValveItemViewModel StartValveViewModel { get; set; }
    public ObservableCollection<MasterDeviceItemViewModel> MasterDeviceItemViewModels { get; set; } = new();
    public ObservableCollection<NozzleItemViewModel> NozzleItemViewModels { get; set; } = new();
    public ValveItemViewModel EndValveViewModel { get; set; }
    
    public ValveItemViewModel EndCommonValveViewModel { get; set; }
    public ObservableCollection<FanItemViewModel> FanItemViewModels { get; set; } = new();
    public ObservableCollection<VacuumItemViewModel> VacuumItemViewModels { get; set; } = new();

    public int LineNumber
    {
        get => _lineNumber;
        set => SetProperty(ref _lineNumber, value);
    }

    public bool IsReverseLine
    {
        get => _isReverseLine;
        set => SetProperty(ref _isReverseLine, value);
    }

    public bool IsLineActive
    {
        get => _isLineActive;
        set => SetProperty(ref _isLineActive, value);
    }

    public LineDirectionFlowState LineDirectionFlowState
    {
        get => _lineDirectionFlowState;
        set => SetProperty(ref _lineDirectionFlowState, value);
    }

    public DeviceLineType SelectedDeviceLineType
    {
        get => _selectedDeviceLineType;
        set => SetProperty(ref _selectedDeviceLineType, value);
    }

    public LineType SelectedLineType
    {
        get => _selectedLineType;
        set => SetProperty(ref _selectedLineType, value);
    }

    public DelegateCommand ReverseFlowCommand { get; }

    public async void ReverseFlowCommandHandler()
    {
        switch (LineDirectionFlowState)
        {
            case LineDirectionFlowState.AllOpen:
                await _standController.SetFlowDirectionAsync(LineDirectionFlowState.DirectDirection, LineNumber);
                break;
            case LineDirectionFlowState.AllClose:
                await _standController.SetFlowDirectionAsync(LineDirectionFlowState.DirectDirection, LineNumber);
                break;
            case LineDirectionFlowState.DirectDirection:
                await _standController.SetFlowDirectionAsync(LineDirectionFlowState.ReverseDirection, LineNumber);
                break;
            case LineDirectionFlowState.ReverseDirection:
                await _standController.SetFlowDirectionAsync(LineDirectionFlowState.DirectDirection, LineNumber);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public DelegateCommand SetLineStateCommand { get; }

    private void SetLineStateCommandHandler()
    {
        IsLineActive = !IsLineActive;

        _standController.SetActiveLine(LineNumber, IsLineActive);
    }


    public DelegateCommand CloseAllCommand { get; set; }

    private async void CloseAllCommandHandler()
    {
        await _standController.SetFlowDirectionAsync(LineDirectionFlowState.AllClose, LineNumber);
    }

    public DelegateCommand OpenAllCommand { get; set; }


    private async void OpenAllCommandHandler()
    {
        await _standController.SetFlowDirectionAsync(LineDirectionFlowState.AllOpen, LineNumber);
    }
}