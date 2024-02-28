using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SPU_7.Common.Scripts;
using SPU_7.Common.Settings;
using SPU_7.Common.Stand;
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
    private readonly IManualOperationService _manualOperationService;
    private ValidationOperationResult _validationOperationResult;

    public override async Task<OperationResult> Execute(CancellationTokenSource operationCancellationTokenSource)
    {
        try
        {
            _validationOperationResult = new ValidationOperationResult()
            {
                VendorAddress = ((ValidationOperationConfigurationModel)_configuration).VendorAddress,
                VendorName = ((ValidationOperationConfigurationModel)_configuration).VendorName,
                DeviceName = ((ValidationOperationConfigurationModel)_configuration).DeviceName,
                ValidationType = ((ValidationOperationConfigurationModel)_configuration).ValidationType,
                MinimumFlow = ((ValidationOperationConfigurationModel)_configuration).MinimumFlow,
                MaximumFlow = ((ValidationOperationConfigurationModel)_configuration).MaximumFlow,
                NominalFlow = ((ValidationOperationConfigurationModel)_configuration).NominalFlow,
            };

            var activeLine = _standController.GetActiveLine();

            if (activeLine == null)
                return new OperationResult(OperationResultType.Error, "Не выбрана активная линия", _validationOperationResult);

            var deviceList = new List<DeviceInformation>();

#if !DEBUGGUI
            //Открыть S1
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №1", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №1", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

            await Task.Delay(500);

            //Закрыть S2
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.CloseSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                {
                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №2", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №2", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

            //Открыть М21
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenValveAsync(_standSettingsService.StandSettingsModel.ValveViewModels
                        .FirstOrDefault(valve => valve.IsTubeValve)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть клапан №21", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось открыть клапан №21", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

            //Открыть М23
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenValveAsync(_standSettingsService.StandSettingsModel.ValveViewModels
                        .FirstOrDefault(valve => valve.IsPressureDifferenceValve)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть клапан №23", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось открыть клапан №23", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

            _standController.PidEnable();

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.EnableVacuumCreator())
                {
                    _logger.Logging(new LogMessage("Не удалось включить насос", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось включить насос", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

#endif
            foreach (var point in ((ValidationOperationConfigurationModel)_configuration).Points)
            {
                var validationPointResult = new ValidationPointResult();
                _validationOperationResult.ValidationPointResults.Add(validationPointResult);

                switch (((ValidationOperationConfigurationModel)_configuration).ValidationType)
                {
                    case ValidationType.ValidationByVolume:
                    {
                        for (var measureIndex = 0; measureIndex < point.MeasureCount; measureIndex++)
                        {
#if !DEBUGGUI
                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.CloseSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №1", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1", null);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                            }

                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.OpenSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №2", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №2", null);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                            }
#endif

                            var deltaConsumption = await _standController.SetConsumptionAsync(point.TargetConsumption,
                                ((ValidationOperationConfigurationModel)_configuration).MinimumFlow,
                                ((ValidationOperationConfigurationModel)_configuration).MaximumFlow);

                            if (deltaConsumption == null)
                            {
                                _logger.Logging(new LogMessage($"Не удалось установить заданный расход в точке {point.Number}", LogLevel.Error));
                                return new OperationResult(OperationResultType.Error, $"Не удалось установить заданный расход в точке {point.Number}",
                                    _validationOperationResult);
                            }

                            var factConsumption = point.TargetConsumption - deltaConsumption;
                            var realConsumption = CalculateRealFlow(factConsumption,
                                _standController.TemperatureTube,
                                _standController.PressureDifference,
                                _standController.PressureAtmosphere,
                                _standController.Humidity);
                            var timeValidation = point.TargetVolume / realConsumption;

                            _standController.SetTargetFlowValue(realConsumption);

                            if (timeValidation == null)
                            {
                                _logger.Logging(new LogMessage($"Не удалось расчитать время поверки в точке {point.Number}", LogLevel.Error));
                                return new OperationResult(OperationResultType.Error, $"Не удалось расчитать время поверки в точке {point.Number}",
                                    _validationOperationResult);
                            }

                            var validationMeasureResult = new ValidationMeasureResult();
                            validationPointResult.ValidationMeasureResults.Add(validationMeasureResult);

                            for (var deviceIndex = 0;
                                 deviceIndex < _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine].DeviceViewModels.Count;
                                 deviceIndex++)
                            {
                                if (!_standController.GetDeviceManualEnable(deviceIndex)) continue;

                                var validationDeviceResult = new ValidationDeviceResult()
                                {
                                    PointNumber = point.Number,
                                    MeasureNumber = measureIndex + 1,
                                    TargetVolume = point.TargetVolume,
                                    TargetVolumeDifference = point.Inaccuracy,
                                    ValidationVolumeTime = ((double)timeValidation * 3600),
                                    VendorNumber = _standController.GetVendorNumber(deviceIndex),
                                    PressureDifference = _standController.PressureDifference,
                                    TargetFlow = point.TargetConsumption
                                };

                                if (_validationOperationResult.ValidationPointResults.Any(vp => vp.ValidationMeasureResults.Count > 1))
                                {
                                    var measureCount = _validationOperationResult.ValidationPointResults
                                        .LastOrDefault()
                                        .ValidationMeasureResults.Count;

                                    validationDeviceResult.StartVolumeValue = _validationOperationResult.ValidationPointResults
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

                                    _manualOperationService.ShowManualValidationResultDialog(OkAction, CancelAction
                                        , _validationOperationResult.ValidationPointResults,
                                        ((ValidationOperationConfigurationModel)_configuration).ValidationType,
                                        deviceIndex);

                                    while (!_cancellationTokenSource.Token.IsCancellationRequested)
                                    {
                                        if (operationCancellationTokenSource.IsCancellationRequested)
                                        {
                                            return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                                        }

                                        await Task.Delay(1000);
                                    }
                                }
                            }

#if !DEBUGGUI
                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.CloseSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №2", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №2",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }

                            await Task.Delay(500);

                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.OpenSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №1", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }
#endif


                            _logger.Logging(new LogMessage(
                                $"Установленный расход для точки №{point.Number} отличается от заданного на {point.TargetConsumption - realConsumption}",
                                LogLevel.Warning));
                            _logger.Logging(new LogMessage(
                                $"Время ожидания для точки №{point.Number} измерения №{measureIndex + 1}: {Math.Round((double)(timeValidation * 3600), 2)} сек",
                                LogLevel.Info));

                            _timerService.TimeSeconds = (int)(timeValidation * 3600);
                            _timerService.OperationName = OperationName;
                            _timerService.Message = "Прогон расхода через СГ";
                            _timerService.InfoTimerEnable();

                            await Task.Delay(TimeSpan.FromHours((double)timeValidation));

                            _timerService.InfoTimerDisable();
#if !DEBUGGUI
                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.CloseSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №1", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }

                            await Task.Delay(500);

                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.OpenSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №2", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №2",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }
#endif
                            _standController.SetTargetFlowValue(0);

                            for (var deviceIndex = 0;
                                 deviceIndex < _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine].DeviceViewModels.Count;
                                 deviceIndex++)
                            {
                                if (!_standController.GetDeviceManualEnable(deviceIndex)) continue;

                                _cancellationTokenSource = new CancellationTokenSource();

                                _manualOperationService.ShowManualValidationResultDialog(OkAction, CancelAction
                                    , _validationOperationResult.ValidationPointResults, ((ValidationOperationConfigurationModel)_configuration).ValidationType,
                                    deviceIndex);

                                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    if (operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                                    }

                                    await Task.Delay(1000);
                                }
                            }
                        }
                    }
                        break;
                    case ValidationType.ValidationByFlow:
                    {
#if !DEBUGGUI
                        if (!operationCancellationTokenSource.IsCancellationRequested)
                        {
                            if (await _standController.CloseSolenoidValveAsync(
                                    _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                        svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                            {
                                _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №2", LogLevel.Error));
                                return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №2", null);
                            }
                        }
                        else
                        {
                            return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                        }
#endif
                        var deltaConsumption = await _standController.SetConsumptionAsync(point.TargetConsumption,
                            ((ValidationOperationConfigurationModel)_configuration).MinimumFlow,
                            ((ValidationOperationConfigurationModel)_configuration).MaximumFlow);

                        if (deltaConsumption == null)
                        {
                            _logger.Logging(new LogMessage($"Не удалось установить заданный расход в точке {point.Number}", LogLevel.Error));
                            return new OperationResult(OperationResultType.Error, $"Не удалось установить заданный расход в точке {point.Number}", null);
                        }
#if !DEBUGGUI
                        if (!operationCancellationTokenSource.IsCancellationRequested)
                        {
                            if (await _standController.OpenSolenoidValveAsync(
                                    _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                        svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                            {
                                _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №2", LogLevel.Error));
                                return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №2", null);
                            }
                        }
                        else
                        {
                            return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                        }
#endif


                        while (_standController.PressureResiver < -50)
                        {
                            if (operationCancellationTokenSource.IsCancellationRequested)
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                            }

                            await Task.Delay(5000);
                        }

                        var realConsumption = point.TargetConsumption - deltaConsumption;

                        for (var i = 0; i < point.MeasureCount; i++)
                        {
                            var validationMeasureResult = new ValidationMeasureResult();
                            validationPointResult.ValidationMeasureResults.Add(validationMeasureResult);

                            var consumptionList = new List<double?>();
                            consumptionList.Add(CalculateRealFlow(realConsumption,
                                _standController.TemperatureTube,
                                _standController.PressureDifference,
                                _standController.PressureAtmosphere,
                                _standController.Humidity));

                            /*for (var j = 0; j < _standSettingsService.StandSettingsModel.DeviceViewModels.Count; j++)
                            {
                                var validationDeviceResult = new ValidationDeviceResult();
                                validationMeasureResult.ValidationDeviceResults.Add(validationDeviceResult);

                                _cancellationTokenSource = new CancellationTokenSource();

                                _manualOperationService.ShowManualValidationResultDialog(OkAction, CancelAction,
                                    _validationOperationResult.ValidationPointResults, ((ValidationOperationConfigurationModel)_configuration).ValidationType,
                                    j);

                                while (!_cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    if (operationCancellationTokenSource.IsCancellationRequested)
                                    {
                                        return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                                    }

                                    await Task.Delay(10000);

                                    consumptionList.Add(CalculateRealFlow(realConsumption,
                                        _standController.TemperatureTube,
                                        _standController.PressureDifference,
                                        _standController.PressureAtmosphere,
                                        _standController.Humidity));
                                }

                                consumptionList.RemoveAll(cl => cl == null);

                                validationMeasureResult.ValidationDeviceResults.Add(new ValidationDeviceResult
                                {
                                    TargetFlow = consumptionList.Average(),
                                    CalculateFlow = null,
                                    StartVolumeValue = null,
                                    EndVolumeValue = null,
                                    ValidationFlowTime = null,
                                });
                            }*/

                            validationPointResult.ValidationMeasureResults.Add(validationMeasureResult);
                        }
                    }

                        break;
                    case ValidationType.AutoValidationByVolume:
                    {
                        for (var measureIndex = 0; measureIndex < point.MeasureCount; measureIndex++)
                        {
                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.CloseSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №1", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1", null);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                            }

                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.OpenSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №2", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №2", null);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", null);
                            }


                            var deltaConsumption = await _standController.SetConsumptionAsync(point.TargetConsumption,
                                ((ValidationOperationConfigurationModel)_configuration).MinimumFlow,
                                ((ValidationOperationConfigurationModel)_configuration).MaximumFlow);

                            if (deltaConsumption == null)
                            {
                                _logger.Logging(new LogMessage($"Не удалось установить заданный расход в точке {point.Number}", LogLevel.Error));
                                return new OperationResult(OperationResultType.Error, $"Не удалось установить заданный расход в точке {point.Number}",
                                    _validationOperationResult);
                            }

                            var factConsumption = point.TargetConsumption - deltaConsumption;
                            var realConsumption = CalculateRealFlow(factConsumption,
                                _standController.TemperatureTube,
                                _standController.PressureDifference,
                                _standController.PressureAtmosphere,
                                _standController.Humidity);
                            _standController.SetTargetFlowValue(realConsumption);

                            var pulseCountList = new List<int>();
                            
                            for (var deviceIndex = 0; deviceIndex < _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine].DeviceViewModels.Count; deviceIndex++)
                            {
                                var pulseCount = (int)Math.Ceiling(point.TargetVolume / ((ValidationOperationConfigurationModel)_configuration) .PulseMeterConfigurations[deviceIndex].PulseWeight);
                                pulseCountList.Add(pulseCount);

                                if (!await _standController.SetPulseCountAsync(pulseCount, deviceIndex))
                                {
                                    _logger.Logging(new LogMessage($"Не удалось установить количество импульсов в точке {point.Number}", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, $"Не удалось установить количество импульсов в точке {point.Number}",
                                        _validationOperationResult);
                                }
                            }
                            
                            var validationMeasureResult = new ValidationMeasureResult();
                            validationPointResult.ValidationMeasureResults.Add(validationMeasureResult);
                            
                             if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.CloseSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №2", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №2",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }

                            await Task.Delay(500);

                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.OpenSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №1", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }
                            var timeValidation = point.TargetVolume / realConsumption;
                            _logger.Logging(new LogMessage(
                                $"Установленный расход для точки №{point.Number} отличается от заданного на {point.TargetConsumption - realConsumption}",
                                LogLevel.Warning));
                            _logger.Logging(new LogMessage(
                                $"Примерное время ожидания для точки №{point.Number} измерения №{measureIndex + 1}: {Math.Round((double)(timeValidation * 3600), 2)} сек",
                                LogLevel.Info));
                            
                            _timerService.TimeSeconds = (int)(timeValidation * 3600);
                            _timerService.OperationName = OperationName;
                            _timerService.Message = "Прогон расхода через СГ";
                            _timerService.InfoTimerEnable();


                            var deviceReadyList = _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine].DeviceViewModels
                                .Select(_ => false)
                                .ToList();
                            while (true)
                            {
                                if (deviceReadyList.TrueForAll(ready => ready)) break;
                                
                                for (var deviceIndex = 0; deviceIndex < _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine].DeviceViewModels.Count; deviceIndex++)
                                {
                                    if (deviceReadyList[deviceIndex]) continue;

                                    var nowPulseCount = await _standController.ReadPulseCountAsync(deviceIndex);

                                    if (nowPulseCount == null) continue;

                                    if (nowPulseCount >= pulseCountList[deviceIndex]) deviceReadyList[deviceIndex] = true;
                                }
                            }
                            
                            _timerService.InfoTimerDisable();
                            
                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.CloseSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №1", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }

                            await Task.Delay(500);

                            if (!operationCancellationTokenSource.IsCancellationRequested)
                            {
                                if (!await _standController.OpenSolenoidValveAsync(
                                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                                            svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                                {
                                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №2", LogLevel.Error));
                                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №2",
                                        _validationOperationResult);
                                }
                            }
                            else
                            {
                                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
                            }

                            _standController.SetTargetFlowValue(0);
                            
                            for (var deviceIndex = 0; deviceIndex < _standSettingsService.StandSettingsModel.LineViewModels[(int)activeLine].DeviceViewModels.Count; deviceIndex++)
                            {
                                var startTime = await _standController.ReadStartPulseMeasureTimeAsync(deviceIndex);
                                var endTime = await _standController.ReadEndPulseMeasureTimeAsync(deviceIndex);

                                var time = endTime - startTime;
                                var realVolume = realConsumption * time;
                                var deviceVolume = point.TargetVolume;

                                var difference = (deviceVolume - realVolume) / realVolume * 100;
                            }
                        }
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

#if !DEBUGGUI
            _standController.PidDisable();
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.DisableVacuumCreator())
                {
                    _logger.Logging(new LogMessage("Не удалось выключить насос", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось выключить насос", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenSolenoidValveAsync(
                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                            svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №1", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

            await Task.Delay(500);

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.CloseSolenoidValveAsync(
                        _standSettingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(svm =>
                            svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                {
                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №2", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №2", _validationOperationResult);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", _validationOperationResult);
            }

            if (((ValidationOperationConfigurationModel)_configuration).IsProtocolNeed)
            {
                var line = _standController.GetActiveLine();

                if (line == null) throw new ApplicationException("Не выбрана активная линия, протоколы не удалось создать. ");

                for (var i = 0; i < _standSettingsService.StandSettingsModel.LineViewModels[(int)line].DeviceViewModels.Count; i++)
                {
                    if (!_standController.GetDeviceManualEnable(i)) continue;
                    var deviceViewModel = _standSettingsService.StandSettingsModel.LineViewModels[(int)line].DeviceViewModels[i];
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
                        VendorAddress = ((ValidationOperationConfigurationModel)_configuration).VendorAddress,
                        VendorNumber = _standController.GetVendorNumber(i),
                        VendorName = ((ValidationOperationConfigurationModel)_configuration).VendorName,
                        DeviceName = ((ValidationOperationConfigurationModel)_configuration).DeviceName,
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

    private double? CalculateRealFlow(double? factFlow, float? temperature, float? pressureDifference, float? pressureAtmosphere, float? humidity)
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

        return (float?)(factFlow * Math.Sqrt(((double)temperature + 273.15f) / 293.15f) * (1 - pressureDifference / pressureAtmosphere) / coefficient);
    }
}