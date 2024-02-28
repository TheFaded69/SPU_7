using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Extensions;
using SPU_7.Common.Scripts;
using SPU_7.Extensions;
using SPU_7.Models.Scripts;
using SPU_7.Models.Scripts.Operations;
using SPU_7.Models.Scripts.Operations.Configurations;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.ViewModels.DeviceInformationViewModels;
using SPU_7.ViewModels.ScriptViewModels;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;
using SPU_7.ViewModels.WorkReportViewModels;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels
{
    public class MnemonicSchemeControlViewModel : ViewModelBase, IStandObserver
    {
        public MnemonicSchemeControlViewModel(IDialogService dialogService,
            INotificationService notificationService,
            ILogger logger,
            IStandSettingsService settingsService,
            IStandController standController,
            IScriptController scriptController,
            IManualOperationService manualOperationService,
            ITimerService timerService,
            IOperationActionService operationActionService)
        {
            _dialogService = dialogService;
            _notificationService = notificationService;
            _logger = logger;
            _settingsService = settingsService;
            _standController = standController;
            _scriptController = scriptController;
            _manualOperationService = manualOperationService;
            _timerService = timerService;
            _operationActionService = operationActionService;
            ChooseScriptCommand = new DelegateCommand(ChooseScriptCommandHandler);
            StartScriptCommand = new DelegateCommand(StartScriptCommandHandler);
            StopScriptCommand = new DelegateCommand(StopScriptCommandHandler);
            WriteDeviceInformationCommand = new DelegateCommand(WriteDeviceInformationCommandHandler);
            OpenWorkReportCommand = new DelegateCommand(OpenWorkReportCommandHandler);

            standController.RegisterObserver(this);
            timerService.SetInfoTimerEnableAction(TimerEnable);
            timerService.SetInfoTimerDisableAction(TimerDisable);
        }

        private readonly IScriptController _scriptController;
        private readonly IManualOperationService _manualOperationService;
        private readonly ITimerService _timerService;
        private readonly IOperationActionService _operationActionService;
        private readonly IDialogService _dialogService;
        private readonly ILogger _logger;
        private readonly IStandSettingsService _settingsService;
        private readonly IStandController _standController;
        private readonly INotificationService _notificationService;

        private ObservableCollection<OperationViewModel> _operations;
        private string _scriptName = "Не выбран";
        private ScriptViewModel _selectedScript;
        private float? _temperatureTube;
        private float? _temperature;
        private float? _humidity;
        private float? _pressure;
        private float? _pressureDifference;
        private float? _pressureResiver;
        private int _timeSeconds;
        private string _message;
        private string _operationName;
        private bool _isTimerVisible;
        private string _timeString;
        private double? _targetFlow;
        private bool _timerStop;
        private bool _isChooseScriptEnable = true;
        private bool _isStopEnable;
        private bool _isStartEnable;
        private bool _isWriteDeviceInformationEnabled;
        private int? _lineIndex;


        #region Сценарий

        public bool IsChooseScriptEnable
        {
            get => _isChooseScriptEnable;
            set => SetProperty(ref _isChooseScriptEnable, value);
        }

        public ScriptViewModel SelectedScript
        {
            get => _selectedScript;
            set => SetProperty(ref _selectedScript, value);
        }

        public ObservableCollection<OperationViewModel> Operations
        {
            get => _operations;
            set => SetProperty(ref _operations, value);
        }

        public string ScriptName
        {
            get => _scriptName;
            set => SetProperty(ref _scriptName, value);
        }

        #endregion

        #region Параметры среды

        public float? TemperatureTube
        {
            get => _temperatureTube;
            set => SetProperty(ref _temperatureTube, value == null ? value : (float?)Math.Round((float)value, 2));
        }

        public float? Pressure
        {
            get => _pressure;
            set => SetProperty(ref _pressure, value == null ? value : (float?)Math.Round((float)value / 1000, 3));
        }

        public float? Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value == null ? value : (float?)Math.Round((float)value, 2));
        }

        public float? Humidity
        {
            get => _humidity;
            set => SetProperty(ref _humidity, value == null ? value : (float?)Math.Round((float)value, 2));
        }

        public float? PressureDifference
        {
            get => _pressureDifference;
            set => SetProperty(ref _pressureDifference, value == null ? value : (float?)Math.Round((float)value / 1000, 3));
        }

        public float? PressureResiver
        {
            get => _pressureResiver;
            set => SetProperty(ref _pressureResiver, value == null ? value : (float?)Math.Round((float)value, 3));
        }

        public double? TargetFlow
        {
            get => _targetFlow;
            set => SetProperty(ref _targetFlow, value);
        }

        #endregion

        #region Управление стендом

        public bool IsStartEnable
        {
            get => _isStartEnable;
            set => SetProperty(ref _isStartEnable, value);
        }

        public bool IsStopEnable
        {
            get => _isStopEnable;
            set => SetProperty(ref _isStopEnable, value);
        }

        public DelegateCommand ChooseScriptCommand { get; }

        public void ChooseScriptCommandHandler()
        {
            ScriptMenuViewModel.Show(_dialogService, SetScript, null);
        }

        private void SetScript(ScriptViewModel script)
        {
            SelectedScript = script;
            Operations = script.Operations;
            ScriptName = script.Name;

            var operations = new List<OperationModel>();
            foreach (var operationViewModel in Operations)
            {
                OperationModel operationModel = operationViewModel.OperationType switch
                {
                    OperationType.Validation => new ValidationOperationModel(_standController,
                        _logger,
                        _settingsService,
                        (ValidationOperationConfigurationModel)operationViewModel.ConfigurationModel,
                        _manualOperationService,
                        _timerService,
                        _operationActionService),
                    OperationType.CheckTightness => new CheckTightnessOperationModel(_standController,
                        _logger,
                        _settingsService,
                        (CheckTightnessOperationConfigurationModel)operationViewModel.ConfigurationModel,
                        _timerService,
                        _operationActionService),
                    OperationType.SetStandWorkMode => new SetStandWorkModeOperationModel(_standController,
                        _logger,
                        _settingsService,
                        operationViewModel.ConfigurationModel,
                        _timerService,
                        _operationActionService),
                    _ => throw new ArgumentOutOfRangeException()
                };

                operationModel.OperationType = operationViewModel.OperationType;
                operationModel.OperationName = operationViewModel.Name;
                operationModel.RegisterObserver(operationViewModel);
                operationModel.NumberOperation = operations.Count + 1;
                operations.Add(operationModel);
            }

            var scriptModel = new ScriptModel
            {
                ScriptName = script.Name,
                OperationModels = operations,
                DeviceType = script.DeviceType,
                Description = script.Description,
                DeviceGroupType = script.DeviceType.GetDeviceGroupType(),
                LineNumber = script.LineNumber
            };

            _scriptController.SetScript(scriptModel);
            _standController.SetActiveLine(scriptModel.LineNumber, true);
            
            IsStartEnable = true;
        }

        public DelegateCommand StartScriptCommand { get; }

        private void StartScriptCommandHandler()
        {
            IsStartEnable = false;
            _scriptController.Start();
            IsStopEnable = true;
        }

        public DelegateCommand StopScriptCommand { get; }

        private void StopScriptCommandHandler()
        {
            IsStopEnable = false;
            _scriptController.Stop();
            IsStartEnable = true;
        }

        #endregion

        #region Stand

        public DelegateCommand WriteDeviceInformationCommand { get; set; }

        private void WriteDeviceInformationCommandHandler()
        {
            WriteDeviceInformationViewModel.Show(_dialogService,  LineIndex, null, null);
        }
        
        public bool IsWriteDeviceInformationEnabled
        {
            get => _isWriteDeviceInformationEnabled;
            set => SetProperty(ref _isWriteDeviceInformationEnabled, value);
        }

        public int? LineIndex
        {
            get => _lineIndex;
            set => SetProperty(ref _lineIndex, value);
        }

        #endregion

        #region Timer

        public int TimeSeconds
        {
            get => _timeSeconds;
            set => SetProperty(ref _timeSeconds, value);
        }

        public string TimeString
        {
            get => _timeString;
            set => SetProperty(ref _timeString, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string OperationName
        {
            get => _operationName;
            set => SetProperty(ref _operationName, value);
        }

        public bool IsTimerVisible
        {
            get => _isTimerVisible;
            set => SetProperty(ref _isTimerVisible, value);
        }

        private async void TimerEnable()
        {
            IsTimerVisible = true;
            TimerStop = false;
            Message = _timerService.Message;
            OperationName = _timerService.OperationName;
            TimeSeconds = _timerService.TimeSeconds;

            while (TimeSeconds > 0)
            {
                if (TimerStop)
                {
                    TimeSeconds = 0;
                    IsTimerVisible = false;

                    break;
                }
                else
                {
                    TimeString = TimeSpan.FromSeconds(TimeSeconds).ToString();
                    TimeSeconds--;

                    await Task.Delay(1000);
                }
            }

            IsTimerVisible = false;
        }

        private async void TimerDisable()
        {
            TimerStop = true;
        }

        public bool TimerStop
        {
            get => _timerStop;
            set => SetProperty(ref _timerStop, value);
        }

        #endregion

        #region Дополнительные параметры

        public ObservableCollection<CustomDataViewModel> CustomDataViewModels { get; set; }

        #endregion

        #region Отчет

        public DelegateCommand OpenWorkReportCommand { get; }

        private void OpenWorkReportCommandHandler()
        {
            WorkReportViewModel.Show(_dialogService, null, null);
        }

        #endregion

        #region Observer

        public void Update(object obj)
        {
            //throw new NotImplementedException();
        }

        public void UpdateFromDataPair(DataPair dataPair)
        {
            if (dataPair.Data == null) return;

            switch (dataPair.DataType)
            {
                case DeviceInfoParameterType.TemperatureTube:
                    TemperatureTube = (float)dataPair.Data;
                    break;
                case DeviceInfoParameterType.Pressure:
                    Pressure = (float)dataPair.Data;
                    break;
                case DeviceInfoParameterType.PressureResiver:
                    PressureResiver = (float)dataPair.Data;
                    break;
                case DeviceInfoParameterType.PressureDifference:
                    PressureDifference = (float)dataPair.Data;
                    break;
                case DeviceInfoParameterType.Temperature:
                    Temperature = (float)dataPair.Data;
                    break;
                case DeviceInfoParameterType.Humidity:
                    Humidity = (float)dataPair.Data;
                    break;
                case DeviceInfoParameterType.TargetFlow:
                    TargetFlow = (double)dataPair.Data;
                    break;
                case DeviceInfoParameterType.LineState:
                    if (dataPair.Data is LineInfoData lineInfoData)
                    {
                        IsWriteDeviceInformationEnabled = lineInfoData.LineIndex != null;
                        LineIndex = lineInfoData.LineIndex;
                    }

                    break;
            }
        }

        #endregion
    }
}