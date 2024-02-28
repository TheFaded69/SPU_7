using System.Collections.ObjectModel;
using SPU_7.Common.Line;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsLineModel 
{
    public ObservableCollection<StandSettingsDeviceModel> DeviceViewModels { get; set; }
    
    public StandSettingsValveModel DirectValveViewModel { get; set; }
    public StandSettingsValveModel ReverseValveViewModel { get; set; }
    
    public int LineNumber { get; set; }
    
    public string SelectedStringDeviceLineType { get; set; }
    
    public DeviceLineType SelectedDeviceLineType { get; set; }
    
    public bool IsReverseLine { get; set; }
}