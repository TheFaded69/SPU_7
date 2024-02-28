using System.Collections.ObjectModel;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.Models.Scripts.Operations.Configurations.Points;

public class ValidationPointModel
{
    public int Number { get; set; }

    public bool IsValveUse { get; set; }

    public int Delay { get; set; }

    public double TargetConsumption { get; set; }

    public int MeasureCount { get; set; }

    public double TargetVolume { get; set; }
    
    public double Inaccuracy { get; set; }
    
    public ObservableCollection<StandSettingsNozzleModel> SelectedNozzles { get; set; }
}