using System.Collections.ObjectModel;
using SPU_7.Common.Line;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsLineModel
{
    /// <summary>
    /// Настройки поверяемых устройств
    /// </summary>
    public ObservableCollection<StandSettingsDeviceModel> DeviceViewModels { get; set; }
    
    /// <summary>
    /// Настройки мастер устройств
    /// </summary>
    public ObservableCollection<StandSettingsMasterDeviceModel> MasterDeviceViewModels { get; set; }
    
    /// <summary>
    /// Настроки вентиляторов (обычно один, но мало ли)
    /// </summary>
    public ObservableCollection<StandSettingsFanModel> FanViewModels { get; set; }
    
    /// <summary>
    /// Настройки сопел
    /// </summary>
    public ObservableCollection<StandSettingsNozzleModel> NozzleViewModels { get; set; }
    
    /// <summary>
    /// Настройки вакуумных насосов (обычно один, но мало ли)
    /// </summary>
    public ObservableCollection<StandSettingsVacuumValveModel> VacuumValveViewModels { get; set; }

    /// <summary>
    /// Есть ли общий клапан до поверочного блока со следующей линией
    /// </summary>
    public bool IsStartCommonValve { get; set; }
    
    /// <summary>
    /// Есть ли общий клапан после поверочного блока со следующей линией
    /// </summary>
    public bool IsEndCommonValve { get; set; }
    
    /// <summary>
    /// Есть ли клапан перед мастер устройствами (поверочный блок)
    /// </summary>
    public bool IsStartValveMasterDevice { get; set; }

    /// <summary>
    /// Есть ли клапан после мастер устройств (поверочный блок)
    /// </summary>
    public bool IsEndValveMasterDevice { get; set; }

    /// <summary>
    /// Настройки клапана перед мастер устройствами
    /// </summary>
    public StandSettingsValveModel StartValveMasterDeviceViewModel { get; set; }
    
    /// <summary>
    /// Настройки клапана после мастер устройств
    /// </summary>
    public StandSettingsValveModel EndValveMasterDeviceViewModel { get; set; }

    /// <summary>
    /// Настройки общего клапана до мастер устройств
    /// </summary>
    public StandSettingsValveModel StartCommonValveViewModel { get; set; }
    
    /// <summary>
    /// Настройки общего клапана после мастер устройств
    /// </summary>
    public StandSettingsValveModel EndCommonValveViewModel { get; set; }
    
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
    /// Выбранный тип линии (строка)
    /// </summary>
    public string SelectedStringLineType { get; set; }

    /// <summary>
    /// Выбранный тип линии
    /// </summary>
    public LineType SelectedLineType { get; set; }

    /// <summary>
    /// Доступен ли обратный ход воздуха (и соответственно клапана направления потока)
    /// </summary>
    public bool IsReverseLine { get; set; }
}