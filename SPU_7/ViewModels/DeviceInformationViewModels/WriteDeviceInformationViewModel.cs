using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Views;

namespace SPU_7.ViewModels.DeviceInformationViewModels;

public class WriteDeviceInformationViewModel : ViewModelBase, IDialogAware
{
    public WriteDeviceInformationViewModel(IStandController standController, IStandSettingsService standSettingsService,
        IDeviceNameDbService deviceNameDbService,
        IDialogService dialogService)
    {
        Title = "Устройства";

        _standController = standController;
        _standSettingsService = standSettingsService;
        _deviceNameDbService = deviceNameDbService;
        _dialogService = dialogService;

        WriteDeviceInformationCommand = new DelegateCommand(WriteDeviceInformationCommandHandler);
        OpenDeviceNamesEditorCommand = new DelegateCommand(OpenDeviceNamesEditorCommandHandler);
        ClearDeviceInfoCommand = new DelegateCommand(ClearDeviceInfoCommandHandler);
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);

    }

    private readonly IStandController _standController;
    private readonly IStandSettingsService _standSettingsService;
    private readonly IDeviceNameDbService _deviceNameDbService;
    private readonly IDialogService _dialogService;

    public ObservableCollection<DeviceAboutViewModel> DeviceInformationViewModels { get; set; } = [];

    public DelegateCommand WriteDeviceInformationCommand { get; set; }

    private void WriteDeviceInformationCommandHandler()
    {
        foreach (var deviceInformationViewModel in DeviceInformationViewModels)
        {
            _standController.UpdateDeviceInformation(deviceInformationViewModel);
        }

        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }
    
    public DelegateCommand OpenDeviceNamesEditorCommand { get; }
    private void OpenDeviceNamesEditorCommandHandler()
    {
        DeviceNamesViewModel.Show(_dialogService, UpdateDeviceInformationViewModel, null);
    }
    
    public DelegateCommand ClearDeviceInfoCommand { get; }

    private void ClearDeviceInfoCommandHandler()
    {
        foreach (var deviceInformationViewModel in DeviceInformationViewModels)
        {
            deviceInformationViewModel.VendorName = string.Empty;
            deviceInformationViewModel.DeviceVendorNumber = string.Empty;
            deviceInformationViewModel.IsManualEnabled = false;
            deviceInformationViewModel.IsTemperatureCorrect = false;
            deviceInformationViewModel.SelectedDeviceTypeInfo = null;
        }
    }
    
    private void UpdateDeviceInformationViewModel()
    {
        var deviceNames = new ObservableCollection<DeviceNameViewModel>(_deviceNameDbService.GetDeviceNames());
        
        foreach (var deviceInformationViewModel in DeviceInformationViewModels)
        {
            deviceInformationViewModel.DeviceTypesInfo = deviceNames;
        }
    }

    public DelegateCommand CloseWindowCommand { get; }

    private void CloseWindowCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }
    
    public bool CanCloseDialog() => true;

    public void OnDialogClosed()
    {
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        var lineIndex = parameters.GetValue<int?>("LineIndex");
        var deviceNames = new ObservableCollection<DeviceNameViewModel>(_deviceNameDbService.GetDeviceNames());
        
        if (lineIndex == null) return;
        
        for (var i = 0; i < _standSettingsService.StandSettingsModel.LineViewModels[(int)lineIndex].DeviceViewModels.Count; i++)
        {
            DeviceInformationViewModels.Add(new DeviceAboutViewModel()
            {
                DeviceNumber = DeviceInformationViewModels.Count + 1,
                DeviceVendorNumber = _standController.GetVendorNumber(i, (int)lineIndex),
                DeviceName = _standController.GetDeviceName(i, (int)lineIndex),
                IsManualEnabled = _standController.GetDeviceManualEnable((int)lineIndex, i),
                IsTemperatureCorrect = _standController.GetTemperatureCorrect((int)lineIndex, i),
                DeviceTypesInfo = new ObservableCollection<DeviceNameViewModel>(deviceNames),
                VendorName = _standController.GetVendorName((int)lineIndex,i),
                SelectedDeviceTypeInfo = deviceNames
                    .FirstOrDefault(name => name.DeviceTypeInfo == _standController.GetDeviceInfoType((int)lineIndex,i).DeviceTypeInfo),
            });
        }
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, int? lineIndex, Action positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(WriteDeviceInformationView), new DialogParameters(){{"LineIndex", lineIndex}}, result =>
        {
            switch (result.Result)
            {
                case ButtonResult.Abort:
                    break;
                case ButtonResult.Cancel:
                    break;
                case ButtonResult.Ignore:
                    break;
                case ButtonResult.No:
                    negativeAction?.Invoke();
                    break;
                case ButtonResult.None:
                    break;
                case ButtonResult.OK:
                    positiveAction?.Invoke();
                    break;
                case ButtonResult.Retry:
                    break;
                case ButtonResult.Yes:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        });
    }
}