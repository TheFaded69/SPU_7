using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models;

public class DbOperationResult: DbEntityGuid
{
    public Guid ScriptResultId { get; set; }

    public DbScriptResult ScriptResult { get; set; }
    
    public List<DbDevice> Device { get; set; }
    
    public string Result { get; set; }
}