using System;
using System.Threading.Tasks;
using Prism.Commands;
using SPU_7.Models.Services.ContentServices;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels
{
    public class NozzleItemViewModel : ViewModelBase
    {
        public NozzleItemViewModel(INotificationService notificationService,
            StandSettingsNozzleModel standSettingsNozzleModel,
            IStandController standController)
        {
            _notificationService = notificationService;
            _standSettingsNozzleModel = standSettingsNozzleModel;
            _standController = standController;

            _nozzleValue = standSettingsNozzleModel.NozzleValue;
            _nozzleFactValue = standSettingsNozzleModel.NozzleFactValue;

            UseNozzleCommand = new DelegateCommand(UseNozzleCommandHandler);
        }

        private readonly INotificationService _notificationService;
        private readonly StandSettingsNozzleModel _standSettingsNozzleModel;
        private readonly IStandController _standController;

        private int _nozzleNumber;
        private double? _nozzleValue;
        private double? _nozzleFactValue;
        private StateType _stateType = StateType.Open;

        public int NozzleNumber
        {
            get => _nozzleNumber;
            set => SetProperty(ref _nozzleNumber, value);
        }

        public double? NozzleValue
        {
            get => _nozzleValue;
            set => SetProperty(ref _nozzleValue, value);
        }

        public double? NozzleFactValue
        {
            get => _nozzleFactValue;
            set => SetProperty(ref _nozzleFactValue, value);
        }

        public StateType StateType
        {
            get => _stateType;
            set => SetProperty(ref _stateType, value);
        }

        public DelegateCommand UseNozzleCommand { get; }

        private async void UseNozzleCommandHandler()
        {
            switch (StateType)
            {
                case StateType.None:
                    break;
                case StateType.Open:
                    StateType = StateType.Work;
#if DEBUGGUI
                    await Task.Delay(5000);
#else
                    await _standController.CloseNozzleAsync(_standSettingsNozzleModel);
#endif
                    StateType = StateType.Close;
                    break;
                case StateType.Close:
                    StateType = StateType.Work;
#if DEBUGGUI
                    await Task.Delay(5000);
#else
                    await _standController.OpenNozzleAsync(_standSettingsNozzleModel);
#endif
                    StateType = StateType.Open;
                    break;
                case StateType.Work:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}