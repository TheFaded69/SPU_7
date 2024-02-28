using System.IO;
using Newtonsoft.Json;
using SPU_7.Models.Stand.Settings.Stand;

namespace SPU_7.Models.Services.StandSetting;

public class StandSettingsService : IStandSettingsService
{
    public StandSettingsService()
    {
        LoadSettings();
    }

    /// <summary>
    /// Директория хранения файлов настроек
    /// </summary>
    private const string Dir = "./Settings";
    private const string FileName = "StandSettings.json";

    
    /// <summary>
    /// Объект настроек для доступа к ним в других классах
    /// </summary>
    public StandSettingsModel StandSettingsModel { get; set; }
    
    /// <summary>
    /// Загружает сохраненные на ПК настройки
    /// </summary>
    public void LoadSettings()
    {
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        var fullPath = $"{Directory.GetCurrentDirectory()}/{Dir}/{FileName}";
        StandSettingsModel = !File.Exists(fullPath) ? null : JsonConvert.DeserializeObject<StandSettingsModel>(File.ReadAllText(fullPath));
    }

    /// <summary>
    /// Сохраняет в файл в json-формате настройки на ПК
    /// </summary>
    /// <param name="standSettingsModel"></param>
    public void SaveSettings(StandSettingsModel standSettingsModel)
    {
        if (!Directory.Exists(Dir))
            Directory.CreateDirectory(Dir);
        var fullPath = $"{Directory.GetCurrentDirectory()}/{Dir}/{FileName}";
        var json = JsonConvert.SerializeObject(standSettingsModel, Formatting.Indented);
        File.WriteAllText(fullPath, json);
    }
}