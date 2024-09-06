using System.Collections.Generic;
using System.Collections.ObjectModel;
using SPU_7.ViewModels;

namespace SPU_7.Models.Services.DbServices;

public interface IDeviceNameDbService
{
    List<DeviceNameViewModel> GetDeviceNames();

    void UpdateDeviceNames(ObservableCollection<DeviceNameViewModel> names);
    
    void AddDeviceName(DeviceNameViewModel name);

    void DeleteDeviceName(DeviceNameViewModel name);
}