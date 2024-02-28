using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Prism.Commands;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels
{
    public class ValveItemViewModel : ViewModelBase
    {
        public ValveItemViewModel(StandSettingsValveModel standSettingsValveModel,
            IStandController standController)
        {
            StandSettingsValveModel = standSettingsValveModel;
            _standController = standController;

            UseValveCommand = new DelegateCommand(UseValveCommandHandler);

            //DeviceInformationView = new DeviceInformationView() { DataContext = new DeviceInformationViewModel() };
        }

        public readonly StandSettingsValveModel StandSettingsValveModel;
        private readonly IStandController _standController;

        private int _valveNumber;

        public int ValveNumber
        {
            get => _valveNumber;
            set => SetProperty(ref _valveNumber, value);
        }

        private DeviceViewModel _device;
        private StateType _stateType = StateType.Open;

        public DeviceViewModel Device
        {
            get => _device;
            set => SetProperty(ref _device, value);
        }

        public StateType StateType
        {
            get => _stateType;
            set => SetProperty(ref _stateType, value);
        }

        public UserControl DeviceInformationView { get; set; }

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
                    await _standController.CloseValveAsync(StandSettingsValveModel);
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
                    await _standController.OpenValveAsync(StandSettingsValveModel);
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
}