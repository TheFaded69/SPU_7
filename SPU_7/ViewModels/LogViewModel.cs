using System.Collections.ObjectModel;
using SPU_7.Common.Stand;

namespace SPU_7.ViewModels;

public class LogViewModel : ViewModelBase
{
    private ObservableCollection<LogMessage> _logs;

    public LogViewModel()
    {
        
    }

    public ObservableCollection<LogMessage> Logs
    {
        get => _logs;
        set => SetProperty(ref _logs, value);
    }

    public string LogName { get; set; }
}