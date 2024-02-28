using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using AutoMapper;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Extensions;
using SPU_7.Common.Settings;
using SPU_7.Common.Stand;
using SPU_7.Models.Services.Autorization;
using SPU_7.Models.Services.DbServices;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand.Settings.Stand;
using SPU_7.Views.Settings;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsViewModel : ViewModelBase, IDialogAware
{
    public StandSettingsViewModel(IMapper mapper, 
        IStandSettingsDbService standSettingsDbService, 
        IStandSettingsService standSettingsService,
        IDialogService dialogService,
        IAuthorizationService authorizationService)
    {
        Title = "Настройки стенда";
        
        _mapper = mapper;
        _standSettingsDbService = standSettingsDbService;
        _standSettingsService = standSettingsService;
        _dialogService = dialogService;

        StandSettingsProfiles = new ObservableCollection<StandSettingsProfileViewModel>();
        foreach (var standSettingsProfile in _standSettingsDbService.GetSettingsProfiles())
        {
            StandSettingsProfiles.Add(new StandSettingsProfileViewModel
            {
                Id = standSettingsProfile.Id,
                Name = standSettingsProfile.ProfileName
            });
        }

        NozzleViewModels = new ObservableCollection<StandSettingsNozzleViewModel>();
        ValveViewModels = new ObservableCollection<StandSettingsValveViewModel>();
        ComparatorSettingsViewModels = new ObservableCollection<ComparatorSettingsViewModel>();
        DeviceViewModels = new ObservableCollection<StandSettingsDeviceViewModel>();
        SolenoidValveViewModels = new ObservableCollection<StandSettingsSolenoidValveViewModel>();
        LineViewModels = new ObservableCollection<StandSettingsLineViewModel>();
        PulseMeterViewModels = new ObservableCollection<StandSettingsPulseMeterViewModel>();
        if (_standSettingsService.StandSettingsModel != null)
            _mapper.Map(_standSettingsService.StandSettingsModel, this);
        
        
        AddNozzleCommand = new DelegateCommand(AddNozzleCommandHandler);
        RemoveNozzleCommand = new DelegateCommand(RemoveNozzleCommandHandler);
        AddValveCommand = new DelegateCommand(AddValveCommandHandler);
        RemoveValveCommand = new DelegateCommand(RemoveValveCommandHandler);
        AddSolenoidValveCommand = new DelegateCommand(AddSolenoidValveCommandHandler);
        RemoveSolenoidValveCommand = new DelegateCommand(RemoveSolenoidValveCommandHandler);
        AddComparatorCommand = new DelegateCommand(AddComparatorCommandHandler);
        RemoveComparatorCommand = new DelegateCommand(RemoveComparatorCommandHandler);
        AddDeviceCommand = new DelegateCommand(AddDeviceCommandHandler);
        RemoveDeviceCommand = new DelegateCommand(RemoveDeviceCommandHandler);
        AddLineCommand = new DelegateCommand(AddLineCommandHandler);
        RemoveLineCommand = new DelegateCommand(RemoveLineCommandHandler);
        AddPulseMeterCommand = new DelegateCommand(AddPulseMeterCommandHandler);
        RemovePulseMeterCommand = new DelegateCommand(RemovePulseMeterCommandHandler);
        SaveSettingsCommand = new DelegateCommand(SaveSettingsCommandHandler);
        CancelCommand = new DelegateCommand(CancelCommandHandler);
        CreateNewSettingsProfile = new DelegateCommand(CreateNewSettingsProfileHandler);
        CreateNewSettingsProfileFromOtherCommand = new DelegateCommand(CreateNewSettingsProfileFromOtherCommandHandler);
        DeleteSettingsProfileCommand = new DelegateCommand(DeleteSettingsProfileCommandHandler);
        CloseWindowCommand = new DelegateCommand(CloseWindowCommandHandler);

        ComPorts = new ObservableCollection<string>(SerialPort.GetPortNames());
        BaudRates = new ObservableCollection<int>
        {
            300,
            600,
            1200,
            2400,
            4800,
            9600,
            19200,
            38400,
            57600,
            115200,
            230400,
            460800,
            921600
        };
        
        StringStandTypes = new ObservableCollection<string>(Enum
            .GetValues<StandType>()
            .Where(st => st != StandType.None)
            .Select(st => new string(st.GetDescription())));

        StringNozzleManualTypes = new ObservableCollection<string>(Enum
            .GetValues<NozzleManualType>()
            .Select(st => new string(st.GetDescription())));

        IsProtocolVisible = true;
        if (authorizationService.GetUser().UserType == UserType.Developer)
        {
            IsMnemonicSchemeVisible = true;
            IsDeviceVisible = true;
        }
    }

    private readonly IMapper _mapper;
    private readonly IStandSettingsDbService _standSettingsDbService;
    private readonly IStandSettingsService _standSettingsService;
    private readonly IDialogService _dialogService;

    private double? _contractualTemperature;
    private double? _contractualCoefficientCompressibility;
    private double? _contractualPressure;
    private int? _modeCalculateCoefficientCompressibility;
    private double? _dioxideCarbonValue;
    private double? _nitrogenValue;
    private double? _gasDensityValue;
    private string _ipMain;
    private int? _portMain;
    private string _ipAdditional;
    private int? _portAdditional;
    private string _dbNameDontel;
    private string _userName;
    private string _password;
    private string _ipDontel;
    private int? _portDontel;
    private StandSettingsProfileViewModel _selectedSettingsProfile;
    private StandSettingsNozzleViewModel _selectedNozzleViewModels;
    private StandSettingsValveViewModel _selectedValveViewModel;
    private string _standNumber;
    private ComparatorSettingsViewModel _selectedComparatorSettingsViewModel;
    private string _selectedCheckConnectionWithPlatform;
    private double? _frequencyCheck;
    private double? _frequencyCheckDifference;
    private int? _frequencyStabilization;
    private bool _isCheckOnlyConnection;
    private bool _isGetFrequencyFromSpecialRegister;
    private int? _pauseForCheckingOutsideValve;
    private double? _temperatureSet;
    private bool _isTemperatureSet;
    private double? _pressureCorrection;
    private double? _fortyPeriod;
    private double? _thirdPeriod;
    private double? _secondPeriod;
    private double? _firstPeriod;
    private bool _isTemperatureCorrection;
    private double? _temperatureCorrection;
    private bool _isPressureCorrection;
    private string _profileName;
    private string _selectedStringStandType;
    private ObservableCollection<StandSettingsNozzleViewModel> _nozzleViewModels;
    private ObservableCollection<StandSettingsValveViewModel> _valveViewModels;
    private ObservableCollection<ComparatorSettingsViewModel> _comparatorSettingsViewModels;
    private string _selectedEquipmentPort;
    private int _selectedEquipmentBaudRate;
    private StandSettingsDeviceViewModel _selectedDevice;
    private string _selectedStringNozzleManualType;
    private int _temperatureSensorAddress;
    private int _pressureSensorAddress;
    private int _thMeterAddress;
    private int _pressureResiverSensorAddress;
    private int _pressureDifferenceSensorAddress;
    private ObservableCollection<StandSettingsSolenoidValveViewModel> _solenoidValveViewModels;
    private StandSettingsSolenoidValveViewModel _selectedSolenoidValveViewModel;
    private string _validationVendorName;
    private string _validationVendorUniqueNumber;
    private string _postInfo;
    private string _deviceTestInfo;
    private string _outsideCheckInfo;
    private string _tightnessInfo;
    private string _validationMethod;
    private string _validationDeviceInfo;
    private string _lastValidationInfo;
    private string _ownerName;
    private string _deviceRangeInfo;
    private string _vendorDate;
    private string _vendorName;
    private string _deviceInfo;
    private string _validationVendorAddress;
    private string _validationVendorType;
    private string _validationVendorShortName;
    private ObservableCollection<StandSettingsLineViewModel> _lineViewModels;
    private StandSettingsLineViewModel _selectedLineViewModel;
    private ObservableCollection<StandSettingsPulseMeterViewModel> _pulseMeterViewModels;
    private StandSettingsPulseMeterViewModel _selectedPulseMeterViewModel;

    #region Profiles

    public Guid Id { get; set; }
    
    public ObservableCollection<StandSettingsProfileViewModel> StandSettingsProfiles { get; set; }
    public StandSettingsProfileViewModel SelectedSettingsProfile
    {
        get => _selectedSettingsProfile;
        set
        {
            SetProperty(ref _selectedSettingsProfile, value);
            UpdateStandSettings();
        }
    }

    private void UpdateStandSettings()
    {
        _mapper.Map(_standSettingsDbService.GetStandSettings(SelectedSettingsProfile.Id), this);
    }

    public string ProfileName
    {
        get => _profileName;
        set => SetProperty(ref _profileName, value);
    }

    #endregion

    #region TabVisible

    public bool IsProtocolVisible { get;set; }
    public bool IsMnemonicSchemeVisible { get; set; }
    public bool IsDeviceVisible { get; set; }

    #endregion
    
    #region Stand

    public string StandNumber
    {
        get => _standNumber;
        set => SetProperty(ref _standNumber, value);
    }

    public double? TemperatureSet
    {
        get => _temperatureSet;
        set => SetProperty(ref _temperatureSet, value);
    }

    public bool IsTemperatureSet
    {
        get => _isTemperatureSet;
        set => SetProperty(ref _isTemperatureSet, value);
    }

    public double? PressureCorrection
    {
        get => _pressureCorrection;
        set => SetProperty(ref _pressureCorrection, value);
    }

    public bool IsPressureCorrection
    {
        get => _isPressureCorrection;
        set => SetProperty(ref _isPressureCorrection, value);
    }

    public double? TemperatureCorrection
    {
        get => _temperatureCorrection;
        set => SetProperty(ref _temperatureCorrection, value);
    }

    public bool IsTemperatureCorrection
    {
        get => _isTemperatureCorrection;
        set => SetProperty(ref _isTemperatureCorrection, value);
    }

    public double? FirstPeriod
    {
        get => _firstPeriod;
        set => SetProperty(ref _firstPeriod, value);
    }

    public double? SecondPeriod
    {
        get => _secondPeriod;
        set => SetProperty(ref _secondPeriod, value);
    }

    public double? ThirdPeriod
    {
        get => _thirdPeriod;
        set => SetProperty(ref _thirdPeriod, value);
    }

    public double? FortyPeriod
    {
        get => _fortyPeriod;
        set => SetProperty(ref _fortyPeriod, value);
    }
    
    public ObservableCollection<string> StringStandTypes { get; }

    public string SelectedStringStandType
    {
        get => _selectedStringStandType;
        set
        {
            SetProperty(ref _selectedStringStandType, value);
            StandType = Enum
                .GetValues<StandType>()
                .FirstOrDefault(st => st.GetDescription() == value);
        }
    }

    public StandType StandType { get; set; }

    
    public ObservableCollection<string> StringNozzleManualTypes { get; }

    public string SelectedStringNozzleManualType
    {
        get => _selectedStringNozzleManualType;
        set
        {
            SetProperty(ref _selectedStringNozzleManualType, value);
            NozzleManualType = Enum
                .GetValues<NozzleManualType>()
                .FirstOrDefault(nt => nt.GetDescription() == value);
        }
    }
    public NozzleManualType NozzleManualType { get; set; }
    
    #endregion
    
    #region Protocols

    public string ValidationVendorType
    {
        get => _validationVendorType;
        set => SetProperty(ref _validationVendorType, value);
    }

    public string ValidationVendorName
    {
        get => _validationVendorName;
        set => SetProperty(ref _validationVendorName, value);
    }

    public string ValidationVendorShortName
    {
        get => _validationVendorShortName;
        set => SetProperty(ref _validationVendorShortName, value);
    }

    public string ValidationVendorUniqueNumber
    {
        get => _validationVendorUniqueNumber;
        set => SetProperty(ref _validationVendorUniqueNumber, value);
    }

    public string ValidationVendorAddress
    {
        get => _validationVendorAddress;
        set => SetProperty(ref _validationVendorAddress, value);
    }

    public string DeviceInfo
    {
        get => _deviceInfo;
        set => SetProperty(ref _deviceInfo, value);
    }

    public string VendorName
    {
        get => _vendorName;
        set => SetProperty(ref _vendorName, value);
    }

    public string VendorDate
    {
        get => _vendorDate;
        set => SetProperty(ref _vendorDate, value);
    }

    public string DeviceRangeInfo
    {
        get => _deviceRangeInfo;
        set => SetProperty(ref _deviceRangeInfo, value);
    }

    public string OwnerName
    {
        get => _ownerName;
        set => SetProperty(ref _ownerName, value);
    }

    public string LastValidationInfo
    {
        get => _lastValidationInfo;
        set => SetProperty(ref _lastValidationInfo, value);
    }

    public string ValidationDeviceInfo
    {
        get => _validationDeviceInfo;
        set => SetProperty(ref _validationDeviceInfo, value);
    }

    public string ValidationMethod
    {
        get => _validationMethod;
        set => SetProperty(ref _validationMethod, value);
    }

    public string TightnessInfo
    {
        get => _tightnessInfo;
        set => SetProperty(ref _tightnessInfo, value);
    }

    public string OutsideCheckInfo
    {
        get => _outsideCheckInfo;
        set => SetProperty(ref _outsideCheckInfo, value);
    }

    public string DeviceTestInfo
    {
        get => _deviceTestInfo;
        set => SetProperty(ref _deviceTestInfo, value);
    }

    public string PostInfo
    {
        get => _postInfo;
        set => SetProperty(ref _postInfo, value);
    }

    #endregion
    
    #region Mnemoscheme

    public ObservableCollection<StandSettingsNozzleViewModel> NozzleViewModels
    {
        get => _nozzleViewModels;
        set => SetProperty(ref _nozzleViewModels, value);
    }

    public StandSettingsNozzleViewModel SelectedNozzleViewModels
    {
        get => _selectedNozzleViewModels;
        set => SetProperty(ref _selectedNozzleViewModels, value);
    }

    public ObservableCollection<StandSettingsValveViewModel> ValveViewModels
    {
        get => _valveViewModels;
        set => SetProperty(ref _valveViewModels, value);
    }

    public StandSettingsValveViewModel SelectedValveViewModel
    {
        get => _selectedValveViewModel;
        set => SetProperty(ref _selectedValveViewModel, value);
    }

    public ObservableCollection<StandSettingsSolenoidValveViewModel> SolenoidValveViewModels
    {
        get => _solenoidValveViewModels;
        set => SetProperty(ref _solenoidValveViewModels, value);
    }

    public StandSettingsSolenoidValveViewModel SelectedSolenoidValveViewModel
    {
        get => _selectedSolenoidValveViewModel;
        set => SetProperty(ref _selectedSolenoidValveViewModel, value);
    }

    public ObservableCollection<StandSettingsLineViewModel> LineViewModels
    {
        get => _lineViewModels;
        set => SetProperty(ref _lineViewModels, value);
    }

    public ObservableCollection<StandSettingsPulseMeterViewModel> PulseMeterViewModels
    {
        get => _pulseMeterViewModels;
        set => SetProperty(ref _pulseMeterViewModels, value);
    }

    public StandSettingsPulseMeterViewModel SelectedPulseMeterViewModel
    {
        get => _selectedPulseMeterViewModel;
        set => SetProperty(ref _selectedPulseMeterViewModel, value);
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
        NozzleViewModels.Remove(SelectedNozzleViewModels);
    }
    
    public DelegateCommand AddValveCommand { get; set; }
    private void AddValveCommandHandler()
    {
        ValveViewModels.Add(new StandSettingsValveViewModel
        {
            Number = ValveViewModels.Count + 1
        });
    }
    
    public DelegateCommand RemoveValveCommand { get; set; }
    private void RemoveValveCommandHandler()
    {
        ValveViewModels.Remove(SelectedValveViewModel);
    }
    
    public DelegateCommand AddSolenoidValveCommand { get; set; }
    private void AddSolenoidValveCommandHandler()
    {
        SolenoidValveViewModels.Add(new StandSettingsSolenoidValveViewModel
        {
            Number = SolenoidValveViewModels.Count + 1
        });
    }
    
    public DelegateCommand RemoveSolenoidValveCommand { get; set; }
    private void RemoveSolenoidValveCommandHandler()
    {
        SolenoidValveViewModels.Remove(SelectedSolenoidValveViewModel);
    }
    
    public DelegateCommand AddLineCommand { get; }

    private void AddLineCommandHandler()
    {
        LineViewModels.Add(new StandSettingsLineViewModel()
        {
            LineNumber = LineViewModels.Count + 1
        });
    }
    
    public DelegateCommand RemoveLineCommand { get; }

    private void RemoveLineCommandHandler()
    {
        LineViewModels.Remove(LineViewModels.Last());
    }
    
    public DelegateCommand AddPulseMeterCommand { get; }

    private void AddPulseMeterCommandHandler()
    {
        PulseMeterViewModels.Add(new StandSettingsPulseMeterViewModel()
        {
            Number = PulseMeterViewModels.Count + 1,
        });
    }
    
    public DelegateCommand RemovePulseMeterCommand { get; }

    private void RemovePulseMeterCommandHandler()
    {
        PulseMeterViewModels.Remove(SelectedPulseMeterViewModel);
    }
    
    #endregion

    #region Equipment

    public ObservableCollection<string> ComPorts { get; set; }

    public ObservableCollection<int> BaudRates { get; set; }

    public string SelectedEquipmentPort
    {
        get => _selectedEquipmentPort;
        set => SetProperty(ref _selectedEquipmentPort, value);
    }

    public int SelectedEquipmentBaudRate
    {
        get => _selectedEquipmentBaudRate;
        set => SetProperty(ref _selectedEquipmentBaudRate, value);
    }

    public int TemperatureSensorAddress
    {
        get => _temperatureSensorAddress;
        set => SetProperty(ref _temperatureSensorAddress, value);
    }

    public int PressureSensorAddress
    {
        get => _pressureSensorAddress;
        set => SetProperty(ref _pressureSensorAddress, value);
    }

    public int THMeterAddress
    {
        get => _thMeterAddress;
        set => SetProperty(ref _thMeterAddress, value);
    }

    public int PressureResiverSensorAddress
    {
        get => _pressureResiverSensorAddress;
        set => SetProperty(ref _pressureResiverSensorAddress, value);
    }

    public int PressureDifferenceSensorAddress
    {
        get => _pressureDifferenceSensorAddress;
        set => SetProperty(ref _pressureDifferenceSensorAddress, value);
    }


    public ObservableCollection<StandSettingsDeviceViewModel> DeviceViewModels { get; set; }

    public StandSettingsDeviceViewModel SelectedDevice
    {
        get => _selectedDevice;
        set => SetProperty(ref _selectedDevice, value);
    }
    
    public DelegateCommand AddDeviceCommand { get; }

    private void AddDeviceCommandHandler()
    {
        DeviceViewModels.Add(new StandSettingsDeviceViewModel()
        {
            Number = DeviceViewModels.Count + 1
        });
    }
    
    public DelegateCommand RemoveDeviceCommand { get; }

    private void RemoveDeviceCommandHandler()
    {
        DeviceViewModels.Remove(SelectedDevice);
    }
    
    #endregion
    
    #region SPI settings

    public double? ContractualTemperature
    {
        get => _contractualTemperature;
        set => SetProperty(ref _contractualTemperature, value);
    }

    public double? ContractualCoefficientCompressibility
    {
        get => _contractualCoefficientCompressibility;
        set => SetProperty(ref _contractualCoefficientCompressibility, value);
    }

    public double? ContractualPressure
    {
        get => _contractualPressure;
        set => SetProperty(ref _contractualPressure, value);
    }

    public int? ModeCalculateCoefficientCompressibility
    {
        get => _modeCalculateCoefficientCompressibility;
        set => SetProperty(ref _modeCalculateCoefficientCompressibility, value);
    }

    public double? DioxideCarbonValue
    {
        get => _dioxideCarbonValue;
        set => SetProperty(ref _dioxideCarbonValue, value);
    }

    public double? NitrogenValue
    {
        get => _nitrogenValue;
        set => SetProperty(ref _nitrogenValue, value);
    }

    public double? GasDensityValue
    {
        get => _gasDensityValue;
        set => SetProperty(ref _gasDensityValue, value);
    }

    public string IpMain
    {
        get => _ipMain;
        set => SetProperty(ref _ipMain, value);
    }

    public int? PortMain
    {
        get => _portMain;
        set => SetProperty(ref _portMain, value);
    }

    public string IpAdditional
    {
        get => _ipAdditional;
        set => SetProperty(ref _ipAdditional, value);
    }

    public int? PortAdditional
    {
        get => _portAdditional;
        set => SetProperty(ref _portAdditional, value);
    }

    public string DbNameDontel
    {
        get => _dbNameDontel;
        set => SetProperty(ref _dbNameDontel, value);
    }

    public string UserName
    {
        get => _userName;
        set => SetProperty(ref _userName, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string IpDontel
    {
        get => _ipDontel;
        set => SetProperty(ref _ipDontel, value);
    }

    public int? PortDontel
    {
        get => _portDontel;
        set => SetProperty(ref _portDontel, value);
    }

    #endregion

    #region NBIoT settings

    public ObservableCollection<ComparatorSettingsViewModel> ComparatorSettingsViewModels
    {
        get => _comparatorSettingsViewModels;
        set => SetProperty(ref _comparatorSettingsViewModels, value);
    }

    public ComparatorSettingsViewModel SelectedComparatorSettingsViewModel
    {
        get => _selectedComparatorSettingsViewModel;
        set => SetProperty(ref _selectedComparatorSettingsViewModel, value);
    }
    
    public ObservableCollection<string> CheckConnectionWithPlatform { get; set; }

    public string SelectedCheckConnectionWithPlatform
    {
        get => _selectedCheckConnectionWithPlatform;
        set
        {
            SetProperty(ref _selectedCheckConnectionWithPlatform, value);
            PlatformType = Enum
                .GetValues<PlatformType>()
                .FirstOrDefault(pt => pt.GetDescription() == value);
        }
    }

    public double? FrequencyCheck
    {
        get => _frequencyCheck;
        set => SetProperty(ref _frequencyCheck, value);
    }

    public double? FrequencyCheckDifference
    {
        get => _frequencyCheckDifference;
        set => SetProperty(ref _frequencyCheckDifference, value);
    }

    public int? FrequencyStabilization
    {
        get => _frequencyStabilization;
        set => SetProperty(ref _frequencyStabilization, value);
    }

    public PlatformType PlatformType { get; set; }

    public bool IsCheckOnlyConnection
    {
        get => _isCheckOnlyConnection;
        set => SetProperty(ref _isCheckOnlyConnection, value);
    }

    public bool IsGetFrequencyFromSpecialRegister
    {
        get => _isGetFrequencyFromSpecialRegister;
        set => SetProperty(ref _isGetFrequencyFromSpecialRegister, value);
    }

    public int? PauseForCheckingOutsideValve
    {
        get => _pauseForCheckingOutsideValve;
        set => SetProperty(ref _pauseForCheckingOutsideValve, value);
    }

    public DelegateCommand AddComparatorCommand { get; set; }
    private void AddComparatorCommandHandler()
    {
        ComparatorSettingsViewModels.Add(new ComparatorSettingsViewModel
        {
            Number = ValveViewModels.Count + 1
        });
    }
    
    
    public DelegateCommand RemoveComparatorCommand { get; set; }
    private void RemoveComparatorCommandHandler()
    {
        ComparatorSettingsViewModels.Remove(SelectedComparatorSettingsViewModel);
    }
    
    #endregion

    #region Grand

    

    #endregion

    #region Printer

    

    #endregion

    #region Dialog

    
    public DelegateCommand SaveSettingsCommand { get; }
    private void SaveSettingsCommandHandler()
    {
        var isNewGuid = false;
        
        if (Id == Guid.Empty)
        {
            Id = Guid.NewGuid();
            
            isNewGuid = true;
        } 
        
        var settingsModel = _mapper.Map<StandSettingsModel>(this);
        
        _standSettingsService.SaveSettings(settingsModel);
        _standSettingsService.StandSettingsModel = settingsModel;
        
        if (isNewGuid) _standSettingsDbService.AddStandSettings(settingsModel);
        else _standSettingsDbService.EditStandSettings(settingsModel);
        
        RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
    }
    
    public DelegateCommand CancelCommand { get; }
    private void CancelCommandHandler()
    {
        RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
    }
    
    public DelegateCommand CreateNewSettingsProfile { get; }

    private void CreateNewSettingsProfileHandler()
    {
        _mapper.Map(new StandSettingsModel(), this);
    }
    
    public DelegateCommand CreateNewSettingsProfileFromOtherCommand { get; }
    
    private void CreateNewSettingsProfileFromOtherCommandHandler()
    {
        var settingsModel = _mapper.Map<StandSettingsModel>(this);
        settingsModel.ProfileName = string.Empty;
        settingsModel.Id = Guid.Empty;
        _mapper.Map(settingsModel, this);
        
        MessageViewModel.Show(_dialogService, "Новый профиль скопирован из выбранного", null, null);
    }
    
    public DelegateCommand DeleteSettingsProfileCommand { get; }

    private void DeleteSettingsProfileCommandHandler()
    {
        
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
        
    }

    public static void Show(IDialogService dialogService, Action positiveAction, Action negativeAction)
    {
        dialogService.ShowDialog(nameof(StandSettingsView),
            null,
            result =>
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
    
    public event Action<IDialogResult> RequestClose;
    
    #endregion

}