using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Views;

namespace SPU_7.ViewModels.DeviceInformationViewModels;

public class WriteDeviceInformationViewModel : ViewModelBase, IDialogAware
{
    public WriteDeviceInformationViewModel(IStandController standController, IStandSettingsService standSettingsService)
    {
        Title = "Устройства";

        _standController = standController;
        _standSettingsService = standSettingsService;

        WriteDeviceInformationCommand = new DelegateCommand(WriteDeviceInformationCommandHandler);
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);

    }

    private readonly IStandController _standController;
    private readonly IStandSettingsService _standSettingsService;

    public ObservableCollection<DeviceAboutViewModel> DeviceInformationViewModels { get; set; } = new();

    public DelegateCommand WriteDeviceInformationCommand { get; set; }

    private void WriteDeviceInformationCommandHandler()
    {
        foreach (var deviceInformationViewModel in DeviceInformationViewModels)
        {
            _standController.UpdateDeviceInformation(deviceInformationViewModel);
        }

        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
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

        if (lineIndex == null) return;
        
        for (var i = 0; i < _standSettingsService.StandSettingsModel.LineViewModels[(int)lineIndex].DeviceViewModels.Count; i++)
        {
            DeviceInformationViewModels.Add(new DeviceAboutViewModel()
            {
                DeviceNumber = DeviceInformationViewModels.Count + 1,
                DeviceVendorNumber = _standController.GetVendorNumber(i),
                DeviceName = _standController.GetDeviceName(i),
                IsManualEnabled = _standController.GetDeviceManualEnable(i),
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