using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using Prism.Commands;
using Prism.Services.Dialogs;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;
using SPU_7.Common.Settings;
using SPU_7.Models.Scripts.Operations.Configurations;
using SPU_7.Models.Scripts.Operations.Configurations.Extensions;
using SPU_7.Models.Scripts.Operations.Configurations.Points;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.ViewModels.Settings;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public class ValidationOperationConfigurationViewModel : ViewModelBase, IOperationConfiguration
{
    private readonly IStandController _standController;
    private readonly IDialogService _dialogService;
    private readonly IMapper _mapper;
    private readonly IStandSettingsService _standSettingsService;

    public ValidationOperationConfigurationViewModel(BaseOperationConfigurationModel configurationModel,
        IStandController standController,
        IDialogService dialogService, 
        IMapper mapper,
        IStandSettingsService standSettingsService,
        int lineNumber)
    {
        _standController = standController;
        _dialogService = dialogService;
        _mapper = mapper;
        _standSettingsService = standSettingsService;
        StringValidationTypes = new ObservableCollection<string>(Enum
            .GetValues<ValidationType>()
            .Where(vt => vt != ValidationType.None)
            .Select(vt => vt.GetDescription()));
        
        if (configurationModel is ValidationOperationConfigurationModel validationOperationConfigurationModel)
        {
            Points = new ObservableCollection<PointConfigurationViewModel>();
            foreach (var point in validationOperationConfigurationModel.Points)
            {
                Points.Add(new PointConfigurationViewModel(_standController, _dialogService)
                {
                    Delay = point.Delay,
                    Number = point.Number,
                    TargetFlow = point.TargetConsumption,
                    MeasureCount = point.MeasureCount,
                    IsValveUse = point.IsValveUse,
                    TargetVolume = point.TargetVolume,
                    Inaccuracy = point.Inaccuracy,
                    SelectedNozzles = _mapper.Map<ObservableCollection<StandSettingsNozzleViewModel>>(point.SelectedNozzles)
                });
            }

            SelectedStringValidationType = validationOperationConfigurationModel.ValidationType.GetDescription();
            ValidationType = validationOperationConfigurationModel.ValidationType;
            IsAutoPulseMeasure = validationOperationConfigurationModel.IsAutoPulseMeasure;
            IsProtocolNeed = validationOperationConfigurationModel.IsProtocolNeed;
            VendorName = validationOperationConfigurationModel.VendorName;
            VendorAddress = validationOperationConfigurationModel.VendorAddress;
            DeviceName = validationOperationConfigurationModel.DeviceName;
            MaximumFlow = validationOperationConfigurationModel.MaximumFlow;
            MinimumFlow = validationOperationConfigurationModel.MinimumFlow;
            NominalFlow = validationOperationConfigurationModel.NominalFlow;
            
            foreach (var pulseMeterConfiguration in validationOperationConfigurationModel.PulseMeterConfigurations)
            {
                PulseMeterConfigurationViewModels.Add(_mapper.Map<ValidationPulseMeterConfigurationViewModel>(pulseMeterConfiguration));
            }
            
        }
        else
        {
            Points = new ObservableCollection<PointConfigurationViewModel>();
            foreach (var unused in standSettingsService.StandSettingsModel.LineViewModels[lineNumber - 1].DeviceViewModels)
            {
                PulseMeterConfigurationViewModels.Add(new ValidationPulseMeterConfigurationViewModel());
            }
        }
        
        RemovePointCommand = new DelegateCommand(RemovePointCommandHandler);
        AddPointCommand = new DelegateCommand(AddPointCommandHandler);
    }


    private PointConfigurationViewModel _selectedPoint;
    private bool _isProtocolNeed = true;
    private bool _isAutoPulseMeasure;
    private string _vendorName;
    private string _vendorAddress;
    private string _deviceName;
    private string _selectedStringValidationType;
    private double? _minimumFlow;
    private double? _maximumFlow;
    private double? _nominalFlow;

    public ObservableCollection<string> StringValidationTypes { get; set; }

    public string SelectedStringValidationType
    {
        get => _selectedStringValidationType;
        set
        {
            SetProperty(ref _selectedStringValidationType, value);
            ValidationType = Enum
                .GetValues<ValidationType>()
                .FirstOrDefault(vt => vt.GetDescription() == value);
        }
    }

    public ValidationType ValidationType { get; set; }

    public ObservableCollection<PointConfigurationViewModel> Points { get; set; }
    public PointConfigurationViewModel SelectedPoint
    {
        get => _selectedPoint;
        set => SetProperty(ref _selectedPoint, value);
    }

    public bool IsProtocolNeed
    {
        get => _isProtocolNeed;
        set => SetProperty(ref _isProtocolNeed, value);
    }

    public bool IsAutoPulseMeasure
    {
        get => _isAutoPulseMeasure;
        set => SetProperty(ref _isAutoPulseMeasure, value);
    }

    public string VendorName
    {
        get => _vendorName;
        set => SetProperty(ref _vendorName, value);
    }

    public string VendorAddress
    {
        get => _vendorAddress;
        set => SetProperty(ref _vendorAddress, value);
    }

    public string DeviceName
    {
        get => _deviceName;
        set => SetProperty(ref _deviceName, value);
    }

    public double? MinimumFlow
    {
        get => _minimumFlow;
        set => SetProperty(ref _minimumFlow, value);
    }

    public double? MaximumFlow
    {
        get => _maximumFlow;
        set => SetProperty(ref _maximumFlow, value);
    }

    public double? NominalFlow
    {
        get => _nominalFlow;
        set => SetProperty(ref _nominalFlow, value);
    }

    public ObservableCollection<ValidationPulseMeterConfigurationViewModel> PulseMeterConfigurationViewModels { get; set; } = new();

    public BaseOperationConfigurationModel CreateConfiguration(DeviceType deviceType)
    {
        var points = Points
            .Select(point => new ValidationPointModel
            {
                Delay = point.Delay ?? 0,
                Number = point.Number,
                TargetConsumption = point.TargetFlow ?? 0,
                MeasureCount = point.MeasureCount ?? 0,
                IsValveUse = point.IsValveUse,
                TargetVolume = point.TargetVolume ?? 0,
                Inaccuracy = point.Inaccuracy ?? 0,
                SelectedNozzles = _mapper.Map<ObservableCollection<StandSettingsNozzleModel>>(point.SelectedNozzles)
            })
            .ToList();

        var pulseMeterConfig = PulseMeterConfigurationViewModels
            .Select(config => new ValidationPulseMeterConfiguration()
            {
                PulseWeight = (double)config.PulseWeight!
            })
            .ToList();
        
        return new ValidationOperationConfigurationModel
        {
            DeviceType = deviceType,
            GroupType = deviceType.GetDeviceGroupType(),
            Points = points,
            PulseMeterConfigurations = pulseMeterConfig,
            IsProtocolNeed = IsProtocolNeed,
            IsAutoPulseMeasure = IsAutoPulseMeasure,
            DeviceName = string.IsNullOrEmpty(DeviceName)? "-" : DeviceName,
            VendorName = string.IsNullOrEmpty(VendorName)? "-" : VendorName,
            VendorAddress = string.IsNullOrEmpty(VendorAddress)? "-" : VendorAddress,
            ValidationType = ValidationType,
            MaximumFlow = MaximumFlow,
            MinimumFlow = MinimumFlow,
            NominalFlow = NominalFlow,
        };
    }
    
    public DelegateCommand AddPointCommand { get; }
    private void AddPointCommandHandler()
    {
        Points.Add(new PointConfigurationViewModel(_standController, _dialogService)
        {
            Number = Points.Count + 1,
            SelectedNozzles = new ObservableCollection<StandSettingsNozzleViewModel>()
        });
    }
    
    public DelegateCommand RemovePointCommand { get; }
    private void RemovePointCommandHandler()
    {
        Points.Remove(SelectedPoint);

        for (var i = 0; i < Points.Count; i++)
        {
            Points[i].Number = i + 1;
        }
    }
}