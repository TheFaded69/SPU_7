using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SPU_7.Common.Scripts;
using SPU_7.Common.Stand;
using SPU_7.Models.Scripts.Operations.Configurations;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.Models.Scripts.Operations;

public class CheckTightnessOperationModel : OperationModel
{
    public CheckTightnessOperationModel(IStandController standController,
        ILogger logger,
        IStandSettingsService standSettingsService,
        CheckTightnessOperationConfigurationModel configuration,
        ITimerService timerService,
        IOperationActionService operationActionService) : base(standController, logger, standSettingsService, configuration, timerService, operationActionService)
    {
    }

    public async override Task<OperationResult> Execute(CancellationTokenSource operationCancellationTokenSource)
    {
        try
        {
            var result = new BaseOperationResult();
            //Закрыть М21
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.CloseValveAsync(_standSettingsService.StandSettingsModel.ValveViewModels
                        .FirstOrDefault(valve => valve.IsTubeValve)))
                {
                    _logger.Logging(new LogMessage("Не удалось закрыть клапан №21", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть клапан №21", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }


            //Открыть S1
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.Number == 1)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №1", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №1", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            //Открыть М23
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenValveAsync(_standSettingsService.StandSettingsModel.ValveViewModels
                        .FirstOrDefault(valve => valve.IsPressureDifferenceValve)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть клапан №23", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось открыть клапан №23", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }
            

            //Закрыть S1
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.CloseSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.Number == 1)))
                {
                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №1", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            _standController.PidEnable();
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.EnableVacuumCreator())
                {
                    _logger.Logging(new LogMessage("Не удалось включить вакуумный насос", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось включить вакуумный насос", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            _timerService.TimeSeconds = ((CheckTightnessOperationConfigurationModel)_configuration).VacuumWaitTime;
            _timerService.OperationName = OperationName;
            _timerService.Message = "Ожидание вакуума";
            _timerService.InfoTimerEnable();

            while (_standController.PressureResiver > -50)
            {
                if (operationCancellationTokenSource.IsCancellationRequested)
                {
                    return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
                }

                await Task.Delay(3000);
            }
            
            _timerService.InfoTimerDisable();

            //Включаем сопло
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenNozzleAsync(((CheckTightnessOperationConfigurationModel)_configuration).SelectedStandSettingsNozzleModel))
                {
                    _logger.Logging(new LogMessage(
                        $"Не удалось включить сопло {((CheckTightnessOperationConfigurationModel)_configuration).SelectedStandSettingsNozzleModel.NozzleValue}", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error,
                        $"Не удалось включить сопло {((CheckTightnessOperationConfigurationModel)_configuration).SelectedStandSettingsNozzleModel.NozzleValue}", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }
            _standController.SetTargetFlowValue(((CheckTightnessOperationConfigurationModel)_configuration).SelectedStandSettingsNozzleModel.NozzleFactValue);

            _timerService.TimeSeconds = 30;
            _timerService.OperationName = OperationName;
            _timerService.Message = "Ожидание перепада давления";
            _timerService.InfoTimerEnable();
            
            while (_standController.PressureDifference < ((CheckTightnessOperationConfigurationModel)_configuration).PressureVacuumMinimum)
            {
                if (operationCancellationTokenSource.IsCancellationRequested)
                {
                    return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
                }

                await Task.Delay(1000);
            }
            
            _timerService.InfoTimerDisable();
            
            //Выключаем сопло
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.CloseNozzleAsync(((CheckTightnessOperationConfigurationModel)_configuration).SelectedStandSettingsNozzleModel))
                {
                    _logger.Logging(new LogMessage(
                        $"Не удалось выключить сопло {((CheckTightnessOperationConfigurationModel)_configuration).SelectedStandSettingsNozzleModel.NozzleValue}", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error,
                        $"Не удалось выключить сопло {((CheckTightnessOperationConfigurationModel)_configuration).SelectedStandSettingsNozzleModel.NozzleValue}", null);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }
            _standController.SetTargetFlowValue(0);

            _standController.PidDisable();
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.DisableVacuumCreator())
                {
                    _logger.Logging(new LogMessage("Не удалось выключить вакуумный насос", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось выключить вакуумный насос", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            _logger.Logging(new LogMessage($"Стабилизация - {((CheckTightnessOperationConfigurationModel)_configuration).StabilizationTime * 60} сек.", LogLevel.Info));
            
            _timerService.TimeSeconds = ((CheckTightnessOperationConfigurationModel)_configuration).StabilizationTime * 60;
            _timerService.OperationName = OperationName;
            _timerService.Message = "Стабилизация";
            _timerService.InfoTimerEnable();
            
            await Task.Delay(TimeSpan.FromMinutes(((CheckTightnessOperationConfigurationModel)_configuration).StabilizationTime));
            _logger.Logging(new LogMessage($"Стабилизация закончена", LogLevel.Success));

            _timerService.InfoTimerDisable();

            await Task.Delay(2000);
            
            //Фиксируем стартовое давление
            var startPressureDifference = _standController.PressureDifference;
            _logger.Logging(new LogMessage($"Начальное давление перепада - {startPressureDifference * 1000} Па", LogLevel.Info));

            _logger.Logging(new LogMessage($"Проверка на вакуум - {((CheckTightnessOperationConfigurationModel)_configuration).TestTime * 60} сек.", LogLevel.Info));
            //Ожидание в тесте
            
            _timerService.TimeSeconds = ((CheckTightnessOperationConfigurationModel)_configuration).TestTime * 60;
            _timerService.OperationName = OperationName;
            _timerService.Message = "Проверка на вакуум";
            _timerService.InfoTimerEnable();
            
            await Task.Delay(TimeSpan.FromMinutes(((CheckTightnessOperationConfigurationModel)_configuration).TestTime));
            _logger.Logging(new LogMessage($"Проверка на вакуум окончена", LogLevel.Success));
            
            _timerService.InfoTimerDisable();

            //Фиксируем конечное давление
            var endPressureDifference = _standController.PressureDifference;
            _logger.Logging(new LogMessage($"Конечное давление перепада - {endPressureDifference * 1000} Па", LogLevel.Info));
            _logger.Logging(new LogMessage($"Разница давления перепада - {(startPressureDifference - endPressureDifference) * 1000} Па", LogLevel.Info));

            if (startPressureDifference == null || endPressureDifference == null)
            {
                _logger.Logging(new LogMessage($"Не удалось считать давление ДРД", LogLevel.Error));
                return new OperationResult(OperationResultType.Error, $"Не удалось считать давление ДРД", result);
            }

            if (Math.Abs((float)(startPressureDifference - (float)endPressureDifference)) >
                ((CheckTightnessOperationConfigurationModel)_configuration).PressureDifferenceMaximum * 1000)
            {
                _logger.Logging(new LogMessage($"Установка не гермитична", LogLevel.Error));
                return new OperationResult(OperationResultType.Failed, "Установка не гермитична", result);
            }


            //Открыть S1
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.OpenSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.Number == 1)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №1", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №1", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            await Task.Delay(2000);

            //Закрыть S1
            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.CloseSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.Number == 1)))
                {
                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №1", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            _logger.Logging(new LogMessage($"Установка гермитична", LogLevel.Success));
            return new OperationResult(OperationResultType.Success, null, result);
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Fatal));
            return new OperationResult(OperationResultType.FatalError, e.Message, new BaseOperationResult(){Message = e.Message});
        }
        finally
        {
            _timerService.InfoTimerDisable();
        }
    }
}