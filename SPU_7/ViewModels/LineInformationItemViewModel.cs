using System;

namespace SPU_7.ViewModels;

public class LineInformationItemViewModel : ViewModelBase
{
    public LineInformationItemViewModel()
    {
        
    }
    private float? _pressureDifference;
    private float? _pressure;
    private float? _pressureDischarged;
    private float? _currentFlow;
    private float? _temperature;
    private string _lineName;
    private bool _lineVisible = true;

    public string LineName
    {
        get => _lineName;
        set => SetProperty(ref _lineName, value);
    }

    public bool LineVisible
    {
        get => _lineVisible;
        set => SetProperty(ref _lineVisible, value);
    }

    public float? PressureDifference
    {
        get => _pressureDifference;
        set => SetProperty(ref _pressureDifference, value == null ? value : (float?)Math.Round((float)value / 1000, 3));
    }
    
    public float? Pressure
    {
        get => _pressure;
        set => SetProperty(ref _pressure, value == null ? value : (float?)Math.Round((float)value, 3));
    }

    public float? PressureDischarged
    {
        get => _pressureDischarged;
        set => SetProperty(ref _pressureDischarged, value == null ? value : (float?)Math.Round((float)value / 1000, 3));
    }

    public float? CurrentFlow
    {
        get => _currentFlow;
        set => SetProperty(ref _currentFlow, value);
    }

    public float? Temperature
    {
        get => _temperature;
        set => SetProperty(ref _temperature, value);
    }
}