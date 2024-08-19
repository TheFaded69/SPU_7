using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Ports;
using System.Reactive.Linq;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsNeedleValveViewModel : ViewModelBase
{
    public ObservableCollection<string> ComPorts { get; set; } = new(SerialPort.GetPortNames());
    
    private string _selectedComPort;
    private int _moduleAddress;

    public string SelectedComPort
    {
        get => _selectedComPort;
        set => SetProperty(ref _selectedComPort, value);
    }

    public int ModuleAddress
    {
        get => _moduleAddress;
        set => SetProperty(ref _moduleAddress, value);
    }
}