using System.Threading.Tasks;
using Prism.Commands;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class StandExtensionViewModel : ViewModelBase
{
    public StandExtensionViewModel(IStandController standController,
        INotificationService notificationService,
        IStandSettingsService standSettingsService,
        ILogger logger)
    {
        _standController = standController;
        _notificationService = notificationService;
        _standSettingsService = standSettingsService;
        _logger = logger;

        ResetPressureSensorCommand = new DelegateCommand(ResetPressureSensorCommandHandler);
    }

    private readonly IStandController _standController;
    private readonly INotificationService _notificationService;
    private readonly IStandSettingsService _standSettingsService;
    private readonly ILogger _logger;

    public DelegateCommand ResetPressureSensorCommand { get; set; }

    public async void ResetPressureSensorCommandHandler()
    {
        _notificationService.Show("Обнуление", "Начато обнуление датчиков давления");

#if !DEBUGGUI
        foreach (var lineViewModel in _standSettingsService.StandSettingsModel.LineViewModels)
        {
            foreach (var deviceViewModel in lineViewModel.DeviceViewModels)
            {
                if (!await _standController.ResetToZeroAsync(deviceViewModel.Number))
                    _logger.Logging(new LogMessage($"Не удалось сбросить на ноль ДД №{deviceViewModel.Number}", LogLevel.Error));
            }
        }

        if (!await _standController.ResetToZeroPressureDifferenceAsync())
            _logger.Logging(new LogMessage($"Не удалось сбросить на ноль ДPД", LogLevel.Error));
#else
        await Task.Delay(2000);
#endif

        _notificationService.Show("Обнуление", "Обнуление датчиков давления успешно завершено");
    }
}