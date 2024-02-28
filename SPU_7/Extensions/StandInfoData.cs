using SPU_7.ViewModels;

namespace SPU_7.Extensions;

public class StandInfoData
{
    public StandInfoData(int number, StateType stateType)
    {
        Number = number;
        StateType = stateType;
    }

    public int Number { get; set; }
    
    public StateType StateType { get; set; }
}