using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.DbServices;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;
using SPU_7.Views.ScriptViews;

namespace SPU_7.ViewModels.ScriptViewModels
{
    public class ScriptMenuViewModel : ViewModelBase, IDialogAware
    {
        public ScriptMenuViewModel(IDialogService dialogService, IScriptDbService scriptDbService, IOperationDbService operationDbService)
        {
            _dialogService = dialogService;
            _scriptDbService = scriptDbService;
            _operationDbService = operationDbService;

            Title = "Сценарии";
            
            StringStandTypes = new ObservableCollection<string>(Enum
                .GetValues<StandType>()
                .Where(ot => ot != StandType.None)
                .Select(ot => new string(ot.GetDescription())));

            CreateScriptCommand = new DelegateCommand(CreateScriptCommandHandler);
            EditScriptCommand = new DelegateCommand(EditScriptCommandHandler);
            DeleteScriptCommand = new DelegateCommand(DeleteScriptCommandHandler);
            ChooseScriptCommand = new DelegateCommand(ChooseScriptCommandHandler);
            ClearFilterCommand = new DelegateCommand(ClearFilterCommandHandler);
            CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);
            CopyScriptCommand = new DelegateCommand(CopyScriptCommandHandler);
        }
        private readonly IDialogService _dialogService;
        private readonly IScriptDbService _scriptDbService;
        private readonly IOperationDbService _operationDbService;

        public ObservableCollection<ScriptViewModel> Scripts { get; set; } = new();

        private ScriptViewModel _selectedScript;
        public ScriptViewModel SelectedScript
        {
            get => _selectedScript; set => SetProperty(ref _selectedScript, value);
        }

        #region Создать сценарий

        public DelegateCommand CreateScriptCommand { get; }
        private void CreateScriptCommandHandler()
        {
            ScriptEditorViewModel.Show(_dialogService, new ScriptViewModel(),false, AddNewScript, null);
        }

        private void AddNewScript(ScriptViewModel script)
        {
            Scripts.Add(script);

            _scriptDbService.AddScript(script);
        }

        #endregion

        #region Редактировать сценарий

        public DelegateCommand EditScriptCommand { get; }
        private void EditScriptCommandHandler()
        {
            ScriptEditorViewModel.Show(_dialogService, SelectedScript, true, UpdateScript, null);
        }

        private void UpdateScript(ScriptViewModel script)
        {
            _scriptDbService.EditScript(script);
        }

        #endregion

        #region Удалить

        public DelegateCommand DeleteScriptCommand { get; }
        private void DeleteScriptCommandHandler()
        {
            _scriptDbService.DeleteScript(SelectedScript.Id);

            Scripts.Remove(SelectedScript);
        }

        #endregion

        #region Выбрать

        public DelegateCommand ChooseScriptCommand { get; }
        private void ChooseScriptCommandHandler()
        {
            foreach (var operation in _operationDbService.GetOperations(SelectedScript.Id).Where(om => 
                         SelectedScript.Operations.All(om1 => om1.Number != om.Number)))
            {
                operation.Number = SelectedScript.Operations.Count + 1;
                SelectedScript.Operations.Add(operation);
            }
            
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK,
                new DialogParameters { { "SelectedScript", SelectedScript } }));
        }

        #endregion

        #region Copy

        public DelegateCommand CopyScriptCommand { get; }

        private int _copyCounter;
        
        private void CopyScriptCommandHandler()
        {
            _copyCounter++;
            
            var newScript = new ScriptViewModel
            {
                Id = Guid.NewGuid(),
                Description = SelectedScript.Description,
                Name = SelectedScript.Name + $" (копия {_copyCounter})",
                LineNumber = SelectedScript.LineNumber
            };
            
            _scriptDbService.AddScript(newScript);

            newScript.Operations = new ObservableCollection<OperationViewModel>(_operationDbService.GetOperations(SelectedScript.Id)
                .Select(ovm => new OperationViewModel()
                {
                    OperationStatus = ovm.OperationStatus,
                    Description = ovm.Description,
                    Name = ovm.Name,
                    OperationType = ovm.OperationType,
                    Id = Guid.NewGuid(),
                    ScriptId = newScript.Id,
                    Number = ovm.Number,
                    ConfigurationModel = ovm.ConfigurationModel
                }));

            foreach (var operationViewModel in newScript.Operations)
            {
                _operationDbService.AddOperation(operationViewModel);
            }
            
            Scripts.Add(newScript);
        }

        #endregion
        
        #region Фильтр

        public ObservableCollection<string> StringStandTypes { get; set; }
        private string _selectedStringStandType;
        public string SelectedStringStandType
        {
            get => _selectedStringStandType;
            set
            {
                SetProperty(ref _selectedStringStandType, value);
                
                if (!string.IsNullOrEmpty(SelectedStringStandType))
                    SelectedStandType = Enum
                        .GetValues<StandType>()
                        .FirstOrDefault(st => st.GetDescription() == value);

                
            }
        }
        private StandType? _selectedStandType;
        public StandType? SelectedStandType
        {
            get => _selectedStandType;
            set
            {
                SetProperty(ref _selectedStandType, value);
                
                StringDeviceTypes.Clear();

                if (SelectedStandType != null)
                    foreach (var deviceType in Enum
                                 .GetValues<DeviceType>()
                                 .Where(dt => dt.GetStandType() == SelectedStandType))
                    {
                        StringDeviceTypes.Add(deviceType.GetDescription());
                    }
                
                UpdateScripts();
            }
        }

        public ObservableCollection<string> StringDeviceTypes { get; set; } = new();
        private string _selectedStringDeviceType;
        public string SelectedStringDeviceType
        {
            get => _selectedStringDeviceType;
            set
            {
                SetProperty(ref _selectedStringDeviceType, value);
                
                if (!string.IsNullOrEmpty(SelectedStringDeviceType))
                    SelectedDeviceType = Enum
                        .GetValues<DeviceType>()
                        .FirstOrDefault(st => st.GetDescription() == value);
            }
        }
        private DeviceType? _selectedDeviceType;
        public DeviceType? SelectedDeviceType
        {
            get => _selectedDeviceType;
            set
            {
                SetProperty(ref _selectedDeviceType, value);
                UpdateScripts();
            }
        }

        private void UpdateScripts()
        {
            Scripts.Clear();
            
            foreach (var script in _scriptDbService.GetFilterScripts(SelectedStandType, SelectedDeviceType))
            {
                Scripts.Add(script);
            }
        }
        
        public DelegateCommand ClearFilterCommand { get; }

        private void ClearFilterCommandHandler()
        {
            SelectedStringDeviceType = string.Empty;
            SelectedStringStandType = string.Empty;
            SelectedDeviceType = null;
            SelectedStandType = null;
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
            foreach (var script in _scriptDbService.GetAllScripts())
            {
                Scripts.Add(script);
            }
        }

        public static void Show(IDialogService dialogService, Action<ScriptViewModel> positiveAction, Action negativeAction)
        {
            dialogService.ShowDialog(nameof(ScriptMenuView), null, dialogResult =>
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
                            positiveAction?.Invoke(dialogResult.Parameters.GetValue<ScriptViewModel>("SelectedScript"));
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
