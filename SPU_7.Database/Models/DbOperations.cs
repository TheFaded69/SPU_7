using SPU_7.Common.Scripts;
using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models
{
    public class DbOperations : DbEntityGuid
    {
        public Guid ScriptId { get; set; }
        public DbScripts Script { get; set; }
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string Configuration { get; set; }
        
        public OperationType OperationType { get; set; }
        
        public int Number { get; set; }
    }
}
