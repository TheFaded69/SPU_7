using System.Collections.ObjectModel;
using System.IO.Ports;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsFrequencyRegulatorViewModel: ViewModelBase
{
    public ObservableCollection<string> PortNames { get; set; } = new(SerialPort.GetPortNames());
    private string _portName;
    private int _moduleAddress;
    private double _kP;
    private double _kI;
    private double _kD;
    private double _pvMax;
    private double _pvMin;
    private double _outMax;
    private double _outMin;

    public string PortName
    {
        get => _portName;
        set => SetProperty(ref _portName, value);
    }

    public int ModuleAddress
    {
        get => _moduleAddress;
        set => SetProperty(ref _moduleAddress, value);
    }

    public double kP
    {
        get => _kP;
        set => SetProperty(ref _kP, value);
    }

    public double kI
    {
        get => _kI;
        set => SetProperty(ref _kI, value);
    }

    public double kD
    {
        get => _kD;
        set => SetProperty(ref _kD, value);
    }

    public double pvMax
    {
        get => _pvMax;
        set => SetProperty(ref _pvMax, value);
    }

    public double pvMin
    {
        get => _pvMin;
        set => SetProperty(ref _pvMin, value);
    }

    public double outMax
    {
        get => _outMax;
        set => SetProperty(ref _outMax, value);
    }

    public double outMin
    {
        get => _outMin;
        set => SetProperty(ref _outMin, value);
    }
}