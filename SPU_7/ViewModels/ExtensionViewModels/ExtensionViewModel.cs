using System;
using Avalonia.Controls;
using Prism.Services.Dialogs;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Views;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class ExtensionViewModel : ViewModelBase
{
    public ExtensionViewModel(IStandController standController,
        IStandSettingsService settingsService,
        ILogger logger,
        INotificationService notificationService,
        IDialogService dialogService)
    {
        _standController = standController;
        _settingsService = settingsService;
        _logger = logger;
        _notificationService = notificationService;
        _dialogService = dialogService;

        Init();
    }

    private readonly IStandController _standController;
    private readonly IStandSettingsService _settingsService;
    private readonly ILogger _logger;
    private readonly INotificationService _notificationService;
    private readonly IDialogService _dialogService;

    private UserControl _standExtensionUserControl;
    private UserControl _vacuumCreatorExtensionUserControl;
    private UserControl _userControlWorkResult;
    private UserControl _pulseMeterExtensionUserControl;
    private UserControl _pulseCountMeterModuleExtensionUserControl;
    private UserControl _pulseMeterModuleExtensionUserControl;
    private UserControl _checkTightnessExtensionUserControl;
    private UserControl _checkAverageQuadraticDifferenceUserControl;


    public UserControl UserControlWorkResult
    {
        get => _userControlWorkResult;
        set => SetProperty(ref _userControlWorkResult, value);
    }

    public UserControl StandExtensionUserControl
    {
        get => _standExtensionUserControl;
        set => SetProperty(ref _standExtensionUserControl, value);
    }

    public UserControl VacuumCreatorExtensionUserControl
    {
        get => _vacuumCreatorExtensionUserControl;
        set => SetProperty(ref _vacuumCreatorExtensionUserControl, value);
    }

    public UserControl PulseMeterExtensionUserControl
    {
        get => _pulseMeterExtensionUserControl;
        set => SetProperty(ref _pulseMeterExtensionUserControl, value);
    }

    public UserControl PulseCountMeterModuleExtensionUserControl
    {
        get => _pulseCountMeterModuleExtensionUserControl;
        set => SetProperty(ref _pulseCountMeterModuleExtensionUserControl, value);
    }
    
    public UserControl PulseMeterModuleExtensionUserControl
    {
        get => _pulseMeterModuleExtensionUserControl;
        set => SetProperty(ref _pulseMeterModuleExtensionUserControl, value);
    }

    public UserControl CheckTightnessExtensionUserControl
    {
        get => _checkTightnessExtensionUserControl;
        set => SetProperty(ref _checkTightnessExtensionUserControl, value);
    }

    public UserControl CheckAverageQuadraticDifferenceUserControl
    {
        get => _checkAverageQuadraticDifferenceUserControl;
        set => SetProperty(ref _checkAverageQuadraticDifferenceUserControl, value);
    }

    private void Init()
    {
        try
        {
            StandExtensionUserControl = new StandExtensionView()
            {
                DataContext =
                    new StandExtensionViewModel(_standController, _notificationService, _settingsService, _logger)
            };

            UserControlWorkResult = new WorkResultView
            {
                DataContext = new WorkResultViewModel(_logger, _standController, _settingsService)
            };

            if (_settingsService.StandSettingsModel.StandSettingsExtensionViewModel.IsCheckTightnessEnable)
            {
                CheckTightnessExtensionUserControl = new CheckTightnessExtensionView()
                {
                    DataContext = new CheckTightnessExtensionViewModel(_dialogService)
                };
            }

            if (_settingsService.StandSettingsModel.StandSettingsExtensionViewModel.IsVacuumCreatorEnable)
            {
                VacuumCreatorExtensionUserControl = new VacuumCreatorExtensionView()
                {
                    DataContext = new VacuumCreatorExtensionViewModel(_standController)
                };
            }

            if (_settingsService.StandSettingsModel.StandSettingsExtensionViewModel
                .IsCheckAverageQuadraticDifferenceEnable)
            {
                CheckAverageQuadraticDifferenceUserControl = new CheckAverageQuadraticDifferenceExtensionView()
                {
                    DataContext = new CheckAverageQuadraticDifferenceExtensionViewModel(_dialogService)
                };
            }
            if (_settingsService.StandSettingsModel.StandSettingsExtensionViewModel.IsCheckPulseCountMeterModuleEnable)
            {
                PulseMeterModuleExtensionUserControl = new PulseMeterModuleExtensionView()
                {
                    DataContext = new PulseMeterModuleExtensionViewModel(_dialogService)
                };
            }
            if (_settingsService.StandSettingsModel.StandSettingsExtensionViewModel.IsPulseCountMeterModuleEnable)
            {
                PulseCountMeterModuleExtensionUserControl = new PulseCountMeterModuleExtensionView()
                {
                    DataContext = new PulseCountMeterModuleExtensionViewModel(_dialogService)
                };
            }

            if (_settingsService.StandSettingsModel.StandSettingsExtensionViewModel
                .IsPulseMeterCoefficientSettingsEnable)
            {
                PulseMeterExtensionUserControl = new PulseMeterExtensionView()
                {
                    DataContext = new PulseMeterExtensionViewModel(_dialogService)
                };
            }

            
        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Fatal));
        }
    }
}