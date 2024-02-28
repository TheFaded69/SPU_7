using System.Collections.ObjectModel;
using System.Linq;
using SPU_7.Extensions;
using SPU_7.Models;

namespace SPU_7.ViewModels;

public class DeviceInformationViewModel : ViewModelBase, IObserver
{
    public DeviceInformationViewModel()
    {
        Parameters = new ObservableCollection<DeviceInfoParameterViewModel>();
        //todo тестовый набор параметров для отладки GUI
        /*Parameters.Add(new DeviceInfoParameterViewModel {Name = "SPI2.4", IsVisible = true, ParameterType = DeviceInfoParameterType.DeviceType});
        Parameters.Add(new DeviceInfoParameterViewModel {Name = "P, МПа", Value = 45.56, IsVisible = true, ParameterType = DeviceInfoParameterType.Pressure});
        Parameters.Add(new DeviceInfoParameterViewModel {Name = "T, C", Value = 25, IsVisible = true, ParameterType = DeviceInfoParameterType.Temperature});
        Parameters.Add(new DeviceInfoParameterViewModel {Name = "F, КГц", Value = 1000, ParameterType = DeviceInfoParameterType.Frequency});
        Parameters.Add(new DeviceInfoParameterViewModel {Name = "хз чо", Value = 12345, ParameterType = DeviceInfoParameterType.DeviceType});*/
    }
    
    private bool _isSettingsMode;
    
    public ObservableCollection<DeviceInfoParameterViewModel> Parameters { get; set; }

    public bool IsSettingsMode
    {
        get => _isSettingsMode;
        set => SetProperty(ref _isSettingsMode, value);
    }

    public void Update(object obj)
    {
        if (obj is DataPair { DataType: var deviceInfoParameterType } dataPair)
        {
            Parameters.FirstOrDefault(p => p.ParameterType == deviceInfoParameterType)!.Value = dataPair.Data;
        }
    }
}