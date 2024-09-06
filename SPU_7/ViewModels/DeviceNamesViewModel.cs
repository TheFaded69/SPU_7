using System;
using System.Collections.ObjectModel;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Models.Services.DbServices;
using SPU_7.Views;

namespace SPU_7.ViewModels;

public class DeviceNamesViewModel : ViewModelBase, IDialogAware
{
    private readonly IDeviceNameDbService _deviceNameDbService;

    public DeviceNamesViewModel(IDeviceNameDbService deviceNameDbService)
    {
        _deviceNameDbService = deviceNameDbService;

        OkCommand = new DelegateCommand(OkCommandHandler);
        AddNameCommand = new DelegateCommand(AddNameCommandHandler);
        RemoveNameCommand = new DelegateCommand(RemoveNameCommandHandler);

        Title = "Редактор типов СГ";
    }

    private ObservableCollection<DeviceNameViewModel> _deviceNames;
    private DeviceNameViewModel _selectedDeviceName;

    public ObservableCollection<DeviceNameViewModel> DeviceNames
    {
        get => _deviceNames;
        set => SetProperty(ref _deviceNames, value);
    }

    public DeviceNameViewModel SelectedDeviceName
    {
        get => _selectedDeviceName;
        set => SetProperty(ref _selectedDeviceName, value);
    }

    public DelegateCommand AddNameCommand { get; }

    private void AddNameCommandHandler()
    {
        DeviceNames.Add(new DeviceNameViewModel());
    }
    
    public DelegateCommand RemoveNameCommand { get; }

    private void RemoveNameCommandHandler()
    {
        DeviceNames.Remove(SelectedDeviceName);
    }
    
    public DelegateCommand OkCommand { get; }

    private void OkCommandHandler()
    {
        _deviceNameDbService.UpdateDeviceNames(DeviceNames);
        
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }
    
    public bool CanCloseDialog()
    {
        return true;
    }

    public void OnDialogClosed()
    {
        
    }

    public void OnDialogOpened(IDialogParameters parameters)
    {
        DeviceNames = new ObservableCollection<DeviceNameViewModel>(_deviceNameDbService.GetDeviceNames());
    }

    public event Action<IDialogResult>? RequestClose;

    public static void Show(IDialogService dialogService, Action positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(DeviceNamesView), null, result =>
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