using System.Collections.Generic;
using SPU_7.Common.Settings;
using SPU_7.Models.Scripts.Operations.Results.Extensions;

namespace SPU_7.Models.Scripts.Operations.Results;

public class ValidationOperationResult : BaseOperationResult
{
    public List<ValidationPointResult> ValidationPointResults { get; set; } = new();
    
    public string VendorName { get; set; }
    
    public string VendorAddress { get; set; }
    
    public string DeviceName { get; set; }
    
    public ValidationType ValidationType { get; set; }
    
    public double? MinimumFlow { get; set; }
    public double? MaximumFlow { get; set; }
    public double? NominalFlow { get; set; }
}