using System;
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

    public PulseCountMeterModuleTestViewModel(IStandController standController, IStandSettingsService standSettingsService)
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
    private int? _pulseCount;
    private float? _pulseDuration;
    private bool _isPulseCountWork;
    private bool _isPulseDurationWork;


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

    public int? PulseCount
    {
        get => _pulseCount;
        set => SetProperty(ref _pulseCount, value);
    }

    public float? PulseDuration
    {
        get => _pulseDuration;
        set => SetProperty(ref _pulseDuration, value);
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
        await _standController.StartPulseCountModuleMeasureAsync(PulseCountMeterModuleNumber - 1);
        
        IsPulseCountWork = true;
    }
    
    public DelegateCommand StartPulseDurationCommand { get; }

    private async void StartPulseDurationCommandHandler()
    {
        await _standController.StartPulseCountModuleMeasureAsync(PulseCountMeterModuleNumber - 1);
        
        IsPulseDurationWork = true;
    }
    
    public DelegateCommand StopPulseCountCommand { get; }

    private async void StopPulseCountCommandHandler()
    {
        await _standController.StopPulseCountModuleMeasureAsync(PulseCountMeterModuleNumber - 1);
        
        IsPulseCountWork = false;
        
        PulseCount = (int?)await _standController.ReadPulseCountFromPulseCountMeterAsync(PulseCountMeterModuleNumber - 1, ChannelNumber);
    }
    
    public DelegateCommand StopPulseDurationCommand { get; }

    private async void StopPulseDurationCommandHandler()
    {
        await _standController.StopPulseCountModuleMeasureAsync(PulseCountMeterModuleNumber - 1);

        IsPulseDurationWork = false;
        
        PulseDuration = await _standController.ReadPulseDurationFromPulseCountMeterAsync(PulseCountMeterModuleNumber - 1, ChannelNumber);
    }
    
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        //todo переделать текстбокс выбора МПКИ и канала на комбобокс исходя из настроек
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