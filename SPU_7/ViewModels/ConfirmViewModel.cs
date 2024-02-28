using System;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Views;

namespace SPU_7.ViewModels
{
    public class ConfirmViewModel : ViewModelBase, IDialogAware
    {
        public ConfirmViewModel()
        {
            Title = "СПУ-5";

            PositiveConfirmCommand = new DelegateCommand(PositiveConfirmCommandHandler);
            NegativeConfirmCommand = new DelegateCommand(NegativeConfirmCommandHandler);
        }

        private string _message;
        public string Message
        {
            get => _message; set => SetProperty(ref _message, value);
        }

        public DelegateCommand PositiveConfirmCommand { get; }

        public void PositiveConfirmCommandHandler()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        public DelegateCommand NegativeConfirmCommand { get; }

        public void NegativeConfirmCommandHandler()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        #region Dialog

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("Message");
        }

        public static void Show(IDialogService dialogService, string message, Action positiveAction, Action negativeAction)
        {
            dialogService.ShowDialog(nameof(ConfirmView), 
                new DialogParameters { { "Message", message} }, 
                dialogResult =>
            {
                switch (dialogResult.Result)
                {
                    case ButtonResult.Abort:
                    case ButtonResult.Ignore:
                    case ButtonResult.Cancel:
                    case ButtonResult.None:
                    case ButtonResult.Retry:
                    case ButtonResult.Yes:
                        break;
                    case ButtonResult.No:
                        break;
                    case ButtonResult.OK:
                        positiveAction?.Invoke();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        #endregion
    }
}
