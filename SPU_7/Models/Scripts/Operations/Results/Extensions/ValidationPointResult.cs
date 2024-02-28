using System.Collections.Generic;

namespace SPU_7.Models.Scripts.Operations.Results.Extensions;

public class ValidationPointResult
{
    public List<ValidationMeasureResult> ValidationMeasureResults { get; set; } = new();
}