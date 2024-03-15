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
        MasterDeviceViewModels = new ObservableCollection<StandSettingsMasterDeviceViewModel>();
        FanViewModels = new ObservableCollection<StandSettingsFanViewModel>();
        VacuumValveViewModels = new ObservableCollection<StandSettingsVacuumValveViewModel>();
        NozzleViewModels = new ObservableCollection<StandSettingsNozzleViewModel>();
        
        StringDeviceLineTypes = new ObservableCollection<string>(Enum
            .GetValues<DeviceLineType>()
            .Select(dt => dt.GetDescription()));
        StringLineTypes = new ObservableCollection<string>(Enum
            .GetValues<LineType>()
            .Where(lt => lt != LineType.None)
            .Select(dt => dt.GetDescription()));

        AddDeviceCommand = new DelegateCommand(AddDeviceCommandHandler);
        RemoveDeviceCommand = new DelegateCommand(RemoveDeviceCommandHandler);
        AddMasterDeviceCommand = new DelegateCommand(AddMasterDeviceCommandHandler);
        RemoveMasterDeviceCommand = new DelegateCommand(RemoveMasterDeviceCommandHandler);
        AddNozzleCommand = new DelegateCommand(AddNozzleCommandHandler);
        RemoveNozzleCommand = new DelegateCommand(RemoveNozzleCommandHandler);
        AddFanCommand = new DelegateCommand(AddFanCommandHandler);
        RemoveFanCommand = new DelegateCommand(RemoveFanCommandHandler);
        AddVacuumValveCommand = new DelegateCommand(AddVacuumValveCommandHandler);
        RemoveVacuumValveCommand = new DelegateCommand(RemoveVacuumValveCommandHandler);
    }

    private int _lineNumber;
    private StandSettingsDeviceViewModel _selectedDeviceViewModel;
    private ObservableCollection<StandSettingsDeviceViewModel> _deviceViewModels;
    private string _selectedStringDeviceLineType;
    private bool _isReverseLine;
    private ObservableCollection<string> _stringDeviceLineTypes;
    private ObservableCollection<StandSettingsMasterDeviceViewModel> _masterDeviceViewModels;
    private StandSettingsMasterDeviceViewModel _selectedMasterDeviceViewModel;
    private ObservableCollection<StandSettingsNozzleViewModel> _nozzleViewModels;
    private StandSettingsNozzleViewModel _selectedNozzleViewModel;
    private ObservableCollection<StandSettingsFanViewModel> _fanViewModels;
    private StandSettingsFanViewModel _selectedFanViewModel;
    private ObservableCollection<StandSettingsVacuumValveViewModel> _vacuumValveViewModels;
    private StandSettingsVacuumValveViewModel _selectedVacuumValveViewModel;
    private ObservableCollection<string> _stringLineTypes;
    private string _selectedStringLineType;
    private bool _isStartValveMasterDevice;
    private bool _isEndValveMasterDevice;
    private StandSettingsValveViewModel _startValveMasterDeviceViewModel;
    private StandSettingsValveViewModel _endValveMasterDeviceViewModel;
    private bool _isStartCommonValve;
    private bool _isEndCommonValve;
    private StandSettingsValveViewModel _startCommonValveViewModel;
    private StandSettingsValveViewModel _endCommonValveViewModel;

    public int LineNumber
    {
        get => _lineNumber;
        set => SetProperty(ref _lineNumber, value);
    }

    public bool IsStartCommonValve
    {
        get => _isStartCommonValve;
        set
        {
            SetProperty(ref _isStartCommonValve, value);
            StartCommonValveViewModel = new StandSettingsValveViewModel();
        }
    }

    public bool IsEndCommonValve
    {
        get => _isEndCommonValve;
        set
        {
            SetProperty(ref _isEndCommonValve, value);
            EndCommonValveViewModel = new StandSettingsValveViewModel();
        }
    }

    public StandSettingsValveViewModel StartCommonValveViewModel
    {
        get => _startCommonValveViewModel;
        set => SetProperty(ref _startCommonValveViewModel, value);
    }

    public StandSettingsValveViewModel EndCommonValveViewModel
    {
        get => _endCommonValveViewModel;
        set => SetProperty(ref _endCommonValveViewModel, value);
    }

    public bool IsStartValveMasterDevice
    {
        get => _isStartValveMasterDevice;
        set
        {
            SetProperty(ref _isStartValveMasterDevice, value);
            StartValveMasterDeviceViewModel = new StandSettingsValveViewModel();
        }
    }

    public bool IsEndValveMasterDevice
    {
        get => _isEndValveMasterDevice;
        set
        {
            SetProperty(ref _isEndValveMasterDevice, value);
            EndValveMasterDeviceViewModel = new StandSettingsValveViewModel();
        }
    }

    public StandSettingsValveViewModel StartValveMasterDeviceViewModel
    {
        get => _startValveMasterDeviceViewModel;
        set => SetProperty(ref _startValveMasterDeviceViewModel, value);
    }

    public StandSettingsValveViewModel EndValveMasterDeviceViewModel
    {
        get => _endValveMasterDeviceViewModel;
        set => SetProperty(ref _endValveMasterDeviceViewModel, value);
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


    public ObservableCollection<string> StringLineTypes
    {
        get => _stringLineTypes;
        set => SetProperty(ref _stringLineTypes, value);
    }

    public string SelectedStringLineType
    {
        get => _selectedStringLineType;
        set
        {
            SetProperty(ref _selectedStringLineType, value);
            SelectedLineType = Enum.GetValues<LineType>()
                .FirstOrDefault(lt => lt.GetDescription() == value);
        }
    }

    public LineType SelectedLineType { get; set; }
    
    public ObservableCollection<string> StringDeviceLineTypes
    {
        get => _stringDeviceLineTypes;
        set => SetProperty(ref _stringDeviceLineTypes, value);
    }

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
    
    public ObservableCollection<StandSettingsMasterDeviceViewModel> MasterDeviceViewModels
    {
        get => _masterDeviceViewModels;
        set => SetProperty(ref _masterDeviceViewModels, value);
    }

    public StandSettingsMasterDeviceViewModel SelectedMasterDeviceViewModel
    {
        get => _selectedMasterDeviceViewModel;
        set => SetProperty(ref _selectedMasterDeviceViewModel, value);
    }

    public ObservableCollection<StandSettingsNozzleViewModel> NozzleViewModels
    {
        get => _nozzleViewModels;
        set => SetProperty(ref _nozzleViewModels, value);
    }

    public StandSettingsNozzleViewModel SelectedNozzleViewModel
    {
        get => _selectedNozzleViewModel;
        set => SetProperty(ref _selectedNozzleViewModel, value);
    }

    public ObservableCollection<StandSettingsFanViewModel> FanViewModels
    {
        get => _fanViewModels;
        set => SetProperty(ref _fanViewModels, value);
    }

    public StandSettingsFanViewModel SelectedFanViewModel
    {
        get => _selectedFanViewModel;
        set => SetProperty(ref _selectedFanViewModel, value);
    }

    public ObservableCollection<StandSettingsVacuumValveViewModel> VacuumValveViewModels
    {
        get => _vacuumValveViewModels;
        set => SetProperty(ref _vacuumValveViewModels, value);
    }

    public StandSettingsVacuumValveViewModel SelectedVacuumValveViewModel
    {
        get => _selectedVacuumValveViewModel;
        set => SetProperty(ref _selectedVacuumValveViewModel, value);
    }


    public bool IsReverseLine
    {
        get => _isReverseLine;
        set => SetProperty(ref _isReverseLine, value);
    }
    
    public DelegateCommand AddDeviceCommand { get; set; }
    private void AddDeviceCommandHandler()
    {
        DeviceViewModels.Add(new StandSettingsDeviceViewModel()
        {
            Number = DeviceViewModels.Count + 1, 
            IsValveEnable = !IsReverseLine
        });
    }

    public DelegateCommand RemoveDeviceCommand { get; }
    private void RemoveDeviceCommandHandler()
    {
        DeviceViewModels.Remove(SelectedDeviceViewModel);
    }
    
    public DelegateCommand AddMasterDeviceCommand { get; set; }
    private void AddMasterDeviceCommandHandler()
    {
        MasterDeviceViewModels.Add(new StandSettingsMasterDeviceViewModel()
        {
            Number = MasterDeviceViewModels.Count + 1, 
        });
    }

    public DelegateCommand RemoveMasterDeviceCommand { get; }
    private void RemoveMasterDeviceCommandHandler()
    {
        MasterDeviceViewModels.Remove(SelectedMasterDeviceViewModel);
    }
    
    public DelegateCommand AddNozzleCommand { get; set; }
    private void AddNozzleCommandHandler()
    {
        NozzleViewModels.Add(new StandSettingsNozzleViewModel
        {
            Number = NozzleViewModels.Count + 1
        });
    }
    
    public DelegateCommand RemoveNozzleCommand { get; set; }
    private void RemoveNozzleCommandHandler()
    {
        NozzleViewModels.Remove(SelectedNozzleViewModel);
    }
    
    public DelegateCommand AddFanCommand { get; set; }
    private void AddFanCommandHandler()
    {
        FanViewModels.Add(new StandSettingsFanViewModel()
        {
            Number = FanViewModels.Count + 1, 
        });
    }

    public DelegateCommand RemoveFanCommand { get; }
    private void RemoveFanCommandHandler()
    {
        FanViewModels.Remove(SelectedFanViewModel);
    }
    
    public DelegateCommand AddVacuumValveCommand { get; set; }
    private void AddVacuumValveCommandHandler()
    {
        VacuumValveViewModels.Add(new StandSettingsVacuumValveViewModel()
        {
            Number = VacuumValveViewModels.Count + 1, 
        });
    }

    public DelegateCommand RemoveVacuumValveCommand { get; }
    private void RemoveVacuumValveCommandHandler()
    {
        VacuumValveViewModels.Remove(SelectedVacuumValveViewModel);
    }
}