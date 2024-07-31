using Prism.Commands;
using Prism.Services.Dialogs;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class CheckTightnessExtensionViewModel
{
    private readonly IDialogService _dialogService;

    public CheckTightnessExtensionViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;

        OpenCheckTightnessWindowCommand = new DelegateCommand(OpenCheckTightnessWindowCommandHandler);
    }

    public DelegateCommand OpenCheckTightnessWindowCommand { get; }

    private void OpenCheckTightnessWindowCommandHandler()
    {
        CheckTightnessViewModel.Show(_dialogService);
    }
}