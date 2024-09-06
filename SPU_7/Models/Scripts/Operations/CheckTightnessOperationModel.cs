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