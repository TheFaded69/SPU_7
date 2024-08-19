using System;
using System.Threading.Tasks;
using Prism.Commands;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class NeedleValveItemViewModel : ViewModelBase
{
    public NeedleValveItemViewModel(StandSettingsNeedleValveModel standSettingsValveModel,
        IStandController standController, 
        StateType stateType)
    {
        _standSettingsValveModel = standSettingsValveModel;
        _standController = standController;
        _stateType = stateType;
        
        UseValveCommand = new DelegateCommand(UseValveCommandHandler);
    }
    private readonly StandSettingsNeedleValveModel _standSettingsValveModel;
    private readonly IStandController _standController;
   
    private StateType _stateType;
    private int _selectedNeedleValue;
    
    public StateType StateType
    {
        get => _stateType;
        set => SetProperty(ref _stateType, value);
    }
    public int SelectedNeedleValue
    {
        get => _selectedNeedleValue;
        set => SetProperty(ref _selectedNeedleValue, value);
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
                //await _standController.CloseValveAsync(StandSettingsValveModel);
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
                //await _standController.OpenValveAsync(StandSettingsValveModel);
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