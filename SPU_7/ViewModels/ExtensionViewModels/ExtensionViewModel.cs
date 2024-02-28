using Avalonia.Controls;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Views;
using SPU_7.Views.ExtensionViews;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class ExtensionViewModel : ViewModelBase
{
    public ExtensionViewModel(IStandController standController, IStandSettingsService settingsService, ILogger logger, INotificationService notificationService)
    {
        _standController = standController;
        _settingsService = settingsService;
        _logger = logger;
        _notificationService = notificationService;

        Init();
    }
    private readonly IStandController _standController;
    private readonly IStandSettingsService _settingsService;
    private readonly ILogger _logger;
    private readonly INotificationService _notificationService;

    private UserControl _standExtensionUserControl;
    private UserControl _vacuumCreatorExtensionUserControl;
    private UserControl _userControlWorkResult;

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

    private void Init()
    {
        
        StandExtensionUserControl = new StandExtensionView()
        {
            DataContext = new StandExtensionViewModel(_standController, _notificationService, _settingsService, _logger)
        };
        VacuumCreatorExtensionUserControl = new VacuumCreatorExtensionView()
        {
            DataContext = new VacuumCreatorExtensionViewModel(_standController)
        };
        UserControlWorkResult = new WorkResultView
        {
            DataContext = new WorkResultViewModel(_logger, _standController, _settingsService)
        };
    }
}