using Prism.Commands;
using Prism.Services.Dialogs;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class CheckAverageQuadraticDifferenceExtensionViewModel : ViewModelBase
{
    public CheckAverageQuadraticDifferenceExtensionViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;

        OpenCheckAverageQuadraticDifferenceCommand = new DelegateCommand(OpenCheckAverageQuadraticDifferenceCommandHandler);
    }  
    private readonly IDialogService _dialogService;
    
    public DelegateCommand OpenCheckAverageQuadraticDifferenceCommand { get; }
    private void OpenCheckAverageQuadraticDifferenceCommandHandler()
    {
        CheckAverageQuadraticDifferenceViewModel.Show(_dialogService, null, null);
    }
}