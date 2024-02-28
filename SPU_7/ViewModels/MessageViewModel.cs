using System;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Views;

namespace SPU_7.ViewModels
{
    public class MessageViewModel : ViewModelBase, IDialogAware
    {
        public MessageViewModel()
        {
            Title = "СПУ-5";

            CloseErrorMessageCommand = new DelegateCommand(CloseErrorMessageCommandHandler);
        }

        private string _message;
        public string Message
        {
            get => _message; set => SetProperty(ref _message, value);
        }

        public DelegateCommand CloseErrorMessageCommand { get; }

        public void CloseErrorMessageCommandHandler()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
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
            dialogService.ShowDialog(nameof(MessageView), 
                new DialogParameters { { "Message", message} }, 
                dialogResult =>
            {
                switch (dialogResult.Result)
                {
                    case ButtonResult.Abort:
                    case ButtonResult.Ignore:
                    case ButtonResult.Cancel:
                    case ButtonResult.None:
                    case ButtonResult.OK:
                    case ButtonResult.Retry:
                    case ButtonResult.Yes:
                        break;
                    case ButtonResult.No:
                        negativeAction?.Invoke();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }

        #endregion
    }
}
