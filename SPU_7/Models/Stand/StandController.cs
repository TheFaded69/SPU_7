using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SPU_7.Common.Line;
using SPU_7.Common.Modbus;
using SPU_7.Common.Stand;
using SPU_7.Domain.Devices.Device.UniversalDevice;
using SPU_7.Domain.Devices.StandDevices.FrequencyRegulator;
using SPU_7.Domain.Devices.StandDevices.PressureSensor;
using SPU_7.Domain.Devices.StandDevices.PressureSensor415M;
using SPU_7.Domain.Devices.StandDevices.PulseMeter;
using SPU_7.Domain.Devices.StandDevices.TemperatureSensor;
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

        private IModbusProcessor _modbusProcessor;
        
        private List<StandLine> _lines = new();
        private StandLine _line => SelectedLineIndex == null ? null : _lines[(int)SelectedLineIndex];
        
        private List<StandDevice> _standDevices = new();
        //private List<IDevice> _devices = new();
        private List<IPulseMeter2Channel> _pulseMeter2Channels = new();
        private IFrequencyRegulatorDevice _frequencyRegulatorDevice;
        private IPressureSensor _pressureSensor;
        private IPressureSensor _pressureDifferenceSensor;
        private IPressureSensor415M _pressureSensor415M;
        private ITemperatureSensor _temperatureSensor;
        private ITHMeter _thMeter;

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
            _modbusProcessor = new ModbusProcessor(new RequestSerializer(), new ResponseDeserializer())
            {
                Communicator = new SerialCommunicator()
            };


            ((ISerialCommunicator)_modbusProcessor.Communicator).SetSerialPort(_settingsService.StandSettingsModel.SelectedEquipmentPort,
                _settingsService.StandSettingsModel.SelectedEquipmentBaudRate,
                8, Parity.None, StopBits.Two, Handshake.None, false, false);
            _modbusProcessor.ProtocolSettings = new ProtocolSettings
            {
                Preamble = new byte[] { 0xFF, },
                DelayAfterPreamble = 10,
                IsPreambleNeed = false,
                ReadTimeout = 3000,
                WriteTimeout = 3000,
                AttemptCount = 3,
                IsPoolingNeed = false,
                PoolingPeriod = 5000
            };
            IRegisterMapEnum<StandDeviceRegisterMap> standDeviceRegisterMap = new RegisterMapEnum<StandDeviceRegisterMap>();

            var addressList = new List<int>();

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

                if (valveViewModel.StateAddress != null && !addressList.Contains((int)valveViewModel.StateAddress))
                    addressList.Add((int)valveViewModel.StateAddress);
            }

            foreach (var lineViewModel in _settingsService.StandSettingsModel.LineViewModels)
            {
                foreach (var deviceViewModel in lineViewModel.DeviceViewModels)
                {
                    if (deviceViewModel.Address != null && !addressList.Contains((int)deviceViewModel.Address))
                        addressList.Add((int)deviceViewModel.Address);
                    
                    if (deviceViewModel.StateAddress != null && !addressList.Contains((int)deviceViewModel.StateAddress))
                        addressList.Add((int)deviceViewModel.StateAddress);
                }
            }

            foreach (var solenoidValveViewModel in _settingsService.StandSettingsModel.SolenoidValveViewModels)
            {
                if (solenoidValveViewModel.Address != null && !addressList.Contains((int)solenoidValveViewModel.Address))
                    addressList.Add((int)solenoidValveViewModel.Address);
            }
            
            for (var i = 0; i < _settingsService.StandSettingsModel.LineViewModels.Count; i++)
            {
                _lines.Add(new StandLine(_settingsService, _modbusProcessor, i));
            }
            
            _standDevices = new List<StandDevice>();
            foreach (var address in addressList)
            {
                _standDevices.Add(new StandDevice(_modbusProcessor, standDeviceRegisterMap, address));
            }

            _temperatureSensor = new TemperatureSensor(_modbusProcessor, new RegisterMapEnum<TemperatureSensorRegisterMap>(),
                _settingsService.StandSettingsModel.TemperatureSensorAddress);
            _pressureSensor = new PressureSensor(_modbusProcessor, new RegisterMapEnum<PressureSensorRegisterMap>(),
                _settingsService.StandSettingsModel.PressureSensorAddress);
            _pressureDifferenceSensor = new PressureSensor(_modbusProcessor, new RegisterMapEnum<PressureSensorRegisterMap>(),
                _settingsService.StandSettingsModel.PressureDifferenceSensorAddress);
            _pressureSensor415M = new PressureSensor415M(_modbusProcessor, new RegisterMapEnum<PressureSensor415MRegisterMap>(),
                _settingsService.StandSettingsModel.PressureResiverSensorAddress);
            _thMeter = new THMeter(_modbusProcessor, new RegisterMapEnum<THMeterRegisterMap>(),
                _settingsService.StandSettingsModel.THMeterAddress);

            _frequencyRegulatorDevice = new FrequencyRegulatorDevice(_modbusProcessor, new RegisterMapEnum<FrequencyRegulatorRegisterMap>(),
                GetPressureResiver,
                -80,
                129);
            _frequencyRegulatorDevice?.SetPidParameters(10, 0.1, 0.01, 100, 0, 50, 0);

            ((ISerialCommunicator)_modbusProcessor.Communicator).AddCollectionToLogger(_portLogMessages);

            //_modbusProcessor.Start();

            _requestTaskCancellationTokenSource = new CancellationTokenSource();
            _requestTask = new Task(RequestTaskHandler, _requestTaskCancellationTokenSource.Token);
            //_requestTask.Start();
        }

        public void TestInitialization()
        {
            _modbusProcessor = new ModbusProcessor(new RequestSerializer(), new ResponseDeserializer())
            {
                Communicator = new SerialCommunicator()
            };

            var addressList = new List<int>();

            foreach (var valveViewModel in _settingsService.StandSettingsModel.NozzleViewModels)
            {
                if (valveViewModel.Address != null && !addressList.Contains((int)valveViewModel.Address))
                    addressList.Add((int)valveViewModel.Address);

                if (valveViewModel.StateAddress != null && !addressList.Contains((int)valveViewModel.StateAddress))
                    addressList.Add((int)valveViewModel.StateAddress);
            }

            foreach (var nozzleViewModel in _settingsService.StandSettingsModel.ValveViewModels)
            {
                if (nozzleViewModel.Address != null && !addressList.Contains((int)nozzleViewModel.Address))
                    addressList.Add((int)nozzleViewModel.Address);

                if (nozzleViewModel.StateAddress != null && !addressList.Contains((int)nozzleViewModel.StateAddress))
                    addressList.Add((int)nozzleViewModel.StateAddress);
            }

            foreach (var solenoidValveModel in _settingsService.StandSettingsModel.SolenoidValveViewModels)
            {
                if (solenoidValveModel.Address != null && !addressList.Contains((int)solenoidValveModel.Address))
                    addressList.Add((int)solenoidValveModel.Address);
            }

            for (var i = 0; i < _settingsService.StandSettingsModel.LineViewModels.Count; i++)
            {
                _lines.Add(new StandLine(_settingsService, _modbusProcessor, i));
            }
            

            _standDevices = new List<StandDevice>();
            foreach (var address in addressList)
            {
                _standDevices.Add(new StandDevice(_modbusProcessor, new RegisterMapEnum<StandDeviceRegisterMap>(), address));
            }

            _temperatureSensor = new TemperatureSensor(_modbusProcessor, new RegisterMapEnum<TemperatureSensorRegisterMap>(),
                _settingsService.StandSettingsModel.TemperatureSensorAddress);
            _pressureSensor = new PressureSensor(_modbusProcessor, new RegisterMapEnum<PressureSensorRegisterMap>(),
                _settingsService.StandSettingsModel.PressureSensorAddress);
            _pressureDifferenceSensor = new PressureSensor(_modbusProcessor, new RegisterMapEnum<PressureSensorRegisterMap>(),
                _settingsService.StandSettingsModel.PressureDifferenceSensorAddress);
            _pressureSensor415M = new PressureSensor415M(_modbusProcessor, new RegisterMapEnum<PressureSensor415MRegisterMap>(),
                _settingsService.StandSettingsModel.PressureResiverSensorAddress);
            _thMeter = new THMeter(_modbusProcessor, new RegisterMapEnum<THMeterRegisterMap>(),
                _settingsService.StandSettingsModel.THMeterAddress);

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
                    Temperature = new Random().Next(15, 25);
                    PressureAtmosphere = 101325f;
                    PressureResiver = 4f;
                    PressureDifference = 2000f;
                    Humidity = new Random().Next(30, 90);
                    TemperatureTube = new Random().Next(15, 25);

                    
                    if (_line != null)
                        foreach (var device in _line.Devices)
                        {
                            await ((IUniversalDevice)device).ReadPressureAsync();
                        }
                    

#else
                    if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                        TemperatureTube = _temperatureSensor == null ? null : await _temperatureSensor.ReadTemperatureAsync();
                    if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                        PressureAtmosphere = _pressureSensor == null ? null : await _pressureSensor.ReadPressureAsync();
                    if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                        PressureDifference = _pressureDifferenceSensor == null ? null : await _pressureDifferenceSensor.ReadPressureAsync();
                    if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                        PressureResiver = _pressureSensor415M == null ? null : await _pressureSensor415M.ReadPressureAsync();
                    if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                        Temperature = _thMeter == null ? null : await _thMeter.ReadTemperatureAsync() / 100f;
                    if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                        Humidity = _thMeter == null ? null : await _thMeter.ReadHumidityAsync() / 100f;

                    if (_line != null)
                        foreach (var device in _line.Devices)
                        {
                            await ((IUniversalDevice)device).ReadPressureAsync();
                        }

                    /*foreach (var device in _devices)
                    {
                        if (!_requestTaskCancellationTokenSource.Token.IsCancellationRequested)
                            if (device != null)
                                await ((IUniversalDevice)device).ReadPressureAsync();
                    }*/
#endif

                    await Task.Delay(3000);
                }

                _isTaskExecute = false;
            }
            catch (Exception e)
            {
                //ignore
            }
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
                NotifyObserverByDataPair(new DataPair(new LineInfoData(_selectedLineIndex, _selectedLineIndex != null), DeviceInfoParameterType.LineState));
            }
        }

        public void UpdateDeviceInformation(DeviceAboutViewModel deviceInformationViewModel)
        {
            _line.Devices[deviceInformationViewModel.DeviceNumber - 1].VendorNumberString = deviceInformationViewModel.DeviceVendorNumber;
            _line.Devices[deviceInformationViewModel.DeviceNumber - 1].DeviceName = deviceInformationViewModel.DeviceName;
            _line.Devices[deviceInformationViewModel.DeviceNumber - 1].IsManualEnabled = deviceInformationViewModel.IsManualEnabled;
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
        public async Task<bool> OpenNozzleAsync(StandSettingsNozzleModel standSettingsNozzleModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Work), DeviceInfoParameterType.NozzleState));
            
            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsNozzleModel.Address)
                    ?.SetBitState(standSettingsNozzleModel.BitNumber, false, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Open),
                    DeviceInfoParameterType.NozzleState));
                return true;
            }

            if (!standSettingsNozzleModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Open),
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

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Open), DeviceInfoParameterType.NozzleState));

            
            return !isWork;
        }

        /// <summary>
        /// Открыть клапан
        /// </summary>
        /// <param name="standSettingsValveModel">Настройки клапана</param>
        /// <returns>Результат отправки запроса</returns>
        public async Task<bool> OpenValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work), DeviceInfoParameterType.ValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, false, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.ValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.ValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.ValveState));
            return !isWork;
        }

        /// <summary>
        /// Закрыть сопло
        /// </summary>
        /// <param name="standSettingsNozzleModel">Настройки сопла</param>
        /// <param name="withoutWrite">Без записи состояние в регистр (по умолчанию записывается всегда)</param>
        /// <returns>Результат отправки запроса</returns>
        public async Task<bool> CloseNozzleAsync(StandSettingsNozzleModel standSettingsNozzleModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Work), DeviceInfoParameterType.NozzleState));
            
            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsNozzleModel.Address)
                    ?.SetBitState(standSettingsNozzleModel.BitNumber, true, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.NozzleState));
                return true;
            }

            if (!standSettingsNozzleModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Close),
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

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsNozzleModel.Number - 1, StateType.Close),
                DeviceInfoParameterType.NozzleState));

            
            return !isWork;
        }

        /// <summary>
        /// Закрыть клапан
        /// </summary>
        /// <param name="standSettingsValveModel">Настройки клапана</param>
        /// <returns>Результат отправки запроса</returns>
        public async Task<bool> CloseValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work), DeviceInfoParameterType.ValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, true, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close), DeviceInfoParameterType.ValveState));
            return !isWork;
        }
        
        public async Task<bool> CloseDeviceValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work), DeviceInfoParameterType.LineValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, true, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.LineValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.LineValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close), DeviceInfoParameterType.LineValveState));
            return !isWork;
        }

        public async Task<bool> OpenDeviceValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work), DeviceInfoParameterType.LineValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, false, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.LineValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.LineValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.LineValveState));
            return !isWork;
        }

        public async Task<bool> CloseReverseValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work), DeviceInfoParameterType.ReverseValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, true, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close),
                    DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Close), DeviceInfoParameterType.ReverseValveState));
            return !isWork;
        }

        public async Task<bool> OpenReverseValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false)
        {
            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Work), DeviceInfoParameterType.ReverseValveState));

            if (!await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.Address)
                    ?.SetBitState(standSettingsValveModel.BitNumber, false, withoutWrite)!) return false;

            if (withoutWrite)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            if (!standSettingsValveModel.IsControlState)
            {
                NotifyObserverByDataPair(
                    new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.ReverseValveState));
                return true;
            }

            var isWork = true;
            var count = 60;

            while (isWork && count > 0)
            {
                await Task.Delay(1000);

                var moduleState = await _standDevices
                    .FirstOrDefault(sd => sd.ModuleAddressInt == standSettingsValveModel.StateAddress)
                    .GetInfoRegisterBitAsync((int)standSettingsValveModel.StateBitNumber);

                isWork = moduleState;
                count--;
            }

            NotifyObserverByDataPair(new DataPair(new StandInfoData(standSettingsValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.ReverseValveState));
            return !isWork;
        }

        public async Task<bool> SetPulseCountAsync(int pulseCount, int deviceIndex)
        {
            return await _line.Devices[deviceIndex].SetPulseCountAsync(pulseCount);
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

        public async Task<bool> SetConsumptionWithoutSelectionAsync(ObservableCollection<StandSettingsNozzleModel> pointSelectedNozzles)
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

        public async Task SetFlowDirectionAsync(LineDirectionFlowState reverseDirection, int lineNumber)
        {
            switch (reverseDirection)
            {
                case LineDirectionFlowState.AllOpen:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.AllOpen, lineNumber), DeviceInfoParameterType.ReverseValveState));
#else 
                    
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].DirectValveViewModel);
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.AllOpen, lineNumber), DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                case LineDirectionFlowState.AllClose:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.AllClose, lineNumber), DeviceInfoParameterType.ReverseValveState));
#else 

                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].DirectValveViewModel);
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.AllClose, lineNumber), DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                case LineDirectionFlowState.DirectDirection:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.DirectDirection, lineNumber), DeviceInfoParameterType.ReverseValveState));
#else 

                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].DirectValveViewModel);
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.DirectDirection, lineNumber), DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                case LineDirectionFlowState.ReverseDirection:
#if DEBUGGUI
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await Task.Delay(5000);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.ReverseDirection, lineNumber), DeviceInfoParameterType.ReverseValveState));
#else 

                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.Working, lineNumber), DeviceInfoParameterType.ReverseValveState));
                    await CloseReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].DirectValveViewModel);
                    await OpenReverseValveAsync(_settingsService.StandSettingsModel.LineViewModels[lineNumber - 1].ReverseValveViewModel);
                    NotifyObserverByDataPair(new DataPair (new DirectFlowInfoData(LineDirectionFlowState.ReverseDirection, lineNumber), DeviceInfoParameterType.ReverseValveState));
#endif
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reverseDirection), reverseDirection, null);
            }
        }

        public void SetActiveLine(int lineNumber, bool isActiveLine) 
            => SelectedLineIndex = isActiveLine ?  lineNumber - 1 : null;

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
                if (!await OpenNozzleAsync(_settingsService.StandSettingsModel.NozzleViewModels.FirstOrDefault(nz => nz.Number == nozzleNumber)))
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

            var result =  await _standDevices
                .FirstOrDefault(sd => sd.ModuleAddressInt == solenoidValveModel.Address)
                ?.SetBitState(solenoidValveModel.BitNumber, solenoidValveModel.SolenoidValveType switch
                {
                    SolenoidValveType.NormalOpen => false,
                    SolenoidValveType.NormalClose => true,
                    _ => throw new ArgumentOutOfRangeException()
                })!;

            NotifyObserverByDataPair(new DataPair(new StandInfoData(solenoidValveModel.Number - 1, StateType.Open), DeviceInfoParameterType.SolenoidValveState));
            
            return result;
        }

        public async Task<bool> CloseSolenoidValveAsync(StandSettingsSolenoidValveModel solenoidValveModel)
        {
            var result =  await _standDevices
                .FirstOrDefault(sd => sd.ModuleAddressInt == solenoidValveModel.Address)
                ?.SetBitState(solenoidValveModel.BitNumber, solenoidValveModel.SolenoidValveType switch
                {
                    SolenoidValveType.NormalOpen => true,
                    SolenoidValveType.NormalClose => false,
                    _ => throw new ArgumentOutOfRangeException()
                })!;

            NotifyObserverByDataPair(new DataPair(new StandInfoData(solenoidValveModel.Number - 1, StateType.Close), DeviceInfoParameterType.SolenoidValveState));

            return result;
        }

        public async Task<bool> EndWorkAsync()
        {
            PidDisable();

            if (!await OpenSolenoidValveAsync(
                    _settingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(sv => sv.SolenoidValveType == SolenoidValveType.NormalClose)))
                return false;

            if (!await CloseAllNozzleAsync()) return false;
            if (!await CloseAllValveAsync()) return false;

            if (!await CloseSolenoidValveAsync(
                    _settingsService.StandSettingsModel.SolenoidValveViewModels.FirstOrDefault(sv => sv.SolenoidValveType == SolenoidValveType.NormalClose)))
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

        public void RegisterPressureSensorObserver(IPressureSensorObserver observer, int deviceNumber, int lineNumber) 
            => ((IPressureSensorObservable)_lines[lineNumber].Devices[deviceNumber]).RegisterPressureSensorObserver(observer);
        
        public void RegisterDeviceObserver(IDeviceObserver observer, int deviceNumber, int lineNumber) => 
            ((IDeviceObservable)_lines[lineNumber].Devices[deviceNumber]).RegisterDeviceObserver(observer);

        public string GetVendorNumber(int deviceNumber) => _line.Devices[deviceNumber].VendorNumberString;
        
        public string GetDeviceName(int deviceNumber) => _line.Devices[deviceNumber].DeviceName;
        
        public async Task EmergencyPowerOffAsync()
        {
            await OpenSolenoidValveAsync(_settingsService.StandSettingsModel.SolenoidValveViewModels
                .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalClose));
            await CloseAllNozzleAsync();
            PidDisable();
            await DisableVacuumCreator();

            await Task.Delay(5000);
            
            await OpenSolenoidValveAsync(_settingsService.StandSettingsModel.SolenoidValveViewModels
                .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalOpen));
            await CloseSolenoidValveAsync(_settingsService.StandSettingsModel.SolenoidValveViewModels
                .FirstOrDefault(svm => svm.SolenoidValveType == SolenoidValveType.NormalClose));
        }

        public async Task<bool> ResetToZeroAsync(int deviceNumber) => await _line.Devices[deviceNumber - 1].ResetToZeroAsync();


        public async Task<bool> SetStandWorkModeAsync()
        {
            foreach (var nozzleViewModel in _settingsService.StandSettingsModel.NozzleViewModels)
            {
                if (!await CloseNozzleAsync(nozzleViewModel, true))
                    return false;
            }

            foreach (var lineViewModel in _settingsService.StandSettingsModel.LineViewModels)
            {
                if (lineViewModel.IsReverseLine)
                {
                    if (!await CloseReverseValveAsync(lineViewModel.DirectValveViewModel, true))
                        return false;
                    if (!await CloseReverseValveAsync(lineViewModel.ReverseValveViewModel, true))
                        return false;
                }

                if (lineViewModel.SelectedDeviceLineType == DeviceLineType.JetDevice)
                {
                    foreach (var deviceViewModel in lineViewModel.DeviceViewModels)
                    {
                        if (!await CloseDeviceValveAsync(new StandSettingsValveModel
                            {
                                Number = 0,
                                Address = deviceViewModel.Address,
                                RegisterAddress = deviceViewModel.RegisterAddress,
                                BitNumber = deviceViewModel.BitNumber,
                                StateAddress = deviceViewModel.StateAddress,
                                StateRegisterAddress = deviceViewModel.StateRegisterAddress,
                                StateBitNumber = deviceViewModel.StateBitNumber,
                                IsControlState = deviceViewModel.IsControlState,
                                LineNumber = (int)_selectedLineIndex
                            }, true))
                            return false;
                    }
                }
            }

            foreach (var standDevice in _standDevices.Where(d => d.NeedUpdateState))
            {
                if (!await standDevice.SetWorkRegisterAsync())
                    return false;
            }

            return true;
        }

        private Task<bool> ChoseValveForOpenAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Вакуумный насос

        public async Task<bool> EnableVacuumCreator()
        {
            return await _frequencyRegulatorDevice.StartFrequencyWorkAsync();
        }

        public async Task<bool> DisableVacuumCreator()
        {
            return await _frequencyRegulatorDevice.StopFrequencyWorkAsync();
        }

        public void PidEnable()
        {
            _frequencyRegulatorDevice.PidEnable();
        }

        public void PidDisable()
        {
            _frequencyRegulatorDevice.PidDisable();
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

        private readonly List<IObserver> _observers = new();
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

            _modbusProcessor?.ShutDown();
        }

        public async Task<bool> ResetToZeroPressureDifferenceAsync()
        {
            return await _pressureDifferenceSensor.ResetToZeroAsync();
        }

        

        #endregion
    }
}