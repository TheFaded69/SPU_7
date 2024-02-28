using System.Collections.Generic;
using SPU_7.Common.Settings;
using SPU_7.Models.Stand.Settings.Stand.Devices.Extensions;

namespace SPU_7.Models.Stand.Settings.Stand.Devices;

public class NBIoTDeviceConfiguration : BaseDeviceConfiguration
{
    public List<ComparatorConfiguration> ComparatorConfigurations { get; set; }
    
    public double FrequencyCheck { get; set; }
    
    public double FrequencyCheckDifference { get; set; }
    
    public int FrequencyStabilization { get; set; }
    
    public PlatformType PlatformType { get; set; }
    
    public bool IsCheckOnlyConnection { get; set; }
    
    public bool IsGetFrequencyFromSpecialRegister { get; set; }
    
    public int PauseForCheckingOutsideValve { get; set; }
}