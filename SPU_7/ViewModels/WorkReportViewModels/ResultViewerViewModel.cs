using System;
using Avalonia.Controls;
using Avalonia.Media.Transformation;
using Prism.Services.Dialogs;
using SPU_7.Common.Scripts;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Services.Logger;
using SPU_7.ViewModels.WorkReportViewModels.ResultViewModels.Validation;
using SPU_7.Views.WorkReportViews;
using SPU_7.Views.WorkReportViews.ResultView;

namespace SPU_7.ViewModels.WorkReportViewModels;

public class ResultViewerViewModel : ViewModelBase, IDialogAware
{
    private readonly IDialogService _dialogService;
    private readonly ILogger _logger;

    public ResultViewerViewModel(IDialogService dialogService, ILogger logger)
    {
        _dialogService = dialogService;
        _logger = logger;
    }

    private UserControl _resultView;
    private string _operationName;
    private string _message;

    public UserControl ResultView
    {
        get => _resultView;
        set => SetProperty(ref _resultView, value);
    }

    public string OperationName
    {
        get => _operationName;
        set => SetProperty(ref _operationName, value);
    }

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }


    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        var operationResult = parameters.GetValue<BaseOperationResult>("OperationResult");
        var operationType = parameters.GetValue<OperationType>("OperationType");

        ResultView = operationType switch
        {

            OperationType.Validation => new ValidationResultView()
            {
                DataContext = new ValidationResultViewModel(operationResult as ValidationOperationResult, _dialogService, _logger) 
            },
            _ => new BaseResultView()
        };
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, BaseOperationResult operationResult, TransformOperation.OperationType operationType ,Action positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(ResultViewerView), 
            new DialogParameters{{"OperationResult", operationResult}, {"OperationType", operationType}}, 
            result =>
        {
            switch (result.Result)
            {
                case ButtonResult.Abort:
                    break;
                case ButtonResult.Cancel:
                    negativeAction?.Invoke();
                    break;
                case ButtonResult.Ignore:
                    break;
                case ButtonResult.No:
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