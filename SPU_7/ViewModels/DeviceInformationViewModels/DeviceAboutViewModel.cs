namespace SPU_7.ViewModels.DeviceInformationViewModels;

public class DeviceAboutViewModel : ViewModelBase
{
    private int _deviceNumber;
    private string _deviceName;
    private string _deviceVendorNumber;
    private bool _isManualEnabled = true;

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
        set => SetProperty(ref _deviceVendorNumber, value);
    }

    public bool IsManualEnabled
    {
        get => _isManualEnabled;
        set => SetProperty(ref _isManualEnabled, value);
    }
}