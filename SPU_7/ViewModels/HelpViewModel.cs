using System;
using System.Collections.ObjectModel;
using Prism.Services.Dialogs;
using SPU_7.Models.HelpModel;

namespace SPU_7.ViewModels
{
    public class HelpViewModel : ViewModelBase, IDialogAware
    {
        public HelpViewModel()
        {
            Title = "Справка";
        }


        public ObservableCollection<HelpModel> HelpModels { get; }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
