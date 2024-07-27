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

    public ObservableCollection<string> MasterDeviceTypesString { get; set; } = new(Enum
        .GetValues<MasterDeviceType>()
        .Where(mdt => mdt != MasterDeviceType.None)
        .Select(mdt => mdt.GetDescription()));

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

    public MasterDeviceType SelectedMasterDeviceType { get; set; }

    public ObservableCollection<string> PortNames { get; set; } = new(SerialPort.GetPortNames());

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
}