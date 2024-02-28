using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Media.Transformation;
using SPU_7.Common.Scripts;
using SPU_7.Models.Scripts.Operations.Configurations;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Devices;

namespace SPU_7.Models.Scripts;

public abstract class OperationModel : IObservable
{
    protected OperationModel(IStandController standController, 
        ILogger logger, 
        IStandSettingsService standSettingsService, 
        BaseOperationConfigurationModel configuration,
        ITimerService timerService,
        IOperationActionService operationActionService)
    {
        _standController = standController;
        _logger = logger;
        _standSettingsService = standSettingsService;
        _timerService = timerService;
        _operationActionService = operationActionService;
        _configuration = configuration;
    }

    protected readonly IStandController _standController;
    protected readonly ILogger _logger;
    protected readonly IStandSettingsService _standSettingsService;
    protected readonly ITimerService _timerService;
    protected readonly IOperationActionService _operationActionService;

    protected string Message;

    protected BaseOperationConfigurationModel _configuration { get; set; }
    public BaseDeviceConfiguration DeviceConfiguration { get; set; }
    
    public int NumberOperation { get; set; }
    public string OperationName { get; set; }
    public OperationType OperationType { get; set; }

    public OperationStatus OperationStatus
    {
        get => _operationStatus;
        set
        {
            _operationStatus = value;
            NotifyObservers(value);
        }
    }

    public abstract Task<OperationResult> Execute(CancellationTokenSource operationCancellationTokenSource);
    

    private List<IObserver> _observers = new();
    private OperationStatus _operationStatus;

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