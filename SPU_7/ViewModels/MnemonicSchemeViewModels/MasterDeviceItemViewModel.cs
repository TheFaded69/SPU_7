using SPU_7.Models.Stand;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class MasterDeviceItemViewModel : ViewModelBase
{
    public MasterDeviceItemViewModel(IStandController standController)
    {
        _standController = standController;
    }
    private readonly IStandController _standController;
    
    private StateType _stateType;
    private float? _pressure;
    private float? _temperature;
    
    public float? Pressure
    {
        get => _pressure;
        set => SetProperty(ref _pressure, value);
    }

    public float? Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value);
    }

    public StateType StateType
    {
        get => _stateType;
        set => SetProperty(ref _stateType, value);
    }
    
    public string VendorNumber { get; set; }
}