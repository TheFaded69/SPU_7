namespace SPU_7.ViewModels.Settings;

public class StandSettingsDeviceViewModel : ViewModelBase
{
    public StandSettingsDeviceViewModel()
    {
        
    }
    
    private int _number;
    private int? _pressureSensorAddress;
    private int? _pulseMeterNumber;
    private int? _pulseMeterChannelNumber;
    private StandSettingsValveViewModel _standSettingsValveViewModel;
    private bool _isValveEnable;
    private int? _address;
    private string _registerAddress;
    private int? _bitNumber;
    private int? _stateAddress;
    private string _stateRegisterAddress;
    private int? _stateBitNumber;
    private bool _isControlState;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public int? PressureSensorAddress
    {
        get => _pressureSensorAddress;
        set => SetProperty(ref _pressureSensorAddress, value);
    }


    public int? PulseMeterNumber
    {
        get => _pulseMeterNumber;
        set => SetProperty(ref _pulseMeterNumber, value);
    }

    public int? PulseMeterChannelNumber
    {
        get => _pulseMeterChannelNumber;
        set => SetProperty(ref _pulseMeterChannelNumber, value);
    }

    public bool IsValveEnable
    {
        get => _isValveEnable;
        set => SetProperty(ref _isValveEnable, value);
    }

    public int? Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public string RegisterAddress
    {
        get => _registerAddress;
        set => SetProperty(ref _registerAddress, value);
    }

    public int? BitNumber
    {
        get => _bitNumber;
        set => SetProperty(ref _bitNumber, value);


    }

    public int? StateAddress
    {
        get => _stateAddress;
        set => SetProperty(ref _stateAddress, value);
    }

    public string StateRegisterAddress
    {
        get => _stateRegisterAddress;
        set => SetProperty(ref _stateRegisterAddress, value);
    }

    public int? StateBitNumber
    {
        get => _stateBitNumber;
        set => SetProperty(ref _stateBitNumber, value);
    }

    public bool IsControlState
    {
        get => _isControlState;
        set => SetProperty(ref _isControlState, value);
    }
}