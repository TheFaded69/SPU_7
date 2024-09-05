using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Domain.Devices.StandDevices.PulseMeter;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class PulseMeterModuleTestViewModel : ViewModelBase, IDialogAware
{
    private readonly IStandController _standController;
    private readonly IStandSettingsService _standSettingsService;

    public PulseMeterModuleTestViewModel(IStandController standController,
        IStandSettingsService standSettingsService)
    {
        Title = "Поверка импульсного канала БИПЧ";

        _standController = standController;
        _standSettingsService = standSettingsService;


        StartPulseCountCommand = new DelegateCommand(StartPulseCountCommandHandler);
        StartPulseDurationCommand = new DelegateCommand(StartPulseDurationCommandHandler);
        StopPulseCountCommand = new DelegateCommand(StopPulseCountCommandHandler);
        StopPulseDurationCommand = new DelegateCommand(StopPulseDurationCommandHandler);

        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
    }

    private int? _pulseCountMeterModuleNumber;
    private int? _channelNumber = 1;
    private float? _pulseCount;
    private float? _pulsePeriod;
    private bool _isPulseCountWork;
    private bool _isPulseDurationWork;
    private DevicePulseCountMeterModuleViewModel _selectedDevicePulseCountMeterModuleViewModel;

    public ObservableCollection<DevicePulseCountMeterModuleViewModel> DeviceInformation { get; set; } = [];

    public DevicePulseCountMeterModuleViewModel SelectedDevicePulseCountMeterModuleViewModel
    {
        get => _selectedDevicePulseCountMeterModuleViewModel;
        set => SetProperty(ref _selectedDevicePulseCountMeterModuleViewModel, value);
    }

    public int? PulseCountMeterModuleNumber
    {
        get => _pulseCountMeterModuleNumber;
        set => SetProperty(ref _pulseCountMeterModuleNumber, value);
    }

    public int? ChannelNumber
    {
        get => _channelNumber;
        set => SetProperty(ref _channelNumber, value);
    }

    public float? PulseCount
    {
        get => _pulseCount;
        set => SetProperty(ref _pulseCount, value);
    }
    
    public float? PulseCountStart { get; set; }

    public float? PulsePeriod
    {
        get => _pulsePeriod;
        set => SetProperty(ref _pulsePeriod, value);
    }

    public bool IsPulseCountWork
    {
        get => _isPulseCountWork;
        set => SetProperty(ref _isPulseCountWork, value);
    }

    public bool IsPulseDurationWork
    {
        get => _isPulseDurationWork;
        set => SetProperty(ref _isPulseDurationWork, value);
    }

    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    public DelegateCommand StartPulseCountCommand { get; }
    
    private async void StartPulseCountCommandHandler()
    {
        IsPulseCountWork = true;
        
        var pulseCount = await _standController.ReadFreeRunPulseCount(SelectedDevicePulseCountMeterModuleViewModel.PulseMeterNumber, SelectedDevicePulseCountMeterModuleViewModel.PulseMeterChannelNumber);
        PulseCountStart = pulseCount;
    }

    public DelegateCommand StartPulseDurationCommand { get; }

    private async void StartPulseDurationCommandHandler()
    {
        try
        {

        IsPulseDurationWork = true;
        
        await _standController.StartPulseMeterPeriodMeasureAsync(1, 
            SelectedDevicePulseCountMeterModuleViewModel.PulseMeterNumber,
            SelectedDevicePulseCountMeterModuleViewModel.PulseMeterChannelNumber);

        while (await _standController.GetPulseMeterStatusAsync(
                   SelectedDevicePulseCountMeterModuleViewModel.PulseMeterNumber,
                   SelectedDevicePulseCountMeterModuleViewModel.PulseMeterChannelNumber) != PulseMeter2ChannelState.Ok)
        {
            await Task.Delay(1000);
        }

        var period = await _standController.ReadPeriodFromPulseMeterAsync(
            SelectedDevicePulseCountMeterModuleViewModel.PulseMeterNumber,
            SelectedDevicePulseCountMeterModuleViewModel.PulseMeterChannelNumber) / 1000;

        PulsePeriod = period;
        IsPulseDurationWork = false;
        
        }
        catch (Exception e)
        {
            
        }
    }

    public DelegateCommand StopPulseCountCommand { get; }

    private async void StopPulseCountCommandHandler()
    {
        IsPulseCountWork = false;
        
        var pulseCount = await _standController.ReadFreeRunPulseCount(SelectedDevicePulseCountMeterModuleViewModel.PulseMeterNumber, SelectedDevicePulseCountMeterModuleViewModel.PulseMeterChannelNumber);
        PulseCount = pulseCount - PulseCountStart;
    }

    public DelegateCommand StopPulseDurationCommand { get; }

    private async void StopPulseDurationCommandHandler()
    {
        
    }

    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        foreach (var lineViewModel in _standSettingsService.StandSettingsModel.LineViewModels)
        {
            foreach (var deviceViewModel in lineViewModel.DeviceViewModels)
            {
               
                if (deviceViewModel.PulseMeterChannelNumber != null)
                    DeviceInformation.Add(new DevicePulseCountMeterModuleViewModel()
                    {
                        LineNumber = lineViewModel.LineNumber,
                        DeviceNumber = deviceViewModel.Number,
                        PulseMeterChannelNumber = deviceViewModel.PulseMeterChannelNumber,
                        PulseMeterNumber = deviceViewModel.PulseMeterNumber
                    });
                    
            }
        }
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, Action positive, Action negative)
    {
        dialogService.ShowDialog(nameof(PulseMeterModuleTestView), null, result =>
        {
            switch (result.Result)
            {
                case ButtonResult.Abort:
                    break;
                case ButtonResult.Cancel:
                    break;
                case ButtonResult.Ignore:
                    break;
                case ButtonResult.No:
                    negative?.Invoke();
                    break;
                case ButtonResult.None:
                    break;
                case ButtonResult.OK:
                    positive?.Invoke();
                    break;
                case ButtonResult.Retry:
                    break;
                case ButtonResult.Yes:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }
}