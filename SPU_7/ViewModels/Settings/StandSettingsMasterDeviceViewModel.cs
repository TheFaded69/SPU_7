using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsMasterDeviceViewModel : ViewModelBase
{
    public StandSettingsMasterDeviceViewModel()
    {
        
    }
    private int _number;
    private string _selectedMasterDeviceTypeString;
    private string _selectedComPort;
    private int _masterDeviceAddress;
    private bool _isConnectionInfoVisible;
    private string _vendorNumber;
    private int _pressureSensorAddress;
    private int _temperatureSensorAddress;
    private string _selectedPressureSensorComPort;
    private string _selectedTemperatureSensorComPort;
    private string _masterDeviceName;
    private StandSettingsValveViewModel _pressureSensorValveViewModel;
    private StandSettingsValveViewModel _masterDeviceValveViewModel;
    private ObservableCollection<string> _masterDeviceTypesString = new(Enum
        .GetValues<MasterDeviceType>()
        .Where(mdt => mdt != MasterDeviceType.None)
        .Select(mdt => mdt.GetDescription()));

    private MasterDeviceType _selectedMasterDeviceType;
    private ObservableCollection<string> _portNames = new(SerialPort.GetPortNames());
    private int? _pulseCountMeterModuleNumber;
    private int? _pulseCountMeterModuleChannelNumber;
    private int? _temperatureChannelNumber;
    private float? _maximumFlow;
    private float _pulseWeight;

    public ObservableCollection<string> MasterDeviceTypesString
    {
        get => _masterDeviceTypesString;
        set => SetProperty(ref _masterDeviceTypesString, value);
    }

    public string SelectedMasterDeviceTypeString
    {
        get => _selectedMasterDeviceTypeString;
        set
        {
            SetProperty(ref _selectedMasterDeviceTypeString, value);
            SelectedMasterDeviceType = Enum.GetValues<MasterDeviceType>()
                .FirstOrDefault(mdt => mdt.GetDescription() == value);
            
            IsConnectionInfoVisible = Enum.GetValues<MasterDeviceType>()
                .FirstOrDefault(mdt => mdt.GetDescription() == value) == MasterDeviceType.GFG;
        }
    }

    public MasterDeviceType SelectedMasterDeviceType
    {
        get => _selectedMasterDeviceType;
        set => SetProperty(ref _selectedMasterDeviceType, value);
    }

    public ObservableCollection<string> PortNames
    {
        get => _portNames;
        set => SetProperty(ref _portNames, value);
    }

    public string SelectedComPort
    {
        get => _selectedComPort;
        set => SetProperty(ref _selectedComPort, value);
    }

    public int MasterDeviceAddress
    {
        get => _masterDeviceAddress;
        set => SetProperty(ref _masterDeviceAddress, value);
    }

    public bool IsConnectionInfoVisible
    {
        get => _isConnectionInfoVisible;
        set => SetProperty(ref _isConnectionInfoVisible, value);
    }

    public string VendorNumber
    {
        get => _vendorNumber;
        set => SetProperty(ref _vendorNumber, value);
    }

    public float? MaximumFlow
    {
        get => _maximumFlow;
        set => SetProperty(ref _maximumFlow, value);
    }

    public int PressureSensorAddress
    {
        get => _pressureSensorAddress;
        set => SetProperty(ref _pressureSensorAddress, value);
    }

    public int TemperatureSensorAddress
    {
        get => _temperatureSensorAddress;
        set => SetProperty(ref _temperatureSensorAddress, value);
    }

    public string SelectedPressureSensorComPort
    {
        get => _selectedPressureSensorComPort;
        set => SetProperty(ref _selectedPressureSensorComPort, value);
    }

    public string SelectedTemperatureSensorComPort
    {
        get => _selectedTemperatureSensorComPort;
        set => SetProperty(ref _selectedTemperatureSensorComPort, value);
    }

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public string MasterDeviceName { get => _masterDeviceName; set => SetProperty(ref _masterDeviceName, value); }

    public StandSettingsValveViewModel PressureSensorValveViewModel
    {
        get => _pressureSensorValveViewModel;
        set => SetProperty(ref _pressureSensorValveViewModel, value);
    }

    public StandSettingsValveViewModel MasterDeviceValveViewModel
    {
        get => _masterDeviceValveViewModel;
        set => SetProperty(ref _masterDeviceValveViewModel, value);
    }

    public int? PulseCountMeterModuleNumber
    {
        get => _pulseCountMeterModuleNumber;
        set => SetProperty(ref _pulseCountMeterModuleNumber, value);
    }

    public int? PulseCountMeterModuleChannelNumber
    {
        get => _pulseCountMeterModuleChannelNumber;
        set => SetProperty(ref _pulseCountMeterModuleChannelNumber, value);
    }

    public int? TemperatureChannelNumber
    {
        get => _temperatureChannelNumber;
        set => SetProperty(ref _temperatureChannelNumber, value);
    }

    public float PulseWeight
    {
        get => _pulseWeight;
        set => SetProperty(ref _pulseWeight, value);
    }
}