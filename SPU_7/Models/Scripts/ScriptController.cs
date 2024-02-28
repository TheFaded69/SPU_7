using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SPU_7.Common.Scripts;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.Models.Scripts
{
    public class ScriptController : IScriptController
    {
        public ScriptController(ILogger logger,
            IStandController standController,
            IScriptResultsDbService scriptResultsDbService,
            IStandSettingsService standSettingsService)
        {
            _logger = logger;
            _standController = standController;
            _scriptResultsDbService = scriptResultsDbService;
            _standSettingsService = standSettingsService;
        }

        private readonly ILogger _logger;
        private readonly IStandController _standController;
        private readonly IScriptResultsDbService _scriptResultsDbService;
        private readonly IStandSettingsService _standSettingsService;

        private ScriptModel _scriptModel;
        private ScriptResult _scriptResult;

        private Task _scriptExecuteTask;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationTokenSource _operationCancellationTokenSource;

        private bool _isScriptExecuting;
        private bool _needStopping;
        private bool _isScriptStop;

        public void Start()
        {
            if (_isScriptExecuting) return;

            _cancellationTokenSource = new CancellationTokenSource();
            _operationCancellationTokenSource = new CancellationTokenSource();
            _scriptExecuteTask = new Task(ExecuteScript, _cancellationTokenSource.Token);
            _scriptExecuteTask.Start();
        }

        public async void Stop()
        {
            if (!_isScriptExecuting) return;

            _operationCancellationTokenSource.Cancel();
            _cancellationTokenSource.Cancel();
            _isScriptStop = true;

            await _standController.EmergencyPowerOffAsync();
        }

        public void SetScript(ScriptModel scriptModel)
        {
            _scriptModel = scriptModel;
        }

        public void Dispose()
        {
            _standController.Dispose();
            _operationCancellationTokenSource.Dispose();
            _scriptExecuteTask.Dispose();
            _cancellationTokenSource.Dispose();
        }

        #region ScriptControl

        private async void ExecuteScript()
        {
            try
            {
                PrepareForExecuteScript();

                _logger.Logging(new LogMessage($"Сценарий \"{_scriptModel.ScriptName}\" запущен", LogLevel.Success));
                
                _isScriptExecuting = true;

                await ProcessExecutingScript();

                _isScriptExecuting = false;

                if (_isScriptStop)
                {
                    _isScriptStop = false;
                    _scriptModel.OperationModels.ForEach(om => om.OperationStatus = OperationStatus.Stop);
                    _logger.Logging(new LogMessage($"Выполнение сценария \"{_scriptModel.ScriptName}\" остановлено", LogLevel.Warning));
                    return;
                }

                SaveScriptResult();

                _logger.Logging(new LogMessage($"Сценарий \"{_scriptModel.ScriptName}\" выполнен", LogLevel.Success));
            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
                if (e.InnerException != null)
                    _logger.Logging(new LogMessage(e.InnerException.Message, LogLevel.Error));
            }
        }
        
        private void PrepareForExecuteScript()
        {
            _scriptResult = new ScriptResult()
            {
                Name = _scriptModel.ScriptName,
                Description = _scriptModel.Description,
            };

            _scriptModel.OperationModels
                .ForEach(om => om.OperationStatus = OperationStatus.ExecuteWaiting);
            
        }


        private async Task ProcessExecutingScript()
        {
            try
            {
                foreach (var operationModel in _scriptModel.OperationModels
                             .TakeWhile(_ => !_operationCancellationTokenSource.IsCancellationRequested)
                             .Where(operation => operation.OperationStatus == OperationStatus.ExecuteWaiting))
                {
                    operationModel.OperationStatus = OperationStatus.Executing;

                    _logger.Logging(new LogMessage($"Выполнение операции \"{operationModel.OperationName}\"", LogLevel.Info));

                    var operationResult = await operationModel.Execute(_operationCancellationTokenSource);
                    operationResult.OperationType = operationModel.OperationType;
                    
                    _scriptResult.OperationResults.Add(operationResult);
                    
                    operationModel.OperationStatus = operationResult.ResultStatus switch
                    {
                        OperationResultType.Failed => OperationStatus.CompletedFailed,
                        OperationResultType.Success => OperationStatus.CompletedPassed,
                        OperationResultType.Error => OperationStatus.CompletedError,
                        OperationResultType.WaitingConfirm => OperationStatus.ExecuteWaiting,
                        OperationResultType.Stop => OperationStatus.Stop,
                        _ => throw new ArgumentOutOfRangeException(nameof(operationResult.ResultStatus))
                    };

                    switch (operationModel.OperationStatus)
                    {
                        case OperationStatus.ExecuteWaiting:
                        case OperationStatus.Executing:
                            break;
                        case OperationStatus.CompletedPassed:
                            _logger.Logging(new LogMessage($"Операция \"{operationModel.OperationName}\" выполнена", LogLevel.Success));
                            break;
                        case OperationStatus.CompletedFailed:
                            _logger.Logging(new LogMessage($"Операция \"{operationModel.OperationName}\" выполнена неудачно", LogLevel.Error));
                            break;
                        case OperationStatus.CompletedError:
                            _logger.Logging(new LogMessage($"Операция \"{operationModel.OperationName}\" не выполнена из-за ошибки", LogLevel.Fatal));
                            break;
                        case OperationStatus.Stop:
                            _logger.Logging(new LogMessage($"Операция \"{operationModel.OperationName}\" не выполнена, сценарий прерван пользователем", LogLevel.Fatal));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message + " " + new StackTrace(false).GetFrame(0).GetMethod().Name, LogLevel.Fatal));
                throw;
            }
        }

        private void SaveScriptResult()
        {

            _scriptResultsDbService.AddScriptResult(_scriptResult);
        }
        
        #endregion

        private List<IObserver> _observers = new();

        public void RegisterObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers(object obj)
        {
            foreach (var observer in _observers)
            {
                observer.Update(obj);
            }
        }
    }
}