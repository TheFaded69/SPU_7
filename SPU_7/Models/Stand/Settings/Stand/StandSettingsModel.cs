using System;
using System.Collections.ObjectModel;
using SPU_7.Common.Line;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.Models.Stand.Settings.Stand;

public class StandSettingsModel
{
    public Guid Id { get; set; }
    public ObservableCollection<StandSettingsNozzleModel> NozzleViewModels { get; set; }
    public ObservableCollection<StandSettingsValveModel> ValveViewModels { get; set; }
    public ObservableCollection<StandSettingsSolenoidValveModel> SolenoidValveViewModels { get; set; }
    public ObservableCollection<StandSettingsPulseMeterModel> PulseMeterViewModels { get; set; }
    public ObservableCollection<StandSettingsPulseCountMeterModuleModel> PulseCountMeterModuleViewModels { get; set; }
    public ObservableCollection<StandSettingsLineModel> LineViewModels { get; set; }
    public ObservableCollection<StandSettingsPortModel> PortViewModels { get; set; }
    public StandSettingsFrequencyRegulatorModel FrequencyRegulatorViewModel { get; set; }
    public string SelectedEquipmentPortName { get; set; }
    public int TemperatureSensorAddress { get; set; }
    public int PressureSensorAddress { get; set; }
    public int THMeterAddress { get; set; }
    
    public string SelectedTHSensorTypeString{ get; set; }

    public SensorType SelectedTHSensorType { get; set; }
    
    public int PressureResiverSensorAddress { get; set; }
    public int PressureDifferenceSensorAddress { get; set; }
    public string SelectedTemperatureSensorPortName { get; set; }
    public string SelectedPressureSensorPortName { get; set; }
    public string SelectedTHMeterPortName { get; set; }
    public string SelectedPressureResiverSensorPortName { get; set; }
    public string SelectedPressureDifferenceSensorPortName { get; set; }
    public int? StandNumber { get; set; }
    public string ProfileName { get; set; }
    public string SelectedEquipmentPort { get; set; }
    public int SelectedEquipmentBaudRate { get; set; }

    public string ValidationVendorType { get; set; }
    public string ValidationVendorName { get; set; }
    public string ValidationVendorShortName { get; set; }
    public string ValidationVendorUniqueNumber { get; set; }
    public string ValidationVendorAddress { get; set; }
    public string DeviceInfo { get; set; }
    public string VendorName { get; set; }
    public string VendorDate { get; set; }
    public string DeviceRangeInfo { get; set; }
    public string OwnerName { get; set; }
    public string LastValidationInfo { get; set; }
    public string ValidationDeviceInfo { get; set; }
    public string ValidationMethod { get; set; }
    public string TightnessInfo { get; set; }
    public string OutsideCheckInfo { get; set; }
    public string DeviceTestInfo { get; set; }
    public string PostInfo { get; set; }
    
    public  StandSettingsExtensionModel  StandSettingsExtensionViewModel { get; set; }
}