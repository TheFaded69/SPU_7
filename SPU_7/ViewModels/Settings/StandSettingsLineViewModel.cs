using System;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Commands;
using SPU_7.Common.Extensions;
using SPU_7.Common.Line;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsLineViewModel : ViewModelBase
{
    public StandSettingsLineViewModel()
    {
        DeviceViewModels = new ObservableCollection<StandSettingsDeviceViewModel>();

        StringDeviceLineTypes = new ObservableCollection<string>(Enum
            .GetValues<DeviceLineType>()
            .Select(dt => dt.GetDescription()));

        AddDeviceCommand = new DelegateCommand(AddDeviceCommandHandler);
        RemoveDeviceCommand = new DelegateCommand(RemoveDeviceCommandHandler);
    }

    private int _lineNumber;
    private StandSettingsDeviceViewModel _selectedDeviceViewModel;
    private ObservableCollection<StandSettingsDeviceViewModel> _deviceViewModels;
    private string _selectedStringDeviceLineType;
    private bool _isReverseLine;

    public int LineNumber
    {
        get => _lineNumber;
        set => SetProperty(ref _lineNumber, value);
    }

    public ObservableCollection<StandSettingsDeviceViewModel> DeviceViewModels
    {
        get => _deviceViewModels;
        set => SetProperty(ref _deviceViewModels, value);
    }

    public StandSettingsDeviceViewModel SelectedDeviceViewModel
    {
        get => _selectedDeviceViewModel;
        set => SetProperty(ref _selectedDeviceViewModel, value);
    }

    public ObservableCollection<string> StringDeviceLineTypes { get; set; }

    public string SelectedStringDeviceLineType
    {
        get => _selectedStringDeviceLineType;
        set
        {
            SetProperty(ref _selectedStringDeviceLineType, value);
            SelectedDeviceLineType = Enum
                .GetValues<DeviceLineType>()
                .FirstOrDefault(dt => dt.GetDescription() == value);

            foreach (var deviceViewModel in DeviceViewModels)
            {
                deviceViewModel.IsValveEnable = SelectedDeviceLineType == DeviceLineType.JetDevice;
            }
        }
    }

    public DeviceLineType SelectedDeviceLineType { get; set; }

    public bool IsReverseLine
    {
        get => _isReverseLine;
        set => SetProperty(ref _isReverseLine, value);
    }

    public StandSettingsValveViewModel DirectValveViewModel { get; set; } = new();
    public StandSettingsValveViewModel ReverseValveViewModel { get; set; } = new();
    public DelegateCommand AddDeviceCommand { get; set; }

    private void AddDeviceCommandHandler()
    {
        DeviceViewModels.Add(new StandSettingsDeviceViewModel() { Number = DeviceViewModels.Count + 1, IsValveEnable = !IsReverseLine });
    }

    public DelegateCommand RemoveDeviceCommand { get; }

    private void RemoveDeviceCommandHandler()
    {
        DeviceViewModels.Remove(SelectedDeviceViewModel);
    }
}