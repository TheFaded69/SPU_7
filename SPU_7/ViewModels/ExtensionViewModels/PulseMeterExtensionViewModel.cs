using Prism.Commands;
using Prism.Services.Dialogs;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class PulseMeterExtensionViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    public PulseMeterExtensionViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;

        OpenPulseMeterEditorCommand = new DelegateCommand(OpenPulseMeterEditorCommandHandler);
    }
    
    public DelegateCommand OpenPulseMeterEditorCommand { get; }

    private void OpenPulseMeterEditorCommandHandler()
    {
        PulseMeterCoefficientSettingsViewModel.Show(_dialogService, null, null);
    }
}