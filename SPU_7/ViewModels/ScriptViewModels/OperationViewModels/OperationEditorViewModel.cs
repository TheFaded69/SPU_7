using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Avalonia.Controls;
using Avalonia.Media.Transformation;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;
using SPU_7.Common.Scripts;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;
using SPU_7.Views.ScriptViews.OperationViews;
using SPU_7.Views.ScriptViews.OperationViews.OperationConfigurationViews;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels;

public class OperationEditorViewModel : ViewModelBase, IDialogAware
{
    public OperationEditorViewModel(IDialogService dialogService, IStandSettingsService standSettingsService, IStandController standController, IMapper mapper)
    {
        _dialogService = dialogService;
        _standSettingsService = standSettingsService;
        _standController = standController;
        _mapper = mapper;

        Title = "Редактор операции";

        Operations = new ObservableCollection<string>(Enum
            .GetValues<OperationType>()
            .Where(ot => ot != OperationType.None)
            .Select(ot => ot.GetDescription()));

        SaveOperationCommand = new DelegateCommand(SaveOperationCommandHandler);
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
    }

    private readonly IDialogService _dialogService;
    private readonly IStandSettingsService _standSettingsService;
    private readonly IStandController _standController;
    private readonly IMapper _mapper;

    private UserControl _operationConfigurationEditor;
    private string _name;
    private string _description;
    private string _selectedStringOperation;
    private OperationType _selectedOperationType;
    private OperationViewModel _operation;
    private DeviceType _deviceType;

    private int _lineNumber;
    
    public ObservableCollection<string> Operations { get; }
    public string SelectedStringOperation
    {
        get => _selectedStringOperation;
        set
        {
            SetProperty(ref _selectedStringOperation, value);
            
            SelectedOperationType = Enum
                .GetValues<OperationType>()
                .FirstOrDefault(ot => ot.GetDescription() == value);
        }
    }
    public OperationType SelectedOperationType
    {
        get => _selectedOperationType;
        set
        {
            SetProperty(ref _selectedOperationType, value);
            OperationConfigurationEditor = SelectedOperationType switch
            {
                OperationType.Validation => new ValidationOperationConfigurationView
                {
                    DataContext = new ValidationOperationConfigurationViewModel(_operation.ConfigurationModel, _standController, _dialogService, _mapper, _standSettingsService, _lineNumber)
                },
                OperationType.CheckTightness => new CheckTightnessOperationConfigurationView()
                {
                    DataContext = new CheckTightnessOperationConfigurationViewModel(_operation.ConfigurationModel, _standSettingsService)
                },
                OperationType.SetStandWorkMode => new SetStandWorkModeOperationConfigurationView()
                {
                    DataContext = new SetStandWorkModeOperationConfigurationViewModel(_operation.ConfigurationModel)
                },
                _ => throw new ArgumentOutOfRangeException(nameof(SelectedOperationType))
            };
        }
    }
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }
    public UserControl OperationConfigurationEditor
    {
        get => _operationConfigurationEditor;
        set => SetProperty(ref _operationConfigurationEditor, value);
    }

    #region Сохранить операцию

    public DelegateCommand SaveOperationCommand { get; }
    private void SaveOperationCommandHandler()
    {
        _operation.Name = string.IsNullOrEmpty(Name) ? SelectedOperationType.GetDescription() : Name;
        _operation.Description = string.IsNullOrEmpty(Description) ? "-" : Description;
        _operation.ConfigurationModel = ((IOperationConfiguration)OperationConfigurationEditor.DataContext)?.CreateConfiguration(_deviceType);
        _operation.OperationType = SelectedOperationType;
        _operation.Id = _operation.Id.Equals(Guid.Empty) ? Guid.NewGuid() : _operation.Id;
        
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }

    #endregion
    
    #region Dialog
    
    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
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
        _operation = parameters.GetValue<OperationViewModel>("Operation");
        _deviceType = parameters.GetValue<DeviceType>("DeviceType");
        _lineNumber = parameters.GetValue<int>("LineNumber");
        
        if (!parameters.GetValue<bool>("IsEditMode"))
        {
            OperationConfigurationEditor = new BaseOperationConfigurationView();
            return;
        }
        
        Name = _operation.Name;
        Description = _operation.Description;
        SelectedStringOperation = Operations
            .FirstOrDefault(op => op == _operation.OperationType.GetDescription());
    }

    public event Action<IDialogResult> RequestClose;

    public static void Show(IDialogService dialogService, OperationViewModel selectedOperation, bool isEditMode, DeviceType deviceType, int lineNumber, Action<OperationViewModel> positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(OperationEditorView), 
            new DialogParameters { {"Operation", selectedOperation} , {"IsEditMode", isEditMode}, {"DeviceType", deviceType}, {"LineNumber", lineNumber}},dialogResult =>
        {
            switch (dialogResult.Result)
            {
                case ButtonResult.Abort:
                case ButtonResult.Ignore:
                case ButtonResult.Cancel:
                case ButtonResult.None:
                case ButtonResult.Yes:
                case ButtonResult.Retry:
                    break;
                case ButtonResult.OK:
                    positiveAction?.Invoke(selectedOperation);
                    break;
                case ButtonResult.No:
                    negativeAction?.Invoke();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }
    
    #endregion

}