using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.MasterDevice.Rabo;

public class RaboDevice : IRaboDevice
{
    public RaboDevice()
    {
        
    }
    
    public RaboDevice(IPressureSensor pressureSensor, ITemperatureSensor temperatureSensor)
    {
        _pressureSensor = pressureSensor;
        _temperatureSensor = temperatureSensor;
    }
    
    private float? _pressure;
    private float? _temperature;
    private float? _flow;

    private float? Pressure
    {
        get => _pressure;
        set
        {
            _pressure = value; 
            NotifyPressureSensorObservers(value);
        }
    }

    private float? Temperature
    {
        get => _temperature;
        set
        {
            _temperature = value;
            NotifyTemperatureSensorObservers(value);
        }
    }
    
    private float? Flow
    {
        get => _flow;
        set
        {
            _flow = value;
            NotifyFlowObservers(value);
        }
    }
    
    private double TargetFlow { get; set; }


    private readonly IPressureSensor _pressureSensor;
    private readonly ITemperatureSensor _temperatureSensor;

    public async Task<float?> ReadPressureAsync()
    {
#if DEBUGGUI
        return Pressure = (float?)new Random().NextDouble() * 1000;
#else
        return Pressure = await _pressureSensor.ReadPressureAsync();
#endif
    }

    public async Task<float?> ReadTemperatureAsync()
    {
#if DEBUGGUI
        return Temperature = (float?)new Random().NextDouble() * 1000;
#else
        return Temperature = await _temperatureSensor.ReadTemperatureAsync(true);
#endif
    }
    
    public float? GetPressureDifference()
    {
        return Pressure;
    }

    public float? GetTemperature()
    {
        return Temperature;
    }

    public float? GetFlow()
    {
        return Flow;
    }

    public void SetTargetFlow(double flow)
    {
        TargetFlow = flow;
    }

    public double GetTargetFlow()
    {
        return TargetFlow;
    }
    
    public async Task<float?> ReadFlowAsync(IPulseCountMeterModule? pulseCountMeterModule,
        int? pulseCountMeterModuleChannelNumber, float pulseWeight)
    {
        var currentFrequency = await pulseCountMeterModule.ReadAverageFrequencyAsync((ChannelNumber)pulseCountMeterModuleChannelNumber);
        
        return Flow = currentFrequency * 3600 * pulseWeight;
    }
    
    #region Observable
    
    private List<IPressureSensorObserver> _pressureSensorObservers = [];
    private List<ITemperatureSensorObserver> _temperatureSensorObservers = [];
    
    public void RegisterPressureSensorObserver(IPressureSensorObserver observer)
    {
        _pressureSensorObservers.Add(observer);
    }

    public void RemovePressureSensorObserver(IPressureSensorObserver observer)
    {
        _pressureSensorObservers.Remove(observer);
    }

    public void NotifyPressureSensorObservers(object? obj)
    {
        foreach (var pressureSensorObserver in _pressureSensorObservers)
        {
            pressureSensorObserver.UpdatePressure(obj);
        }
    }

    public void RegisterTemperatureSensorObserver(ITemperatureSensorObserver observer)
    {
        _temperatureSensorObservers.Add(observer);
    }

    public void RemoveTemperatureSensorObserver(ITemperatureSensorObserver observer)
    {
        _temperatureSensorObservers.Remove(observer);
    }

    public void NotifyTemperatureSensorObservers(object? obj)
    {
        foreach (var temperatureSensorObserver in _temperatureSensorObservers)
        {
            temperatureSensorObserver.UpdateTemperature(obj);
        }
    }
    
    private List<IFlowObserver> _flowObservers = [];
    
    public void RegisterFlowObserver(IFlowObserver observer)
    {
        _flowObservers.Add(observer);
    }

    public void RemoveFlowObserver(IFlowObserver observer)
    {
        _flowObservers.Remove(observer);
    }

    public void NotifyFlowObservers(object? obj)
    {
        foreach (var flowObserver in _flowObservers)        
        {
            flowObserver.UpdateFlow(obj);
        }
    }
    
    #endregion
}