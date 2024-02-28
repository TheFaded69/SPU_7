using System.Collections.Generic;

namespace SPU_7.Models.Scripts.Operations.Results.Extensions;

public class ValidationMeasureResult
{
    public List<ValidationDeviceResult> ValidationDeviceResults { get; set; } = new();
}