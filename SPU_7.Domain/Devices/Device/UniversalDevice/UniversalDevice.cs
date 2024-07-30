using SPU_7.Common.Device;
using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.PulseMeter;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.Device.UniversalDevice;

public class UniversalDevice : ModbusUnitProcessor<UniversalDeviceRegisterMap>, IUniversalDevice,  IDeviceObservable
{
    public UniversalDevice(IModbusProcessor modbusProcessor, IRegisterMapEnum<UniversalDeviceRegisterMap> registerMap) : base(modbusProcessor, registerMap)
    {
        
    }
    public UniversalDevice(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<UniversalDeviceRegisterMap> registerMap,
        IModbusProcessor? pressureSensorModbusProcessor,
        int pressureSensorAddress,
        IModbusProcessor? pulseMeterModbusProcessor,
        int pulseMeterAddress,
        int pulseMeterChannel) : base(modbusProcessor, registerMap)
    {
        _pulseMeter2Channel = pulseMeterModbusProcessor == null
            ? null
            : new PulseMeter2Channel(pulseMeterModbusProcessor, new RegisterMapEnum<PulseMeter2ChannelRegisterMap>(),
                pulseMeterAddress);
        _pulseMeterChannelType = pulseMeterChannel switch
        {
            1 => PulseMeterChannel.Channel1,
            2 => PulseMeterChannel.Channel2,
            _ => throw new ArgumentOutOfRangeException(),
        };
        _pressureSensor = pressureSensorModbusProcessor == null
            ? null
            : new PressureSensor(pressureSensorModbusProcessor, new RegisterMapEnum<PressureSensorRegisterMap>(),
                (byte)pressureSensorAddress);}

    private readonly IPressureSensor _pressureSensor;
    private readonly ITemperatureSensor _temperatureSensor;
    private readonly IPulseMeter2Channel? _pulseMeter2Channel;
    private readonly PulseMeterChannel _pulseMeterChannelType;

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

    public bool IsManualEnabled
    {
        get => _isManualEnabled;
        set
        {
            _isManualEnabled = value;
            NotifyDeviceObservers(value);
        }
    }

    public string VendorNumberString
    {
        get => _vendorNumberString;
        set
        {
            _vendorNumberString = value;
            NotifyDeviceObservers(value);
        }
    }

    public string DeviceName { get; set; }

    public string VendorName { get; set; }
    public string DeviceTypeInfo { get; set; }
    public int PulseMeterNumber { get; set; }

    public async Task<bool> ResetToZeroAsync() => await _pressureSensor.ResetToZeroAsync();

    public async Task<bool> SetPulseCountAsync(int pulseCount)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> ReadPulseCountAsync()
    {
        throw new NotImplementedException();
    }
    
    public async Task<bool> StartPeriodMeasureAsync(int pulseCount) 
        => await _pulseMeter2Channel?.StartPeriodMeasureAsync(_pulseMeterChannelType, (uint)pulseCount);

    public async Task<PulseMeter2ChannelState> ReadChannelStatusAsync() 
        => await _pulseMeter2Channel.GetChannelStatusAsync(_pulseMeterChannelType);

    public async Task<uint?> GetStartMeasureTimeAsync()
        => await _pulseMeter2Channel.GetStartMeasureTimeAsync(_pulseMeterChannelType);

    public async Task<uint?> GetEndMeasureTimeAsync()
        => await _pulseMeter2Channel.GetEndMeasureTimeAsync(_pulseMeterChannelType);

    public async Task<(float?, float?)> ReadPulseCoefficientsAsync()
    {
        var firstCoefficient = await _pulseMeter2Channel.GetCorrectedAveargePeriodAsync(PulseMeterChannel.Channel1);
        var secondCoefficient = await _pulseMeter2Channel.GetCorrectedAveargePeriodAsync(PulseMeterChannel.Channel2);

        return (firstCoefficient, secondCoefficient);
    }

    public Task<bool> WritePulseCoefficientsAsync(float firstCoefficient, float secondCoefficient)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> SetPulseTimeOutAsync(int i)
    {
        return await _pulseMeter2Channel.SetTimeOutValueAsync(i);
    }

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
        return Temperature = await _temperatureSensor.ReadTemperatureAsync();
#endif
    }

    #region PresureSensorObserve

    private List<IPressureSensorObserver> _pressureObservers = new();
    private float? _pressure;
    public void RegisterPressureSensorObserver(IPressureSensorObserver observer) => _pressureObservers.Add(observer);
    public void RemovePressureSensorObserver(IPressureSensorObserver observer) => _pressureObservers.Remove(observer);
    public void NotifyPressureSensorObservers(object? obj) => _pressureObservers.ForEach(ob => ob.UpdatePressure(obj));
    
    private List<ITemperatureSensorObserver> _temperatureSensorObservers = [];
    public void RegisterTemperatureSensorObserver(ITemperatureSensorObserver observer) => _temperatureSensorObservers.Add(observer);
    public void RemoveTemperatureSensorObserver(ITemperatureSensorObserver observer) => _temperatureSensorObservers.Remove(observer);
    public void NotifyTemperatureSensorObservers(object? obj) => _temperatureSensorObservers.ForEach(ob => ob.UpdateTemperature(obj));
    
    #endregion

    #region DeviceObserve

    private List<IDeviceObserver> _deviceObservers = new();
    private string _vendorNumberString;
    private bool _isManualEnabled;
    private float? _temperature;
    public void RegisterDeviceObserver(IDeviceObserver observer) => _deviceObservers.Add(observer);
    public void RemoveDeviceObserver(IDeviceObserver observer) => _deviceObservers.Remove(observer);
    public void NotifyDeviceObservers(object? obj) => _deviceObservers.ForEach(ob => ob.UpdateDeviceInformation(obj));

    #endregion
}