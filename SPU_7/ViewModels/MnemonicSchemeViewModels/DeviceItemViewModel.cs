using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Prism.Commands;
using SPU_7.Domain.Extensions;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class DeviceItemViewModel : ViewModelBase, IPressureSensorObserver, IDeviceObserver
{
    public DeviceItemViewModel(IStandController standController, IStandSettingsService settingsService, int deviceIndex, int lineIndex)
    {
        _standController = standController;
        _settingsService = settingsService;
        _deviceIndex = deviceIndex;

        standController.RegisterPressureSensorObserver(this, deviceIndex, lineIndex);
        standController.RegisterDeviceObserver(this, deviceIndex, lineIndex);

        var valveSettings = new StandSettingsValveModel
        {
            Number = deviceIndex + 1,
            Address = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels[deviceIndex].Address,
            RegisterAddress = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels[deviceIndex].RegisterAddress,
            BitNumber = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels[deviceIndex].BitNumber,
            StateAddress = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels[deviceIndex].StateAddress,
            StateRegisterAddress = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels[deviceIndex].StateRegisterAddress,
            StateBitNumber = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels[deviceIndex].StateBitNumber,
            IsControlState = settingsService.StandSettingsModel.LineViewModels[lineIndex].DeviceViewModels[deviceIndex].IsControlState,
            LineNumber = lineIndex + 1,
        };
        ValveItemViewModel = new ValveItemViewModel(valveSettings, standController);
        UseValveCommand = new DelegateCommand(UseValveCommandHandler);
    }

    private readonly IStandController _standController;
    private readonly IStandSettingsService _settingsService;
    private readonly int _deviceIndex;

    private string _vendorNumber;
    private string _deviceName;
    private UserControl _deviceView;
    private float? _pressure;
    private bool _isDeviceEnable;
    private ValveItemViewModel _valveItemViewModel;

    public string VendorNumber
    {
        get => string.IsNullOrEmpty(_vendorNumber) ? "№________" : "№" + _vendorNumber;
        set => SetProperty(ref _vendorNumber, value);
    }

    public string DeviceName
    {
        get => _deviceName;
        set => SetProperty(ref _deviceName, value);
    }

    public UserControl DeviceView
    {
        get => _deviceView;
        set => SetProperty(ref _deviceView, value);
    }

    public float? Pressure
    {
        get => _pressure;
        set => SetProperty(ref _pressure, value == null ? value : (float?)Math.Round((float)value / 1000, 3));
    }

    public bool IsDeviceEnable
    {
        get => _isDeviceEnable;
        set => SetProperty(ref _isDeviceEnable, value);
    }

    public ValveItemViewModel ValveItemViewModel
    {
        get => _valveItemViewModel;
        set => SetProperty(ref _valveItemViewModel, value);
    }

    public void UpdatePressure(object? obj)
    {
        switch (obj)
        {
            case null:
                return;
            case float pressure:
                Pressure = pressure;
                break;
        }
    }

    public void UpdateDeviceInformation(object? obj)
    {
        switch (obj)
        {
            case string vendorNumber:
                VendorNumber = vendorNumber;
                break;
            case bool manualEnabled:
                IsDeviceEnable = manualEnabled;
                break;
        }
    }
    
    public DelegateCommand UseValveCommand { get; }

    private async void UseValveCommandHandler()
    {
        switch (ValveItemViewModel.StateType)
        {
            case StateType.None:
                break;
            case StateType.Open:
            {
                ValveItemViewModel.StateType = StateType.Work;
#if DEBUGGUI
                await Task.Delay(5000);
#else
                    await _standController.CloseDeviceValveAsync(ValveItemViewModel.StandSettingsValveModel);
#endif
                ValveItemViewModel.StateType = StateType.Close;
            }
                break;
            case StateType.Close:
            {
                ValveItemViewModel.StateType = StateType.Work;
#if DEBUGGUI
                await Task.Delay(5000);
#else
                    await _standController.OpenDeviceValveAsync(ValveItemViewModel.StandSettingsValveModel);
#endif
                ValveItemViewModel.StateType = StateType.Open;
            }
                break;
            case StateType.Work:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}