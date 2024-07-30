using System;
using System.Collections.ObjectModel;
using System.IO.Ports;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsPortViewModel : ViewModelBase
{
    private string _portName;
    private int _portBaudRate;
    private int _number;
    private double _selectedStopBitDouble;
    private Parity _selectedParity;

    public ObservableCollection<Parity> Parities { get; set; } =
        new ObservableCollection<Parity>(Enum.GetValues<Parity>());
    
    public ObservableCollection<string> PortNames { get; set; } = new(SerialPort.GetPortNames());

    public ObservableCollection<int> PortBaudRates { get; set; } = new()
    {
        300,
        600,
        1200,
        2400,
        4800,
        9600,
        19200,
        38400,
        57600,
        115200,
        230400,
        460800,
        921600
    };

    public ObservableCollection<double> StopBits { get; set; } = new()
    {
        0,
        1,
        1.5,
        2
    };

    public double SelectedStopBitDouble
    {
        get => _selectedStopBitDouble;
        set
        {
            SetProperty(ref _selectedStopBitDouble, value);

            SelectedStopBit = value switch
            {
                1 => System.IO.Ports.StopBits.One,
                1.5 => System.IO.Ports.StopBits.OnePointFive,
                2 => System.IO.Ports.StopBits.Two,
                0 => System.IO.Ports.StopBits.None
            };
        }
    }

    public StopBits SelectedStopBit { get; set; }
    
    public string PortName
    {
        get => _portName;
        set => SetProperty(ref _portName, value);
    }

    public int PortBaudRate
    {
        get => _portBaudRate;
        set => SetProperty(ref _portBaudRate, value);
    }

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public Parity SelectedParity
    {
        get => _selectedParity;
        set => SetProperty(ref _selectedParity, value);
    }
}