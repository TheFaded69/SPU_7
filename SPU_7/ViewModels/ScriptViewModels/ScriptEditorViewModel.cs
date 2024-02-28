using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.StandSetting;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;
using SPU_7.Views.ScriptViews;

namespace SPU_7.ViewModels.ScriptViewModels
{
    public class ScriptEditorViewModel : ViewModelBase, IDialogAware
    {
        public ScriptEditorViewModel(IDialogService dialogService, IOperationDbService operationDbService, IStandSettingsService settingsService)
        {
            _dialogService = dialogService;
            _operationDbService = operationDbService;
            _settingsService = settingsService;

            Title = "Редактор сценария";

            LineNumbers = new ObservableCollection<int>();
            foreach (var unused in settingsService.StandSettingsModel.LineViewModels)     
            {
                LineNumbers.Add(LineNumbers.Count + 1);
            }
            
            StringDeviceTypes = new ObservableCollection<string>();
            StringStandTypes = new ObservableCollection<string>(Enum
                .GetValues<StandType>()
                .Where(ot => ot != StandType.None)
                .Select(ot => new string(ot.GetDescription())));

            CreateOperationCommand = new DelegateCommand(CreateOperationCommandHandler);
            EditOperationCommand = new DelegateCommand(EditOperationCommandHandler);
            DeleteOperationCommand = new DelegateCommand(DeleteOperationCommandHandler);
            SaveScriptCommand = new DelegateCommand(SaveScriptCommandHandler);
            CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
            MoveUpCommand = new DelegateCommand(MoveUpCommandHandler);
            MoveDownCommand = new DelegateCommand(MoveDownCommandHandler);
        }

        private readonly IDialogService _dialogService;
        private readonly IOperationDbService _operationDbService;
        private readonly IStandSettingsService _settingsService;

        private ScriptViewModel Script { get; set; }

        private string _name;
        public string Name
        {
            get => _name; set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description; set => SetProperty(ref _description, value);
        }

        public ObservableCollection<string> StringDeviceTypes { get; set; }
        private string _selectedStringDeviceType;
        public string SelectedStringDeviceType
        {
            get => _selectedStringDeviceType;
            set
            {
                SetProperty(ref _selectedStringDeviceType, value);
                SelectedDeviceType = Enum
                    .GetValues<DeviceType>()
                    .FirstOrDefault(st => st.GetDescription() == value);
            }
        }
        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public int Height
        {
            get => _height;
            set => SetProperty(ref _height, value);
        }

        private DeviceType SelectedDeviceType { get; set; }
        
        public ObservableCollection<int> LineNumbers { get; set; }
        
        private int _selectedLineNumber;
        public int SelectedLineNumber
        {
            get => _selectedLineNumber;
            set => SetProperty(ref _selectedLineNumber, value);
        }

        public ObservableCollection<string> StringStandTypes { get; set; }
        private string _selectedStringStandType;
        public string SelectedStringStandType
        {
            get => _selectedStringStandType;
            set
            {
                SetProperty(ref _selectedStringStandType, value);
                SelectedTargetStandType = Enum
                    .GetValues<StandType>()
                    .FirstOrDefault(st => st.GetDescription() == value);

                
            }
        }
        private StandType _selectedTargetStandType;
        public StandType SelectedTargetStandType
        {
            get => _selectedTargetStandType;
            set
            {
                SetProperty(ref _selectedTargetStandType, value);
                
                StringDeviceTypes.Clear();

                foreach (var deviceType in Enum
                             .GetValues<DeviceType>()
                             .Where(dt => dt.GetStandType() == SelectedTargetStandType))
                {
                    StringDeviceTypes.Add(deviceType.GetDescription());
                }
            }
        }

        public ObservableCollection<OperationViewModel> Operations { get; set; } = new();

        private OperationViewModel _selectedOperation;
        private int _height;

        public OperationViewModel SelectedOperation
        {
            get => _selectedOperation; set => SetProperty(ref _selectedOperation, value);
        }

        #region Создать операцию

        public DelegateCommand CreateOperationCommand { get; }
        private void CreateOperationCommandHandler()
        {
            OperationEditorViewModel.Show(_dialogService, new OperationViewModel {ScriptId = Script.Id}, false, SelectedDeviceType, SelectedLineNumber,AddOperationToScript, null);
        }

        private void AddOperationToScript(OperationViewModel operation)
        {
            operation.Number = Operations.Count + 1;
            _operationDbService.AddOperation(operation);
            
            Operations.Add(operation);
        }

        #endregion

        #region Редактировать операцию

        public DelegateCommand EditOperationCommand { get; }
        private void EditOperationCommandHandler()
        {
            OperationEditorViewModel.Show(_dialogService, SelectedOperation,true, SelectedDeviceType, SelectedLineNumber,UpdateScript, null);
        }

        private void UpdateScript(OperationViewModel operation)
        {
            _operationDbService.UpdateOperation(operation);
        }

        #endregion

        #region Удалить операцию

        public DelegateCommand DeleteOperationCommand { get; }
        private void DeleteOperationCommandHandler()
        {
            _operationDbService.DeleteOperation(SelectedOperation.Id);
            
            Operations.Remove(SelectedOperation);
        }

        #endregion

        #region Перемещение операций в сценарии

        public DelegateCommand MoveUpCommand { get; set; }
        private void MoveUpCommandHandler()
        {
            if (SelectedOperation.Number == 1) return;

            var previousOperation = Operations[SelectedOperation.Number - 2];
            previousOperation.Number += 1;
            SelectedOperation.Number -= 1;
            
            Operations[SelectedOperation.Number - 1] = SelectedOperation;
            Operations[SelectedOperation.Number] = previousOperation;
            
            
        }
        
        public DelegateCommand MoveDownCommand { get; set; }
        private void MoveDownCommandHandler()
        {
            if (SelectedOperation.Number == Operations.Count) return;
            
            var nextOperation = Operations[SelectedOperation.Number];
            nextOperation.Number -= 1;
            SelectedOperation.Number += 1;
            
            Operations[SelectedOperation.Number - 1] = SelectedOperation;
            Operations[SelectedOperation.Number - 2] = nextOperation;
        }

        #endregion
        
        #region Сохранить сценарий

        public DelegateCommand SaveScriptCommand { get; }
        private void SaveScriptCommandHandler()
        {
            Script.Name = string.IsNullOrEmpty(Name) ? "-" : Name;
            Script.Description = string.IsNullOrEmpty( Description) ? "-" : Description;
            
            foreach (var operationViewModel in Operations)
            {
                _operationDbService.UpdateOperation(operationViewModel);
            }
            
            Script.DeviceType = SelectedDeviceType;
            Script.TargetStandType = SelectedTargetStandType;
            Script.Operations = Operations;
            Script.Id = Script.Id.Equals(Guid.Empty)? Guid.NewGuid() : Script.Id;
            Script.LineNumber = SelectedLineNumber;

            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        #endregion

        #region Dialog

        public DelegateCommand CloseWindowCommand { get; }

        private void CloseWindowCommandHandler()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }
        
        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {

        }

        
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Script = parameters.GetValue<ScriptViewModel>("Script");
            IsEditMode = parameters.GetValue<bool>("IsEditMode");
            Height = IsEditMode ? 700 : 200;
            
            if (!parameters.GetValue<bool>("IsEditMode"))
            {
                Script.Id = Guid.NewGuid();
                return;
            }

            foreach (var operation in _operationDbService.GetOperations(Script.Id))
            {
                Operations.Add(operation);
            }
            
            Name = Script.Name;
            Description = Script.Description;
            SelectedTargetStandType = Script.TargetStandType;
            SelectedStringStandType =
                StringStandTypes.FirstOrDefault(sst => sst == SelectedTargetStandType.GetDescription());
            
            SelectedDeviceType = Script.DeviceType;
            SelectedStringDeviceType =
                StringDeviceTypes.FirstOrDefault(sdt => sdt == SelectedDeviceType.GetDescription());

            SelectedLineNumber = Script.LineNumber;
        }

        public static void Show(IDialogService dialogService, ScriptViewModel script, bool isEditMode, Action<ScriptViewModel> positiveAction, Action negativeAction)
        {
            dialogService.ShowDialog(nameof(ScriptEditorView),
                new DialogParameters { { "Script", script } , {"IsEditMode", isEditMode}},
                dialogResult =>
            {
                switch (dialogResult.Result)
                {
                    case ButtonResult.Abort:
                    case ButtonResult.Ignore:
                    case ButtonResult.Cancel:
                    case ButtonResult.None:
                    case ButtonResult.Retry:
                    case ButtonResult.Yes:
                        break;
                    case ButtonResult.OK:
                        positiveAction?.Invoke(script);
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
}
