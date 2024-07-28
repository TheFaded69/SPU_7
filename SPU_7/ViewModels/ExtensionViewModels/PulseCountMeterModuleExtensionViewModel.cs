using System;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;

namespace SPU_7.ViewModels.ExtensionViewModels;

public class PulseCountMeterModuleExtensionViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    public PulseCountMeterModuleExtensionViewModel(IDialogService dialogService)
    {
        _dialogService = dialogService;

        OpenPulseCountMeterModuleExtensionCommand =
            new DelegateCommand(OpenPulseCountMeterModuleExtensionCommandHandler);
    }
    
    public DelegateCommand OpenPulseCountMeterModuleExtensionCommand { get; }

    private void OpenPulseCountMeterModuleExtensionCommandHandler()
    {
        PulseCountMeterModuleTestViewModel.Show(_dialogService, null,  null);   
    }
}