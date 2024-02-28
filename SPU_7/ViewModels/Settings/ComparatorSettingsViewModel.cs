namespace SPU_7.ViewModels.Settings;

public class ComparatorSettingsViewModel : ViewModelBase
{
    public ComparatorSettingsViewModel()
    {
            
    }

    private int _number;
    private int? _levelComparator;
    private int? _minimumHysteresisBorder;
    private int? _maximumHysteresisBorder;
    
    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public int? LevelComparator
    {
        get => _levelComparator;
        set => SetProperty(ref _levelComparator, value);
    }

    public int? MinimumHysteresisBorder
    {
        get => _minimumHysteresisBorder;
        set => SetProperty(ref _minimumHysteresisBorder, value);
    }

    public int? MaximumHysteresisBorder
    {
        get => _maximumHysteresisBorder;
        set => SetProperty(ref _maximumHysteresisBorder, value);
    }
}