using System.Collections.ObjectModel;

namespace SPU_7.ViewModels.DeviceInformationViewModels;

public class DeviceAboutViewModel : ViewModelBase
{
    private int _deviceNumber;
    private string _deviceName;
    private string _deviceVendorNumber;
    private bool _isManualEnabled = true;
    private DeviceNameViewModel _selectedDeviceTypeInfo;
    private ObservableCollection<DeviceNameViewModel> _deviceTypesInfo;
    private string _vendorName;
    private bool _isTemperatureCorrect;

    public int DeviceNumber
    {
        get => _deviceNumber;
        set => SetProperty(ref _deviceNumber, value);
    }

    public string DeviceName
    {
        get => _deviceName;
        set => SetProperty(ref _deviceName, value);
    }

    public string DeviceVendorNumber
    {
        get => _deviceVendorNumber;
        set
        {
            SetProperty(ref _deviceVendorNumber, value);

            if (!string.IsNullOrEmpty(value)) IsManualEnabled = true;
        }
    }

    public bool IsManualEnabled
    {
        get => _isManualEnabled;
        set => SetProperty(ref _isManualEnabled, value);
    }

    public bool IsTemperatureCorrect
    {
        get => _isTemperatureCorrect;
        set => SetProperty(ref _isTemperatureCorrect, value);
    }

    public ObservableCollection<DeviceNameViewModel> DeviceTypesInfo
    {
        get => _deviceTypesInfo;
        set => SetProperty(ref _deviceTypesInfo, value);
    }

    public DeviceNameViewModel SelectedDeviceTypeInfo
    {
        get => _selectedDeviceTypeInfo;
        set => SetProperty(ref _selectedDeviceTypeInfo, value);
    }

    public string VendorName
    {
        get => _vendorName;
        set => SetProperty(ref _vendorName, value);
    }
}