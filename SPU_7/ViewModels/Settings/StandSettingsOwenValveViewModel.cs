using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Ports;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsOwenValveViewModel : ViewModelBase
{
    private int _owenNumber;
    private int? _address;
    private string _selectedPort;

    public int OwenNumber
    {
        get => _owenNumber;
        set => SetProperty(ref _owenNumber, value);
    }

    public int? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public string SelectedPort
    {
        get => _selectedPort;
        set => SetProperty(ref _selectedPort, value);
    }

    public ObservableCollection<string> ComPorts { get; set; } = new(SerialPort.GetPortNames());
}