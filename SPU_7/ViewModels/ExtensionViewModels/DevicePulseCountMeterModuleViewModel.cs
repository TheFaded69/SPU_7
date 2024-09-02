namespace SPU_7.ViewModels.ExtensionViewModels;

public class DevicePulseCountMeterModuleViewModel : ViewModelBase
{
    public int LineNumber { get; set; }
    public int DeviceNumber { get; set; }
    public int  PulseCountMeterChannelNumber { get; set; }
    
    public int  PulseMeterChannelNumber { get; set; }
    
    public int PulseMeterNumber { get; set; }
}