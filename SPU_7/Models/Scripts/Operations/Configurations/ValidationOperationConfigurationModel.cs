using System.Collections.Generic;
using SPU_7.Common.Settings;
using SPU_7.Models.Scripts.Operations.Configurations.Extensions;
using SPU_7.Models.Scripts.Operations.Configurations.Points;

namespace SPU_7.Models.Scripts.Operations.Configurations;

public class ValidationOperationConfigurationModel : BaseOperationConfigurationModel
{
    public List<ValidationPointModel> Points { get; set; }
    
    public List<ValidationPulseMeterConfiguration> PulseMeterConfigurations { get; set; }
    public bool IsProtocolNeed { get; set; }
    
    public bool IsAutoPulseMeasure { get; set; }
    
    public string DeviceName { get; set; }
    
    public string VendorName { get; set; }
    
    public string VendorAddress { get; set; }

    public ValidationType ValidationType { get; set; }
    public double? MinimumFlow { get; set; }
    public double? MaximumFlow { get; set; }
    public double? NominalFlow { get; set; }
}