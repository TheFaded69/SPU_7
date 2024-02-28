using System;
using System.Collections.ObjectModel;
using System.Linq;
using SPU_7.Common.Extensions;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsSolenoidValveViewModel : ViewModelBase
{
    private int _number;
    private int? _address;
    private string _registerAddress;
    private int? _bitNumber;
    private string _selectedStringSolenoidValveType;
    private SolenoidValveType _solenoidValveType;

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

    public ObservableCollection<string> StringSolenoidValveTypes { get; set; } = new(Enum
        .GetValues<SolenoidValveType>()
        .Select(sv => sv.GetDescription()));
    
    public string SelectedStringSolenoidValveType
    {
        get => _selectedStringSolenoidValveType;
        set
        {
            SetProperty(ref _selectedStringSolenoidValveType, value);
            SolenoidValveType = Enum
                .GetValues<SolenoidValveType>()
                .FirstOrDefault(sv => sv.GetDescription() == value);
        }
    }

    public SolenoidValveType SolenoidValveType
    {
        get => _solenoidValveType;
        set => SetProperty(ref _solenoidValveType, value);
    }
}