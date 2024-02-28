using System;
using System.Collections.ObjectModel;
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

        switch (settingsService.StandSettingsModel.LineViewModels[lineIndex].SelectedDeviceLineType)
        {
            case DeviceLineType.MembraneDevice:
                IsMembraneDevice = true;
                break;
            case DeviceLineType.JetDevice:
                IsJetDevice = true;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        for (var i = 0; i < settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels.Count; i++)
        {
            DeviceItemViewModels.Add(new DeviceItemViewModel(standController, settingsService,i, lineIndex));
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
    private bool _isMembraneDevice;
    private bool _isJetDevice;

    public ObservableCollection<DeviceItemViewModel> DeviceItemViewModels { get; set; } = new();

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

    public bool IsMembraneDevice
    {
        get => _isMembraneDevice;
        set => SetProperty(ref _isMembraneDevice, value);
    }

    public bool IsJetDevice
    {
        get => _isJetDevice;
        set => SetProperty(ref _isJetDevice, value);
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
    
    private async void  CloseAllCommandHandler()
    {
        await _standController.SetFlowDirectionAsync(LineDirectionFlowState.AllClose, LineNumber);
    }
    
    public DelegateCommand OpenAllCommand { get; set; }
    
    private async void  OpenAllCommandHandler()
    {
        await _standController.SetFlowDirectionAsync(LineDirectionFlowState.AllOpen, LineNumber);
    }
}