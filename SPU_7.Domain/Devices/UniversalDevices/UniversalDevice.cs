using SPU_7.Common.Device;
using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.PulseMeter;
using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Modbus.Processor;

namespace SPU_7.Domain.Devices.UniversalDevices;

public class UniversalDevice : ModbusUnitProcessor<UniversalDeviceRegisterMap>, IUniversalDevice, IPressureSensorObservable, IDeviceObservable
{
    public UniversalDevice(IModbusProcessor modbusProcessor,
        IRegisterMapEnum<UniversalDeviceRegisterMap> registerMap,
        int pressureSensorAddress, int pulseMeterAddress, int pulseMeterChannel) : base(modbusProcessor, registerMap)
    {
        DeviceGroupType = DeviceGroupType.Universal;

        _pulseMeter2Channel = new PulseMeter2Channel(modbusProcessor, new RegisterMapEnum<PulseMeter2ChannelRegisterMap>(), pulseMeterAddress);
        _pulseMeterChannel = pulseMeterChannel switch
        {
            1 => PulseMeterChannel.Channel1,
            2 => PulseMeterChannel.Channel2,
            _ => PulseMeterChannel.None
        };
        _pressureSensor = new PressureSensor(modbusProcessor, new RegisterMapEnum<PressureSensorRegisterMap>(), (byte)pressureSensorAddress);
    }

    private readonly IPressureSensor _pressureSensor;
    private readonly PulseMeterChannel _pulseMeterChannel;
    private readonly IPulseMeter2Channel _pulseMeter2Channel;

    private float? Pressure
    {
        get => _pressure;
        set
        {
            _pressure = value;
            NotifyPressureSensorObservers(value);
        }
    }

    public DeviceType DeviceType { get; set; }
    public DeviceGroupType DeviceGroupType { get; set; }
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

    public Task<bool> SetPasswordAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckConnectionWithDeviceAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeactivateDeviceAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CalibrateDeviceAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> ProgrammingDeviceAsync(byte[] firmware)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ValidationDeviceAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SynchronizeTimeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckValveAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckConnectionAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> CheckPlatformAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetRangeAsync(float value)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SetSensitivityAsync(ushort value, ushort comparatorMax, ushort comparatorMin)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ResetToZeroAsync() => await _pressureSensor.ResetToZeroAsync();
    public async Task<bool> SetPulseCountAsync(int pulseCount)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> ReadPulseCountAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<float?> ReadPressureAsync()
    {
#if DEBUGGUI
        return Pressure = (float?)new Random().NextDouble() * 1000;
#else
        return Pressure = await _pressureSensor.ReadPressureAsync();
#endif
    }

    #region PresureSensorObserve
    
    private List<IPressureSensorObserver> _pressureObservers = new();
    private float? _pressure;
    public void RegisterPressureSensorObserver(IPressureSensorObserver observer) => _pressureObservers.Add(observer);
    public void RemovePressureSensorObserver(IPressureSensorObserver observer) => _pressureObservers.Remove(observer);
    public void NotifyPressureSensorObservers(object? obj) => _pressureObservers.ForEach(ob => ob.UpdatePressure(obj));

    #endregion

    #region DeviceObserve
    
    private List<IDeviceObserver> _deviceObservers = new();
    private string _vendorNumberString;
    private bool _isManualEnabled;
    public void RegisterDeviceObserver(IDeviceObserver observer) => _deviceObservers.Add(observer);
    public void RemoveDeviceObserver(IDeviceObserver observer) => _deviceObservers.Remove(observer);
    public void NotifyDeviceObservers(object? obj) => _deviceObservers.ForEach(ob => ob.UpdateDeviceInformation(obj));
    
    #endregion
}