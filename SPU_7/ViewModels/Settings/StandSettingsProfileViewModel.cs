using System;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsProfileViewModel : ViewModelBase
{
    public Guid Id { get; set; }

    private string _name;
    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }
}