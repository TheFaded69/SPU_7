using Avalonia.Controls;
using Avalonia.Threading;
using Prism.Services.Dialogs;
using SPU_7.Common.Stand;
using SPU_7.Models.Scripts;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.ViewModels.ExtensionViewModels;
using SPU_7.ViewModels.MnemonicSchemeViewModels;
using SPU_7.Views;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels
{
    public class StandViewModel : ViewModelBase
    {
        public StandViewModel(IDialogService dialogService,
            INotificationService notifyService,
            ILogger logger,
            IStandSettingsService settingsService,
            IStandController standController,
            IScriptController scriptController,
            IManualOperationService manualOperationService,
            ITimerService timerService,
            IOperationActionService operationActionService)
        {
            _notificationService = notifyService;
            _settingsService = settingsService;
            _standController = standController;
            _scriptController = scriptController;
            _manualOperationService = manualOperationService;
            _timerService = timerService;
            _operationActionService = operationActionService;
            _logger = logger;
            _dialogService = dialogService;

            Dispatcher.UIThread.Invoke(() => CreateForm());
        }

        private readonly INotificationService _notificationService;
        private readonly IDialogService _dialogService;
        private readonly ILogger _logger;
        private readonly IStandSettingsService _settingsService;
        private readonly IStandController _standController;
        private readonly IScriptController _scriptController;
        private readonly IManualOperationService _manualOperationService;
        private readonly ITimerService _timerService;
        private readonly IOperationActionService _operationActionService;

        private StandType _selectedStandType;

        public StandType SelectedStandType
        {
            get => _selectedStandType;
            set => SetProperty(ref _selectedStandType, value);
        }

        private UserControl _userControlMnemonicScheme;

        public UserControl UserControlMnemonicScheme
        {
            get => _userControlMnemonicScheme;
            set => SetProperty(ref _userControlMnemonicScheme, value);
        }

        private UserControl _userControlMnemonicSchemeControl;

        public UserControl UserControlMnemonicSchemeControl
        {
            get => _userControlMnemonicSchemeControl;
            set => SetProperty(ref _userControlMnemonicSchemeControl, value);
        }

        private UserControl _userControlWorkResult;

        public UserControl UserControlWorkResult
        {
            get => _userControlWorkResult;
            set => SetProperty(ref _userControlWorkResult, value);
        }

        private UserControl _userControlExtension;

        public UserControl UserControlExtension
        {
            get => _userControlExtension;
            set => SetProperty(ref _userControlExtension, value);
        }

        private void CreateForm()
        {
            UserControlMnemonicSchemeControl = new MnemonicSchemeControlView
            {
                DataContext = new MnemonicSchemeControlViewModel(_dialogService, _notificationService, _logger, _settingsService, _standController,
                    _scriptController, _manualOperationService, _timerService, _operationActionService)
            };
            UserControlMnemonicScheme = new MnemonicSchemeView
            {
                DataContext = new MnemonicSchemeViewModel(_dialogService, _notificationService, _logger, _settingsService, _standController, _scriptController)
            };

            UserControlExtension = new ExtensionView()
            {
                DataContext = new ExtensionViewModel(_standController, _settingsService, _logger, _notificationService)
            };
        }
    }
}