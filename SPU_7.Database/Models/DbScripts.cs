using SPU_7.Common.Device;
using SPU_7.Common.Stand;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models
{
    public class DbScripts : DbEntityGuid
    {
        public List<DbOperations> Operations { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public int LineNumber { get; set; }

        public StandType TargetStandType { get; set; }

        public DeviceType DeviceType { get; set; }
    }
}
