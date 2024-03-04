using System;
using System.Collections.ObjectModel;
using SPU_7.Common.Settings;
using SPU_7.Common.Stand;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.Models.Stand.Settings.Stand;

public class StandSettingsModel
{
    public Guid Id { get; set; }
    public ObservableCollection<StandSettingsNozzleModel> NozzleViewModels { get; set; }
    public ObservableCollection<StandSettingsValveModel> ValveViewModels { get; set; }
    public ObservableCollection<StandSettingsSolenoidValveModel> SolenoidValveViewModels { get; set; }
    public ObservableCollection<StandSettingsPulseMeterModel> PulseMeterViewModels { get; set; }
    public ObservableCollection<StandSettingsLineModel> LineViewModels { get; set; }

    public int TemperatureSensorAddress { get; set; }
    public int PressureSensorAddress { get; set; }
    public int THMeterAddress { get; set; }
    public int PressureResiverSensorAddress { get; set; }
    public int PressureDifferenceSensorAddress { get; set; }
    public int? StandNumber { get; set; }
    public StandType StandType { get; set; }
    public NozzleManualType NozzleManualType { get; set; }
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
}