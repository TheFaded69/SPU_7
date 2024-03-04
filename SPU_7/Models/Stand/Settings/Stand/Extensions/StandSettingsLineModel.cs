using System.Collections.ObjectModel;
using SPU_7.Common.Line;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsLineModel 
{
    public ObservableCollection<StandSettingsDeviceModel> DeviceViewModels { get; set; }
    public ObservableCollection<StandSettingsMasterDeviceModel> StandSettingsMasterDeviceViewModels { get; set; }
    public ObservableCollection<StandSettingsFanModel> StandSettingsFanViewModels { get; set; }
    public ObservableCollection<StandSettingsNozzleModel> StandSettingsNozzleViewModels { get; set; }
    /// <summary>
    /// Клапан прямой подачи воздуха (из СПУ-5)
    /// </summary>
    public StandSettingsValveModel DirectValveViewModel { get; set; }
    
    /// <summary>
    /// Клапан обратной подачи воздуха (из СПУ-5)
    /// </summary>
    public StandSettingsValveModel ReverseValveViewModel { get; set; }
    
    /// <summary>
    /// Номер линии
    /// </summary>
    public int LineNumber { get; set; }
    
    /// <summary>
    /// Выбранный тип поверяемых устройств линии (строка)
    /// </summary>
    public string SelectedStringDeviceLineType { get; set; }
    
    /// <summary>
    /// Выбранный тип поверяемых устройств линии (строка)
    /// </summary>
    public DeviceLineType SelectedDeviceLineType { get; set; }
    
    /// <summary>
    /// Выбранный тип линии
    /// </summary>
    public LineType SelectedLineType { get; set; }
    
    /// <summary>
    /// Доступен ли обратный ход воздуха (и соответственно клапана направления потока)
    /// </summary>
    public bool IsReverseLine { get; set; }
    
}