using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Reactive.Linq;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsPulseCountMeterModuleViewModel : ViewModelBase
{
    private int _number;
    private int _moduleAddress;
    private string _portName;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public int ModuleAddress
    {
        get => _moduleAddress;
        set => SetProperty(ref _moduleAddress, value);
    }

    public string PortName
    {
        get => _portName;
        set => SetProperty(ref _portName, value);
    }

    public ObservableCollection<string> PortNames { get; set; } = new(SerialPort.GetPortNames());
}