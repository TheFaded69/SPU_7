using System;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using SPU_7.Common.Extensions;
using SPU_7.Common.Line;
using SPU_7.Models.Stand.Settings.Stand.Extensions;

namespace SPU_7.ViewModels.Settings;

public class StandSettingsFanViewModel : ViewModelBase
{
    private int _number;
    private bool _isNeedleValveEnable;
    private bool _isValveEnable;
    private StandSettingsValveViewModel _fanValveViewModel;
    private StandSettingsFrequencyRegulatorViewModel _frequencyRegulatorViewModel;
    private float? _minimumFlow;
    private float? _maximumFlow;
    private StandSettingsNeedleValveViewModel _needleValveViewModel;
    private string _selectedFanTypeString;
    private FanType _selectedFanType;
    private int _address;
    private int _bitIndex;
    private string _selectedComPort;
    private bool _isFrequencyRegulatorSettingsEnable;
    private bool _isControlModuleSettingsEnable;

    public int Number
    {
        get => _number;
        set => SetProperty(ref _number, value);
    }

    public bool IsNeedleValveEnable
    {
        get => _isNeedleValveEnable;
        set
        {
            SetProperty(ref _isNeedleValveEnable, value);
            NeedleValveViewModel = value ? new StandSettingsNeedleValveViewModel() : null;
        }
    }

    public bool IsValveEnable
    {
        get => _isValveEnable;
        set => SetProperty(ref _isValveEnable, value);
    }

    public StandSettingsValveViewModel FanValveViewModel
    {
        get => _fanValveViewModel;
        set => SetProperty(ref _fanValveViewModel, value);
    }

    public StandSettingsFrequencyRegulatorViewModel FrequencyRegulatorViewModel
    {
        get => _frequencyRegulatorViewModel;
        set => SetProperty(ref _frequencyRegulatorViewModel, value);
    }

    public StandSettingsNeedleValveViewModel NeedleValveViewModel
    {
        get => _needleValveViewModel;
        set => SetProperty(ref _needleValveViewModel, value);
    }

    public float? MinimumFlow
    {
        get => _minimumFlow;
        set => SetProperty(ref _minimumFlow, value);
    }

    public float? MaximumFlow
    {
        get => _maximumFlow;
        set => SetProperty(ref _maximumFlow, value);
    }

    public ObservableCollection<string> FanTypesString { get; set; } = new(Enum
        .GetValues<FanType>()
        .Select(ft => ft.GetDescription()));

    public ObservableCollection<string> ComPorts { get; set; }= new(SerialPort.GetPortNames());

    public string SelectedFanTypeString
    {
        get => _selectedFanTypeString;
        set
        {
            SetProperty(ref _selectedFanTypeString, value);

            SelectedFanType = Enum
                .GetValues<FanType>()
                .FirstOrDefault(ft => ft.GetDescription() == value);

            switch (SelectedFanType)
            {
                case FanType.FrequencyControlFan:
                    IsFrequencyRegulatorSettingsEnable = true;
                    IsControlModuleSettingsEnable = false;
                    break;
                case FanType.ControlModuleControlFan:
                    IsFrequencyRegulatorSettingsEnable = false;
                    IsControlModuleSettingsEnable = true;
                    break;
            }
        }
    }

    public FanType SelectedFanType
    {
        get => _selectedFanType;
        set => SetProperty(ref _selectedFanType, value);
    }

    public int Address
    {
        get => _address;
        set => SetProperty(ref _address, value);
    }

    public int BitIndex
    {
        get => _bitIndex;
        set => SetProperty(ref _bitIndex, value);
    }

    public string SelectedComPort
    {
        get => _selectedComPort;
        set => SetProperty(ref _selectedComPort, value);
    }

    public bool IsFrequencyRegulatorSettingsEnable
    {
        get => _isFrequencyRegulatorSettingsEnable;
        set => SetProperty(ref _isFrequencyRegulatorSettingsEnable, value);
    }

    public bool IsControlModuleSettingsEnable
    {
        get => _isControlModuleSettingsEnable;
        set => SetProperty(ref _isControlModuleSettingsEnable, value);
    }
}