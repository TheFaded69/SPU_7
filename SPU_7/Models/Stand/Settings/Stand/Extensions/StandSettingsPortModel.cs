using System.IO.Ports;

namespace SPU_7.Models.Stand.Settings.Stand.Extensions;

public class StandSettingsPortModel
{
    public string PortName { get; set; }

    public int PortBaudRate { get; set; }

    public StopBits SelectedStopBit { get; set; }

    public double SelectedStopBitDouble { get; set; }
}