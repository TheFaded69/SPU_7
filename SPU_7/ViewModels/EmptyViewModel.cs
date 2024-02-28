namespace SPU_7.ViewModels;

public class EmptyViewModel : ViewModelBase
{
    private string _infoMessage;

    public EmptyViewModel(string infoMessage)
    {
        InfoMessage = infoMessage;
    }

    public string InfoMessage
    {
        get => _infoMessage;
        set => SetProperty(ref _infoMessage, value);
    }
}