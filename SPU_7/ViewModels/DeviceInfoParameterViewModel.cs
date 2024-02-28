namespace SPU_7.ViewModels;

public class DeviceInfoParameterViewModel : ViewModelBase
{
    private string _name;
    private object _value;
    private bool _isVisible = true;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public object Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }

    public bool IsVisible
    {
        get => _isVisible;
        set => SetProperty(ref _isVisible, value);
    }

    public DeviceInfoParameterType ParameterType { get; set; }

}