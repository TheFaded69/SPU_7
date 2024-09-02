using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.IO.Ports;
using System.Reactive.Linq;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsPulseMeterViewModel : ViewModelBase
{
    private int _number;
    private int? _address;
    private double? _firstCalibrateCoefficient;
    private double? _secondCalibrateCoefficient;
    private string _selectedComPort;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public int? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public double? FirstCalibrateCoefficient
    {
        get => _firstCalibrateCoefficient;
        set => SetProperty(ref _firstCalibrateCoefficient, value);
    }

    public double? SecondCalibrateCoefficient
    {
        get => _secondCalibrateCoefficient;
        set => SetProperty(ref _secondCalibrateCoefficient, value);
    }

    public ObservableCollection<string> ComPorts { get; set; } = new(SerialPort.GetPortNames());

    public string SelectedComPort
    {
        get => _selectedComPort;
        set => SetProperty(ref _selectedComPort, value);
    }
}