using SPU_7.Common.Line;

namespace SPU_7.Extensions;

public class DirectFlowInfoData
{
     public DirectFlowInfoData(LineDirectionFlowState directionFlowState, int lineIndex)
     {
          DirectionFlowState = directionFlowState;
          LineIndex = lineIndex;
     }

     public LineDirectionFlowState DirectionFlowState { get; set; }
     
     public int LineIndex { get; set; }
}