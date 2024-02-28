using System.Collections.ObjectModel;
using Avalonia.Media.Transformation;
using SPU_7.Common.Scripts;
using SPU_7.Models.Scripts.Operations.Results;

namespace SPU_7.ViewModels.WorkReportViewModels;

public class OperationReportViewModel : ViewModelBase
{
    public OperationReportViewModel()
    {
    }

    private string _message;
    private OperationResultType _operationResultType;
    private OperationType _operationType;
    private int _operationNumber;
    private string _result;
    private ObservableCollection<string> _stringResults;

    public string Message
    {
        get => _message;
        set => SetProperty(ref _message, value);
    }

    public OperationResultType OperationResultType
    {
        get => _operationResultType;
        set => SetProperty(ref _operationResultType, value);
    }

    public OperationType OperationType
    {
        get => _operationType;
        set => SetProperty(ref _operationType, value);
    }

    public int OperationNumber
    {
        get => _operationNumber;
        set => SetProperty(ref _operationNumber, value);
    }

    public string Result
    {
        get => _result;
        set => SetProperty(ref _result, value);
    }
    
    public BaseOperationResult BaseOperationResult { get; set; }
}