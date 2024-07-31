using System;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Extensions;
using SPU_7.Domain.Extensions;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.MnemonicSchemeViewModels;

public class MasterDeviceItemViewModel : ViewModelBase, IPressureSensorObserver, ITemperatureSensorObserver
{
    public MasterDeviceItemViewModel(IDialogService dialogService, 
        IStandController standController, 
        StandSettingsValveModel valveViewModel, 
        StandSettingsValveModel pressureValveViewModel,
        StandSettingsMasterDeviceModel masterDeviceModel)
    {
        _dialogService = dialogService;
        _standController = standController;
        _masterDeviceModel = masterDeviceModel;

        ValveItemViewModel = new ValveItemViewModel(valveViewModel, standController, StateType.Open);
        PressureValveItemViewModel = new ValveItemViewModel(pressureValveViewModel, standController, StateType.Close);
        DeviceName = masterDeviceModel.MasterDeviceName;
        VendorNumber = "№" + masterDeviceModel.VendorNumber;
        
        OpenMasterDeviceInfoCommand = new DelegateCommand(OpenMasterDeviceInfoCommandHandler);
    }

    private readonly IDialogService _dialogService;
    private readonly IStandController _standController;
    private readonly StandSettingsMasterDeviceModel _masterDeviceModel;

    private StateType _stateType;
    private float? _pressure;
    private float? _temperature;
    private ValveItemViewModel _valveItemViewModel;
    private ValveItemViewModel _pressureValveItemViewModel;

    public string DeviceName { get; set; }
    
    public ValveItemViewModel ValveItemViewModel
    {
        get => _valveItemViewModel;
        set => SetProperty(ref _valveItemViewModel, value);
    }

    public ValveItemViewModel PressureValveItemViewModel
    {
        get => _pressureValveItemViewModel;
        set => SetProperty(ref _pressureValveItemViewModel, value);
    }

    public float? Pressure
    {
        get => _pressure;
        set => SetProperty(ref _pressure, value == null ? value : (float?)Math.Round((float)value / 1000, 3));
    }

    public float? Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value == null ? value : (float?)Math.Round((float)value, 2));
    }

    public StateType StateType
    {
        get => _stateType;
        set => SetProperty(ref _stateType, value);
    }
    
    public string VendorNumber { get; set; }
    
    public DelegateCommand OpenMasterDeviceInfoCommand { get; }

    private void OpenMasterDeviceInfoCommandHandler()
    {
        MasterDeviceInfoViewModel.Show(_dialogService, _masterDeviceModel, null, null);
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

    public void UpdateTemperature(object? obj)
    {
        switch (obj)
        {
            case null:
                return;
            case float temperature:
                Temperature = temperature;
                break;
        }
    }
}