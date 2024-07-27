using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsFanViewModel : ViewModelBase
{
    private int _number;
    private bool _isNeedleValveEnable;
    private bool _isValveEnable;
    private StandSettingsValveViewModel _fanValveViewModel;
    private StandSettingsFrequencyRegulatorViewModel _frequencyRegulatorViewModel;

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

    public StandSettingsValveViewModel FanValveViewModel
    {
        get => _fanValveViewModel;
        set => SetProperty(ref _fanValveViewModel, value);
    }

    public StandSettingsFrequencyRegulatorViewModel FrequencyRegulatorViewModel
    {
        get => _frequencyRegulatorViewModel;
        set => SetProperty(ref _frequencyRegulatorViewModel, value);
    }
    
}