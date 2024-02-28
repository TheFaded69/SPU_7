namespace SPU_7.ViewModels.Settings;

public class StandSettingsValveViewModel : ViewModelBase
{
    private int _number;
    private int? _address;
    private string _registerAddress;
    private int? _bitNumber;
    private int? _stateAddress;
    private string _stateRegisterAddress;
    private int? _stateBitNumber;
    private bool _isControlState;
    private bool _isTubeValve;
    private bool _isPressureDifferenceValve;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
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

    public bool IsTubeValve
    {
        get => _isTubeValve;
        set => SetProperty(ref _isTubeValve, value);
    }

    public bool IsPressureDifferenceValve
    {
        get => _isPressureDifferenceValve;
        set => SetProperty(ref _isPressureDifferenceValve, value);
    }
}