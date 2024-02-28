using System;
using System.Collections.Generic;
using Avalonia.Threading;
using Prism.Services.Dialogs;
using SPU_7.Common.Settings;
using SPU_7.Models.Scripts.Operations.Results.Extensions;
using SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationExecutingViewModels;

namespace SPU_7.Models.Services.ContentServices;

public class ManualOperationService : IManualOperationService
{
    private readonly IDialogService _dialogService;

    public ManualOperationService(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }
    
    public void ShowManualValidationResultDialog(Action<List<ValidationPointResult>> positiveAction, Action<List<ValidationPointResult>> negativeAction, List<ValidationPointResult> validationPointModels, ValidationType validationType, int deviceNumber)
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            ManualValidationResultViewModel.Show(_dialogService, positiveAction, negativeAction, validationPointModels, validationType, deviceNumber);
        });
    }
}