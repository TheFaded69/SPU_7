using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.MasterDevice.GFG;

public class GfgDevice : ModbusUnitProcessor<GFGRegisterMap>, IGFGDevice
{
    public GfgDevice(IModbusProcessor modbusProcessor, IRegisterMapEnum<GFGRegisterMap> registerMap)
        : base(modbusProcessor, registerMap)
    {
    }
    
    public GfgDevice(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<GFGRegisterMap> registerMap, 
        IPressureSensor pressureSensor, 
        ITemperatureSensor temperatureSensor) : base(modbusProcessor, registerMap)
    {
        _pressureSensor = pressureSensor;
        _temperatureSensor = temperatureSensor;
    }
    private float? _pressure;
    private float? _temperature;
    
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
        return Pressure = (float?)new Random().NextDouble() * 1000;
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
    
    #endregion
}