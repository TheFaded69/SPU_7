namespace SPU_7.ViewModels.Settings;

public class StandSettingsValveViewModel : ViewModelBase
{
    private int _number;
    private int? _address;
    private string _registerAddress;
    private int? _bitNumber;
    private int? _stateOnAddress;
    private string _stateOnRegisterAddress;
    private int? _stateOnBitNumber;
    private bool _isControlState;
    private bool _isTubeValve;
    private bool _isPressureDifferenceValve;
    private int? _stateOffAddress;
    private string _stateOffRegisterAddress;
    private int? _stateOffBitNumber;
    private bool _isReverseValve;

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

    public int? StateOnAddress
    {
        get => _stateOnAddress;
        set => SetProperty(ref _stateOnAddress, value);
    }

    public string StateOnRegisterAddress
    {
        get => _stateOnRegisterAddress;
        set => SetProperty(ref _stateOnRegisterAddress, value);
    }

    public int? StateOnBitNumber
    {
        get => _stateOnBitNumber;
        set => SetProperty(ref _stateOnBitNumber, value);
    }

    public int? StateOffAddress
    {
        get => _stateOffAddress;
        set => SetProperty(ref _stateOffAddress, value);
    }

    public string StateOffRegisterAddress
    {
        get => _stateOffRegisterAddress;
        set => SetProperty(ref _stateOffRegisterAddress, value);
    }

    public int? StateOffBitNumber
    {
        get => _stateOffBitNumber;
        set => SetProperty(ref _stateOffBitNumber, value);
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

    public bool IsReverseValve
    {
        get => _isReverseValve;
        set => SetProperty(ref _isReverseValve, value);
    }
}