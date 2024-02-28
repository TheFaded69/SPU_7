using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Settings;
using SPU_7.Models.Scripts.Operations.Results.Extensions;
using SPU_7.Views.ScriptViews.OperationResultViews;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationExecutingViewModels;

public class ManualValidationResultViewModel : ViewModelBase, IDialogAware 
{

    public ManualValidationResultViewModel()
    {
        OkCommand = new DelegateCommand(OkCommandHandler);
    }

    private List<ValidationPointResult> _validationPointResults;
    private int _deviceNumber;
    private ValidationType _validationType;

    public ObservableCollection<ValidationResultViewModel> ResultViewModels { get; set; } = new();

    public int DeviceNumber
    {
        get => _deviceNumber;
        set => SetProperty(ref _deviceNumber, value);
    }
    
    public ValidationType ValidationType
    {
        get => _validationType;
        set => SetProperty(ref _validationType, value);
    }

    
    #region Dialog
    
    public DelegateCommand OkCommand { get; }

    private void OkCommandHandler()
    {
        foreach (var resultViewModel in ResultViewModels)
        {
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .PointNumber = resultViewModel.PointNumber;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .MeasureNumber = resultViewModel.MeasureNumber;

            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .ValidationVolumeTime = resultViewModel.ValidationVolumeTime;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber- 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .TargetVolume = resultViewModel.TargetVolume;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .VolumeDifference = resultViewModel.VolumeDifference;
            
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .StartVolumeValue = resultViewModel.StartVolumeValue;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .EndVolumeValue = resultViewModel.EndVolumeValue;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .ValidationFlowTime = resultViewModel.ValidationFlowTime;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .TargetFlow = resultViewModel.TargetFlow;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .CalculateFlow = resultViewModel.CalculateFlow;
            _validationPointResults[resultViewModel.PointNumber - 1]
                .ValidationMeasureResults[resultViewModel.MeasureNumber - 1]
                .ValidationDeviceResults[_deviceNumber - 1]
                .FlowDifference = resultViewModel.FlowDifference;
        }
        
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK, new DialogParameters(){{"ValidationPointResults", _validationPointResults}}));
    }
    
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        _validationPointResults = parameters.GetValue<List<ValidationPointResult>>("ValidationPointResults");

        ValidationType = parameters.GetValue<ValidationType>("ValidationType");
        DeviceNumber = parameters.GetValue<int>("DeviceNumber") + 1;
        
        if (_validationPointResults == null) return;

        foreach (var deviceResult in _validationPointResults
                     .SelectMany(validationPointResult => validationPointResult.ValidationMeasureResults
                         .Select(validationMeasureResult => validationMeasureResult.ValidationDeviceResults[_deviceNumber - 1])))
        {
            ResultViewModels.Add(new ValidationResultViewModel()
            {
                TargetFlow = deviceResult.TargetFlow,
                TargetVolume = deviceResult.TargetVolume,
                CalculateFlow = deviceResult.CalculateFlow,
                FlowDifference = deviceResult.FlowDifference,
                MeasureNumber = deviceResult.MeasureNumber,
                PointNumber = deviceResult.PointNumber,
                ValidationFlowTime = deviceResult.ValidationFlowTime,
                ValidationVolumeTime = deviceResult.ValidationVolumeTime,
                VolumeDifference = deviceResult.VolumeDifference,
                EndVolumeValue = deviceResult.EndVolumeValue,
                StartVolumeValue = deviceResult.StartVolumeValue
            });
        }
    }

    public event Action<IDialogResult> RequestClose;

    public static void Show(IDialogService dialogService, Action<List<ValidationPointResult>> positiveAction, Action<List<ValidationPointResult>> negativeAction, List<ValidationPointResult> validationPointResults, ValidationType validationType, int deviceNumber)
    {
        dialogService.ShowDialog(nameof(ManualValidationResultView), 
            new DialogParameters{{ "ValidationPointResults", validationPointResults}, {"ValidationType", validationType}, {"DeviceNumber", deviceNumber}}, 
            result =>
        {
            switch (result.Result)
            
            {
                case ButtonResult.Retry:
                case ButtonResult.Yes:
                case ButtonResult.Ignore:
                case ButtonResult.No:
                case ButtonResult.None:
                case ButtonResult.Abort:
                case ButtonResult.Cancel:
                    negativeAction?.Invoke(result.Parameters.GetValue<List<ValidationPointResult>>("ValidationPointResults"));
                    break;
                case ButtonResult.OK:
                    positiveAction?.Invoke(result.Parameters.GetValue<List<ValidationPointResult>>("ValidationPointResults"));
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }
    
    #endregion
}