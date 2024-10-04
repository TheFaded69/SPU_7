using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class PulseCountMeterModuleTestViewModel : ViewModelBase, IDialogAware
{
    private readonly IStandController _standController;
    private readonly IStandSettingsService _standSettingsService;

    public PulseCountMeterModuleTestViewModel(IStandController standController,
        IStandSettingsService standSettingsService)
    {
        Title = "Поверка импульсного канала";

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
    private bool _isOnPulseCountWork = true;
    private bool _isOnPulseDurationWork;
    private DevicePulseCountMeterModuleViewModel _selectedDevicePulseCountMeterModuleViewModel;
    private bool _isOffPulseDurationWork;
    private bool _isOffPulseCountWork;

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

    public float? PulsePeriod
    {
        get => _pulsePeriod;
        set => SetProperty(ref _pulsePeriod, value);
    }

    public bool IsOnPulseCountWork
    {
        get => _isOnPulseCountWork;
        set => SetProperty(ref _isOnPulseCountWork, value);
    }

    public bool IsOffPulseCountWork
    {
        get => _isOffPulseCountWork;
        set => SetProperty(ref _isOffPulseCountWork, value);
    }

    public bool IsOnPulseDurationWork
    {
        get => _isOnPulseDurationWork;
        set => SetProperty(ref _isOnPulseDurationWork, value);
    }

    public bool IsOffPulseDurationWork
    {
        get => _isOffPulseDurationWork;
        set => SetProperty(ref _isOffPulseDurationWork, value);
    }

    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    public DelegateCommand StartPulseCountCommand { get; }

    private async void StartPulseCountCommandHandler()
    {
        IsOnPulseCountWork = false;

        var pulseCountMeterModuleNumber = _standSettingsService.StandSettingsModel
            .LineViewModels[SelectedDevicePulseCountMeterModuleViewModel.LineNumber - 1]
            .DeviceViewModels[SelectedDevicePulseCountMeterModuleViewModel.DeviceNumber - 1]
            .PulseCountMeterModuleNumber;
        var pulseCountMeterModuleChannelNumber = SelectedDevicePulseCountMeterModuleViewModel.PulseCountMeterChannelNumber;
        
        await _standController.ResetPulseCountMeterAsync();
        await _standController.TurnOnPulseCountMeterControlRegister();
        await _standController.SetPulseCountMeterModuleChannelSettingsAsync(pulseCountMeterModuleNumber - 1,
            pulseCountMeterModuleChannelNumber);

        await Task.Delay(2000);

        await _standController.StartPulseCountMeterModuleMeasureAsync(pulseCountMeterModuleNumber - 1);

        await Task.Delay(2000);

        await _standController.SendStartPulseCountMeterCommandAsync();

        IsOffPulseCountWork = true;
    }

    public DelegateCommand StartPulseDurationCommand { get; }

    private async void StartPulseDurationCommandHandler()
    {
        var pulseCountMeterModuleNumber = _standSettingsService.StandSettingsModel
            .LineViewModels[SelectedDevicePulseCountMeterModuleViewModel.LineNumber - 1]
            .DeviceViewModels[SelectedDevicePulseCountMeterModuleViewModel.DeviceNumber - 1]
            .PulseCountMeterModuleNumber;
        var pulseCountMeterModuleChannelNumber = SelectedDevicePulseCountMeterModuleViewModel.PulseCountMeterChannelNumber;

        IsOnPulseDurationWork = true;

        await _standController.ResetPulseCountMeterAsync();
        await _standController.TurnOnPulseCountMeterControlRegister();
        await _standController.SetPulseCountMeterModuleChannelSettingsAsync(pulseCountMeterModuleNumber - 1,
            pulseCountMeterModuleChannelNumber);

        await Task.Delay(2000);

        await _standController.StartPulseCountMeterModuleMeasureAsync(pulseCountMeterModuleNumber - 1);

        await Task.Delay(2000);

        await _standController.SendStartPulseCountMeterCommandAsync();
    }

    public DelegateCommand StopPulseCountCommand { get; }

    private async void StopPulseCountCommandHandler()
    {
        IsOffPulseCountWork = false;
        
        var pulseCountMeterModuleNumber = _standSettingsService.StandSettingsModel
            .LineViewModels[SelectedDevicePulseCountMeterModuleViewModel.LineNumber - 1]
            .DeviceViewModels[SelectedDevicePulseCountMeterModuleViewModel.DeviceNumber - 1]
            .PulseCountMeterModuleNumber;
        var pulseCountMeterModuleChannelNumber = SelectedDevicePulseCountMeterModuleViewModel.PulseCountMeterChannelNumber;

        await _standController.SendStartPulseCountMeterCommandAsync();

        await Task.Delay(2000);


        PulseCount = await _standController.ReadPulseCountFromPulseCountMeterAsync(pulseCountMeterModuleNumber - 1,
            pulseCountMeterModuleChannelNumber);
        PulsePeriod = await _standController.ReadPulsePeriodFromPulseCountMeterAsync(pulseCountMeterModuleNumber - 1,
            pulseCountMeterModuleChannelNumber) / 1000;
        
        IsOnPulseCountWork = true;

    }

    public DelegateCommand StopPulseDurationCommand { get; }

    private async void StopPulseDurationCommandHandler()
    {
        var pulseCountMeterModuleNumber = _standSettingsService.StandSettingsModel
            .LineViewModels[SelectedDevicePulseCountMeterModuleViewModel.LineNumber - 1]
            .DeviceViewModels[SelectedDevicePulseCountMeterModuleViewModel.DeviceNumber - 1]
            .PulseCountMeterModuleNumber;
        var pulseCountMeterModuleChannelNumber = SelectedDevicePulseCountMeterModuleViewModel.PulseCountMeterChannelNumber;

        await _standController.SendStartPulseCountMeterCommandAsync();

        await Task.Delay(2000);

        IsOnPulseDurationWork = false;

        PulsePeriod = await _standController.ReadPulsePeriodFromPulseCountMeterAsync(pulseCountMeterModuleNumber - 1,
            pulseCountMeterModuleChannelNumber);
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
                if (deviceViewModel.PulseCountMeterModuleChannelNumber is not (null and 0))
                    DeviceInformation.Add(new DevicePulseCountMeterModuleViewModel()
                    {
                        LineNumber = lineViewModel.LineNumber,
                        DeviceNumber = deviceViewModel.Number,
                        PulseCountMeterChannelNumber = (int)deviceViewModel.PulseCountMeterModuleChannelNumber
                    });
                
                if (deviceViewModel.PulseMeterChannelNumber != 0)
                    DeviceInformation.Add(new DevicePulseCountMeterModuleViewModel()
                    {
                        LineNumber = lineViewModel.LineNumber,
                        DeviceNumber = deviceViewModel.Number,
                        PulseCountMeterChannelNumber = deviceViewModel.PulseMeterChannelNumber
                    });
            }
        }
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, Action positive, Action negative)
    {
        dialogService.ShowDialog(nameof(PulseCountMeterModuleTestView), null, result =>
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