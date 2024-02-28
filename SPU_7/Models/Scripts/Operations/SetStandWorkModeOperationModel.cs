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
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.Models.Scripts.Operations;

public class SetStandWorkModeOperationModel : OperationModel
{
    public SetStandWorkModeOperationModel(IStandController standController,
        ILogger logger,
        IStandSettingsService standSettingsService,
        BaseOperationConfigurationModel configuration,
        ITimerService timerService,
        IOperationActionService operationActionService) : base(standController, logger, standSettingsService, configuration, timerService, operationActionService)
    {
    }

    public async override Task<OperationResult> Execute(CancellationTokenSource operationCancellationTokenSource)
    {
        try
        {
            var result = new BaseOperationResult();
            
            _timerService.TimeSeconds = 120;
            _timerService.OperationName = OperationName;
            _timerService.Message = "Подготовка стенда к работе";
            _timerService.InfoTimerEnable();

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                if (!await _standController.SetStandWorkModeAsync())
                {
                    _logger.Logging(new LogMessage("Не удалось установить рабочий режим установки", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось установить рабочий режим установки", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                //Открыть S2
                if (!await _standController.OpenSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalOpen)))
                {
                    _logger.Logging(new LogMessage("Не удалось открыть соленоидный клапан №2", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось открыть соленоидный клапан №2", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                //Закрыть М23
                if (!await _standController.CloseValveAsync(_standSettingsService.StandSettingsModel.ValveViewModels
                        .FirstOrDefault(valve => valve.IsPressureDifferenceValve)))
                {
                    _logger.Logging(new LogMessage("Не удалось закрыть клапан №23", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть клапан №23", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                //Закрыть М21
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

            if (!operationCancellationTokenSource.IsCancellationRequested)
            {
                //Закрыть S1
                if (!await _standController.CloseSolenoidValveAsync(_standSettingsService.StandSettingsModel.SolenoidValveViewModels
                        .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalClose)))
                {
                    _logger.Logging(new LogMessage("Не удалось закрыть соленоидный клапан №1", LogLevel.Error));
                    return new OperationResult(OperationResultType.Error, "Не удалось закрыть соленоидный клапан №1", result);
                }
            }
            else
            {
                return new OperationResult(OperationResultType.Stop, "Выполнение сценария прервано", result);
            }

            _timerService.InfoTimerDisable();

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