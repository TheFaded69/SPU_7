using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Line;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class CheckTightnessViewModel : ViewModelBase, IDialogAware
{
    private readonly IStandController _standController;
    private readonly IStandSettingsService _standSettingsService;
    private readonly ILogger _logger;
    private readonly ITimerService _timerService;

    public CheckTightnessViewModel(IStandController standController, IStandSettingsService standSettingsService,
        ILogger logger, ITimerService timerService)
    {
        _standController = standController;
        _standSettingsService = standSettingsService;
        _logger = logger;
        _timerService = timerService;

        Title = "Проверка герметичности";
        NozzleValues = new ObservableCollection<double>(standSettingsService.StandSettingsModel.NozzleViewModels
            .Where(nvm => nvm.NozzleValue != null)
            .Select(nvm => (double)nvm.NozzleValue!));

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


        StartCheckTightnessCommand = new DelegateCommand(StartCheckTightnessCommandHandler);
        StopCheckTightnessCommand = new DelegateCommand(StopCheckTightnessCommandHandler);
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
    }

    private CancellationTokenSource _cancellationTokenSource;

    private int _vacuumWaitTime = 60;
    private float _pressureDifferenceMinimum = 2.0f;
    private float _pressureResiverMinimum = 50f;
    private float _pressureDifferenceMaximum = 0.1f;
    private int _testTime = 600;
    private int _stabilizationTime = 300;
    private double _selectedNozzleValue;
    private bool _needCheckLine;
    private StandSettingsLineModel _selectedLineViewModel;
    private string _selectedDeviceName;
    private bool _isDeviceVisible;
    private bool _isChecking;
    private double? _volumeMinimum = 0.04;
    private double? _insideVolumeOfStand = 0.00553;
    private double? _goodRange = 0.33;
    private float? _startPressureDifference;
    private float? _endPressureDifference;
    private float? _realFlowLeak;
    private string _nowActionString;
    private float? _metrologyCalculateForTightness;
    private float? _startTemperature;
    private float? _endTemperature;
    private float? _flowLeak;
    private float? _targetFrequency;
    private StandSettingsFanModel _selectedFanViewModel;
    private int? _selectedFanIndex;
    private int? _selectedLineIndex;

    public float? TargetFrequency
    {
        get => _targetFrequency;
        set => SetProperty(ref _targetFrequency, value);
    }

    public int VacuumWaitTime
    {
        get => _vacuumWaitTime;
        set => SetProperty(ref _vacuumWaitTime, value);
    }

    public float PressureDifferenceMinimum
    {
        get => _pressureDifferenceMinimum;
        set => SetProperty(ref _pressureDifferenceMinimum, value);
    }

    public float PressureResiverMinimum
    {
        get => _pressureResiverMinimum;
        set => SetProperty(ref _pressureResiverMinimum, value);
    }

    public float PressureDifferenceMaximum
    {
        get => _pressureDifferenceMaximum;
        set => SetProperty(ref _pressureDifferenceMaximum, value);
    }

    public int TestTime
    {
        get => _testTime;
        set => SetProperty(ref _testTime, value);
    }

    public int StabilizationTime
    {
        get => _stabilizationTime;
        set => SetProperty(ref _stabilizationTime, value);
    }

    public double? VolumeMinimum
    {
        get => _volumeMinimum;
        set => SetProperty(ref _volumeMinimum, value);
    }

    public double? InsideVolumeOfStand
    {
        get => _insideVolumeOfStand;
        set => SetProperty(ref _insideVolumeOfStand, value);
    }

    public double? GoodRange
    {
        get => _goodRange;
        set => SetProperty(ref _goodRange, value);
    }

    public string NowActionString
    {
        get => _nowActionString;
        set => SetProperty(ref _nowActionString, value);
    }

    public float? StartPressureDifference
    {
        get => _startPressureDifference;
        set => SetProperty(ref _startPressureDifference, value);
    }

    public float? EndPressureDifference
    {
        get => _endPressureDifference;
        set => SetProperty(ref _endPressureDifference, value);
    }

    public float? StartTemperature
    {
        get => _startTemperature;
        set => SetProperty(ref _startTemperature, value);
    }

    public float? EndTemperature
    {
        get => _endTemperature;
        set => SetProperty(ref _endTemperature, value);
    }

    public float? FlowLeak
    {
        get => _flowLeak;
        set => SetProperty(ref _flowLeak, value);
    }

    public float? RealFlowLeak
    {
        get => _realFlowLeak;
        set => SetProperty(ref _realFlowLeak, value);
    }



    public ObservableCollection<double> NozzleValues { get; set; }

    /*public double SelectedNozzleValue
    {
        get => _selectedNozzleValue;
        set
        {
            SetProperty(ref _selectedNozzleValue, value);
            SelectedStandSettingsNozzleModel =
                _standSettingsService.StandSettingsModel.NozzleViewModels.FirstOrDefault(
                    bvm => bvm.NozzleValue == value);
        }
    }*/

    /*public StandSettingsLineModel SelectedStandSettingsLineModel { get; set; }

    public StandSettingsMasterDeviceModel SelectedStandSettingsMasterDeviceModel { get; set; }

    public StandSettingsNozzleModel SelectedStandSettingsNozzleModel { get; set; }*/

    public bool NeedCheckLine
    {
        get => _needCheckLine;
        set
        {
            SetProperty(ref _needCheckLine, value);
            if (!value) IsDeviceVisible = false;
        }
    }

    public ObservableCollection<StandSettingsLineModel> LineViewModels { get; set; } = [];

    public ObservableCollection<StandSettingsFanModel> FanViewModels { get; set; } = [];

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

    public StandSettingsLineModel SelectedLineViewModel
    {
        get => _selectedLineViewModel;
        set => SetProperty(ref _selectedLineViewModel, value);
    }

    public int? SelectedLineIndex
    {
        get => _selectedLineIndex;
        set => SetProperty(ref _selectedLineIndex, value);
    }

    public bool IsDeviceVisible
    {
        get => _isDeviceVisible;
        set => SetProperty(ref _isDeviceVisible, value);
    }

    public bool IsChecking
    {
        get => _isChecking;
        set => SetProperty(ref _isChecking, value);
    }

    public DelegateCommand StartCheckTightnessCommand { get; }

    private async void StartCheckTightnessCommandHandler()
    {
        IsChecking = true;

        _cancellationTokenSource = new CancellationTokenSource();
        var token = _cancellationTokenSource.Token;

        var result = Task.Run(async () => { await CheckTightnessProcess(_cancellationTokenSource); }, token);
    }

    public DelegateCommand StopCheckTightnessCommand { get; }

    private async void StopCheckTightnessCommandHandler()
    {
        await _cancellationTokenSource?.CancelAsync();

        await _standController.DisableFrequencyRegulatorAsync((int)SelectedFanIndex);
        
        IsChecking = false;
    }

    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }

    private async Task CheckTightnessProcess(CancellationTokenSource operationCancellationTokenSource)
    {
        try
        {
            switch (SelectedLineViewModel.SelectedLineType)
            {
                case LineType.None:
                    break;
                case LineType.MasterDeviceLineType:
                {
                    /*NowActionString = $"Подготовка стенда к проверке герметичности";

                    _timerService.TimeSeconds = 180;
                    _timerService.OperationName = "Проверка герметичности";
                    _timerService.Message = "Подготовка стенда к работе";
                    _timerService.InfoTimerEnable();

                    if (!operationCancellationTokenSource.IsCancellationRequested)
                    {
                        if (!await _standController.SetStandWorkModeAsync())
                        {
                            _logger.Logging(new LogMessage("Не удалось установить рабочий режим установки",
                                LogLevel.Error));
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    await Task.Delay(30000);

                    _timerService.InfoTimerDisable();

                    if (SelectedLineViewModel.IsEndValveMasterDevice)
                        await _standController.OpenValveAsync(SelectedLineViewModel.EndValveMasterDeviceViewModel,
                            true);

                    await _standController.OpenValveAsync(SelectedLineViewModel.MasterDeviceViewModels
                        .MinBy(md => md.MaximumFlow).MasterDeviceValveViewModel, true);

                    await _standController.OpenValveAsync(SelectedLineViewModel.MasterDeviceViewModels
                        .MinBy(md => md.MaximumFlow).PressureSensorValveViewModel, true);

                    await _standController.OpenValveAsync(SelectedFanViewModel.FanValveViewModel, true);

                    var currentLine =
                        _standSettingsService.StandSettingsModel.LineViewModels.IndexOf(SelectedLineViewModel);

                    var fanLineIndex = _standSettingsService.StandSettingsModel.LineViewModels.IndexOf(
                        _standSettingsService.StandSettingsModel.LineViewModels.FirstOrDefault(line =>
                            line.FanViewModels.Contains(SelectedFanViewModel)));

                    if (fanLineIndex > currentLine)
                    {
                        while (currentLine < fanLineIndex)
                        {
                            await _standController.OpenValveAsync(
                                _standSettingsService.StandSettingsModel.LineViewModels[currentLine]
                                    .EndCommonValveViewModel, true);

                            currentLine++;
                        }
                    }
                    else if (fanLineIndex < currentLine)
                    {
                        while (currentLine > fanLineIndex)
                        {
                            currentLine--;

                            await _standController.OpenValveAsync(
                                _standSettingsService.StandSettingsModel.LineViewModels[currentLine]
                                    .EndCommonValveViewModel, true);
                        }
                    }

                    if (!operationCancellationTokenSource.IsCancellationRequested)
                    {
                        if (!await _standController.UpdateAllDevice())
                        {
                            _logger.Logging(new LogMessage("Не удалось открыть краны для проверки герметичности",
                                LogLevel.Error));
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    await Task.Delay(30000);*/

                    if (!operationCancellationTokenSource.IsCancellationRequested)
                    {
                        if (!await _standController.SetRegulatorFrequencyAsync((int)SelectedFanIndex,
                                (float)TargetFrequency))
                        {
                            _logger.Logging(new LogMessage("Не удалось установить частоту ПЧВ",
                                LogLevel.Error));
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    if (!operationCancellationTokenSource.IsCancellationRequested)
                    {
                        if (!await _standController.EnableFrequencyRegulatorAsync((int)SelectedFanIndex))
                        {
                            _logger.Logging(new LogMessage("Не удалось запустить ПЧВ",
                                LogLevel.Error));
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    NowActionString = $"Ожидание перепада";

                    _timerService.TimeSeconds = 30;
                    _timerService.OperationName = "Проверка герметичности";
                    _timerService.Message = "Ожидание перепада давления";
                    _timerService.InfoTimerEnable();

                    while (_standController.GetPressureDifference((int)SelectedLineIndex) <
                           0.8 * PressureDifferenceMinimum * 1000)
                    {
                        if (operationCancellationTokenSource.IsCancellationRequested)
                        {
                            return;
                        }

                        await Task.Delay(100);
                    }

                    if (!operationCancellationTokenSource.IsCancellationRequested)
                    {
                        if (!await _standController.CloseValveAsync(SelectedLineViewModel.MasterDeviceViewModels
                                .MinBy(md => md.MaximumFlow).MasterDeviceValveViewModel))
                        {
                            _logger.Logging(new LogMessage("Не удалось установить рабочий режим установки",
                                LogLevel.Error));
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    await Task.Delay(40000);

                    if (!operationCancellationTokenSource.IsCancellationRequested)
                    {
                        if (!await _standController.SetRegulatorFrequencyAsync((int)SelectedFanIndex, 0) &&
                            !await _standController.DisableFrequencyRegulatorAsync((int)SelectedFanIndex))
                        {
                            _logger.Logging(new LogMessage("Не удалось установить рабочий режим установки",
                                LogLevel.Error));
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }

                    _timerService.InfoTimerDisable();

                    NowActionString = $"Стабилизация - {StabilizationTime} сек.";
                    _logger.Logging(new LogMessage(
                        $"Стабилизация - {StabilizationTime} сек.",
                        LogLevel.Info));

                    _timerService.TimeSeconds = StabilizationTime;
                    _timerService.OperationName = "Проверка герметичности";
                    _timerService.Message = "Стабилизация";
                    _timerService.InfoTimerEnable();

                    await Task.Delay(TimeSpan.FromSeconds(StabilizationTime));
                    _logger.Logging(new LogMessage($"Стабилизация закончена", LogLevel.Success));

                    _timerService.InfoTimerDisable();

                    await Task.Delay(2000);

                    //Фиксируем стартовое давление
                    var startPressureDifference = _standController.GetPressureDifference((int)SelectedLineIndex);
                    var startTemperature = _standController.GetTemperature((int)SelectedLineIndex,
                        SelectedLineViewModel.MasterDeviceViewModels.IndexOf(SelectedLineViewModel
                            .MasterDeviceViewModels
                            .MinBy(md => md.MaximumFlow)));

                    _logger.Logging(new LogMessage($"Начальное давление перепада - {startPressureDifference} Па",
                        LogLevel.Info));

                    StartPressureDifference = startPressureDifference;
                    StartTemperature = startTemperature;

                    NowActionString = $"Проверка на вакуум - {TestTime} сек.";
                    _logger.Logging(new LogMessage(
                        $"Проверка на вакуум - {TestTime} сек.",
                        LogLevel.Info));
                    //Ожидание в тесте

                    _timerService.TimeSeconds = TestTime;
                    _timerService.OperationName = "Проверка герметичности";
                    _timerService.Message = "Проверка на вакуум";
                    _timerService.InfoTimerEnable();

                    await Task.Delay(
                        TimeSpan.FromSeconds(TestTime));
                    _logger.Logging(new LogMessage($"Проверка на вакуум окончена", LogLevel.Success));

                    _timerService.InfoTimerDisable();

                    //Фиксируем конечное давление
                    var endPressureDifference = _standController.GetPressureDifference((int)SelectedLineIndex);
                    var endTemperature = _standController.GetTemperature((int)SelectedLineIndex,
                        SelectedLineViewModel.MasterDeviceViewModels.IndexOf(SelectedLineViewModel
                            .MasterDeviceViewModels
                            .MinBy(md => md.MaximumFlow)));

                    _logger.Logging(new LogMessage($"Конечное давление перепада - {endPressureDifference} Па",
                        LogLevel.Info));

                    EndPressureDifference = endPressureDifference;
                    EndTemperature = endTemperature;

                    _logger.Logging(new LogMessage(
                        $"Разница давления перепада - {(startPressureDifference - endPressureDifference)} Па",
                        LogLevel.Info));
                    _logger.Logging(new LogMessage(
                        $"Разница температуры - {(startTemperature - endTemperature)} °С",
                        LogLevel.Info));

                    FlowLeak = (float?)(VolumeMinimum * GoodRange * 0.01 * 0.125);
                    RealFlowLeak = (float?)(InsideVolumeOfStand / TestTime * (endPressureDifference /
                        startPressureDifference *
                        (startTemperature + 273.15) / (endTemperature + 273.15) - 1) * 60);

                    var isCheckTightnessGood = RealFlowLeak <= FlowLeak;
                    
                    NowActionString = isCheckTightnessGood ? "Установка герметична" : "Установка не герметична!!!";
                }
                    break;
                case LineType.NozzleLineType:
                {
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        catch (Exception e)
        {
            await _standController.EmergencyPowerOffAsync();
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
            NowActionString = e.Message;
        }
        finally
        {
            _timerService.InfoTimerDisable();
            IsChecking = false;
        }
    }

    #region Dialog

    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService)
    {
        dialogService.ShowDialog(nameof(CheckTightnessView), null, result =>
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
                    break;
                case ButtonResult.None:
                    break;
                case ButtonResult.OK:
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

    #endregion
}