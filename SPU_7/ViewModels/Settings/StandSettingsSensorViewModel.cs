using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using SPU_7.Common.Extensions;
using SPU_7.Common.Line;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsSensorViewModel : ViewModelBase
{
    public ObservableCollection<string> SensorPurposes { get; } = new(Enum.GetValues<SensorPurpose>().Select(en => en.GetDescription()));
    public ObservableCollection<string> SensorTypes { get; } = [];
    public ObservableCollection<string> ComPorts { get; set; } = new(SerialPort.GetPortNames());
    
    private int _number;
    private string _selectedComPort;
    private int _address;
    private string _selectedSensorPurposes;
    private string _selectedSensorType;
    private int _channelNumber;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }
    
    public string SelectedComPort
    {
        get => _selectedComPort;
        set => SetProperty(ref _selectedComPort, value);
    }

    public int Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public string SelectedSensorPurposes
    {
        get => _selectedSensorPurposes;
        set
        {
            SetProperty(ref _selectedSensorPurposes, value);
            
            SensorTypes.Clear();
            foreach (var sensorType in Enum.GetValues<SensorPurpose>()
                         .FirstOrDefault(sp => sp.GetDescription() == value)
                         .GetSensorTypes())
                SensorTypes.Add(sensorType.GetDescription());

            SensorPurpose = Enum.GetValues<SensorPurpose>()
                .FirstOrDefault(sp => sp.GetDescription() == value);
        }
    }
    
    public SensorPurpose SensorPurpose { get; set; }

    public string SelectedSensorType
    {
        get => _selectedSensorType;
        set
        {
            SetProperty(ref _selectedSensorType, value);
            
            SensorType = Enum.GetValues<SensorType>()
                .FirstOrDefault(sp => sp.GetDescription() == value);
        }
    }

    public SensorType SensorType { get; set; }

    public int ChannelNumber
    {
        get => _channelNumber;
        set => SetProperty(ref _channelNumber, value);
    }
}