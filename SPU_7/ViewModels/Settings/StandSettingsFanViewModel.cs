namespace SPU_7.ViewModels.Settings;

public class StandSettingsFanViewModel : ViewModelBase
{
    private int _number;
    private bool _isNeedleValveEnable;
    private bool _isValveEnable;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public bool IsNeedleValveEnable
    {
        get => _isNeedleValveEnable;
        set => SetProperty(ref _isNeedleValveEnable, value);
    }

    public bool IsValveEnable
    {
        get => _isValveEnable;
        set => SetProperty(ref _isValveEnable, value);
    }
}