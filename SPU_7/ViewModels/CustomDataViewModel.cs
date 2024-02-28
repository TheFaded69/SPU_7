using SPU_7.Common.Modbus;

namespace SPU_7.ViewModels;

public class CustomDataViewModel : ViewModelBase
{
    private object _data;
    private string _name;

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public object Data
    {
        get => _data;
        set => SetProperty(ref _data, value);
    }

    public RegisterDataType RegisterDataType { get; set; }
}