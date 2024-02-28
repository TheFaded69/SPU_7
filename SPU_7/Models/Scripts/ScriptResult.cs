using System.Collections.Generic;

namespace SPU_7.Models.Scripts;

public class ScriptResult
{
    public List<OperationResult> OperationResults { get; set; } = new();
    
    public string Name { get; set; }

    public string Description { get; set; }
}