using SPU_7.Common.Extensions;
using SPU_7.Common.Stand;

namespace SPU_7.Models.HelpModel
{
    public class HelpModel
    {
        public HelpModel(StandType standType)
        {
            HelpModelName = standType.GetDescription();
        }

        public string HelpModelName { get; set; }

        
    }
}
