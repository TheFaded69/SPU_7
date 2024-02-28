namespace SPU_7.ViewModels.Settings;

public class StandSettingsNozzleViewModel : ViewModelBase
{
    private int _number;
    private double? _nozzleValue;
    private double? _nozzleFactValue;
    private int? _address;
    private string _registerAddress;
    private int? _bitNumber;
    private int? _stateAddress;
    private string _stateRegisterAddress;
    private int? _stateBitNumber;
    private bool _isControlState;
    private bool _isChecked;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public double? NozzleValue
    {
        get => _nozzleValue;
        set => SetProperty(ref _nozzleValue, value);
    }

    public double? NozzleFactValue
    {
        get => _nozzleFactValue;
        set => SetProperty(ref _nozzleFactValue, value);
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

    public bool IsChecked
    {
        get => _isChecked;
        set => SetProperty(ref _isChecked, value);
    }
}