using System;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class CheckAverageQuadraticDifferenceViewModel : ViewModelBase, IDialogAware
{
    private readonly IStandController _standController;
    private readonly IStandSettingsService _settingsService;

    public CheckAverageQuadraticDifferenceViewModel(IStandController standController, IStandSettingsService settingsService)
    {
        _standController = standController;
        _settingsService = settingsService;
    }
    
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show()
    {
        
    }
}