using System;
using Prism.Services.Dialogs;

namespace SPU_7.ViewModels;

public class AboutViewModel : ViewModelBase, IDialogAware
{
    public AboutViewModel()
    {
        Title = "О программе";
    }

    public bool CanCloseDialog() => true;
    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
    }

    public event Action<IDialogResult>? RequestClose;
}