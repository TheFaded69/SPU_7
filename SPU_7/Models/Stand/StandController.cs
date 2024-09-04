using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SPU_7.Common.Device;
using SPU_7.Common.Line;
using SPU_7.Common.Modbus;
using SPU_7.Common.Stand;
using SPU_7.CommonDevice.Devices;
using SPU_7.CommonDevice.Devices.ElmetroPascal;
using SPU_7.DeviceCommunication.Communication;
using SPU_7.Domain.Devices.Device.UniversalDevice;
using SPU_7.Domain.Devices.StandDevices.FrequencyRegulator;
using SPU_7.Domain.Devices.StandDevices.NeedleValveController;
using SPU_7.Domain.Devices.StandDevices.Owen;
using SPU_7.Domain.Devices.StandDevices.Owen.OwenPBR10A;
using SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;
using SPU_7.Domain.Devices.StandDevices.PulseMeter;
using SPU_7.Domain.Devices.StandDevices.THMeter;
using SPU_7.Domain.Extensions;
using SPU_7.Domain.Modbus;
using SPU_7.Domain.Stands;
using SPU_7.Extensions;
using SPU_7.Modbus.Processor;
using SPU_7.Modbus.Processor.Communicators;
using SPU_7.Modbus.Requests;
using SPU_7.Modbus.Responses;
using SPU_7.Models.Services.Logger;
using SPU_7.Models.Services.StandSetting;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.Models.Stand.StaticData;
using SPU_7.ViewModels;
using SPU_7.ViewModels.DeviceInformationViewModels;

namespace SPU_7.Models.Stand
{
    public class StandController : IStandController
    {
        public StandController(ILogger logger, IStandSettingsService settingsService)
        {
            _logger = logger;
            _settingsService = settingsService;
        }

        private readonly ILogger _logger;
        private readonly IStandSettingsService _settingsService;

        private List<IModbusProcessor> _modbusProcessors = [];
        private List<StandLine> _lines = [];
        private StandLine _line => SelectedLineIndex == null ? null : _lines[(int)SelectedLineIndex];
        private List<StandDevice> _standDevices = [];
        private List<StandDevice> _standDevicesForFan = [];

        private IFrequencyRegulatorDevice _frequencyRegulatorDevice;
        private List<IFrequencyRegulatorDevice> _frequencyRegulatorDevices = [];
        private List<IPulseCountMeterModule> _pulseCountMeterModules = [];
        private List<IPulseMeter2Channel> _pulseMeter2Channels = [];
        private List<INeedleValveController> _needleValveControllers = [];
        private List<IOwen> _owens = [];
        private PulseCountMeterStarter _pulseCountMeterStarter;

        private ITemperatureHumiditySensor _temperatureHumiditySensor;
        private ElmetroDigitalDevice _elmetroDigitalDevice;

        private ObservableCollection<LogMessage> _portLogMessages;

        private CancellationTokenSource _requestTaskCancellationTokenSource;
        private Task _requestTask;
        private bool _isTaskExecute;

        private int? _selectedLineIndex;

        #region Инициализация

        /// <summary>
        /// Инициализация устройств стенда
        /// </summary>
        public void Initialization()
        {
            foreach (var portViewModel in _settingsService.StandSettingsModel.PortViewModels)
            {
                if (portViewModel.PortName ==
                    _settingsService.StandSettingsModel.SelectedPressureSensorPortName) continue;

                var modbusProcessor = new ModbusProcessor(new RequestSerializer(), new ResponseDeserializer())
                {
                    Communicator = new SerialCommunicator(),
                    PortName = portViewModel.PortName,
                };
                ((ISerialCommunicator)modbusProcessor.Communicator).SetSerialPort(portViewModel.PortName,
                    portViewModel.PortBaudRate,
                    8,
                    portViewModel.SelectedParity,
                    portViewModel.SelectedStopBit,
                    Handshake.None,
                    false,
                    false);
                modbusProcessor.ProtocolSettings = new ProtocolSettings
                {
                    Preamble = [0xFF],
                    DelayAfterPreamble = 10,
                    IsPreambleNeed = false,
                    ReadTimeout = 5000,
                    WriteTimeout = 5000,
                    AttemptCount = 3,
                    IsPoolingNeed = false,
                    PoolingPeriod = 5000
                };

                modbusProcessor.Start();
                _modbusProcessors.Add(modbusProcessor);
            }

            IRegisterMapEnum<StandDeviceRegisterMap> standDeviceRegisterMap =
                new RegisterMapEnum<StandDeviceRegisterMap>();

            var addressList = new List<int>();
            var addressFanList = new List<int>();

            foreach (var nozzleViewModel in _settingsService.StandSettingsModel.NozzleViewModels)
            {
                if (nozzleViewModel.Address != null && !addressList.Contains((int)nozzleViewModel.Address))
                    addressList.Add((int)nozzleViewModel.Address);

                if (nozzleViewModel.StateAddress != null && !addressList.Contains((int)nozzleViewModel.StateAddress))
                    addressList.Add((int)nozzleViewModel.StateAddress);
            }

            foreach (var valveViewModel in _settingsService.StandSettingsModel.ValveViewModels)
            {
                if (valveViewModel.Address != null && !addressList.Contains((int)valveViewModel.Address))
                    addressList.Add((int)valveViewModel.Address);

                if (valveViewModel.StateOnAddress != null && !addressList.Contains((int)valveViewModel.StateOnAddress))
                    addressList.Add((int)valveViewModel.StateOnAddress);
            }

            foreach (var lineViewModel in _settingsService.StandSettingsModel.LineViewModels)
            {
                if (lineViewModel.DeviceViewModels != null)
                    foreach (var deviceViewModel in lineViewModel.DeviceViewModels)
                    {
                        if (deviceViewModel.Address != null && !addressList.Contains((int)deviceViewModel.Address))
                            addressList.Add((int)deviceViewModel.Address);

                        if (deviceViewModel.StateAddress != null &&
                            !addressList.Contains((int)deviceViewModel.StateAddress))
                            addressList.Add((int)deviceViewModel.StateAddress);
                    }

                if (lineViewModel.NozzleViewModels != null)
                    foreach (var nozzleViewModel in lineViewModel.NozzleViewModels)
                    {
                        if (nozzleViewModel.Address != null && !addressList.Contains((int)nozzleViewModel.Address))
                            addressList.Add((int)nozzleViewModel.Address);

                        if (nozzleViewModel.StateAddress != null &&
                            !addressList.Contains((int)nozzleViewModel.StateAddress))
                            addressList.Add((int)nozzleViewModel.StateAddress);
                    }

                if (lineViewModel.FanViewModels != null)
                    foreach (var fanViewModel in lineViewModel.FanViewModels)
                    {
                        switch (fanViewModel.SelectedFanType)
                        {
                            case FanType.FrequencyControlFan:
                                if (fanViewModel.FanValveViewModel.Address != null &&
                                    !addressList.Contains((int)fanViewModel.FanValveViewModel.Address))
                                    addressList.Add((int)fanViewModel.FanValveViewModel.Address);
                                if (fanViewModel.FanValveViewModel.StateOnAddress != null &&
                                    !addressList.Contains((int)fanViewModel.FanValveViewModel.StateOnAddress))
                                    addressList.Add((int)fanViewModel.FanValveViewModel.StateOnAddress);
                                if (fanViewModel.FanValveViewModel.StateOffAddress != null &&
                                    !addressList.Contains((int)fanViewModel.FanValveViewModel.StateOffAddress))
                                    addressList.Add((int)fanViewModel.FanValveViewModel.StateOffAddress);

                                _frequencyRegulatorDevices.Add(new FrequencyRegulatorDevice(
                                    _modbusProcessors.FirstOrDefault(mb =>
                                        mb.PortName == fanViewModel.FrequencyRegulatorViewModel.PortName),
                                    new RegisterMapEnum<FrequencyRegulatorRegisterMap>(),
                                    fanViewModel.FrequencyRegulatorViewModel.ModuleAddress));
                                break;
                            case FanType.ControlModuleControlFan:
                                if (fanViewModel.Address != null && !addressFanList.Contains((int)fanViewModel.Address))
                                    addressFanList.Add((int)fanViewModel.Address);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                if (lineViewModel.MasterDeviceViewModels != null)
                    foreach (var masterDeviceViewModel in lineViewModel.MasterDeviceViewModels)
                    {
                        if (masterDeviceViewModel.PressureSensorValveViewModel.Address != null &&
                            !addressList.Contains((int)masterDeviceViewModel.PressureSensorValveViewModel.Address))
                            addressList.Add((int)masterDeviceViewModel.PressureSensorValveViewModel.Address);
                        if (masterDeviceViewModel.PressureSensorValveViewModel.StateOnAddress != null &&
                            !addressList.Contains(
                                (int)masterDeviceViewModel.PressureSensorValveViewModel.StateOnAddress))
                            addressList.Add((int)masterDeviceViewModel.PressureSensorValveViewModel.StateOnAddress);
                        if (masterDeviceViewModel.PressureSensorValveViewModel.StateOffAddress != null &&
                            !addressList.Contains((int)masterDeviceViewModel.PressureSensorValveViewModel
                                .StateOffAddress))
                            addressList.Add((int)masterDeviceViewModel.PressureSensorValveViewModel.StateOffAddress);

                        if (masterDeviceViewModel.MasterDeviceValveViewModel.Address != null &&
                            !addressList.Contains((int)masterDeviceViewModel.MasterDeviceValveViewModel.Address))
                            addressList.Add((int)masterDeviceViewModel.MasterDeviceValveViewModel.Address);
                        if (masterDeviceViewModel.MasterDeviceValveViewModel.StateOnAddress != null &&
                            !addressList.Contains((int)masterDeviceViewModel.MasterDeviceValveViewModel.StateOnAddress))
                            addressList.Add((int)masterDeviceViewModel.MasterDeviceValveViewModel.StateOnAddress);
                        if (masterDeviceViewModel.MasterDeviceValveViewModel.StateOffAddress != null &&
                            !addressList.Contains((int)masterDeviceViewModel.MasterDeviceValveViewModel
                                .StateOffAddress))
                            addressList.Add((int)masterDeviceViewModel.MasterDeviceValveViewModel.StateOffAddress);
                    }

                if (lineViewModel.FanViewModels != null)
                    foreach (var fanViewModel in lineViewModel.FanViewModels)
                    {
                        switch (fanViewModel.SelectedFanType)
                        {
                            case FanType.FrequencyControlFan:
                                if (fanViewModel.NeedleValveViewModel != null)
                                {
                                    _needleValveControllers.Add(new NeedleValveController(_modbusProcessors.First(
                                            modbus =>
                                                modbus.PortName == fanViewModel.NeedleValveViewModel.SelectedComPort),
                                        new RegisterMapEnum<NeedleValveControllerRegisterMap>(),
                                        fanViewModel.NeedleValveViewModel.ModuleAddress)
                                    {
                                        PortName = fanViewModel.NeedleValveViewModel.SelectedComPort
                                    });
                                }

                                break;
                            case FanType.ControlModuleControlFan:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }

                if (lineViewModel.IsDropValveEnable)
                {
                    _owens.Add(new OwenPBR10ADevice(
                        _modbusProcessors.FirstOrDefault(mp =>
                            mp.PortName == lineViewModel.DropValveViewModel.SelectedPort),
                        new RegisterMapEnum<OwenPBR10ARegisterMap>(),
                        (int)lineViewModel.DropValveViewModel.Address,
                        _owens.Count + 1));
                }
            }

            foreach (var pulseCountMeterModuleViewModel in _settingsService.StandSettingsModel
                         .PulseCountMeterModuleViewModels)
            {
                _pulseCountMeterModules.Add(new PulseCountMeterModule(_modbusProcessors.First(modbus =>
                        modbus.PortName == pulseCountMeterModuleViewModel.PortName),
                    new RegisterMapEnum<PulseMeterCountModuleRegisterMap>(),
                    pulseCountMeterModuleViewModel.ModuleAddress)
                {
                    PulseCountMeterModuleNumber = pulseCountMeterModuleViewModel.Number,
                });
            }

            foreach (var pulseMeterModel in _settingsService.StandSettingsModel
                         .PulseMeterViewModels)
            {
                _pulseMeter2Channels.Add(new PulseMeter2Channel(_modbusProcessors.First(modbus =>
                        modbus.PortName == pulseMeterModel.SelectedComPort),
                    new RegisterMapEnum<PulseMeter2ChannelRegisterMap>(),
                    (int)pulseMeterModel.Address)
                {
                    PulseMeterModuleNumber = pulseMeterModel.Number,
                });
            }

            if (_settingsService.StandSettingsModel.PulseCountMeterModuleViewModels.Count > 0)
                _pulseCountMeterStarter = new PulseCountMeterStarter(_modbusProcessors.First(mb => mb.PortName ==
                        _settingsService.StandSettingsModel.PulseCountMeterModuleViewModels.First().PortName),
                    new RegisterMapEnum<PulseCountMeterStarterRegisterMap>());


            foreach (var solenoidValveViewModel in _settingsService.StandSettingsModel.SolenoidValveViewModels)
            {
                if (solenoidValveViewModel.Address != null &&
                    !addressList.Contains((int)solenoidValveViewModel.Address))
                    addressList.Add((int)solenoidValveViewModel.Address);
            }

            for (var i = 0; i < _settingsService.StandSettingsModel.LineViewModels.Count; i++)
            {
                _lines.Add(new StandLine(_settingsService, _modbusProcessors, i));
            }

            foreach (var address in addressList)
            {
                _standDevices.Add(new StandDevice(
                    _modbusProcessors.First(modbus =>
                        modbus.PortName == _settingsService.StandSettingsModel.SelectedEquipmentPortName),
                    standDeviceRegisterMap,
                    address));
            }

            foreach (var address in addressFanList)
            {
                _standDevicesForFan.Add(new StandDevice(
                    _modbusProcessors.First(mb => mb.PortName == _settingsService.StandSettingsModel.LineViewModels
                        .Where(line => line.DropValveViewModel != null)
                        .Select(line => line.DropValveViewModel.SelectedPort)
                        .FirstOrDefault(port => port == mb.PortName)),
                    standDeviceRegisterMap,
                    address));
            }

            if (!string.IsNullOrEmpty(_settingsService.StandSettingsModel.SelectedPressureSensorPortName))
                _elmetroDigitalDevice = new ElmetroDigitalDevice(new SerialPortCommunication(new SerialPort()
                {
                    PortName = _settingsService.StandSettingsModel.SelectedPressureSensorPortName
                }));

            _temperatureHumiditySensor = _settingsService.StandSettingsModel.SelectedTHSensorType switch
            {
                SensorType.IVTM7 => new IVTMSensor(
                    _modbusProcessors.First(
                        mb => mb.PortName == _settingsService.StandSettingsModel.SelectedTHMeterPortName),
                    new RegisterMapEnum<IVTMSensorRegisterMap>(),
                    _settingsService.StandSettingsModel.THMeterAddress),
                SensorType.IVA => new TemperatureHumiditySensor(
                    _modbusProcessors.First(
                        mb => mb.PortName == _settingsService.StandSettingsModel.SelectedTHMeterPortName),
                    new RegisterMapEnum<TemperatureHumiditySensorRegisterMap>(),
                    _settingsService.StandSettingsModel.THMeterAddress),
                _ => throw new ArgumentOutOfRangeException()
            };

            _requestTaskCancellationTokenSource = new CancellationTokenSource();
            _requestTask = new Task(RequestTaskHandler, _requestTaskCancellationTokenSource.Token);
            _requestTask.Start();
        }

        public async Task<bool> SetModeMeasureAsync(ModeMeasure modeMeasure)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SetPerfectFlowAsync(double flowValue)
        {
            throw new NotImplementedException();
        }

        public async Task PauseBeforeCalibration(int delay)
        {
            await Task.Delay(delay);
        }

        public double PerfectFlowCalculate(double flowValue)
        {
            //var perfectFlowCalc = flowValue * Math.Sqrt((273.15 + temperatyra_neIVTM) / 293.15) * (1 - (perepad / atmDavl)) * (1 / koef_vl[h_temp, Convert.ToInt16(vlashnostR / 10 - 3)]);

            throw new NotImplementedException();
        }

        #endregion

        #region Считывание данных для отображения и проверок

        private async void RequestTaskHandler()
        {
            try
            {
                _isTaskExecute = true;

                while (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                {
#if DEBUGGUI
#else
                    if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                        PressureAtmosphere =
                            await _elmetroDigitalDevice.ReadPressureAsync(PressureType.AbsolutePressure);

                    switch (_settingsService.StandSettingsModel.SelectedTHSensorType)
                    {
                        case SensorType.IVTM7:
                            if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                                Temperature = _temperatureHumiditySensor == null
                                    ? null
                                    : await _temperatureHumiditySensor.ReadTemperatureAsync();
                            if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                                Humidity = _temperatureHumiditySensor == null
                                    ? null
                                    : await _temperatureHumiditySensor.ReadHumidityAsync();
                            break;
                        case SensorType.IVA:
                            if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                                Temperature = _temperatureHumiditySensor == null
                                    ? null
                                    : await _temperatureHumiditySensor.ReadTemperatureAsync() / 100f;
                            if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                                Humidity = _temperatureHumiditySensor == null
                                    ? null
                                    : await _temperatureHumiditySensor.ReadHumidityAsync() / 100f;
                            break;
                    }


                    for (var lineIndex = 0; lineIndex < _lines.Count; lineIndex++)
                    {
                        var standLine = _lines[lineIndex];
                        for (var deviceIndex = 0; deviceIndex < standLine.Devices.Count; deviceIndex++)
                        {
                            switch (_settingsService.StandSettingsModel.LineViewModels[lineIndex].SelectedLineType)
                            {
                                case LineType.None:
                                    break;
                                case LineType.MasterDeviceLineType:
                                {
                                    var device = standLine.Devices[deviceIndex];
                                    await device.ReadPressureAsync();
                                    await device.ReadTemperatureAsync();
                                }
                                    break;
                                case LineType.NozzleLineType:
                                {
                                    var device = standLine.Devices[deviceIndex];
                                    device.Temperature = Temperature;

                                    if (deviceIndex == 0)
                                    {
                                        var firstPressure = await _lines
                                            .FirstOrDefault(l => l.Devices.Count == 1).Devices[0]
                                            .ReadPressureAsync();

                                        device.Pressure = firstPressure;
                                    }
                                    else
                                    {
                                        await device.ReadPressureAsync();
                                    }
                                }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        for (var masterDeviceIndex = 0;
                             masterDeviceIndex < standLine.MasterDevices.Count;
                             masterDeviceIndex++)
                        {
                            var masterDevice = standLine.MasterDevices[masterDeviceIndex];
                            await masterDevice.ReadTemperatureAsync();
                            await masterDevice.ReadPressureAsync();
                            await masterDevice.ReadFlowAsync(_pulseCountMeterModules.FirstOrDefault(pcm =>
                                    pcm.PulseCountMeterModuleNumber ==
                                    _settingsService.StandSettingsModel.LineViewModels[lineIndex]
                                        .MasterDeviceViewModels[masterDeviceIndex].PulseCountMeterModuleNumber),
                                _settingsService.StandSettingsModel.LineViewModels[lineIndex]
                                    .MasterDeviceViewModels[masterDeviceIndex].PulseCountMeterModuleChannelNumber,
                                _settingsService.StandSettingsModel.LineViewModels[lineIndex]
                                    .MasterDeviceViewModels[masterDeviceIndex].PulseWeight);
                        }

                        if (standLine.TemperatureSensor != null)
                        {
                            var temperature = await standLine.TemperatureSensor.ReadTemperatureAsync(true);
                            NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, temperature),
                                DeviceInfoParameterType.TemperatureTube));
                        }

                        if (standLine.PressureSensor != null)
                        {
                            switch (_settingsService.StandSettingsModel.LineViewModels[lineIndex].SensorViewModels
                                        .FirstOrDefault(s => s.SensorPurpose == SensorPurpose.PressureSensor)
                                        .SensorType)
                            {
                                case SensorType.TurboFlowPS:
                                {
                                    var pressure =
                                        await ((SPU_7.Domain.Devices.StandDevices.PressureSensor.IPressureSensor)
                                            standLine.PressureSensor).ReadPressureAsync();
                                    NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, pressure),
                                        DeviceInfoParameterType.Pressure));
                                }
                                    break;
                                case SensorType.Pascal04:
                                {
                                    var pressure = PressureAtmosphere;
                                    NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, pressure),
                                        DeviceInfoParameterType.Pressure));
                                }
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        if (standLine.PressureDifferenceSensor != null)
                        {
                            var pressureDifference = await standLine.PressureDifferenceSensor.ReadPressureAsync();
                            NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, pressureDifference),
                                DeviceInfoParameterType.PressureDifference));
                        }

                        if (standLine.PressureDischargeSensor != null)
                        {
                            var pressureResiver = await standLine.PressureDischargeSensor.ReadPressureAsync();
                            NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, pressureResiver),
                                DeviceInfoParameterType.PressureResiver));
                        }

                        if (standLine.CurrentFlow < _settingsService.StandSettingsModel.LineViewModels[lineIndex]
                                .NozzleViewModels.Min(noz => noz.NozzleFactValue * 0.1))
                            standLine.CurrentFlow = 0;
                        var k = SelectMetrologyCoefficient(Temperature, Humidity);
                        
                        var flowK = PressureAtmosphere / (PressureAtmosphere - standLine.Devices.Last().Pressure / 1000);
                        NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, flowK),
                            DeviceInfoParameterType.CoefficientOfCriticalMode));
                        
                        var isNeedFlow = standLine.CurrentFlow switch
                        {
                            < 1 => flowK >= 2.5,
                            >= 1 => flowK >= 1.25,
                            _ => false
                        };
                        NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, isNeedFlow),
                            DeviceInfoParameterType.IsCoefficientOfCriticalModeGood));
                        
                        if (isNeedFlow)
                        {
                            var flow = k == null
                                ? null
                                : standLine.CurrentFlow
                                * Math.Sqrt((double)((273.15 + standLine.TemperatureSensor.Temperature) / 293.15))
                                * (PressureAtmosphere / (PressureAtmosphere + standLine.Devices.Last().Pressure / 1000))
                                * ((standLine.Devices.First().Temperature + 273.15) / (standLine.TemperatureSensor.Temperature + 273.15))
                                * 1 / k;
                            NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, flow),
                                DeviceInfoParameterType.TargetFlow));
                        }
                        else
                        {
                            double? flow = null;
                            NotifyObserverByDataPair(new DataPair(new LineData(lineIndex, flow),
                                DeviceInfoParameterType.TargetFlow));
                        }
                        
                    }
#endif

                    await Task.Delay(3000);
                }

                _isTaskExecute = false;
            }
            catch (Exception e)
            {
                _logger.Logging(new LogMessage(e.Message, LogLevel.Fatal));
            }
        }

        private double? SelectMetrologyCoefficient(float? temperature, float? humidity)
        {
            var i = temperature switch
            {
                < 11 and > 5 => 0,
                >= 11 and < 13 => 1,
                >= 13 and < 15 => 2,
                >= 15 and < 17 => 3,
                >= 17 and < 19 => 4,
                >= 19 and < 21 => 5,
                >= 21 and < 23 => 6,
                >= 23 and < 25 => 7,
                >= 25 and < 27 => 8,
                >= 27 and < 29 => 9,
                >= 29 and <= 35 => 10,
                _ => -1,
            };

            var j = humidity switch
            {
                < 35 and >= 10 => 0,
                >= 35 and < 45 => 1,
                >= 45 and < 55 => 2,
                >= 55 and < 65 => 3,
                >= 65 and < 75 => 4,
                >= 75 and < 85 => 5,
                >= 85 and <= 95 => 6,
                _ => -1
            };

            if (i == -1 || j == -1) return null;

            return MetrologyData.CoefficientCorrectHumidity[i, j];
        }

        #endregion

        #region Текущие значения

        public float? TemperatureTube
        {
            get => _temperatureTube;
            set
            {
                _temperatureTube = value;
                NotifyObserverByDataPair(new DataPair(value, DeviceInfoParameterType.TemperatureTube));
            }
        }

        public float? PressureAtmosphere
        {
            get => _pressure;
            set
            {
                _pressure = value;
                NotifyObserverByDataPair(new DataPair(value, DeviceInfoParameterType.Pressure));
            }
        }

        public float? Humidity
        {
            get => _humidity;
            set
            {
                _humidity = value;
                NotifyObserverByDataPair(new DataPair(value, DeviceInfoParameterType.Humidity));
            }
        }

        public float? Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                NotifyObserverByDataPair(new DataPair(value, DeviceInfoParameterType.Temperature));
            }
        }


        public float? PressureResiver
        {
            get => _pressureResiver;
            set
            {
                _pressureResiver = value;
                NotifyObserverByDataPair(new DataPair(value, DeviceInfoParameterType.PressureResiver));
            }
        }

        public double? TargetFlow
        {
            get => _targetFlow;
            set
            {
                _targetFlow = value;
                NotifyObserverByDataPair(new DataPair(value, DeviceInfoParameterType.TargetFlow));
            }
        }

        /// <summary>
        /// Получить давление ресивера в Па для регулятора
        /// </summary>
        /// <returns></returns>
        private float? GetPressureResiver() => -PressureResiver;

        public float? PressureDifference
        {
            get => _pressureDifference;
            set
            {
                _pressureDifference = value / 1000;
                NotifyObserverByDataPair(new DataPair(value, DeviceInfoParameterType.PressureDifference));
            }
        }


        private int? SelectedLineIndex
        {
            get => _selectedLineIndex;
            set
            {
                _selectedLineIndex = value;
                NotifyObserverByDataPair(new DataPair(new LineInfoData(_selectedLineIndex, _selectedLineIndex != null),
                    DeviceInfoParameterType.LineState));
            }
        }

        public void UpdateDeviceInformation(DeviceAboutViewModel deviceInformationViewModel)
        {
            _line.Devices[deviceInformationViewModel.DeviceNumber - 1].VendorNumberString =
                deviceInformationViewModel.DeviceVendorNumber;
            _line.Devices[deviceInformationViewModel.DeviceNumber - 1].DeviceName =
                deviceInformationViewModel.DeviceName;
            _line.Devices[deviceInformationViewModel.DeviceNumber - 1].IsManualEnabled =
                deviceInformationViewModel.IsManualEnabled;
        }

        public bool GetDeviceManualEnable(int i)
        {
            return _line.Devices[i].IsManualEnabled;
        }

        #endregion

        #region Работа с устройствами

        [Obsolete("Старый вариант отправления требуемого состояния битов в устройство стенда")]
        public async Task<bool> UpdateStateDevice(int address)
        {
            return await _standDevices[address - 1].SetWorkRegisterAsync();
        }

        [Obsolete("Старый вариант отправления требуемого состояния битов для всех устройств")]
        public async Task<bool> UpdateAllDevice()
        {
            foreach (var device in _standDevices)
            {
                if (!await device.SetWorkRegisterAsync()) return false;

                await Task.Delay(200);
            }

            return true;
        }

        #endregion

        #region Управление соплами

        /// <summary>
        /// Открыть сопло
        /// </summary>
        /// <param name="standSettingsNozzleModel">Настройки сопла</param>
        /// <returns>Результат отправки запроса</returns>
        public async Task<bool> OpenNozzleAsync(StandSettingsNozzleModel standSettingsNozzleModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(
                new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.NozzleState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsNozzleModel.Address)
                    ?.SetBitState(standSettingsNozzleModel.BitNumber, false, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Open),
                    DeviceInfoParameterType.NozzleState));
                return true;
            }

            if (!standSettingsNozzleModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Open),
                    DeviceInfoParameterType.NozzleState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsNozzleModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsNozzleModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(
                new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Open),
                DeviceInfoParameterType.NozzleState));


            return !isWork;
        }

        /// <summary>
        /// Открыть клапан
        /// </summary>
        /// <param name="standSettingsValveModel">Настройки клапана</param>
        /// <returns>Результат отправки запроса</returns>
        public async Task<bool> OpenValveAsync(StandSettingsValveModel standSettingsValveModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.ValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, standSettingsValveModel.IsReverseValve,
                        withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                        DeviceInfoParameterType.ValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                        DeviceInfoParameterType.ValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            if (standSettingsValveModel.StateOffAddress == standSettingsValveModel.StateOnAddress)
            {
                while (isWork && count > 0)
                {
                    await Task.Delay(1000);

                    var moduleState = await _standDevices
                        .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOnAddress)
                        .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOnBitNumber);

                    isWork = moduleState;
                    count--;
                }
            }
            else
                while (isWork && count > 0)
                {
                    await Task.Delay(1000);

                    var moduleState = await _standDevices
                        .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOnAddress)
                        .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOnBitNumber);

                    isWork = !moduleState;
                    count--;
                }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                DeviceInfoParameterType.ValveState));
            return !isWork;
        }

        /// <summary>
        /// Закрыть сопло
        /// </summary>
        /// <param name="standSettingsNozzleModel">Настройки сопла</param>
        /// <param name="withoutWrite">Без записи состояние в регистр (по умолчанию записывается всегда)</param>
        /// <returns>Результат отправки запроса</returns>
        public async Task<bool> CloseNozzleAsync(StandSettingsNozzleModel standSettingsNozzleModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(
                new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.NozzleState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsNozzleModel.Address)
                    ?.SetBitState(standSettingsNozzleModel.BitNumber, true, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.NozzleState));
                return true;
            }

            if (!standSettingsNozzleModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.NozzleState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsNozzleModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsNozzleModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(
                new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Close),
                DeviceInfoParameterType.NozzleState));


            return !isWork;
        }

        /// <summary>
        /// Закрыть клапан
        /// </summary>
        /// <param name="standSettingsValveModel">Настройки клапана</param>
        /// <returns>Результат отправки запроса</returns>
        public async Task<bool> CloseValveAsync(StandSettingsValveModel standSettingsValveModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.ValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, !standSettingsValveModel.IsReverseValve,
                        withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ValveState));
                return true;
            }

            var isWork = true;
            var count = 60;


            if (standSettingsValveModel.StateOffAddress == standSettingsValveModel.StateOnAddress)
            {
                while (isWork && count > 0)
                {
                    await Task.Delay(1000);

                    var moduleState = await _standDevices
                        .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOnAddress)
                        .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOnBitNumber);

                    isWork = moduleState;
                    count--;
                }
            }
            else
                while (isWork && count > 0)
                {
                    await Task.Delay(1000);

                    var moduleState = await _standDevices
                        .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOffAddress)
                        .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOffBitNumber);

                    isWork = !moduleState;
                    count--;
                }

            NotifyObserverByDataPair(new DataPair(
                new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                DeviceInfoParameterType.ValveState));
            return !isWork;
        }

        public async Task<bool> UseOwenValveAsync(StandSettingsOwenValveModel standSettingsValveModel, int lineIndex,
            int owenValue)
        {
            return await ((IOwenPBR10ADevice)_owens.FirstOrDefault(owen =>
                    ((owen is IOwenPBR10ADevice) && owen.ModuleAddressInt == standSettingsValveModel.Address)))
                .SetPosition(owenValue);
        }

        public async Task<bool> CloseDeviceValveAsync(StandSettingsValveModel standSettingsValveModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.LineValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, true, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.LineValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.LineValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOnAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOnBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(
                new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                DeviceInfoParameterType.LineValveState));
            return !isWork;
        }

        public async Task<bool> OpenDeviceValveAsync(StandSettingsValveModel standSettingsValveModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.LineValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, false, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                        DeviceInfoParameterType.LineValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                        DeviceInfoParameterType.LineValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOnAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOnBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                DeviceInfoParameterType.LineValveState));
            return !isWork;
        }

        public async Task<bool> CloseReverseValveAsync(StandSettingsValveModel standSettingsValveModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.ReverseValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, true, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(
                    new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOnAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOnBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(
                new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                DeviceInfoParameterType.ReverseValveState));
            return !isWork;
        }

        public async Task<bool> OpenReverseValveAsync(StandSettingsValveModel standSettingsValveModel,
            bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work),
                DeviceInfoParameterType.ReverseValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, false, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                        DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                        DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateOnAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateOnBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open),
                DeviceInfoParameterType.ReverseValveState));
            return !isWork;
        }

        public async Task<bool> SetPulseCountForPulseMeterAsync(int pulseCount, int deviceIndex)
        {
            return await _line.Devices[deviceIndex].SetPulseCountAsync(pulseCount);
        }

        public async Task<bool> SetPulseCountForPulseMeterAsync(int pulseCount, int pulseMeterAddress,
            int pulseMeterChannelNumber)
        {
            return await _lines
                .FirstOrDefault(line => line.Devices.Any(dev => dev.PulseMeterAddress == pulseMeterAddress))
                .Devices.FirstOrDefault(dev => dev.PulseMeterAddress == pulseMeterAddress)
                .SetPulseCountAsync(pulseCount);
        }

        public async Task<float?> ReadStartPulseMeasureTimeAsync(int deviceIndex)
        {
            throw new NotImplementedException();
        }

        public async Task<float?> ReadEndPulseMeasureTimeAsync(int deviceIndex)
        {
            throw new NotImplementedException();
        }

        public async Task<int?> ReadPulseCountAsync(int deviceIndex)
        {
            return await _line.Devices[deviceIndex].ReadPulseCountAsync();
        }

        public string GetVendorName(int activeLine, int i)
        {
            return _lines[activeLine].Devices[i].VendorName;
        }

        public DeviceNameViewModel GetDeviceInfoType(int activeLine, int deviceIndex)
        {
            return new DeviceNameViewModel()
                { DeviceTypeInfo = _lines[activeLine].Devices[deviceIndex].DeviceTypeInfo };
        }

        public async Task<List<(float?, float?, float?)>> ReadPulseCoefficientsAsync()
        {
            var coefficientList = new List<(float?, float?, float?)>();

            foreach (var pulseMeterViewModel in _settingsService.StandSettingsModel.PulseMeterViewModels)
            {
                coefficientList.Add(await ReadPulseCoefficientAsync(pulseMeterViewModel));
            }

            return coefficientList;
        }

        public async Task<bool> WritePulseCoefficientsAsync(List<(float, float, float)> coefficientTuples)
        {
            var result = true;

            for (var i = 0; i < _settingsService.StandSettingsModel.PulseMeterViewModels.Count; i++)
            {
                var pulseMeterViewModel = _settingsService.StandSettingsModel.PulseMeterViewModels[i];
                var coefficientTuple = coefficientTuples[i];

                if (!result) return result;

                result = await WritePulseCoefficientAsync(coefficientTuple, pulseMeterViewModel);
            }

            return result;
        }

        

        public async Task<CommonCommandStatus?> ReadCommonCommandStatusPulseCountMeterAsync(
            int? pulseCountMeterModuleIndex)
        {
            if (pulseCountMeterModuleIndex == null) return null;

            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex].GetCommonCommandStatusAsync();
        }

        public async Task<bool> StartPulseCountMeterModuleMeasureAsync(int? pulseCountMeterModuleIndex)
        {
            if (pulseCountMeterModuleIndex == null) return false;

            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex].StartMeasurePulseCountAsync();
        }

        public async Task<bool> StopPulseCountModuleMeasureAsync(int? pulseCountMeterModuleIndex)
        {
            if (pulseCountMeterModuleIndex == null) return false;

            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex].StopMeasurePulseCountAsync();
        }

        public async Task<float?> ReadPulsePeriodFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex,
            int? channelNumber)
        {
            if (pulseCountMeterModuleIndex == null || channelNumber == null) return null;

            var channel = channelNumber switch
            {
                1 => ChannelNumber.First,
                2 => ChannelNumber.Second,
                3 => ChannelNumber.Third,
                4 => ChannelNumber.Fourth,
                _ => ChannelNumber.None
            };

            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex].ReadPulsePeriodAsync(channel);
        }

        public async Task<float?> ReadPulseDurationFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex,
            int? channelNumber)
        {
            if (pulseCountMeterModuleIndex == null || channelNumber == null) return null;

            var channel = channelNumber switch
            {
                1 => ChannelNumber.First,
                2 => ChannelNumber.Second,
                3 => ChannelNumber.Third,
                4 => ChannelNumber.Fourth,
                _ => ChannelNumber.None
            };

            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex].ReadPulseDurationAsync(channel);
        }

        public async Task<bool> SendStartPulseCountMeterCommandAsync() =>
            await _pulseCountMeterStarter.SendStartCommand();

        public async Task<bool> SetPulseCountMeterModuleChannelSettingsAsync(int? pulseCountMeterModuleIndex,
            int channelNumber)
        {
            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex]
                .SetPulseCountMeterModuleChannelSettingsAsync((ChannelNumber)channelNumber);
        }

        public async Task<bool> TurnOnPulseCountMeterControlRegister()
        {
            foreach (var pulseCountMeterModule in _pulseCountMeterModules)
            {
                if (!await pulseCountMeterModule.TurnOnControlBitControlRegisterAsync()) return false;
            }

            return true;
        }

        public async Task<bool> ResetPulseCountMeterAsync()
        {
            foreach (var pulseCountMeterModule in _pulseCountMeterModules)
            {
                if (!await pulseCountMeterModule.ResetPulseCountMeterAsync()) return false;
            }

            return true;
        }

        public async Task<float?> ReadPulseCountFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex,
            int pulseCountMeterModuleChannelNumber)
        {
            if (pulseCountMeterModuleIndex == null) return null;

            var channel = pulseCountMeterModuleChannelNumber switch
            {
                1 => ChannelNumber.First,
                2 => ChannelNumber.Second,
                3 => ChannelNumber.Third,
                4 => ChannelNumber.Fourth,
                _ => ChannelNumber.None
            };

            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex].ReadPulseCountAsync();
        }

        public async Task<float?> ReadMeasureTimeFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex)
        {
            if (pulseCountMeterModuleIndex == null) return null;

            return await _pulseCountMeterModules[(int)pulseCountMeterModuleIndex].ReadMeasureTimeAsync();
        }

        public float? GetPressureDifferenceFromMasterDevice(int lineIndex)
        {
            return _lines[lineIndex].MasterDevices[0].GetPressureDifference();
        }

        public float? GetTemperatureFromMasterDevice(int selectedLineIndex, int indexOfMasterDevice)
        {
            return _lines[selectedLineIndex].MasterDevices[indexOfMasterDevice].GetTemperature();
        }

        public async Task<bool> UseNeedleValveAsync(StandSettingsNeedleValveModel standSettingsValveModel,
            int selectedNeedleValue)
        {
            var res = await _needleValveControllers.First(ctrl =>
                              ctrl.PortName == standSettingsValveModel.SelectedComPort
                              && ctrl.ModuleAddressInt ==
                              (byte)standSettingsValveModel.ModuleAddress)
                          .SetParameterAsync((uint)selectedNeedleValue)
                      && await _needleValveControllers.First(ctrl =>
                              ctrl.PortName == standSettingsValveModel.SelectedComPort
                              && ctrl.ModuleAddressInt == (byte)standSettingsValveModel.ModuleAddress)
                          .SetCommandAsync(selectedNeedleValue == 0
                              ? NeedleValveControllerCommand.ReturnToZero
                              : NeedleValveControllerCommand.RunToParameter);

            /*while (await _needleValveControllers.First(ctrl => ctrl.PortName == standSettingsValveModel.SelectedComPort
                                                               && ctrl.ModuleAddressInt ==
                                                               (byte)standSettingsValveModel.ModuleAddress)
                       .ReadStatusAsync() != NeedleValveControllerStatus.Done)
            {
                await Task.Delay(1000);
            }*/

            return res;
        }

        public float? GetTemperatureFromLine(int lineIndex)
        {
            return _lines[lineIndex].TemperatureSensor.Temperature;
        }

        public float? GetPressureDifferenceFromLine(int lineIndex)
        {
            return _lines[lineIndex].PressureDifferenceSensor.Pressure;
        }

        public float? GetPressureDifferenceFromDevice(int selectedLineIndex, int deviceIndex)
        {
            return _lines[selectedLineIndex].Devices[deviceIndex].Pressure;
        }

        public async Task<uint?> ReadFreeRunPulseCount(int pulseMeterAddress, int pulseMeterChannelNumber)
        {
            return await _lines
                .FirstOrDefault(line => line.Devices.Any(dev => dev.PulseMeterAddress == pulseMeterAddress))
                .Devices.FirstOrDefault(dev => dev.PulseMeterAddress == pulseMeterAddress)
                .ReadFreeRunningCounterAsync(pulseMeterChannelNumber);
        }

        public async Task<bool> StartPulseMeterPeriodMeasureAsync(int pulseCount, int pulseMeterAddress,
            int pulseMeterChannelNumber)
        {
            return await _lines
                .FirstOrDefault(line => line.Devices.Any(dev => dev.PulseMeterAddress == pulseMeterAddress))
                .Devices.FirstOrDefault(dev => dev.PulseMeterAddress == pulseMeterAddress)
                .StartPeriodMeasureAsync(pulseCount);
        }

        public async Task<float?> ReadPeriodFromPulseMeterAsync(int pulseMeterAddress, int pulseMeterChannelNumber)
        {
            return await _lines
                .FirstOrDefault(line => line.Devices.Any(dev => dev.PulseMeterAddress == pulseMeterAddress))
                .Devices.FirstOrDefault(dev => dev.PulseMeterAddress == pulseMeterAddress)
                .ReadPulseMeterPeriodAsync(pulseMeterChannelNumber);
        }

        public async Task<PulseMeter2ChannelState> GetPulseMeterStatusAsync(int pulseMeterAddress,
            int pulseMeterChannelNumber)
        {
            return await _lines
                .FirstOrDefault(line => line.Devices.Any(dev => dev.PulseMeterAddress == pulseMeterAddress))
                .Devices.FirstOrDefault(dev => dev.PulseMeterAddress == pulseMeterAddress)
                .ReadChannelStatusAsync();
        }

        private async Task<(float?, float?, float?)> ReadPulseCoefficientAsync(
            StandSettingsPulseMeterModel settingsPulseMeterModel)
        {
            return await _lines.First(l =>
                    l.Devices.Any(device => device.PulseMeterAddress == settingsPulseMeterModel.Number))
                .Devices.First(device => device.PulseMeterAddress == settingsPulseMeterModel.Number)
                .ReadPulseCoefficientsAsync();
        }
        
        private async Task<bool> WritePulseCoefficientAsync((float, float, float) coefficientTuple, StandSettingsPulseMeterModel pulseMeterViewModel)
        {
            return await _lines.First(l =>
                    l.Devices.Any(device => device.PulseMeterAddress == pulseMeterViewModel.Number))
                .Devices.First(device => device.PulseMeterAddress == pulseMeterViewModel.Number)
                .WritePulseCoefficientsAsync(coefficientTuple.Item1, coefficientTuple.Item2, coefficientTuple.Item3);
        }

        public async Task<bool> SetConsumptionWithoutSelectionAsync(
            ObservableCollection<StandSettingsNozzleModel> pointSelectedNozzles)
        {
            foreach (var standSettingsNozzleModel in pointSelectedNozzles)
            {
                if (!await OpenNozzleAsync(standSettingsNozzleModel)) return false;
            }

            return true;
        }

        public void SetTargetFlowValue(double? value)
            => TargetFlow = value;

        public void AddTargetFlowValue(double? value)
            => TargetFlow += value;

        public void SetLineTargetFlowValue(double? value, int lineIndex)
        {
            _lines[lineIndex].CurrentFlow = value;
        }

        public void AddLineTargetFlowValue(double? value, int lineIndex)
        {
            _lines[lineIndex].CurrentFlow += value ?? 0;
        }

        public async Task SetFlowDirectionAsync(LineDirectionFlowState reverseDirection, int lineNumber)
        {
            switch (reverseDirection)
            {
                case LineDirectionFlowState.AllOpen:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.AllOpen, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#else
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .DirectValveViewModel);
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.AllOpen, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                case LineDirectionFlowState.AllClose:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.AllClose, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#else
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .DirectValveViewModel);
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.AllClose, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                case LineDirectionFlowState.DirectDirection:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.DirectDirection, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#else
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .DirectValveViewModel);
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.DirectDirection, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                case LineDirectionFlowState.ReverseDirection:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.ReverseDirection, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#else
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .DirectValveViewModel);
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1]
                        .ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair(
                        new DirectFlowInfoData(LineDirectionFlowState.ReverseDirection, lineNumber),
                        DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reverseDirection), reverseDirection, null);
            }
        }

        public void SetActiveLine(int lineNumber, bool isActiveLine)
            => SelectedLineIndex = isActiveLine ? lineNumber - 1 : null;

        public int? GetActiveLine()
            => SelectedLineIndex;


        public async Task<double?> SetConsumptionAsync(double value, double? minimumFlow, double? maximumFlow)
        {
            var consumptions = _settingsService.StandSettingsModel.NozzleViewModels
                .Select(nw => nw.NozzleFactValue).ToList();

            var deltaValue = value;

            if (consumptions.Any(consumption => consumption == null))
            {
                _logger.Logging(new LogMessage("Необходима настройка сопел", LogLevel.Error));
                return null;
            }

            var nozzleNumbers = new List<int>();

#if !DEBUGGUI
            if (!await CloseAllNozzleAsync())
            {
                _logger.Logging(new LogMessage("Не удалось закрыть краны сопел", LogLevel.Error));
                return null;
            }
#endif
            //Алгоритм выбирает сопла для получения заданного расхода или чуть меньше (расход больше заданного не выставляется)
            //Начинаем с последнего сопла, если расход сопла меньше разницы записываем его номер и потом открываем.
            //Далее из разницы вычитаем расход сопла, чтобы определить сколько еще расхода нужно получить и повторяем цикл
            //Если разница совпадает с одним из расходов сопер, цикл прервется т.к. нужные сопла для открытия будут известны (исключительно маловероятная ситуация)
            //Важный момент - если заданный расход меньше минимального расхода среди сопел, то расход не будет установлен
            for (var i = consumptions.Count - 1; i >= 0; i--)
            {
                if (consumptions[i] > deltaValue) continue;

                if (consumptions[i] == deltaValue)
                {
                    nozzleNumbers.Add(i + 1);
                    deltaValue = 0;
                    break;
                }

                nozzleNumbers.Add(i + 1);

                deltaValue = (double)(deltaValue - consumptions[i]);
            }

            for (var i = 0; i < consumptions.Count; i++)
            {
                if (value - deltaValue < minimumFlow)
                {
                    if (nozzleNumbers.Contains(i)) continue;

                    nozzleNumbers.Add(i + 1);

                    deltaValue = (double)(deltaValue - consumptions[i]);
                }
                else
                {
                    break;
                }
            }

            if (deltaValue != 0)
            {
                _logger.Logging(new LogMessage(
                    $"Не удалось выставить расход: {value}\tВыставленный расход: {value - deltaValue}\tПогрешность выставления расхода: {deltaValue}",
                    LogLevel.Warning));
            }
#if !DEBUGGUI
            foreach (var nozzleNumber in nozzleNumbers)
            {
                if (!await OpenNozzleAsync(
                        _settingsService.StandSettingsModel.NozzleViewModels.FirstOrDefault(nz =>
                            nz.Number == nozzleNumber)))
                {
                    _logger.Logging(new LogMessage($"Не удалось октрыть сопло №{nozzleNumbers}", LogLevel.Error));
                    return null;
                }
            }
#endif
            return deltaValue;
        }

        public async Task<bool> CloseAllNozzleAsync()
        {
            foreach (var nozzleViewModel in _settingsService.StandSettingsModel.NozzleViewModels)
            {
                if (!await CloseNozzleAsync(nozzleViewModel, true))
                    return false;
            }

            foreach (var standDevice in _standDevices.Where(d => d.NeedUpdateState))
            {
                if (!await standDevice.SetWorkRegisterAsync())
                    return false;
            }

            return true;
        }

        public async Task<bool> CloseAllValveAsync()
        {
            foreach (var valveViewModel in _settingsService.StandSettingsModel.ValveViewModels)
            {
                if (!await CloseValveAsync(valveViewModel, true))
                    return false;
            }

            foreach (var standDevice in _standDevices.Where(d => d.NeedUpdateState))
            {
                if (!await standDevice.SetWorkRegisterAsync())
                    return false;
            }

            return true;
        }

        public async Task<bool> UpdateAllStatesAsync()
        {
            foreach (var standDevice in _standDevices)
            {
                if (!await standDevice.SetWorkRegisterAsync())
                    return false;
            }

            return true;
        }

        public async Task<bool> OpenSolenoidValveAsync(StandSettingsSolenoidValveModel solenoidValveModel)
        {
            var result = await _standDevices
                .FirstOrDefault(sd => sd.ModuleAddressInt == solenoidValveModel.Address)
                ?.SetBitState(solenoidValveModel.BitNumber, solenoidValveModel.SolenoidValveType switch
                {
                    SolenoidValveType.NormalOpen => false,
                    SolenoidValveType.NormalClose => true,
                    _ => throw new ArgumentOutOfRangeException()
                })!;

            NotifyObserverByDataPair(new DataPair(new StandInfoData(solenoidValveModel.Number - 1, StateType.Open),
                DeviceInfoParameterType.SolenoidValveState));

            return result;
        }

        public async Task<bool> CloseSolenoidValveAsync(StandSettingsSolenoidValveModel solenoidValveModel)
        {
            var result = await _standDevices
                .FirstOrDefault(sd => sd.ModuleAddressInt == solenoidValveModel.Address)
                ?.SetBitState(solenoidValveModel.BitNumber, solenoidValveModel.SolenoidValveType switch
                {
                    SolenoidValveType.NormalOpen => true,
                    SolenoidValveType.NormalClose => false,
                    _ => throw new ArgumentOutOfRangeException()
                })!;

            NotifyObserverByDataPair(new DataPair(new StandInfoData(solenoidValveModel.Number - 1, StateType.Close),
                DeviceInfoParameterType.SolenoidValveState));

            return result;
        }

        public async Task<bool> EnableFrequencyRegulatorAsync(int regulatorIndex)
        {
            return await _frequencyRegulatorDevices[regulatorIndex].StartFrequencyWorkAsync();
        }

        public async Task<bool> EnableLineFanWorkAsync(int lineIndex, int fanIndex)
        {
            return await _standDevicesForFan.FirstOrDefault(sd =>
                    sd.ModuleAddressInt == _settingsService.StandSettingsModel.LineViewModels[lineIndex]
                        .FanViewModels[fanIndex].Address)
                .SetBitState(fanIndex, true);
        }

        public async Task<bool> DisableLineFanWorkAsync(int lineIndex, int fanIndex)
        {
            return await _standDevicesForFan.FirstOrDefault(sd =>
                    sd.ModuleAddressInt == _settingsService.StandSettingsModel.LineViewModels[lineIndex]
                        .FanViewModels[fanIndex].Address)
                .SetBitState(fanIndex, false);
        }

        public async Task<bool> SetRegulatorFrequencyAsync(int regulatorIndex, float frequency)
        {
            return await _frequencyRegulatorDevices[regulatorIndex].SetOutputValueAsync(frequency);
        }

        public async Task<bool> DisableFrequencyRegulatorAsync(int regulatorIndex)
        {
            return await _frequencyRegulatorDevices[regulatorIndex].StopFrequencyWorkAsync();
        }

        public async Task<bool> EndWorkAsync()
        {
            if (!await OpenSolenoidValveAsync(
                    _settingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(sv =>
                        sv.SolenoidValveType == SolenoidValveType.NormalClose)))
                return false;

            if (!await CloseAllNozzleAsync()) return false;
            if (!await CloseAllValveAsync()) return false;

            if (!await CloseSolenoidValveAsync(
                    _settingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(sv =>
                        sv.SolenoidValveType == SolenoidValveType.NormalClose)))
                return false;

            return true;
        }

        public Task<bool> SetFrequencyMeasureModeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SerPeriodMeasureModeAsync()
        {
            throw new NotImplementedException();
        }

        public void AddCollectionForPortLogging(ObservableCollection<LogMessage> portLogMessages)
        {
            _portLogMessages = portLogMessages;
        }

        public void AddCollectionForDevicePortLogging(ObservableCollection<LogMessage> portLogMessages)
        {
            //_devicePortLogMessages = portLogMessages;
        }

        public void RegisterPressureSensorObserver(IPressureSensorObserver observer, DevicePurpose devicePurpose,
            int deviceIndex, int lineIndex)
        {
            switch (devicePurpose)
            {
                case DevicePurpose.MasterDevice:
                    _lines[lineIndex].MasterDevices[deviceIndex]
                        .RegisterPressureSensorObserver(observer);
                    break;
                case DevicePurpose.ValidationDevice:
                    ((IPressureSensorObservable)_lines[lineIndex].Devices[deviceIndex])
                        .RegisterPressureSensorObserver(observer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(devicePurpose), devicePurpose, null);
            }
        }

        public void RegisterTemperatureSensorObserver(ITemperatureSensorObserver observer, DevicePurpose devicePurpose,
            int deviceIndex, int lineIndex)
        {
            switch (devicePurpose)
            {
                case DevicePurpose.MasterDevice:
                    _lines[lineIndex].MasterDevices[deviceIndex]
                        .RegisterTemperatureSensorObserver(observer);
                    break;
                case DevicePurpose.ValidationDevice:
                    ((ITemperatureSensorObservable)_lines[lineIndex].Devices[deviceIndex])
                        .RegisterTemperatureSensorObserver(observer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(devicePurpose), devicePurpose, null);
            }
        }

        public void RegisterFlowObserver(IFlowObserver observer, DevicePurpose devicePurpose, int deviceIndex,
            int lineIndex)
        {
            switch (devicePurpose)
            {
                case DevicePurpose.MasterDevice:
                    _lines[lineIndex].MasterDevices[deviceIndex]
                        .RegisterFlowObserver(observer);
                    break;
                case DevicePurpose.ValidationDevice:
                    ((IFlowObservable)_lines[lineIndex].Devices[deviceIndex])
                        .RegisterFlowObserver(observer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(devicePurpose), devicePurpose, null);
            }
        }

        public void UnsubscribePressureSensorObserver(IPressureSensorObserver observer, DevicePurpose devicePurpose,
            int deviceIndex,
            int lineIndex)
        {
            switch (devicePurpose)
            {
                case DevicePurpose.MasterDevice:
                    _lines[lineIndex].MasterDevices[deviceIndex]
                        .RemovePressureSensorObserver(observer);
                    break;
                case DevicePurpose.ValidationDevice:
                    ((IPressureSensorObservable)_lines[lineIndex].Devices[deviceIndex])
                        .RemovePressureSensorObserver(observer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(devicePurpose), devicePurpose, null);
            }
        }

        public void UnsubscribeTemperatureSensorObserver(ITemperatureSensorObserver observer,
            DevicePurpose devicePurpose,
            int deviceIndex, int lineIndex)
        {
            switch (devicePurpose)
            {
                case DevicePurpose.MasterDevice:
                    _lines[lineIndex].MasterDevices[deviceIndex]
                        .RemoveTemperatureSensorObserver(observer);
                    break;
                case DevicePurpose.ValidationDevice:
                    ((ITemperatureSensorObservable)_lines[lineIndex].Devices[deviceIndex])
                        .RemoveTemperatureSensorObserver(observer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(devicePurpose), devicePurpose, null);
            }
        }

        public void UnsubscribeFlowObserver(IFlowObserver observer, DevicePurpose devicePurpose, int deviceIndex,
            int lineIndex)
        {
            switch (devicePurpose)
            {
                case DevicePurpose.MasterDevice:
                    _lines[lineIndex].MasterDevices[deviceIndex]
                        .RemoveFlowObserver(observer);
                    break;
                case DevicePurpose.ValidationDevice:
                    ((IFlowObservable)_lines[lineIndex].Devices[deviceIndex])
                        .RemoveFlowObserver(observer);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(devicePurpose), devicePurpose, null);
            }
        }

        public void RegisterDeviceObserver(IDeviceObserver observer, int deviceNumber, int lineNumber) =>
            ((IDeviceObservable)_lines[lineNumber].Devices[deviceNumber]).RegisterDeviceObserver(observer);

        public string GetVendorNumber(int deviceNumber) => _line.Devices[deviceNumber].VendorNumberString;

        public string GetDeviceName(int deviceNumber) => _line.Devices[deviceNumber].DeviceName;

        public async Task EmergencyPowerOffAsync()
        {
            /*await OpenSolenoidValveAsync(_settingsService.StandSettingsModel.SolenoidValveViewModels
                .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalClose));
            await CloseAllNozzleAsync();
            PidDisable();
            await DisableVacuumCreator();

            await Task.Delay(5000);

            await OpenSolenoidValveAsync(_settingsService.StandSettingsModel.SolenoidValveViewModels
                .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalOpen));
            await CloseSolenoidValveAsync(_settingsService.StandSettingsModel.SolenoidValveViewModels
                .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalClose));*/
        }

        public async Task<bool> ResetToZeroAsync(int deviceNumber) =>
            await _line.Devices[deviceNumber - 1].ResetToZeroAsync();


        public async Task<bool> SetStandWorkModeAsync()
        {
            foreach (var lineViewModel in _settingsService.StandSettingsModel.LineViewModels)
            {
                switch (lineViewModel.SelectedLineType)
                {
                    case LineType.None:
                        break;
                    case LineType.MasterDeviceLineType:
                    {
                        if (lineViewModel.IsAfterDeviceValve)
                            if (!await CloseValveAsync(lineViewModel.AfterDeviceValveViewModel, true))
                                return false;

                        if (lineViewModel.IsStartValveMasterDevice)
                            if (!await CloseValveAsync(lineViewModel.StartValveMasterDeviceViewModel, true))
                                return false;

                        if (lineViewModel.IsEndValveMasterDevice)
                            if (!await CloseValveAsync(lineViewModel.EndValveMasterDeviceViewModel, true))
                                return false;

                        if (lineViewModel.IsStartCommonValve)
                            if (!await CloseValveAsync(lineViewModel.StartCommonValveViewModel, true))
                                return false;

                        if (lineViewModel.IsEndCommonValve)
                            if (!await CloseValveAsync(lineViewModel.EndCommonValveViewModel, true))
                                return false;

                        if (lineViewModel.IsCheckTightnessLine)
                        {
                            if (!await CloseValveAsync(lineViewModel.FirstTightnessValveViewModel, true))
                                return false;
                            if (!await CloseValveAsync(lineViewModel.SecondTightnessValveViewModel, true))
                                return false;
                            if (!await CloseValveAsync(lineViewModel.TightnessValveViewModel, true))
                                return false;
                        }

                        foreach (var masterDeviceViewModel in lineViewModel.MasterDeviceViewModels)
                        {
                            if (!await CloseValveAsync(masterDeviceViewModel.PressureSensorValveViewModel, true))
                                return false;

                            if (!await CloseValveAsync(masterDeviceViewModel.MasterDeviceValveViewModel, true))
                                return false;
                        }

                        foreach (var fanViewModel in lineViewModel.FanViewModels)
                        {
                            if (!await CloseValveAsync(fanViewModel.FanValveViewModel, true))
                                return false;
                        }
                    }
                        break;
                    case LineType.NozzleLineType:
                    {
                        if (lineViewModel.IsAfterDeviceValve)
                            if (!await CloseValveAsync(lineViewModel.AfterDeviceValveViewModel, true))
                                return false;

                        if (lineViewModel.IsStartCommonValve)
                            if (!await CloseValveAsync(lineViewModel.StartCommonValveViewModel, true))
                                return false;

                        if (lineViewModel.IsEndCommonValve)
                            if (!await CloseValveAsync(lineViewModel.EndCommonValveViewModel, true))
                                return false;

                        if (lineViewModel.IsCheckTightnessLine)
                        {
                            if (!await CloseValveAsync(lineViewModel.FirstTightnessValveViewModel, true))
                                return false;
                            if (!await CloseValveAsync(lineViewModel.SecondTightnessValveViewModel, true))
                                return false;
                            if (!await CloseValveAsync(lineViewModel.TightnessValveViewModel, true))
                                return false;
                        }

                        foreach (var nozzleViewModel in lineViewModel.NozzleViewModels.Where(
                                     noz => !noz.IsReplaceNozzle))
                        {
                            if (!await CloseNozzleAsync(nozzleViewModel, true))
                                return false;
                        }
                    }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            foreach (var standDevice in _standDevices.Where(d => d.NeedUpdateState))
            {
                if (!await standDevice.SetWorkRegisterAsync())
                    return false;
            }

            await Task.Delay(40000);

            return true;
        }

        private Task<bool> ChoseValveForOpenAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Вакуумный насос

        public async Task<bool> DisableVacuumCreator()
        {
            return await _frequencyRegulatorDevice.StopFrequencyWorkAsync();
        }

        public async Task<bool> SetFrequencyRegulatorFrequencyAsync(double value)
        {
            return await _frequencyRegulatorDevice.SetOutputValueAsync(value);
        }

        #endregion

        #region Управление задвижкой

        public Task<bool> RunOpenGateAsync(int numberGate)
        {
            switch (numberGate)
            {
                case 1:

                    break;

                case 2:

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        public Task<bool> RunCloseGateAsync(int numberGate)
        {
            switch (numberGate)
            {
                case 1:

                    break;

                case 2:

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        public Task<bool> StopGateAsync(int numberGate)
        {
            switch (numberGate)
            {
                case 1:

                    break;

                case 2:

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            throw new NotImplementedException();
        }

        #endregion


        #region Синхронизация времени

        /// <summary>
        /// Ввод текущего времени 
        /// </summary>
        /// <param name="deviceIndex">Номер устройства</param>
        /// <returns>Результат ввода</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> SetTimeSynchronizationAsync(int deviceIndex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Настройка диапазонов (макс. дог. расход)

        /// <summary>
        /// Настраивает диапазон в соответствии с устройством
        /// </summary>
        /// <param name="deviceIndex">Индекс устройства</param>
        /// <param name="rangeValue">Значение диапазона</param>
        /// <returns>Результат записи</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException">туду</exception>
        public async Task<bool> SetRangeDeviceAsync(int deviceIndex, float rangeValue)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Поверка платформы и связи

        /// <summary>
        /// Поверка платформы
        /// </summary>
        /// <param name="deviceIndex">Номер устройства</param>
        /// <returns>Результат поверки</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException">туду</exception>
        public async Task<bool> CheckPlatformDeviceAsync(int deviceIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Поверка связи
        /// </summary>
        /// <param name="deviceIndex">Номер устройства</param>
        /// <returns>Результат поверки</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException">to do</exception>
        public async Task<bool> CheckConnectionDeviceAsync(int deviceIndex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Observable implimentation

        #region Observable base

        private readonly List<IObserver> _observers = [];
        private float? _temperatureTube;
        private float? _pressureDifference;
        private float? _pressure;
        private float? _humidity;
        private float? _temperature;
        private float? _pressureResiver;
        private double? _targetFlow = 0;

        public void NotifyObservers(object obj)
        {
            foreach (var observer in _observers)
                observer.Update(obj);
        }

        public void NotifyObserverByDataPair(DataPair dataPair)
        {
            foreach (var observer in _observers)
                if (observer is IStandObserver standObserver)
                    standObserver.UpdateFromDataPair(dataPair);
        }

        public void RegisterObserver(IObserver observer) => _observers.Add(observer);

        public void RemoveObserver(IObserver observer) => _observers.Remove(observer);

        #endregion

        #endregion

        #region Освобождение ресурсов

        public void Dispose()
        {
            _requestTaskCancellationTokenSource?.Cancel();

            while (_isTaskExecute)
            {
                Task.Delay(1000);
            }

            for (var i = _modbusProcessors.Count - 1; i >= 0; i--)
            {
                var modbusProcessor = _modbusProcessors[i];
                modbusProcessor.ShutDown();
            }

            _elmetroDigitalDevice?.CommunicationChannel?.Close();
        }

        public async Task<bool> ResetToZeroPressureDifferenceAsync()
        {
            return true;
        }

        #endregion
    }
}