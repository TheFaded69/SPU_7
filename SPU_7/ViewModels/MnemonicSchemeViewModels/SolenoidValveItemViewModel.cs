using System;
using System.Threading.Tasks;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class SolenoidValveItemViewModel : ViewModelBase
{
    public SolenoidValveItemViewModel(IStandController standController, StandSettingsSolenoidValveModel standSettingsSolenoidValveModel)
    {
        _standController = standController;
        _standSettingsSolenoidValveModel = standSettingsSolenoidValveModel;

        SolenoidValveType = _standSettingsSolenoidValveModel.SolenoidValveType;
        StateType = _standSettingsSolenoidValveModel.SolenoidValveType switch
        {
            SolenoidValveType.NormalOpen => StateType.Open,
            SolenoidValveType.NormalClose => StateType.Close,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private readonly StandSettingsSolenoidValveModel _standSettingsSolenoidValveModel;
    private readonly IStandController _standController;
    
    private StateType _stateType;

    public SolenoidValveType SolenoidValveType { get; set; }
    public StateType StateType
    {
        get => _stateType;
        set => SetProperty(ref _stateType, value);
    }

    public async Task OpenSolenoidValveAsync()
    {
        await _standController.OpenSolenoidValveAsync(_standSettingsSolenoidValveModel);

        StateType = StateType.Open;
    }

    public async Task CloseSolenoidValveAsync()
    {
        await _standController.CloseSolenoidValveAsync(_standSettingsSolenoidValveModel);

        StateType = StateType.Close;
    }
    
}