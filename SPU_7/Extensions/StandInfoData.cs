using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.ViewModels;

namespace SPU_7.Extensions;

public class StandInfoData
{
    

    public StandInfoData(int index, StateType stateType)
    {
        Index = index;
        StateType = stateType;
    }

    public StandInfoData(StandSettingsValveModel standSettingsValveModel, StateType stateType)
    {
        StandSettingsValveModel = standSettingsValveModel;
        StateType = stateType;
    }
    
    public StandInfoData(StandSettingsFanModel standSettingsFanModel, StateType stateType)
    {
        StandSettingsFanModel = standSettingsFanModel;
        StateType = stateType;
    }

    public StandInfoData(StandSettingsNeedleValveModel settingsNeedleValveModel, int value)
    {
        SettingsNeedleValveModel = settingsNeedleValveModel;
        Value = value;
    }
    
    public int Index { get; set; }
    public StandSettingsValveModel StandSettingsValveModel { get; }
    public StandSettingsFanModel StandSettingsFanModel { get; }
    public StandSettingsNeedleValveModel SettingsNeedleValveModel { get; }
    public int Value { get; }
    public StateType StateType { get; set; }
}