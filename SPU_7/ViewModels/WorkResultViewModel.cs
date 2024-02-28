using System.Collections.ObjectModel;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.ViewModels
{
    public class WorkResultViewModel : ViewModelBase
    {
        public WorkResultViewModel(ILogger logger, IStandController standController, IStandSettingsService standSettingsController)
        {
            LogsCollection = new ObservableCollection<LogViewModel>();
            LogsCollection.Add(new LogViewModel()
            {
                LogName = "Вывод",
                Logs = _logMessages
            });
            /*LogsCollection.Add(new LogViewModel()
            {
                LogName = "Порт стенда",
                Logs = _portLogMessages
            });*/
            /*LogsCollection.Add(new LogViewModel()
            {
                LogName = "Порт устройств",
                Logs = _devicePortLogMessages
            });*/
            
            logger.AddCollectionForLogging(_logMessages);
            standController.AddCollectionForPortLogging(_portLogMessages);
            //standController.AddCollectionForDevicePortLogging(_devicePortLogMessages);
        }
        
        private ObservableCollection<LogMessage> _logMessages = new();
        private ObservableCollection<LogMessage> _portLogMessages = new();
        private ObservableCollection<LogMessage> _devicePortLogMessages = new();
        
    
        public ObservableCollection<LogViewModel> LogsCollection { get; set; }
    }
}
