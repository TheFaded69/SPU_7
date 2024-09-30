using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Scripts;
using SPU_7.Common.Settings;
using SPU_7.Common.Stand;
using SPU_7.Models.Scripts;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Scripts.Operations.Results.Extensions;
using SPU_7.Models.Services.Autorization;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.ViewModels.Settings;
using SPU_7.Views;

namespace SPU_7.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IDisposable
    {
        public MainWindowViewModel(IDialogService dialogService,
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            ILogger logger,
            IStandSettingsService standSettingsService,
            IStandController standController,
            IScriptController scriptController,
            IManualOperationService manualOperationService,
            ITimerService timerService,
            IOperationActionService operationActionService,
            IScriptResultsDbService scriptResultsDbService)
        {
            _dialogService = dialogService;
            _notificationService = notificationService;
            _authorizationService = authorizationService;
            _logger = logger;
            _standSettingsService = standSettingsService;
            _standController = standController;
            _scriptController = scriptController;
            _manualOperationService = manualOperationService;
            _timerService = timerService;
            _operationActionService = operationActionService;
            _scriptResultsDbService = scriptResultsDbService;

            CloseWindowCommand = new DelegateCommand<Window>(CloseWindowCommandHandler);
            HideWindowCommand = new DelegateCommand<Window>(HideWindowCommandHandler);
            FullScreenWindowCommand = new DelegateCommand<Window>(FullScreenWindowCommandHandler);
            UnFullScreenWindowCommand = new DelegateCommand<Window>(UnFullScreenCommandHandler);
            OpenHelpCommand = new DelegateCommand(OpenHelpCommandHandler);
            OpenReactionCommand = new DelegateCommand(OpenReactionCommandHandler);
            OpenStandSettingsCommand = new DelegateCommand(OpenStandSettingsCommandHandler);
            OpenAuthorizationCommand = new DelegateCommand(OpenAuthorizationCommandHandler);
            OpenAddUserCommand = new DelegateCommand(OpenAddUserCommandHandler);
            ExitUserCommand = new DelegateCommand(ExitUserCommandHandler);

            Init();
        }

        private readonly IDialogService _dialogService;
        private readonly INotificationService _notificationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger _logger;
        private readonly IStandSettingsService _standSettingsService;
        private readonly IStandController _standController;
        private readonly IScriptController _scriptController;
        private readonly IManualOperationService _manualOperationService;
        private readonly ITimerService _timerService;
        private readonly IOperationActionService _operationActionService;
        private readonly IScriptResultsDbService _scriptResultsDbService;

        #region Боковое меню

        private UserControl _standView;

        public UserControl StandView
        {
            get => _standView;
            set => SetProperty(ref _standView, value);
        }

        private async void CreateWorkSpace()
        {
            try
            {
                _standController?.Dispose();

                if (IsAuthorize) IsCommonUser = _authorizationService.GetUser().UserType == UserType.Common;

                if (_standSettingsService.StandSettingsModel == null)
                {
                    StandView = new EmptyView
                    {
                        DataContext = new EmptyViewModel("Необходима настройка стенда")
                    };
                    return;
                }



                try
                {
#if DEBUGGUI
                //_standController.TestInitialization();
#else
                    _standController.Initialization();
#endif
                }
                catch (Exception e)
                {
                    _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
                }


                StandView = new StandView
                {
                    DataContext = new StandViewModel(_dialogService, _notificationService, _logger,
                        _standSettingsService, _standController, _scriptController, _manualOperationService,
                        _timerService,
                        _operationActionService)
                };

                ///костыль - ждем пока прогрузится интерфейс
                await Task.Delay(10000);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Fatal));
                StandView = new EmptyView
                {
                    DataContext = new EmptyViewModel($"Необходима настройка стенда, ошибка - {e.Message}")
                };
            }
        }

        #endregion

        #region Настройки

        public DelegateCommand OpenStandSettingsCommand { get; }

        private void OpenStandSettingsCommandHandler()
        {
            if (!_authorizationService.IsAuthorize())
            {
                MessageViewModel.Show(_dialogService, "Необходимо авторизоваться!", null, null);
                return;
            }

            StandSettingsViewModel.Show(_dialogService, UpdateView, null);
        }

        private void UpdateView()
        {
            if (IsAuthorize) CreateWorkSpace();
        }

        #endregion

        #region Справка

        public DelegateCommand OpenHelpCommand { get; }

        private void OpenHelpCommandHandler()
        {
            _dialogService.ShowDialog(nameof(AboutView), null, helpResult =>
            {
                switch (helpResult.Result)
                {
                    case ButtonResult.Abort:
                    case ButtonResult.Cancel:
                    case ButtonResult.Ignore:
                    case ButtonResult.No:
                    case ButtonResult.None:
                    case ButtonResult.OK:
                    case ButtonResult.Retry:
                    case ButtonResult.Yes:
                        break;
                }
            });
        }

        #endregion

        #region Авторизация

        private string _employee;

        public string Employee
        {
            get => string.IsNullOrEmpty(_employee) ? "Требуется авторизация" : _employee;
            set => SetProperty(ref _employee, value);
        }

        public DelegateCommand OpenAuthorizationCommand { get; }

        private void OpenAuthorizationCommandHandler()
        {
            _dialogService.ShowDialog(nameof(AuthorizationView), null, authorizationResult =>
            {
                switch (authorizationResult.Result)
                {
                    case ButtonResult.Abort:
                    case ButtonResult.Cancel:
                    case ButtonResult.Ignore:
                    case ButtonResult.No:
                    case ButtonResult.None:
                    case ButtonResult.Retry:
                    case ButtonResult.Yes:
                        break;
                    case ButtonResult.OK:
                        Employee = _authorizationService.GetUser().Employee;
                        IsAuthorize = _authorizationService.IsAuthorize();
                        break;
                }
            });
        }

        private bool _isAuthorize;

        public bool IsAuthorize
        {
            get => _isAuthorize;
            set
            {
                SetProperty(ref _isAuthorize, value);

                if (IsAuthorize) CreateWorkSpace();
            }
        }

        public DelegateCommand ExitUserCommand { get; }

        private void ExitUserCommandHandler()
        {
            ConfirmViewModel.Show(_dialogService, "Выйти из учетной записи?", ExitUser, null);
        }

        private void ExitUser()
        {
            _logger.Logging(new LogMessage(
                $"{Employee} под псевдонимом {_authorizationService.GetUser().UserName} вышел из системы",
                LogLevel.Info));

            Employee = string.Empty;
            IsAuthorize = false;
            IsCommonUser = true;
            _authorizationService.ExitUser();

            StandView = new EmptyView
            {
                DataContext = new EmptyViewModel("Необходимо авторизоваться")
            };
        }

        #endregion

        #region Добавление пользователя

        private bool _isCommonUser = true;

        public bool IsCommonUser
        {
            get => _isCommonUser;
            set => SetProperty(ref _isCommonUser, value);
        }

        public DelegateCommand OpenAddUserCommand { get; }

        private void OpenAddUserCommandHandler()
        {
            _dialogService.ShowDialog(nameof(AddUserView), null, authorizationResult =>
            {
                switch (authorizationResult.Result)
                {
                    case ButtonResult.Abort:
                    case ButtonResult.Cancel:
                    case ButtonResult.Ignore:
                    case ButtonResult.No:
                    case ButtonResult.None:
                    case ButtonResult.Retry:
                    case ButtonResult.Yes:
                        break;
                    case ButtonResult.OK:
                        break;
                }
            });
        }

        #endregion

        #region Обратная связь

        public DelegateCommand OpenReactionCommand { get; }

        private void OpenReactionCommandHandler()
        {
            MessageViewModel.Show(_dialogService, "Находится на стадии разработки :)", null, null);
        }

        #endregion

        #region Освобождение ресурсов и логика окна

        private void Init()
        {
            if (!_authorizationService.AutoAuthorize())
            {
                StandView = new EmptyView
                {
                    DataContext = new EmptyViewModel("Необходимо авторизоваться")
                };
                return;
            }

            Employee = _authorizationService.GetUser().Employee;
            IsAuthorize = _authorizationService.IsAuthorize();
        }

        public DelegateCommand<Window> CloseWindowCommand { get; }

        public void CloseWindowCommandHandler(Window window)
        {
            /*var buff = System.IO.File.ReadAllBytes(@"C:\testPicture.jpeg");
            using var ms = new MemoryStream(buff);
            var bmp = new Bitmap(ms);
            //PicturePreviewViewModel.Show(_dialogService, bmp, null, null);*/

            /*_scriptResultsDbService.AddScriptResult(new ScriptResult()
            {
                Description = "Test description",
                Name = "Test name",
                OperationResults = new List<OperationResult>()
                {
                    new OperationResult(OperationResultType.Success, "Test1", new BaseOperationResult()
                    {
                        OperationNumber = 1,
                    }),
                    new OperationResult(OperationResultType.Success, "Test2", new BaseOperationResult()
                    {
                        OperationNumber = 2,
                    }),
                    new OperationResult(OperationResultType.Success, "TestValidation", new ValidationOperationResult()
                    {
                        OperationNumber = 3,
                        Message = "TestTest",
                        ValidationType = ValidationType.ValidationByVolume,
                        MaximumFlow = 100,
                        MinimumFlow = 0.04,
                        NominalFlow = 10,
                        ValidationPointResults = new List<ValidationPointResult>()
                        {
                            new ValidationPointResult()
                            {
                                ValidationMeasureResults = new List<ValidationMeasureResult>()
                                {
                                    new ValidationMeasureResult()
                                    {
                                        ValidationDeviceResults = new List<ValidationDeviceResult>()
                                        {
                                            new ValidationDeviceResult
                                            {
                                                PointNumber = 1,
                                                MeasureNumber = 1,
                                                ValidationVolumeTime = 0,
                                                TargetVolume = 0,
                                                TargetVolumeDifference = 0,
                                                VolumeDifference = 0,
                                                StartVolumeValue = 0,
                                                EndVolumeValue = 57,
                                                TargetFlow = 10,
                                                CalculateFlow = 10,
                                                ValidationFlowTime = 300,
                                                FlowDifference = 1,
                                                VendorNumber = "1234567890",
                                                OwnerName = "David Bidenko",
                                                DeviceInfo = "Тест СГ",
                                                ProtocolNumber = 1,
                                                PressureDifference = 0.31f,
                                            },
                                            new ValidationDeviceResult
                                            {
                                                PointNumber = 1,
                                                MeasureNumber = 1,
                                                ValidationVolumeTime = 0,
                                                TargetVolume = 0,
                                                TargetVolumeDifference = 0,
                                                VolumeDifference = 0,
                                                StartVolumeValue = 0,
                                                EndVolumeValue = 57,
                                                TargetFlow = 10,
                                                CalculateFlow = 10,
                                                ValidationFlowTime = 300,
                                                FlowDifference = 1,
                                                VendorNumber = "0987654321",
                                                OwnerName = "David Bidenko",
                                                DeviceInfo = "Тест СГ 2",
                                                ProtocolNumber = 2,
                                                PressureDifference = 0.31f,
                                            },
                                        }
                                    },
                                    new ValidationMeasureResult()
                                    {
                                        ValidationDeviceResults = new List<ValidationDeviceResult>()
                                        {
                                            new ValidationDeviceResult
                                            {
                                                PointNumber = 1,
                                                MeasureNumber = 2,
                                                ValidationVolumeTime = 0,
                                                TargetVolume = 0,
                                                TargetVolumeDifference = 0,
                                                VolumeDifference = 0,
                                                StartVolumeValue = 0,
                                                EndVolumeValue = 57,
                                                TargetFlow = 10,
                                                CalculateFlow = 10,
                                                ValidationFlowTime = 300,
                                                FlowDifference = 1,
                                                VendorNumber = "1234567890",
                                                OwnerName = "David Bidenko",
                                                DeviceInfo = "Тест СГ",
                                                ProtocolNumber = 1,
                                                PressureDifference = 0.31f,
                                            },
                                            new ValidationDeviceResult
                                            {
                                                PointNumber = 1,
                                                MeasureNumber = 2,
                                                ValidationVolumeTime = 0,
                                                TargetVolume = 0,
                                                TargetVolumeDifference = 0,
                                                VolumeDifference = 0,
                                                StartVolumeValue = 0,
                                                EndVolumeValue = 57,
                                                TargetFlow = 10,
                                                CalculateFlow = 10,
                                                ValidationFlowTime = 300,
                                                FlowDifference = 1,
                                                VendorNumber = "0987654321",
                                                OwnerName = "David Bidenko",
                                                DeviceInfo = "Тест СГ 2",
                                                ProtocolNumber = 2,
                                                PressureDifference = 0.31f,
                                            },
                                        }
                                    }
                                }
                            }
                        },

                    })
                    {
                        Device = new List<DeviceInformation>()
                        {
                            new DeviceInformation
                            {
                                VendorNumber = "123456789",
                                DeviceName = "Test SG",
                                VendorName = "Test vendorName",
                                VendorAddress = "Test vendorAddress",
                                ProtocolNumber = 1
                            },
                            new DeviceInformation
                            {
                                VendorNumber = "9876543210",
                                DeviceName = "Test SG 2",
                                VendorName = "Test vendorName",
                                VendorAddress = "Test vendorAddress",
                                ProtocolNumber = 1
                            }
                        },
                        OperationType = OperationType.Validation,
                        PictureResults = new List<PictureResultModel>()
                        {
                            new PictureResultModel
                            {
                                DeviceNumber = 1,
                                MeasureNumber = 1,
                                PointNumber = 1,
                                BitMapData = null
                            },
                            new PictureResultModel
                            {
                                DeviceNumber = 1,
                                MeasureNumber = 2,
                                PointNumber = 1,
                                BitMapData = null
                            },
                            new PictureResultModel
                            {
                                DeviceNumber = 2,
                                MeasureNumber = 1,
                                PointNumber = 1,
                                BitMapData = buff
                            }
                        }
                    },
                },
            });*/

            Dispose();

            window.Close();
        }


        private bool _isFullScreen = true;

        public bool IsFullScreen
        {
            get => _isFullScreen;
            set => SetProperty(ref _isFullScreen, value);
        }

        public DelegateCommand<Window> HideWindowCommand { get; }

        public void HideWindowCommandHandler(Window window)
        {
            window.WindowState = WindowState.Minimized;
            IsFullScreen = false;
        }

        public DelegateCommand<Window> FullScreenWindowCommand { get; }

        public void FullScreenWindowCommandHandler(Window window)
        {
            window.WindowState = WindowState.FullScreen;
            IsFullScreen = true;
        }

        public DelegateCommand<Window> UnFullScreenWindowCommand { get; }

        public void UnFullScreenCommandHandler(Window window)
        {
            window.WindowState = WindowState.Normal;

            var size = window.Screens.Primary.Bounds.Size;
            var xSize = size.Width - 300;
            var ySize = size.Height - 300;

            window.Width = xSize;
            window.Height = ySize;
            window.Position = new PixelPoint(150, 150);

            IsFullScreen = false;
        }

        public void Dispose()
        {
            if (IsAuthorize) ExitUser();
            _standController.Dispose();
        }

        #endregion
    }
}