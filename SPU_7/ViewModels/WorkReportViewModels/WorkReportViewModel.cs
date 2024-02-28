using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Media.Transformation;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Scripts;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels.WorkReportViewModels.ResultViewModels.Validation;
using SPU_7.Views.WorkReportViews;
using SPU_7.Views.WorkReportViews.ResultView;

namespace SPU_7.ViewModels.WorkReportViewModels;

public class WorkReportViewModel : ViewModelBase, IDialogAware
{
    public WorkReportViewModel(IDeviceDbService deviceDbService, 
        IDialogService dialogService, 
        IScriptResultsDbService scriptResultsDbService,
        IOperationResultsDbService operationResultsDbService,
        ILogger logger)
    {
        _deviceDbService = deviceDbService;
        _dialogService = dialogService;
        _scriptResultsDbService = scriptResultsDbService;
        _operationResultsDbService = operationResultsDbService;
        _logger = logger;
        Title = "Отчет работы стенда";
        
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
        
        LoadScriptReportsCommand = new DelegateCommand(LoadScriptReportsCommandHandler);
        ResetScriptReportsCommand = new DelegateCommand(ResetScriptReportsCommandHandler);
    }

    private readonly IDeviceDbService _deviceDbService;
    private readonly IDialogService _dialogService;
    private readonly IScriptResultsDbService _scriptResultsDbService;
    private readonly IOperationResultsDbService _operationResultsDbService;
    private readonly ILogger _logger;

    #region ScriptReport

    public ObservableCollection<ScriptReportViewModel> ScriptReportViewModels { get; set; } = new();

    public ScriptReportViewModel SelectedScriptViewModel
    {
        get => _selectedScriptViewModel;
        set
        {
            SetProperty(ref _selectedScriptViewModel, value);
            
            OperationReportViewModels.Clear();

            foreach (var operationReportViewModel in _operationResultsDbService.GetOperationsById(value.Id))
            {
                OperationReportViewModels.Add(operationReportViewModel);
            }
        }
    }

    

    public ObservableCollection<OperationReportViewModel> OperationReportViewModels { get; set; } = new();

    public OperationReportViewModel SelectedOperationReportViewModel
    {
        get => _selectedOperationReportViewModel;
        set
        {
            SetProperty(ref _selectedOperationReportViewModel, value);

            ResultView = value.OperationType switch
            {
                OperationType.Validation => new ValidationResultView()
                {
                    DataContext = new ValidationResultViewModel(value.BaseOperationResult as ValidationOperationResult, _dialogService, _logger)
                },
                _ => new BaseResultView()
            };
        }
    }

    private DateTimeOffset _startDateOffsetScript = new(DateTime.Now.Date);
    private DateTimeOffset _endDateOffsetScript = new(DateTime.Now.Date);
    private ScriptReportViewModel _selectedScriptViewModel;
    private OperationReportViewModel _selectedOperationReportViewModel;

    public DateTimeOffset StartDateOffsetScript
    {
        get => _startDateOffsetScript;
        set => SetProperty(ref _startDateOffsetScript, value);
    }

    public DateTimeOffset EndDateOffsetScript
    {
        get => _endDateOffsetScript;
        set => SetProperty(ref _endDateOffsetScript, value);
    }

    public string VendorNumber
    {
        get => _vendorNumber;
        set => SetProperty(ref _vendorNumber, value);
    }

    public string VendorName
    {
        get => _vendorName;
        set => SetProperty(ref _vendorName, value);
    }

    public string VendorAddress
    {
        get => _vendorAddress;
        set => SetProperty(ref _vendorAddress, value);
    }

    public DelegateCommand LoadScriptReportsCommand { get; }

    private void LoadScriptReportsCommandHandler()
    {
        ScriptReportViewModels.Clear();

        if (string.IsNullOrEmpty(VendorName) && string.IsNullOrEmpty(VendorAddress) && string.IsNullOrEmpty(VendorNumber))
        {
            foreach (var scriptResultViewModel in _scriptResultsDbService.GetScriptResults(StartDateOffsetScript, EndDateOffsetScript)) 
            {
                ScriptReportViewModels.Add(scriptResultViewModel);
            }
        }
        else
        {
            var operationsId = _deviceDbService.GetOperationsIdByParameter(VendorNumber, VendorName, VendorAddress, StartDateOffsetScript, EndDateOffsetScript);
            var scriptsId = _operationResultsDbService.GetScriptsIdByOperationId(operationsId);
            
            foreach (var scriptResultViewModel in _scriptResultsDbService.GetScriptResultsByGuidList(scriptsId)) 
            {
                ScriptReportViewModels.Add(scriptResultViewModel);
            }
        }
        
        
    }
    
    public DelegateCommand ResetScriptReportsCommand { get; }

    private void ResetScriptReportsCommandHandler()
    {
        ScriptReportViewModels.Clear(); 
        OperationReportViewModels.Clear();
        StartDateOffsetScript = new DateTimeOffset(DateTime.Now.Date);
        EndDateOffsetScript = new DateTimeOffset(DateTime.Now.Date);
        VendorAddress = string.Empty;
        VendorName = string.Empty;
        VendorNumber = string.Empty;
    }
    
    private UserControl _resultView;
    private string _vendorNumber;
    private string _vendorName;
    private string _vendorAddress;

    public UserControl ResultView
    {
        get => _resultView;
        set => SetProperty(ref _resultView, value);
    }
    
    #endregion
    
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
    
    public static void Show(IDialogService dialogService, Action positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(WorkReportView), null, Report =>
        {
            switch (Report.Result)
            {
                case ButtonResult.Abort:
                    break;
                case ButtonResult.Cancel:
                    break;
                case ButtonResult.Ignore:
                    break;
                case ButtonResult.No:
                    negativeAction?.Invoke();
                    break;
                case ButtonResult.None:
                    break;
                case ButtonResult.OK:
                    positiveAction?.Invoke();
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
}