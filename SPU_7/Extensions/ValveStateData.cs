using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.ViewModels;

namespace SPU_7.Extensions;

public class ValveStateData
{
    public ValveStateData(StandSettingsValveModel standSettingsValveModel, StateType stateType)
    {
        StandSettingsValveModel = standSettingsValveModel;
        StateType = stateType;
    }
    
    public StandSettingsValveModel StandSettingsValveModel { get; }
    public StateType StateType { get;  }
}