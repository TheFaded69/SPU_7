using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Stand;
using SPU_7.Models.Scripts.Operations.Results;
using SPU_7.Models.Services.Logger;

namespace SPU_7.ViewModels.WorkReportViewModels.ResultViewModels.Validation;

public class ValidationResultViewModel : ViewModelBase
{
    public ValidationResultViewModel(ValidationOperationResult validationOperationResult, IDialogService dialogService, ILogger logger)
    {
        _validationOperationResult = validationOperationResult;
        _dialogService = dialogService;
        _logger = logger;
        VendorNumbers = new ObservableCollection<string>();

        foreach (var validationDeviceResult in validationOperationResult.ValidationPointResults
                     .SelectMany(validationPointResult => validationPointResult.ValidationMeasureResults,
                         (validationPointResult, validationMeasureResult) => new { validationPointResult, validationMeasureResult })
                     .SelectMany(t => t.validationMeasureResult.ValidationDeviceResults, (t, validationDeviceResult) => new { t, validationDeviceResult })
                     .Where(t => !VendorNumbers.Contains(t.validationDeviceResult.VendorNumber))
                     .Select(t => t.validationDeviceResult))
        {
            VendorNumbers.Add(validationDeviceResult.VendorNumber);
        }

        ShowProtocolViewerCommand = new DelegateCommand(ShowProtocolViewerCommandHandler);
    }
    private readonly ValidationOperationResult _validationOperationResult;
    private readonly IDialogService _dialogService;
    private readonly ILogger _logger;

    private string _selectedVendorNumber;
    private ObservableCollection<ValidationDeviceResultViewModel> _validationDeviceResultViewModels;
    private string _goodDevice;
    private string _deviceName;
    private string _vendorName;
    private int? _selectedIndex;
    private float? _temperature;
    private float? _pressure;
    private float? _humidity;

    public float? Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value);
    }

    public float? Humidity
    {
        get => _humidity;
        set => SetProperty(ref _humidity, value);
    }

    public float? Pressure
    {
        get => _pressure;
        set => SetProperty(ref _pressure, value);
    }

    public ObservableCollection<string> VendorNumbers { get; set; }

    public string SelectedVendorNumber
    {
        get => _selectedVendorNumber;
        set
        {
            SetProperty(ref _selectedVendorNumber, value);

            VendorName = _validationOperationResult.VendorName;
            DeviceName = _validationOperationResult.DeviceName;

            ValidationDeviceResultViewModels = new ObservableCollection<ValidationDeviceResultViewModel>();

            foreach (var validationPointResult in _validationOperationResult.ValidationPointResults)
            {
                foreach (var validationMeasureResult in validationPointResult.ValidationMeasureResults)
                {
                    foreach (var validationDeviceResult in validationMeasureResult.ValidationDeviceResults.Where(d => d.VendorNumber == value))
                    {
                        ValidationDeviceResultViewModels.Add(new ValidationDeviceResultViewModel()
                        {
                            PointNumber = validationDeviceResult.PointNumber,
                            MeasureNumber = validationDeviceResult.MeasureNumber,
                            TargetVolume = validationDeviceResult.TargetVolume,
                            StartVolumeValue = validationDeviceResult.StartVolumeValue,
                            EndVolumeValue = validationDeviceResult.EndVolumeValue,
                            TargetFlow = validationDeviceResult.TargetFlow,
                            TargetVolumeDifference = validationDeviceResult.TargetVolumeDifference,
                            ValidationVolumeTime = validationDeviceResult.ValidationVolumeTime,
                            CalculateFlow = validationDeviceResult.CalculateFlow,
                            FlowDifference = validationDeviceResult.FlowDifference,
                            VolumeDifference = validationDeviceResult.VolumeDifference,
                            ValidationFlowTime = validationDeviceResult.ValidationFlowTime
                        });
                    }
                }
            }
            
            GoodDevice = ValidationDeviceResultViewModels.Any(vd => Math.Abs((double)vd.VolumeDifference) > vd.TargetVolumeDifference) ? "Не годен" : "Годен";
        }
    }

    public ObservableCollection<ValidationDeviceResultViewModel> ValidationDeviceResultViewModels
    {
        get => _validationDeviceResultViewModels;
        set => SetProperty(ref _validationDeviceResultViewModels, value);
    }

    public string GoodDevice
    {
        get => _goodDevice;
        set => SetProperty(ref _goodDevice, value);
    }

    public string DeviceName
    {
        get => _deviceName;
        set => SetProperty(ref _deviceName, value);
    }

    public string VendorName
    {
        get => _vendorName;
        set => SetProperty(ref _vendorName, value);
    }
    
    public DelegateCommand ShowProtocolViewerCommand { get; }

    public int? SelectedIndex
    {
        get => _selectedIndex;
        set => SetProperty(ref _selectedIndex, value);
    }

    private void ShowProtocolViewerCommandHandler()
    {
        try
        {
            PdfViewerViewModel.Show(_dialogService, _validationOperationResult, (int)SelectedIndex, Temperature, Humidity, Pressure);

        }
        catch (Exception e)
        {
            _logger.Logging(new LogMessage(e.Message, LogLevel.Error));
        }
    }
}