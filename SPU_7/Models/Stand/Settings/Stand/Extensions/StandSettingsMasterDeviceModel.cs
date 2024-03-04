using System.IO.Ports;
using SPU_7.Common.Device;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsMasterDeviceModel
{
    public MasterDeviceType SelectedMasterDeviceType { get; set; }
    
    public string SelectedComPort { get; set; }
    
    public int SelectedBaudRate { get; set; }
    
    public Parity SelectedParity { get; set; }
    
    public StopBits SelectedStopBits { get; set; }
    
    public Handshake SelectedHandshake { get; set; }
}