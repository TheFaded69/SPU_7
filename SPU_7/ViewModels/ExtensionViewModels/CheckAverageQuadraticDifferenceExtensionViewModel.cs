using Prism.Commands;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class CheckAverageQuadraticDifferenceExtensionViewModel : ViewModelBase
{
    public CheckAverageQuadraticDifferenceExtensionViewModel()
    {
        
    }  
    
    public DelegateCommand OpenCheckAverageQuadraticDifferenceCommand { get; }

    private void OpenCheckAverageQuadraticDifferenceCommandHandler()
    {
        CheckAverageQuadraticDifferenceViewModel.Show();
    }
}