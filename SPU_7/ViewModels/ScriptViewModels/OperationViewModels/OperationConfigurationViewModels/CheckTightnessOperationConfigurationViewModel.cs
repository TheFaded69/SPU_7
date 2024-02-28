using System.Collections.ObjectModel;
using System.Linq;
using SPU_7.Common.Device;
using SPU_7.Common.Extensions;
using SPU_7.Models.Scripts.Operations.Configurations;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.ScriptViewModels.OperationViewModels.OperationConfigurationViewModels;

public class CheckTightnessOperationConfigurationViewModel : ViewModelBase, IOperationConfiguration
{
    public CheckTightnessOperationConfigurationViewModel(BaseOperationConfigurationModel configurationModel, IStandSettingsService standSettingsService)
    {
        _standSettingsService = standSettingsService;
        
        if (configurationModel is CheckTightnessOperationConfigurationModel checkTightnessOperationConfigurationModel)
        {
            VacuumWaitTime = checkTightnessOperationConfigurationModel.VacuumWaitTime;
            PressureDifferenceMaximum = checkTightnessOperationConfigurationModel.PressureDifferenceMaximum;
            PressureResiverMinimum = checkTightnessOperationConfigurationModel.PressureResiverMinimum;
            PressureVacuumMinimum = checkTightnessOperationConfigurationModel.PressureVacuumMinimum;
            TestTime = checkTightnessOperationConfigurationModel.TestTime;
            StabilizationTime = checkTightnessOperationConfigurationModel.StabilizationTime;
            SelectedNozzleValue = checkTightnessOperationConfigurationModel.SelectedNozzleValue;
            SelectedStandSettingsNozzleModel = checkTightnessOperationConfigurationModel.SelectedStandSettingsNozzleModel;
        }

        NozzleValues = new ObservableCollection<double>(standSettingsService.StandSettingsModel.NozzleViewModels
            .Where(nvm => nvm.NozzleValue != null)
            .Select(nvm => (double)nvm.NozzleValue!));
    }
    
    private readonly IStandSettingsService _standSettingsService;
    private int _vacuumWaitTime;
    private float _pressureVacuumMinimum;
    private float _pressureResiverMinimum;
    private float _pressureDifferenceMaximum;
    private int _testTime = 3;
    private int _stabilizationTime = 1;
    private double _selectedNozzleValue;

    public int VacuumWaitTime
    {
        get => _vacuumWaitTime;
        set => SetProperty(ref _vacuumWaitTime, value);
    }

    public float PressureVacuumMinimum
    {
        get => _pressureVacuumMinimum;
        set => SetProperty(ref _pressureVacuumMinimum, value);
    }

    public float PressureResiverMinimum
    {
        get => _pressureResiverMinimum;
        set => SetProperty(ref _pressureResiverMinimum, value);
    }

    public float PressureDifferenceMaximum
    {
        get => _pressureDifferenceMaximum;
        set => SetProperty(ref _pressureDifferenceMaximum, value);
    }

    public int TestTime
    {
        get => _testTime;
        set => SetProperty(ref _testTime, value);
    }

    public int StabilizationTime
    {
        get => _stabilizationTime;
        set => SetProperty(ref _stabilizationTime, value);
    }
    
    public ObservableCollection<double> NozzleValues { get; set; }

    public double SelectedNozzleValue
    {
        get => _selectedNozzleValue;
        set
        {
            SetProperty(ref _selectedNozzleValue, value);
            SelectedStandSettingsNozzleModel = _standSettingsService.StandSettingsModel.NozzleViewModels.FirstOrDefault(bvm => bvm.NozzleValue == value);
        }
    }

    public StandSettingsNozzleModel SelectedStandSettingsNozzleModel { get; set; }

    
    public BaseOperationConfigurationModel CreateConfiguration(DeviceType deviceType) =>
        new CheckTightnessOperationConfigurationModel()
        {
            DeviceType = deviceType,
            GroupType = deviceType.GetDeviceGroupType(),
            StabilizationTime = StabilizationTime,
            TestTime = TestTime,
            PressureDifferenceMaximum = PressureDifferenceMaximum,
            PressureResiverMinimum = PressureResiverMinimum,
            PressureVacuumMinimum = PressureVacuumMinimum, 
            VacuumWaitTime = VacuumWaitTime,
            SelectedStandSettingsNozzleModel = SelectedStandSettingsNozzleModel,
            SelectedNozzleValue = SelectedNozzleValue
        };
}