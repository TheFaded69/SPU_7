﻿namespace SPU_7.ViewModels.Settings;

public class StandSettingsMasterDeviceViewModel : ViewModelBase
{
    private int _number;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }
}