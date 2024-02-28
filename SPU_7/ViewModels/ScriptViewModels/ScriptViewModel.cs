using System;
using System.Collections.ObjectModel;
using SPU_7.Common.Device;
using SPU_7.Common.Stand;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels;

namespace SPU_7.ViewModels.ScriptViewModels
{
    public class ScriptViewModel : ViewModelBase
    {
        public ObservableCollection<OperationViewModel> Operations { get; set; } = new();
        public Guid Id { get; set; }
    
        private StandType _targetStandType;
        public StandType TargetStandType 
        { 
            get => _targetStandType; set => SetProperty(ref _targetStandType, value); 
        }

        private string _name;
        public string Name
        {
            get => _name; set => SetProperty(ref _name, value);
        }

        private DeviceType _deviceType;
        public DeviceType DeviceType
        {
            get => _deviceType; set => SetProperty(ref _deviceType, value);
        }

        private string _description;

        public string Description
        {
            get => _description; set => SetProperty(ref _description, value);
        }

        private int _lineNumber;

        public int LineNumber
        {
            get => _lineNumber;
            set => SetProperty(ref _lineNumber, value);
        }
    }
}
