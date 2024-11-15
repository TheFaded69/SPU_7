using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DynamicData;
using SPU_7.Common.Line;
using SPU_7.Common.Scripts;
using SPU_7.Common.Settings;
using SPU_7.Common.Stand;
using SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;
using SPU_7.Models.Scripts.Operations.Configurations;
using SPU_7.Models.Scripts.Operations.Protocols;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Scripts.Operations.Results.Extensions;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.Models.Stand.StaticData;

namespace SPU_7.Models.Scripts.Operations;

public class ValidationOperationModel : OperationModel
{
    public ValidationOperationModel(IStandController standController,
        ILogger logger,
        IStandSettingsService standSettingsService,
        BaseOperationConfigurationModel configuration,
        IManualOperationService manualOperationService,
        ITimerService timerService,
        IOperationActionService operationActionService)
        : base(standController, logger, standSettingsService, configuration, timerService, operationActionService)
    {
        _manualOperationService = manualOperationService;
    }


    private CancellationTokenSource _cancellationTokenSource = new();
    private CancellationTokenSource _replaceNozzleCancellationTokenSource = new();
    private readonly IManualOperationService _manualOperationService;
    private ValidationOperationResult _validationOperationResult;
    private bool _isReplaceNozzleReady;


    public override async Task<OperationResult> Execute(CancellationTokenSource operationCancellationTokenSource)
    {
        try
        {
            _validationOperationResult = new ValidationOperationResult()
            {
                ValidationType = ((ValidationOperationConfigurationModel)_configuration).ValidationType,
                MinimumFlow = ((ValidationOperationConfigurationModel)_configuration).MinimumFlow,
                MaximumFlow = ((ValidationOperationConfigurationModel)_configuration).MaximumFlow,
                NominalFlow = ((ValidationOperationConfigurationModel)_configuration).NominalFlow,
            };

            var activeLine = _standController.GetActiveLine();

            if (activeLine == null)
                return new OperationResult(OperationResultType.Error, "Не выбрана активная линия",
                    _validationOperationResult);

            var deviceList = new List<DeviceInformation>();

            _timerService.OperationName = OperationName;
            _timerService.TimeSeconds = 60;
            _timerService.Message = "Подготовка стенда к поверке";
            _timerService.InfoTimerEnable();

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.SetStandWorkModeAsync())
                {
                    _logger.Logging(new LogMessage("Не удалось подготовить стенд к поверке", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось подготовить стенд к поверке",
                        null);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
            }

            _timerService.InfoTimerDisable();

            await Task.Delay(2000);

            for (var pointIndex = 0;
                 pointIndex < ((ValidationOperationConfigurationModel)_configuration).Points.Count;
                 pointIndex++)
            {
                var point = ((ValidationOperationConfigurationModel)_configuration).Points[pointIndex];
                var validationPointResult = new ValidationPointResult();
                float? avgFlow;

                _validationOperationResult.ValidationPointResults.Add(validationPointResult);

                switch (_standSettingsService.StandSettingsModel.LineViewModels[point.SelectedLineNumber - 1]
                            .SelectedLineType)
                {
                    case LineType.None:
                        break;
                    case LineType.MasterDeviceLineType:
                        switch (((ValidationOperationConfigurationModel)_configuration).ValidationType)
                        {
                            case ValidationType.None:
                                break;
                            case ValidationType.ValidationByVolume:
                                for (var measureIndex = 0; measureIndex < point.MeasureCount; measureIndex++)
                                {
                                    var targetConsumption = point.TargetConsumption;
                                    var timeValidation = point.TargetVolume / point.TargetConsumption;

                                    var indexOfFanLine =
                                        _standSettingsService.StandSettingsModel.LineViewModels.IndexOf(
                                            _standSettingsService.StandSettingsModel.LineViewModels
                                                .FirstOrDefault(line => line.FanViewModels.Any(fan =>
                                                    fan.MinimumFlow <= targetConsumption &&
                                                    fan.MaximumFlow >= targetConsumption)));
                                    var indexOfFan = _standSettingsService.StandSettingsModel
                                        .LineViewModels[indexOfFanLine].FanViewModels.IndexOf(_standSettingsService
                                            .StandSettingsModel.LineViewModels[indexOfFanLine].FanViewModels
                                            .FirstOrDefault(fan =>
                                                fan.MinimumFlow <= targetConsumption &&
                                                fan.MaximumFlow >= targetConsumption));
                                    var indexOfDeviceLine = _standController.GetActiveLine();
                                    var indexOfMasterDeviceLine =
                                        _standSettingsService.StandSettingsModel.LineViewModels.IndexOf(
                                            _standSettingsService.StandSettingsModel.LineViewModels.FirstOrDefault(
                                                line =>
                                                    line.MasterDeviceViewModels.Any(md =>
                                                        md.MasterDeviceName == point.SelectedMasterDeviceName)));
                                    var indexOfMasterDevice = _standSettingsService.StandSettingsModel
                                        .LineViewModels[indexOfMasterDeviceLine].MasterDeviceViewModels
                                        .IndexOf(_standSettingsService.StandSettingsModel
                                            .LineViewModels[indexOfMasterDeviceLine].MasterDeviceViewModels
                                            .FirstOrDefault(mas =>
                                                mas.MasterDeviceName == point.SelectedMasterDeviceName));

                                    _timerService.OperationName = OperationName;
                                    _timerService.TimeSeconds = 90;
                                    _timerService.Message = $"Открытие кранов для точки №{point.Number}";
                                    _timerService.InfoTimerEnable();

                                    if (point.SelectedLineNumber - 1 > indexOfFanLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;

                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (currentIndex > 0)
                                            {
                                                if (!operationCancellationTokenSource.IsCancellationRequested)
                                                {
                                                    if (!await _standController.OpenValveAsync(
                                                            _standSettingsService.StandSettingsModel
                                                                .LineViewModels[currentIndex - 1]
                                                                .EndCommonValveViewModel, true))
                                                    {
                                                        _logger.Logging(new LogMessage(
                                                            "Не удалось открыть кран после эталонов", LogLevel.Error));
                                                        return new OperationResult(OperationResultType.Error,
                                                            "Не удалось открыть кран после эталонов",
                                                            null);
                                                    }
                                                }
                                                else
                                                {
                                                    return new OperationResult(OperationResultType.Stop,
                                                        "Выполнение сценария прервано", null);
                                                }
                                            }

                                            currentIndex--;
                                        }
                                    }
                                    else if (point.SelectedLineNumber - 1 < indexOfFanLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;


                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (currentIndex > 0)
                                            {
                                                if (!operationCancellationTokenSource.IsCancellationRequested)
                                                {
                                                    if (!await _standController.OpenValveAsync(
                                                            _standSettingsService.StandSettingsModel
                                                                .LineViewModels[currentIndex - 1]
                                                                .EndCommonValveViewModel))
                                                    {
                                                        _logger.Logging(new LogMessage(
                                                            "Не удалось открыть кран после эталонов", LogLevel.Error));
                                                        return new OperationResult(OperationResultType.Error,
                                                            "Не удалось открыть кран после эталонов",
                                                            null);
                                                    }
                                                }
                                                else
                                                {
                                                    return new OperationResult(OperationResultType.Stop,
                                                        "Выполнение сценария прервано", null);
                                                }
                                            }

                                            currentIndex++;
                                        }
                                    }

                                    if (point.SelectedLineNumber - 1 > indexOfDeviceLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;

                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (currentIndex > 0)
                                            {
                                                if (!operationCancellationTokenSource.IsCancellationRequested)
                                                {
                                                    if (!await _standController.OpenValveAsync(
                                                            _standSettingsService.StandSettingsModel
                                                                .LineViewModels[currentIndex - 1]
                                                                .StartCommonValveViewModel, true))
                                                    {
                                                        _logger.Logging(new LogMessage(
                                                            "Не удалось открыть общий кран после устройств",
                                                            LogLevel.Error));
                                                        return new OperationResult(OperationResultType.Error,
                                                            "Не удалось открыть общий кран после устройств",
                                                            null);
                                                    }
                                                }
                                                else
                                                {
                                                    return new OperationResult(OperationResultType.Stop,
                                                        "Выполнение сценария прервано", null);
                                                }
                                            }

                                            currentIndex--;
                                        }
                                    }
                                    else if (point.SelectedLineNumber - 1 < indexOfDeviceLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;

                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (currentIndex > 0)
                                            {
                                                if (!operationCancellationTokenSource.IsCancellationRequested)
                                                {
                                                    if (!await _standController.OpenValveAsync(
                                                            _standSettingsService.StandSettingsModel
                                                                .LineViewModels[currentIndex - 1]
                                                                .StartCommonValveViewModel))
                                                    {
                                                        _logger.Logging(new LogMessage(
                                                            "Не удалось открыть общий кран после устройств",
                                                            LogLevel.Error));
                                                        return new OperationResult(OperationResultType.Error,
                                                            "Не удалось открыть общий кран после устройств",
                                                            null);
                                                    }
                                                }
                                                else
                                                {
                                                    return new OperationResult(OperationResultType.Stop,
                                                        "Выполнение сценария прервано", null);
                                                }
                                            }

                                            currentIndex++;
                                        }
                                    }

                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.OpenValveAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PressureSensorValveViewModel, true))
                                        {
                                            _logger.Logging(new LogMessage(
                                                "Не удалось открыть кран датчика перепада эталона", LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                "Не удалось открыть кран датчика перепада эталона",
                                                null);
                                        }
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }

                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.OpenValveAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .MasterDeviceValveViewModel, true))
                                        {
                                            _logger.Logging(new LogMessage("Не удалось открыть кран эталона",
                                                LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                "Не удалось открыть кран эталона",
                                                null);
                                        }
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }
                                    
                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.OpenValveAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfFanLine]
                                                    .FanViewModels[indexOfFan]
                                                    .FanValveViewModel, true))
                                        {
                                            _logger.Logging(new LogMessage("Не удалось открыть кран подачи расхода",
                                                LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                "Не удалось открыть кран подачи расхода",
                                                null);
                                        }
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }
                                    
                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.OpenValveAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .StartValveMasterDeviceViewModel, true))
                                        {
                                            _logger.Logging(new LogMessage("Не удалось открыть кран перед группой эталонов",
                                                LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                "Не удалось открыть кран перед группой эталонов",
                                                null);
                                        }
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }
                                    
                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.UpdateAllDevice())
                                        {
                                            _logger.Logging(new LogMessage($"Не удалось открыть краны для точки {point.Number}",
                                                LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                $"Не удалось открыть краны для точки {point.Number}",
                                                null);
                                        }

                                        await Task.Delay(40000);
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }

                                    _timerService.InfoTimerDisable();

                                    // какие-то действия
                                    var validationMeasureResult = new ValidationMeasureResult();
                                    validationPointResult.ValidationMeasureResults.Add(validationMeasureResult);

                                    for (var deviceIndex = 0;
                                         deviceIndex < _standSettingsService.StandSettingsModel
                                             .LineViewModels[(int)activeLine].DeviceViewModels.Count;
                                         deviceIndex++)
                                    {
                                        if (!_standController.GetDeviceManualEnable(deviceIndex)) continue;

                                        var validationDeviceResult = new ValidationDeviceResult()
                                        {
                                            PointNumber = point.Number,
                                            MeasureNumber = measureIndex + 1,
                                            TargetVolume = point.TargetVolume,
                                            TargetVolumeDifference = point.Inaccuracy,
                                            ValidationVolumeTime = timeValidation * 3600,
                                            VendorNumber = _standController.GetVendorNumber(deviceIndex),
                                            OwnerName = _standController.GetVendorName((int)activeLine, deviceIndex),
                                            DeviceInfo = _standController
                                                .GetDeviceInfoType((int)activeLine, deviceIndex)
                                                .DeviceTypeInfo,
                                            PressureDifference =
                                                _standController
                                                    .GetPressureDifferenceFromMasterDevice((int)activeLine) / 1000,
                                            TargetFlow = point.TargetConsumption
                                        };

                                        if (_validationOperationResult.ValidationPointResults.Any(vp =>
                                                vp.ValidationMeasureResults.Count > 1))
                                        {
                                            var measureCount = _validationOperationResult.ValidationPointResults
                                                .LastOrDefault()
                                                .ValidationMeasureResults.Count;

                                            validationDeviceResult.StartVolumeValue = _validationOperationResult
                                                .ValidationPointResults
                                                .LastOrDefault()
                                                .ValidationMeasureResults[measureCount - 1]
                                                .ValidationDeviceResults[deviceIndex]
                                                .EndVolumeValue;

                                            validationMeasureResult.ValidationDeviceResults.Add(validationDeviceResult);
                                        }
                                        else
                                        {
                                            validationMeasureResult.ValidationDeviceResults.Add(validationDeviceResult);
                                            _cancellationTokenSource = new CancellationTokenSource();

                                            _manualOperationService.ShowManualValidationResultDialog(OkAction,
                                                CancelAction
                                                , _validationOperationResult.ValidationPointResults,
                                                ((ValidationOperationConfigurationModel)_configuration).ValidationType,
                                                deviceIndex);

                                            while (!_cancellationTokenSource.Token.IsCancellationRequested)
                                            {
                                                if (operationCancellationTokenSource.IsCancellationRequested)
                                                {
                                                    return new OperationResult(OperationResultType.Stop,
                                                        "Выполнение сценария прервано", _validationOperationResult);
                                                }

                                                await Task.Delay(1000);
                                            }
                                        }
                                    }

                                    double? devicePulseCount = null;
                                    float? masterDevicePulseCount = null;

                                    _timerService.OperationName = OperationName;
                                    _timerService.TimeSeconds = 30;
                                    _timerService.Message = $"Запуск подачи расхода в точке №{point.Number}";
                                    _timerService.InfoTimerEnable();

                                    // запуск подча расхода
                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.EnableConsumptionAsync(targetConsumption,
                                                indexOfFanLine, indexOfFan, indexOfMasterDeviceLine,
                                                indexOfMasterDevice,
                                                !point.IsNeedleValveUse ? null : point.NeedleValveValue))
                                        {
                                            _logger.Logging(new LogMessage("Не удалось начать подачу расхода",
                                                LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                "Не удалось начать подачу расхода",
                                                null);
                                        }
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }

                                    _timerService.InfoTimerDisable();

                                    _timerService.OperationName = OperationName;
                                    _timerService.TimeSeconds = 300;
                                    _timerService.Message = "Ожидание стабильности расхода";
                                    _timerService.InfoTimerEnable();

                                    // ожидание стабильности расхода

                                    var dateStart = DateTime.Now;
                                    
                                    var isFLowCorrect = false;
                                    var flowList = new List<float?>();
                                    while (true)
                                    {
                                        if ((DateTime.Now - dateStart).Seconds > 300)
                                        {
                                            _logger.Logging(new LogMessage(
                                                $"Не удалось стабилизировать расхода в точке {point.Number}",
                                                LogLevel.Error));

                                            return new OperationResult(OperationResultType.Error,
                                                $"Не удалось стабилизировать расхода в точке {point.Number}",
                                                _validationOperationResult);
                                        }
                                        
                                        if (flowList.Count == 10)
                                        {
                                            avgFlow = flowList.Average();

                                            if (Math.Abs((double)(avgFlow - point.TargetConsumption)) /
                                                point.TargetConsumption < 0.01)
                                            {
                                                break;
                                            }

                                            var currentFlow =
                                                _standController.GetFlowFromMasterDevice(indexOfMasterDeviceLine,
                                                    indexOfMasterDevice);
                                            flowList.RemoveAt(0);
                                            if (currentFlow != null) flowList.Add(currentFlow);
                                        }
                                        else
                                        {
                                            var currentFlow =
                                                _standController.GetFlowFromMasterDevice(indexOfMasterDeviceLine,
                                                    indexOfMasterDevice);
                                            if (currentFlow != null) flowList.Add(currentFlow);
                                        }

                                        await Task.Delay(3000);
                                    }

                                    _timerService.InfoTimerDisable();

                                    if (((ValidationOperationConfigurationModel)_configuration).IsAutoPulseMeasure)
                                    {
                                        //Настройка МПКИ
                                        await _standController.ResetPulseCountMeterAsync();
                                        await _standController.TurnOnPulseCountMeterControlRegister();

                                        await _standController.SetPulseCountMeterModuleChannelSettingsAsync(
                                            _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine]
                                                .DeviceViewModels[0].PulseCountMeterModuleNumber - 1,
                                            (int)_standSettingsService.StandSettingsModel
                                                .LineViewModels[(int)activeLine].DeviceViewModels[0]
                                                .PulseCountMeterModuleChannelNumber);
                                        await _standController.SetPulseCountMeterModuleChannelSettingsAsync(
                                            _standSettingsService.StandSettingsModel
                                                .LineViewModels[indexOfMasterDeviceLine]
                                                .MasterDeviceViewModels[indexOfMasterDevice]
                                                .PulseCountMeterModuleNumber - 1,
                                            (int)_standSettingsService.StandSettingsModel
                                                .LineViewModels[indexOfMasterDeviceLine]
                                                .MasterDeviceViewModels[indexOfMasterDevice]
                                                .PulseCountMeterModuleChannelNumber);
                                        await Task.Delay(2000);

                                        await _standController.StartPulseCountMeterModuleMeasureAsync(
                                            _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine]
                                                .DeviceViewModels[0].PulseCountMeterModuleNumber - 1);
                                        await _standController.StartPulseCountMeterModuleMeasureAsync(
                                            _standSettingsService.StandSettingsModel
                                                .LineViewModels[indexOfMasterDeviceLine]
                                                .MasterDeviceViewModels[indexOfMasterDevice]
                                                .PulseCountMeterModuleNumber - 1);
                                        await Task.Delay(2000);

                                        await _standController.SendStartPulseCountMeterCommandAsync();

                                        // какие-то действия
                                        _timerService.TimeSeconds = (int)(timeValidation * 3600);
                                        _timerService.OperationName = OperationName;
                                        _timerService.Message = "Прогон расхода через СГ";
                                        _timerService.InfoTimerEnable();

                                        await Task.Delay(TimeSpan.FromHours(timeValidation));

                                        _timerService.InfoTimerDisable();

                                        // счет импульсов с поверяемого СГ , затем с эталона после окончания счета
                                        var targetPulseCount = point.TargetVolume / _standSettingsService
                                            .StandSettingsModel.LineViewModels[indexOfMasterDeviceLine]
                                            .MasterDeviceViewModels[indexOfMasterDevice].PulseWeight;

                                        await _standController.SendStartPulseCountMeterCommandAsync();

                                        await Task.Delay(2000);

                                        masterDevicePulseCount =
                                            await _standController.ReadPulseCountFromPulseCountMeterAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleChannelNumber);
                                        devicePulseCount =
                                            await _standController.ReadPulseCountFromPulseCountMeterAsync(
                                                _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine]
                                                    .DeviceViewModels[0].PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[(int)activeLine].DeviceViewModels[0]
                                                    .PulseCountMeterModuleChannelNumber);

                                        // остановка подачи расхода
                                        if (!operationCancellationTokenSource.IsCancellationRequested)
                                        {
                                            if (!await _standController.DisableConsumptionAsync(targetConsumption,
                                                    indexOfFanLine, indexOfFan))
                                            {
                                                _logger.Logging(new LogMessage("Не удалось прекратить подачу расхода",
                                                    LogLevel.Error));
                                                return new OperationResult(OperationResultType.Error,
                                                    "Не удалось прекратить подачу расхода",
                                                    null);
                                            }
                                        }
                                        else
                                        {
                                            return new OperationResult(OperationResultType.Stop,
                                                "Выполнение сценария прервано", null);
                                        }
                                    }
                                    else
                                    {
                                        _timerService.TimeSeconds = 300;
                                        _timerService.OperationName = OperationName;
                                        _timerService.Message = "Ожидание запуска измерения с пульта";
                                        _timerService.InfoTimerEnable();

                                        while (true)
                                        {
                                            var status1 = await _standController.ReadPulseCountMeterMeasureStatusAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleChannelNumber);
                                            var status2 = await _standController.ReadPulseCountMeterMeasureStatusAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[(int)activeLine].DeviceViewModels[0]
                                                    .PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[(int)activeLine].DeviceViewModels[0]
                                                    .PulseCountMeterModuleChannelNumber);

                                            if (status1 == null || status2 == null)
                                            {
                                                _logger.Logging(new LogMessage("Не удалось считать состояние МПКИ",
                                                    LogLevel.Warning));
                                            }
                                            else if ((bool)status1 && (bool)status2) break;

                                            await Task.Delay(2000);
                                        }
                                        

                                        _timerService.InfoTimerDisable();
                                        
                                        _timerService.TimeSeconds = (int)(timeValidation * 3600);
                                        _timerService.OperationName = OperationName;
                                        _timerService.Message = "Прогон расхода через СГ";
                                        _timerService.InfoTimerEnable();
                                        
                                        while (true)
                                        {
                                            var status1 = await _standController.ReadPulseCountMeterMeasureStatusAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleChannelNumber);
                                            var status2 = await _standController.ReadPulseCountMeterMeasureStatusAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[(int)activeLine].DeviceViewModels[0]
                                                    .PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[(int)activeLine].DeviceViewModels[0]
                                                    .PulseCountMeterModuleChannelNumber);

                                            if (status1 == null || status2 == null)
                                            {
                                                _logger.Logging(new LogMessage("Не удалось считать состояние МПКИ",
                                                    LogLevel.Warning));
                                            }
                                            else if (!(bool)status1 && !(bool)status2) break;

                                            await Task.Delay(2000);
                                        }

                                        _timerService.InfoTimerDisable();
                                        
                                        masterDevicePulseCount =
                                            await _standController.ReadPulseCountFromPulseCountMeterAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PulseCountMeterModuleChannelNumber);
                                        devicePulseCount =
                                            await _standController.ReadPulseCountFromPulseCountMeterAsync(
                                                _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine]
                                                    .DeviceViewModels[0].PulseCountMeterModuleNumber - 1,
                                                (int)_standSettingsService.StandSettingsModel
                                                    .LineViewModels[(int)activeLine].DeviceViewModels[0]
                                                    .PulseCountMeterModuleChannelNumber);

                                        // остановка подачи расхода
                                        if (!operationCancellationTokenSource.IsCancellationRequested)
                                        {
                                            if (!await _standController.DisableConsumptionAsync(targetConsumption,
                                                    indexOfFanLine, indexOfFan))
                                            {
                                                _logger.Logging(new LogMessage("Не удалось прекратить подачу расхода",
                                                    LogLevel.Error));
                                                return new OperationResult(OperationResultType.Error,
                                                    "Не удалось прекратить подачу расхода",
                                                    null);
                                            }
                                        }
                                        else
                                        {
                                            return new OperationResult(OperationResultType.Stop,
                                                "Выполнение сценария прервано", null);
                                        }
                                    }

                                    _timerService.InfoTimerDisable();

                                    var deviceEnableIndex = -1;

                                    for (var deviceIndex = 0;
                                         deviceIndex < _standSettingsService.StandSettingsModel
                                             .LineViewModels[(int)activeLine].DeviceViewModels.Count;
                                         deviceIndex++)
                                    {
                                        if (!_standController.GetDeviceManualEnable((int)activeLine, deviceIndex))
                                            continue;

                                        deviceEnableIndex++;

                                        if (((ValidationOperationConfigurationModel)_configuration).IsAutoPulseMeasure)
                                        {
                                            if (devicePulseCount == null || masterDevicePulseCount == null)
                                            {
                                                _logger.Logging(new LogMessage(
                                                    $"Не удалось измерить импульсы на устройстве №{deviceIndex + 1}",
                                                    LogLevel.Error));

                                                _validationOperationResult.ValidationPointResults[pointIndex]
                                                    .ValidationMeasureResults[measureIndex]
                                                    .ValidationDeviceResults[deviceEnableIndex].TargetVolume = 0;
                                                _validationOperationResult.ValidationPointResults[pointIndex]
                                                        .ValidationMeasureResults[measureIndex]
                                                        .ValidationDeviceResults[deviceEnableIndex].EndVolumeValue =
                                                    _validationOperationResult.ValidationPointResults[pointIndex]
                                                        .ValidationMeasureResults[measureIndex]
                                                        .ValidationDeviceResults[deviceEnableIndex].StartVolumeValue +
                                                    point.TargetVolume;
                                                _validationOperationResult.ValidationPointResults[pointIndex]
                                                        .ValidationMeasureResults[measureIndex]
                                                        .ValidationDeviceResults[deviceEnableIndex]
                                                        .ValidationVolumeTime =
                                                    0;
                                                _validationOperationResult.ValidationPointResults[pointIndex]
                                                    .ValidationMeasureResults[measureIndex]
                                                    .ValidationDeviceResults[deviceEnableIndex].VolumeDifference = null;
                                                continue;
                                            }

                                            var deviceVolume = devicePulseCount *
                                                               ((ValidationOperationConfigurationModel)_configuration)
                                                               .PulseMeterConfigurations[0].PulseWeight;
                                            var masterDeviceVolume = masterDevicePulseCount * _standSettingsService
                                                .StandSettingsModel.LineViewModels[indexOfMasterDeviceLine]
                                                .MasterDeviceViewModels[indexOfMasterDevice].PulseWeight;

                                            var realVolume = masterDeviceVolume;

                                            _validationOperationResult.ValidationPointResults[pointIndex]
                                                    .ValidationMeasureResults[measureIndex]
                                                    .ValidationDeviceResults[deviceEnableIndex].TargetVolume =
                                                (double)realVolume;
                                            _validationOperationResult.ValidationPointResults[pointIndex]
                                                .ValidationMeasureResults[measureIndex]
                                                .ValidationDeviceResults[deviceEnableIndex].StartVolumeValue = 0;
                                            _validationOperationResult.ValidationPointResults[pointIndex]
                                                    .ValidationMeasureResults[measureIndex]
                                                    .ValidationDeviceResults[deviceEnableIndex].EndVolumeValue =
                                                _validationOperationResult.ValidationPointResults[pointIndex]
                                                    .ValidationMeasureResults[measureIndex]
                                                    .ValidationDeviceResults[deviceEnableIndex].StartVolumeValue +
                                                deviceVolume;
                                            _validationOperationResult.ValidationPointResults[pointIndex]
                                                    .ValidationMeasureResults[measureIndex]
                                                    .ValidationDeviceResults[deviceEnableIndex].ValidationVolumeTime =
                                                timeValidation;
                                            _validationOperationResult.ValidationPointResults[pointIndex]
                                                    .ValidationMeasureResults[measureIndex]
                                                    .ValidationDeviceResults[deviceEnableIndex].VolumeDifference =
                                                Math.Round(
                                                    (double)((_validationOperationResult
                                                                  .ValidationPointResults[pointIndex]
                                                                  .ValidationMeasureResults[measureIndex]
                                                                  .ValidationDeviceResults[deviceEnableIndex]
                                                                  .EndVolumeValue -
                                                              _validationOperationResult
                                                                  .ValidationPointResults[pointIndex]
                                                                  .ValidationMeasureResults[measureIndex]
                                                                  .ValidationDeviceResults[deviceEnableIndex]
                                                                  .StartVolumeValue -
                                                              _validationOperationResult
                                                                  .ValidationPointResults[pointIndex]
                                                                  .ValidationMeasureResults[measureIndex]
                                                                  .ValidationDeviceResults[deviceEnableIndex]
                                                                  .TargetVolume) /
                                                             _validationOperationResult
                                                                 .ValidationPointResults[pointIndex]
                                                                 .ValidationMeasureResults[measureIndex]
                                                                 .ValidationDeviceResults[deviceEnableIndex]
                                                                 .TargetVolume *
                                                             100)!, 2);
                                        }
                                        else
                                        {
                                            _cancellationTokenSource = new CancellationTokenSource();

                                            _manualOperationService.ShowManualValidationResultDialog(OkAction,
                                                CancelAction,
                                                _validationOperationResult.ValidationPointResults,
                                                ((ValidationOperationConfigurationModel)_configuration).ValidationType,
                                                deviceIndex);

                                            while (!_cancellationTokenSource.Token.IsCancellationRequested)
                                            {
                                                if (operationCancellationTokenSource.IsCancellationRequested)
                                                {
                                                    return new OperationResult(OperationResultType.Stop,
                                                        "Выполнение сценария прервано", _validationOperationResult);
                                                }

                                                await Task.Delay(1000);
                                            }
                                        }
                                    }

                                    if (point.SelectedLineNumber - 1 > indexOfFanLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;

                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (!operationCancellationTokenSource.IsCancellationRequested)
                                            {
                                                if (!await _standController.CloseValveAsync(
                                                        _standSettingsService.StandSettingsModel
                                                            .LineViewModels[currentIndex - 1].EndCommonValveViewModel))
                                                {
                                                    _logger.Logging(new LogMessage(
                                                        "Не удалось закрыть кран после эталонов", LogLevel.Error));
                                                    return new OperationResult(OperationResultType.Error,
                                                        "Не удалось закрыть кран после эталонов",
                                                        null);
                                                }
                                            }
                                            else
                                            {
                                                return new OperationResult(OperationResultType.Stop,
                                                    "Выполнение сценария прервано", null);
                                            }

                                            currentIndex--;
                                        }
                                    }
                                    else if (point.SelectedLineNumber - 1 < indexOfFanLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;

                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (!operationCancellationTokenSource.IsCancellationRequested)
                                            {
                                                if (!await _standController.CloseValveAsync(
                                                        _standSettingsService.StandSettingsModel
                                                            .LineViewModels[currentIndex - 1].EndCommonValveViewModel))
                                                {
                                                    _logger.Logging(new LogMessage(
                                                        "Не удалось закрыть кран после эталонов", LogLevel.Error));
                                                    return new OperationResult(OperationResultType.Error,
                                                        "Не удалось закрыть кран после эталонов",
                                                        null);
                                                }
                                            }
                                            else
                                            {
                                                return new OperationResult(OperationResultType.Stop,
                                                    "Выполнение сценария прервано", null);
                                            }

                                            currentIndex++;
                                        }
                                    }

                                    if (point.SelectedLineNumber - 1 > indexOfDeviceLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;

                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (!operationCancellationTokenSource.IsCancellationRequested)
                                            {
                                                if (!await _standController.CloseValveAsync(
                                                        _standSettingsService.StandSettingsModel
                                                            .LineViewModels[currentIndex - 1]
                                                            .StartCommonValveViewModel))
                                                {
                                                    _logger.Logging(new LogMessage(
                                                        "Не удалось закрыть общий кран после устройств",
                                                        LogLevel.Error));
                                                    return new OperationResult(OperationResultType.Error,
                                                        "Не удалось закрыть общий кран после устройств",
                                                        null);
                                                }
                                            }
                                            else
                                            {
                                                return new OperationResult(OperationResultType.Stop,
                                                    "Выполнение сценария прервано", null);
                                            }

                                            currentIndex--;
                                        }
                                    }
                                    else if (point.SelectedLineNumber - 1 < indexOfDeviceLine)
                                    {
                                        var currentIndex = point.SelectedLineNumber - 1;

                                        while (currentIndex != indexOfFanLine)
                                        {
                                            if (!operationCancellationTokenSource.IsCancellationRequested)
                                            {
                                                if (!await _standController.CloseValveAsync(
                                                        _standSettingsService.StandSettingsModel
                                                            .LineViewModels[currentIndex - 1]
                                                            .StartCommonValveViewModel))
                                                {
                                                    _logger.Logging(new LogMessage(
                                                        "Не удалось закрыть общий кран после устройств",
                                                        LogLevel.Error));
                                                    return new OperationResult(OperationResultType.Error,
                                                        "Не удалось закрыть общий кран после устройств",
                                                        null);
                                                }
                                            }
                                            else
                                            {
                                                return new OperationResult(OperationResultType.Stop,
                                                    "Выполнение сценария прервано", null);
                                            }

                                            currentIndex++;
                                        }
                                    }

                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.CloseValveAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .PressureSensorValveViewModel))
                                        {
                                            _logger.Logging(new LogMessage(
                                                "Не удалось закрыть кран датчика перепада эталона", LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                "Не удалось закрыть кран датчика перепада эталона",
                                                null);
                                        }
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }

                                    if (!operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        if (!await _standController.CloseValveAsync(
                                                _standSettingsService.StandSettingsModel
                                                    .LineViewModels[indexOfMasterDeviceLine]
                                                    .MasterDeviceViewModels[indexOfMasterDevice]
                                                    .MasterDeviceValveViewModel))
                                        {
                                            _logger.Logging(new LogMessage("Не удалось закрыть кран эталона",
                                                LogLevel.Error));
                                            return new OperationResult(OperationResultType.Error,
                                                "Не удалось закрыть кран эталона",
                                                null);
                                        }
                                    }
                                    else
                                    {
                                        return new OperationResult(OperationResultType.Stop,
                                            "Выполнение сценария прервано", null);
                                    }
                                }

                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

#if !DEBUGGUI
            if (((ValidationOperationConfigurationModel)_configuration).IsProtocolNeed)
            {
                var line = _standController.GetActiveLine();

                if (line == null)
                    throw new ApplicationException("Не выбрана активная линия, протоколы не удалось создать. ");

                for (var i = 0;
                     i < _standSettingsService.StandSettingsModel.LineViewModels[(int)line].DeviceViewModels.Count;
                     i++)
                {
                    if (!_standController.GetDeviceManualEnable(i)) continue;
                    var deviceViewModel = _standSettingsService.StandSettingsModel.LineViewModels[(int)line]
                        .DeviceViewModels[i];
                    var protocolCreator = new ValidationProtocolCreator();

                    //var protocolNumber = protocolCreator.CreateProtocol(_validationOperationResult, deviceViewModel.Number - 1);
                    var protocolNumber = protocolCreator.CreateProtocol(_validationOperationResult,
                        deviceViewModel.Number - 1,
                        _standSettingsService,
                        _standController.Temperature,
                        _standController.PressureAtmosphere,
                        _standController.Humidity);

                    _validationOperationResult.ValidationPointResults
                        .ForEach(vp => vp.ValidationMeasureResults
                            .ForEach(vm => vm.ValidationDeviceResults[i].ProtocolNumber = protocolNumber));

                    deviceList.Add(new DeviceInformation()
                    {
                        VendorAddress = "-",
                        VendorNumber = _standController.GetVendorNumber(i),
                        VendorName = _standController.GetVendorName((int)activeLine, i),
                        DeviceName = _standController.GetDeviceName(i),
                        ProtocolNumber = protocolNumber
                    });
                }
            }
#endif
            return new OperationResult(OperationResultType.Success, null, _validationOperationResult)
            {
                Device = deviceList
            };
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Fatal));
            throw;
        }
        finally
        {
            _timerService.InfoTimerDisable();
        }
    }

    private void OkAction(List<ValidationPointResult> validationPointResults)
    {
        _validationOperationResult.ValidationPointResults = validationPointResults;

        _cancellationTokenSource?.Cancel();
    }

    private void CancelAction(List<ValidationPointResult> validationPointResults)
    {
        _cancellationTokenSource?.Cancel();
    }

    private void PositiveReplaceNozzleAction()
    {
        _isReplaceNozzleReady = true;
        _replaceNozzleCancellationTokenSource?.Cancel();
    }

    private void NegativeReplaceNozzleAction()
    {
        _isReplaceNozzleReady = false;
        _replaceNozzleCancellationTokenSource?.Cancel();
    }

    private double? CalculateRealFlow(double? factFlow, float? temperature, float? pressureDifference,
        float? pressureAtmosphere, float? humidity)
    {
        var i = temperature switch
        {
            < 11 and > 5 => 0,
            >= 11 and < 13 => 1,
            >= 13 and < 15 => 2,
            >= 15 and < 17 => 3,
            >= 17 and < 19 => 4,
            >= 19 and < 21 => 5,
            >= 21 and < 23 => 6,
            >= 23 and < 25 => 7,
            >= 25 and < 27 => 8,
            >= 27 and < 29 => 9,
            >= 29 and <= 35 => 10,
            _ => -1,
        };

        var j = humidity switch
        {
            < 35 and >= 10 => 0,
            >= 35 and < 45 => 1,
            >= 45 and < 55 => 2,
            >= 55 and < 65 => 3,
            >= 65 and < 75 => 4,
            >= 75 and < 85 => 5,
            >= 85 and <= 95 => 6,
            _ => -1
        };

        if (i == -1 || j == -1) return null;

        var coefficient = MetrologyData.CoefficientCorrectHumidity[i, j];

        return (float?)(factFlow * Math.Sqrt(((double)temperature + 273.15f) / 293.15f) *
            (1 - pressureDifference / pressureAtmosphere) / coefficient);
    }
}