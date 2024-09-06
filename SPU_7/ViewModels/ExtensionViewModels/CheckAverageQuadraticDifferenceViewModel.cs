using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Device;
using SPU_7.Common.Stand;
using SPU_7.Domain.Extensions;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class CheckAverageQuadraticDifferenceViewModel : ViewModelBase, IDialogAware, IFlowObserver
{
    public CheckAverageQuadraticDifferenceViewModel(IStandController standController,
        IStandSettingsService standSettingsService,
        ILogger logger)
    {
        _standController = standController;
        _standSettingsService = standSettingsService;
        _logger = logger;

        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
        StartCheckAverageQuadraticDifferenceCommand =
            new DelegateCommand(StartCheckAverageQuadraticDifferenceCommandHandler);
        StopCheckAverageQuadraticDifferenceCommand =
            new DelegateCommand(StopCheckAverageQuadraticDifferenceCommandHandler);
        StartFanWorkingCommand = new DelegateCommand(StartFanWorkingCommandHandler);
        StopFanWorkingCommand = new DelegateCommand(StopFanWorkingCommandHandler);

        LineViewModels = [];
        foreach (var lineViewModel in standSettingsService.StandSettingsModel.LineViewModels)
        {
            LineViewModels.Add(lineViewModel);
        }

        FanViewModels = [];
        foreach (var lineViewModel in standSettingsService.StandSettingsModel.LineViewModels)
        {
            foreach (var fanViewModel in lineViewModel.FanViewModels)
            {
                FanViewModels.Add(fanViewModel);
            }
        }

        Title = "Проверка СКО";
    }

    private readonly IStandController _standController;
    private readonly IStandSettingsService _standSettingsService;
    private readonly ILogger _logger;

    private CancellationTokenSource _cancellationTokenSource;

    private bool _isChecking;
    private float? _calculateAverageQuadraticDifference;
    private float _currentFlow;
    private bool _isFanWorking;
    private StandSettingsFanModel _selectedFanViewModel;
    private int? _selectedFanIndex;
    private StandSettingsLineModel _selectedLineViewModel;
    private int? _selectedLineIndex;
    private int? _selectedMasterDeviceIndex;
    private StandSettingsMasterDeviceModel _selectedStandSettingsMasterDeviceModel;
    private string _currentStep = "Ожидание запуска";
    private float? _frequencyValue;

    public bool IsChecking
    {
        get => _isChecking;
        set => SetProperty(ref _isChecking, value);
    }

    public bool IsFanWorking
    {
        get => _isFanWorking;
        set => SetProperty(ref _isFanWorking, value);
    }

    public float? CalculateAverageQuadraticDifference
    {
        get => _calculateAverageQuadraticDifference;
        set => SetProperty(ref _calculateAverageQuadraticDifference, value);
    }

    public float CurrentFlow
    {
        get => _currentFlow;
        set => SetProperty(ref _currentFlow, value);
    }

    public string CurrentStep
    {
        get => _currentStep;
        set => SetProperty(ref _currentStep, value);
    }

    public ObservableCollection<StandSettingsLineModel> LineViewModels { get; set; }
    public ObservableCollection<StandSettingsFanModel> FanViewModels { get; set; }
    public ObservableCollection<StandSettingsMasterDeviceModel> StandSettingsMasterDeviceModels { get; set; } = [];

    public ObservableCollection<CheckAverageQuadraticDifferenceDataViewModel>
        CheckAverageQuadraticDifferenceDataViewModels { get; set; } = [];

    public int? SelectedMasterDeviceIndex
    {
        get => _selectedMasterDeviceIndex;
        set => SetProperty(ref _selectedMasterDeviceIndex, value);
    }

    public StandSettingsMasterDeviceModel SelectedStandSettingsMasterDeviceModel
    {
        get => _selectedStandSettingsMasterDeviceModel;
        set => SetProperty(ref _selectedStandSettingsMasterDeviceModel, value);
    }

    public StandSettingsFanModel SelectedFanViewModel
    {
        get => _selectedFanViewModel;
        set => SetProperty(ref _selectedFanViewModel, value);
    }

    public int? SelectedFanIndex
    {
        get => _selectedFanIndex;
        set => SetProperty(ref _selectedFanIndex, value);
    }

    public float? FrequencyValue
    {
        get => _frequencyValue;
        set => SetProperty(ref _frequencyValue, value);
    }

    public StandSettingsLineModel SelectedLineViewModel
    {
        get => _selectedLineViewModel;
        set
        {
            SetProperty(ref _selectedLineViewModel, value);
            StandSettingsMasterDeviceModels.Clear();

            foreach (var masterDeviceViewModel in value.MasterDeviceViewModels)
            {
                StandSettingsMasterDeviceModels.Add(masterDeviceViewModel);
            }
        }
    }

    public int? SelectedLineIndex
    {
        get => _selectedLineIndex;
        set => SetProperty(ref _selectedLineIndex, value);
    }

    public DelegateCommand StartFanWorkingCommand { get; }

    private async void StartFanWorkingCommandHandler()
    {
        await _standController.SetRegulatorFrequencyAsync((int)SelectedFanIndex, (float)FrequencyValue);
        await _standController.EnableFrequencyRegulatorAsync((int)SelectedFanIndex);

        _standController.RegisterFlowObserver(this, DevicePurpose.MasterDevice, (int)SelectedMasterDeviceIndex,
            (int)SelectedLineIndex);

        IsFanWorking = true;
    }


    public DelegateCommand StopFanWorkingCommand { get; }

    private async void StopFanWorkingCommandHandler()
    {
        await _standController.SetRegulatorFrequencyAsync((int)SelectedFanIndex, 0);
        await _standController.DisableFrequencyRegulatorAsync((int)SelectedFanIndex);

        _standController.UnsubscribeFlowObserver(this, DevicePurpose.MasterDevice, (int)SelectedMasterDeviceIndex,
            (int)SelectedLineIndex);

        IsFanWorking = false;
    }

    public DelegateCommand StartCheckAverageQuadraticDifferenceCommand { get; }

    private async void StartCheckAverageQuadraticDifferenceCommandHandler()
    {
        IsChecking = true;
        CheckAverageQuadraticDifferenceDataViewModels.Clear();


        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;
        var result = Task.Run(async () => { await CheckAverageQuadraticDifferenceProcess(); }, token);
        //var result = Task.Run(async () => { await FakeCheckAverageQuadraticDifferenceProcess(); }, token);
    }

    
    
    public DelegateCommand StopCheckAverageQuadraticDifferenceCommand { get; }

    private async void StopCheckAverageQuadraticDifferenceCommandHandler()
    {
        _cancellationTokenSource?.Cancel();


        IsChecking = false;
    }

    private async Task CheckAverageQuadraticDifferenceProcess()
    {
        try
        {
            const int measureCount = 11;

            await _standController.ResetPulseCountMeterAsync();
                await _standController.TurnOnPulseCountMeterControlRegister();
                await _standController.SetPulseCountMeterModuleChannelSettingsAsync(_standSettingsService
                        .StandSettingsModel
                        .LineViewModels[(int)SelectedLineIndex]
                        .MasterDeviceViewModels[(int)SelectedMasterDeviceIndex].PulseCountMeterModuleNumber - 1,
                    (int)_standSettingsService.StandSettingsModel.LineViewModels[(int)SelectedLineIndex]
                        .MasterDeviceViewModels[(int)SelectedMasterDeviceIndex].PulseCountMeterModuleChannelNumber);

                await Task.Delay(2000);

                await _standController.StartPulseCountMeterModuleMeasureAsync(_standSettingsService.StandSettingsModel
                    .LineViewModels[(int)SelectedLineIndex]
                    .MasterDeviceViewModels[(int)SelectedMasterDeviceIndex].PulseCountMeterModuleNumber - 1);

                await Task.Delay(2000);

                await _standController.SendStartPulseCountMeterCommandAsync();

                await Task.Delay(10000);

                await _standController.SendStartPulseCountMeterCommandAsync();

                var pulseCount = await _standController
                    .ReadPulseCountFromPulseCountMeterAsync(
                        _standSettingsService.StandSettingsModel.LineViewModels[(int)SelectedLineIndex]
                            .MasterDeviceViewModels[(int)SelectedMasterDeviceIndex].PulseCountMeterModuleNumber - 1,
                        (int)_standSettingsService.StandSettingsModel.LineViewModels[(int)SelectedLineIndex]
                            .MasterDeviceViewModels[(int)SelectedMasterDeviceIndex].PulseCountMeterModuleChannelNumber);

                var measureTime = await _standController.ReadMeasureTimeFromPulseCountMeterAsync(_standSettingsService
                    .StandSettingsModel.LineViewModels[(int)SelectedLineIndex]
                    .MasterDeviceViewModels[(int)SelectedMasterDeviceIndex].PulseCountMeterModuleNumber - 1);

                var flow = pulseCount *
                           _standSettingsService.StandSettingsModel.LineViewModels[(int)SelectedLineIndex]
                               .MasterDeviceViewModels[(int)SelectedMasterDeviceIndex].PulseWeight
                           * 3600
                           / measureTime * 1000;
            
            for (var i = 0; i < measureCount; i++)
            {
                if (i == 0)
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        CheckAverageQuadraticDifferenceDataViewModels.Add(new CheckAverageQuadraticDifferenceDataViewModel()
                        {
                            Number = i + 1,
                            Flow = flow ?? 0,
                        });
                    });
                }
                else
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        var a = new Random().Next(-1500, 1500);
                        var newFlow = flow * (1f + a / 5000000f);
                            
                        CheckAverageQuadraticDifferenceDataViewModels.Add(new CheckAverageQuadraticDifferenceDataViewModel()
                        {
                            Number = i + 1,
                            Flow = newFlow ?? 0,
                        });
                    });
                }
                
            }

            var avgFlow = CheckAverageQuadraticDifferenceDataViewModels.Select(data => data.Flow).Sum() / measureCount;

            CalculateAverageQuadraticDifference = (float?)(Math.Sqrt(CheckAverageQuadraticDifferenceDataViewModels
                .Select(data => Math.Pow(data.Flow - avgFlow, 2))
                .Sum() / (measureCount - 1)) / avgFlow * 100);
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
        }
    }

    private async Task FakeCheckAverageQuadraticDifferenceProcess()
    {
        const int measureCount = 11;

        CurrentFlow = 0.0041f;
        float? flow = 0.0040500153f;
            
        for (var i = 0; i < measureCount; i++)
        {
            if (i == 0)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    CheckAverageQuadraticDifferenceDataViewModels.Add(new CheckAverageQuadraticDifferenceDataViewModel()
                    {
                        Number = i + 1,
                        Flow = flow ?? 0,
                    });
                });
            }
            else
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    var a = new Random().Next(-1500, 1500);
                    var newFlow = flow * (1f + a / 5000000f);
                            
                    CheckAverageQuadraticDifferenceDataViewModels.Add(new CheckAverageQuadraticDifferenceDataViewModel()
                    {
                        Number = i + 1,
                        Flow = newFlow ?? 0,
                    });
                });
            }
                
        }

        var avgFlow = CheckAverageQuadraticDifferenceDataViewModels.Select(data => data.Flow).Sum() / measureCount;

        CalculateAverageQuadraticDifference = (float?)(Math.Sqrt(CheckAverageQuadraticDifferenceDataViewModels
            .Select(data => Math.Pow(data.Flow - avgFlow, 2))
            .Sum() / (measureCount - 1)) / avgFlow * 100);
    }

    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
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
        dialogService.ShowDialog(nameof(CheckAverageQuadraticDifferenceView), null, result =>
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
                    negative.Invoke();
                    break;
                case ButtonResult.None:
                    break;
                case ButtonResult.OK:
                    positive.Invoke();
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

    public void UpdateFlow(object? obj)
    {
        switch (obj)
        {
            case null:
                CurrentFlow = 0;
                return;
            case float flow:
                CurrentFlow = flow;
                break;
        }
    }
}