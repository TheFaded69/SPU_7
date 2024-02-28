using SPU_7.Database.Models.EntityTypes;

namespace SPU_7.Database.Models;

public class DbScriptResult: DbEntityGuid
{
    public List<DbOperationResult> OperationResults { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }
}