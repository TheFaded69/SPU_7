using System;
using System.Threading.Tasks;
using Prism.Commands;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class OwenValveItemViewModel : ViewModelBase
{
    private readonly StandSettingsOwenValveModel _standSettingsValveModel;
    private readonly IStandController _standController;
    private readonly int _lineIndex;

    public OwenValveItemViewModel(StandSettingsOwenValveModel standSettingsValveModel,
        IStandController standController,
        StateType stateType,
        int lineIndex)
    {
        _standSettingsValveModel = standSettingsValveModel;
        _standController = standController;
        _stateType = stateType;
        _lineIndex = lineIndex;

        UseValveCommand = new DelegateCommand(UseValveCommandHandler);
    }

    private StateType _stateType;
    private int _selectedOwenValue = 0;

    public StateType StateType
    {
        get => _stateType;
        set => SetProperty(ref _stateType, value);
    }

    public int SelectedOwenValue
    {
        get => _selectedOwenValue;
        set => SetProperty(ref _selectedOwenValue, value);
    }

    public DelegateCommand UseValveCommand { get; }

    private async void UseValveCommandHandler()
    {
        switch (StateType)
        {
            case StateType.None:
                break;
            case StateType.Open:
            {
                StateType = StateType.Work;
#if DEBUGGUI
                await Task.Delay(5000);
#else
                await _standController.UseOwenValveAsync(_standSettingsValveModel, _lineIndex, 0);
#endif
                StateType = StateType.Close;
            }
                break;
            case StateType.Close:
            {
                StateType = StateType.Work;
#if DEBUGGUI
                await Task.Delay(5000);
#else
                await _standController.UseOwenValveAsync(_standSettingsValveModel, _lineIndex, SelectedOwenValue);
#endif
                StateType = StateType.Open;
            }
                break;
            case StateType.Work:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}