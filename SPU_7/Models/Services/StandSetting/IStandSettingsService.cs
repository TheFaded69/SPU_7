using SPU_7.Models.Stand.Settings.Stand;

namespace SPU_7.Models.Services.StandSetting;

public interface IStandSettingsService
{
    StandSettingsModel StandSettingsModel { get; set; }
    void LoadSettings();
    void SaveSettings(StandSettingsModel standSettingsModel);
}