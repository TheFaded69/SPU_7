using System;
using Avalonia.Media.Transformation;
using SPU_7.Common.Scripts;
using SPU_7.Models;
using SPU_7.Models.Scripts;
using SPU_7.Models.Scripts.Operations.Configurations;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels
{
    public class OperationViewModel : ViewModelBase, IObserver
    {
        public BaseOperationConfigurationModel ConfigurationModel { get; set; }
        public Guid Id { get; set; }
        public Guid ScriptId { get; set; }
        
        private OperationType _operationType;
        public OperationType OperationType
        {
            get => _operationType;
            set => SetProperty(ref _operationType, value);
        }
        
        private string _name;
        public string Name
        {
            get => _name; set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description; set => SetProperty(ref _description, value);
        }
        
        private OperationStatus _operationStatus;
        public OperationStatus OperationStatus
        {
            get => _operationStatus;
            set => SetProperty(ref _operationStatus, value);
        }
        
        private int _number;
        public int Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        public void Update(object obj)
        {
            if (obj is OperationStatus operationStatus)
            {
                OperationStatus = operationStatus;
            }
        }
    }
}
