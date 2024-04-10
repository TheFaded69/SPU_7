namespace SPU_7.ViewModels;

public class DeviceNameViewModel : ViewModelBase
{
    private string _deviceTypeInfo;

    public string DeviceTypeInfo
    {
        get => _deviceTypeInfo;
        set => SetProperty(ref _deviceTypeInfo, value);
    }
}