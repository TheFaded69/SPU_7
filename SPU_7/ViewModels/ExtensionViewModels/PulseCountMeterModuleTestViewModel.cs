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
        Title = "Тест МПКИ";
        
        _standController = standController;
        _standSettingsService = standSettingsService;


        StartPulseCountCommand = new DelegateCommand(StartPulseCountCommandHandler);
        StartPulseDurationCommand = new DelegateCommand(StartPulseDurationCommandHandler);
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
    }

    private int? _pulseCountMeterModuleNumber;
    private int? _channelNumber;
    private int? _pulseCount;
    private float? _pulseDuration;

    
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

    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }
    
    public DelegateCommand StartPulseCountCommand { get; }

    private async void StartPulseCountCommandHandler()
    {
        
    }
    
    public DelegateCommand StartPulseDurationCommand { get; }

    private async void StartPulseDurationCommandHandler()
    {
        
    }
    
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
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