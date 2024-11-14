using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SPU_7.Common.Device;
using SPU_7.Common.Line;
using SPU_7.Common.Stand;
using SPU_7.Domain.Devices.StandDevices.PulseCountMeterModule;
using SPU_7.Domain.Devices.StandDevices.PulseMeter;
using SPU_7.Domain.Extensions;
using SPU_7.Extensions.Interface;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.ViewModels;
using SPU_7.ViewModels.DeviceInformationViewModels;
using SPU_7.ViewModels.MnemonicSchemeViewModels;

namespace SPU_7.Models.Stand
{
    /// <summary>
    /// Интерфейс управления стендом
    /// </summary>
    public interface IStandController : IDisposable, IStandObservable
    {
        /// <summary>
        /// Инициализация устройств стенда
        /// </summary>
        void Initialization();
        
        /// <summary>
        /// Установить режим измерения периодов
        /// </summary>
        /// <param name="modeMeasure">Режим</param>
        /// <returns>Результат запроса</returns>
        Task<bool> SetModeMeasureAsync(ModeMeasure modeMeasure);

        /// <summary>
        /// Устанавить наиболее близкий расход к заданному с помощью сопел для точки
        /// <param name="flowValue">Расход для точки</param>
        /// </summary>
        /// <returns>Результат запроса</returns>
        Task<bool> SetPerfectFlowAsync(double flowValue);

        /// <summary>
        /// Пауза для установки расхода для точки
        /// </summary>
        /// <param name="delay">Задержка</param>
        Task PauseBeforeCalibration(int delay);

        /// <summary>
        /// Перерасчет эталонного расхода с учетом среды
        /// </summary>
        /// <param name="flowValue">Расход</param>
        /// <returns>Рассчитанный расход</returns>
        double PerfectFlowCalculate(double flowValue);
        
        Task<bool> UpdateStateDevice(int address);
        Task<bool> UpdateAllDevice();
        
        /// <summary>
        /// Открыть клапан
        /// </summary>
        /// <param name="standSettingsNozzleModel">Настройки клапана</param>
        /// <returns>Результат отправки запроса</returns>
        Task<bool> OpenNozzleAsync(StandSettingsNozzleModel standSettingsNozzleModel, bool withoutWrite = false);

        /// <summary>
        /// Открыть сопло подачи газа в устройство
        /// </summary>
        /// <param name="standSettingsValveModel">Настройки сопла</param>
        /// <returns>Результат отправки запроса</returns>
        Task<bool> OpenValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false);

        /// <summary>
        /// Закрыть клапан
        /// </summary>
        /// <param name="standSettingsNozzleModel">Настройки клапана</param>
        /// <returns>Результат отправки запроса</returns>
        Task<bool> CloseNozzleAsync(StandSettingsNozzleModel standSettingsNozzleModel, bool withoutWrite = false);

        /// <summary>
        /// Закрыть сопло подачи газа в устройство
        /// </summary>
        /// <param name="standSettingsValveModel">Настройки сопла</param>
        /// <returns>Результат отправки запроса</returns>
        Task<bool> CloseValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false);

        /// <summary>
        /// Использовать клапан подключенный к ОВЕН 10ПРА
        /// </summary>
        /// <param name="standSettingsValveModel"></param>
        /// <param name="lineIndex"></param>
        /// <param name="owenValue"></param>
        /// <returns></returns>
        Task<bool> UseOwenValveAsync(StandSettingsOwenValveModel standSettingsValveModel, int lineIndex, int owenValue);

        
        /// <summary>
        /// Запустить открытие задвижки
        /// </summary>
        /// <param name="numberGate">номер задвижки (1 - 1-5 устройства, 2 - 6-10 устройства)</param>
        /// <returns></returns>
        Task<bool> RunOpenGateAsync(int numberGate);

        /// <summary>
        /// Запустить закрытие задвижки
        /// </summary>
        /// <param name="numberGate">номер задвижки (1 - 1-5 устройства, 2 - 6-10 устройства)</param>
        /// <returns></returns>
        Task<bool> RunCloseGateAsync(int numberGate);

        /// <summary>
        /// Остановить задвижку
        /// </summary>
        /// <param name="numberGate">номер задвижки (1 - 1-5 устройства, 2 - 6-10 устройства)</param>
        /// <returns></returns>
        Task<bool> StopGateAsync(int numberGate);
        

        /// <summary>
        /// Ввод текущего времени 
        /// </summary>
        /// <param name="deviceIndex">Номер устройства</param>
        /// <returns>Результат ввода</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException"></exception>
        Task<bool> SetTimeSynchronizationAsync(int deviceIndex);

        /// <summary>
        /// Настраивает диапазон в соответствии с устройством
        /// </summary>
        /// <param name="deviceIndex">Номер устройства</param>
        /// <param name="rangeValue">Значение диапазона</param>
        /// <returns>Результат записи</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException">туду</exception>
        Task<bool> SetRangeDeviceAsync(int deviceIndex, float rangeValue);

        /// <summary>
        /// Поверка платформы
        /// </summary>
        /// <param name="deviceIndex">Номер устройства</param>
        /// <returns>Результат поверки</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException">туду</exception>
        Task<bool> CheckPlatformDeviceAsync(int deviceIndex);

        /// <summary>
        /// Поверка связи
        /// </summary>
        /// <param name="deviceIndex">Номер устройства</param>
        /// <returns>Результат поверки</returns>
        /// <exception cref="ArgumentOutOfRangeException">Нет выбранного типа счетчика</exception>
        /// <exception cref="NotImplementedException">туду</exception>
        Task<bool> CheckConnectionDeviceAsync(int deviceIndex);

        /// <summary>
        /// Установить расход сопел
        /// </summary>
        /// <param name="value">Расход</param>
        /// <returns>Результат установки расхода</returns>
        Task<double?> SetConsumptionAsync(double value, double? minimumFlow, double? maximumFlow);
        
        /// <summary>
        /// Установить расход
        /// </summary>
        /// <param name="value">Расход</param>
        /// <returns>Результат установки расхода</returns>
        Task<bool> EnableConsumptionAsync(double value, int indexOfFanLine, int indexOfFan, int indexOfMasterDeviceLine, int indexOfMasterDevice, int? needleValveValue = null);
        
        /// <summary>
        /// Выключить расход
        /// </summary>
        /// <param name="value">Расход</param>
        /// <returns>Результат установки расхода</returns>
        Task<bool> DisableConsumptionAsync(double value, int indexOfFanLine, int indexOfFan);

        
        /// <summary>
        /// Установить расход сопел на линии
        /// </summary>
        /// <param name="value">Расход</param>
        /// <returns>Результат установки расхода</returns>
        Task<double?>  SetConsumptionAsync(double pointTargetConsumption, int pointSelectedLineIndex, double? minimumFlow, double? maximumFlow);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="portLogMessages"></param>
        void AddCollectionForPortLogging(ObservableCollection<LogMessage> portLogMessages);
        
        /// <summary>
        /// Зарегистрировать наблюдателя за ДД на стенде с СГ
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        /// <param name="devicePurpose"></param>
        /// <param name="deviceIndex">Номер СГ (от 0)</param>
        /// <param name="lineIndex"></param>
        void RegisterPressureSensorObserver(IPressureSensorObserver observer, DevicePurpose devicePurpose, int deviceIndex, int lineIndex);

        /// <summary>
        /// Зарегистрировать наблюдателя за ДТ на стенде с СГ
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        /// <param name="devicePurpose"></param>
        /// <param name="deviceIndex">Номер СГ (от 0)</param>
        /// <param name="lineIndex"></param>
        void RegisterTemperatureSensorObserver(ITemperatureSensorObserver observer, DevicePurpose devicePurpose, int deviceIndex, int lineIndex);

        /// <summary>
        /// Зарегистрировать наблюдателя за потокком на стенде с СГ
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        /// <param name="devicePurpose"></param>
        /// <param name="deviceIndex">Номер СГ (от 0)</param>
        /// <param name="lineIndex"></param>
        void RegisterFlowObserver(IFlowObserver masterDeviceItemViewModel, DevicePurpose devicePurpose, int deviceIndex, int lineIndex);

        /// <summary>
        /// Зарегистрировать наблюдателя за ДД на стенде с СГ
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        /// <param name="devicePurpose"></param>
        /// <param name="deviceIndex">Номер СГ (от 0)</param>
        /// <param name="lineIndex"></param>
        void UnsubscribePressureSensorObserver(IPressureSensorObserver observer, DevicePurpose devicePurpose, int deviceIndex, int lineIndex);

        /// <summary>
        /// Зарегистрировать наблюдателя за ДТ на стенде с СГ
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        /// <param name="devicePurpose"></param>
        /// <param name="deviceIndex">Номер СГ (от 0)</param>
        /// <param name="lineIndex"></param>
        void UnsubscribeTemperatureSensorObserver(ITemperatureSensorObserver observer, DevicePurpose devicePurpose, int deviceIndex, int lineIndex);

        /// <summary>
        /// Зарегистрировать наблюдателя за потокком на стенде с СГ
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        /// <param name="devicePurpose"></param>
        /// <param name="deviceIndex">Номер СГ (от 0)</param>
        /// <param name="lineIndex"></param>
        void UnsubscribeFlowObserver(IFlowObserver observer, DevicePurpose devicePurpose, int deviceIndex, int lineIndex);
        
        
        /// <summary>
        /// Установить рабочий режим стенда
        /// </summary>
        /// <returns></returns>
        Task<bool> SetStandWorkModeAsync();

        Task<bool> CloseAllNozzleAsync();
        Task<bool> CloseAllNozzleAsync(int selectedLineIndex);

        Task<bool> OpenSolenoidValveAsync(StandSettingsSolenoidValveModel solenoidValveModel);
        Task<bool> CloseSolenoidValveAsync(StandSettingsSolenoidValveModel solenoidValveModel);
        Task<bool> EnableFrequencyRegulatorAsync(int regulatorIndex);
        Task<bool> EnableFrequencyRegulatorAsync(StandSettingsFanModel? settingsFanModel);
        Task<bool> DisableFrequencyRegulatorAsync(int regulatorIndex);
        Task<bool> DisableFrequencyRegulatorAsync(StandSettingsFanModel? settingsFanModel);
        Task<bool> EnableLineFanWorkAsync(int lineIndex, int fanIndex);
        Task<bool> DisableLineFanWorkAsync(int lineIndex, int fanIndex);
        Task<bool> SetRegulatorFrequencyAsync(int regulatorIndex ,float frequency);
        Task<bool> SetRegulatorFrequencyAsync(StandSettingsFanModel? settingsFanModel, float frequency);
        
        float? PressureAtmosphere { get; set; }
        float? Humidity { get; set; }
        float? Temperature { get; set; }
        double? RealFlow { get; set; }
        
        void UpdateDeviceInformation(DeviceAboutViewModel deviceAboutViewModel);
        void RegisterDeviceObserver(IDeviceObserver deviceItemViewModel, int deviceNumber, int lineNumber);
        string GetVendorNumber(int deviceNumber);
        
        string GetVendorNumber(int deviceNumber, int lineIndex);
        
        /// <summary>
        /// Сброс датчика давления на ноль
        /// </summary>
        /// <returns></returns>
        Task<bool> ResetToZeroAsync(int deviceNumber);
        
        /// <summary>
        /// Получить имя устройства (если задавалось ранее)
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <returns></returns>
        string GetDeviceName(int deviceNumber);
        string GetDeviceName(int deviceNumber, int lineIndex);
        bool GetDeviceManualEnable(int i, int lineIndex);

        Task EmergencyPowerOffAsync();
        Task<bool> ResetToZeroPressureDifferenceAsync();
        bool GetDeviceManualEnable(int i);
        Task<bool> SetConsumptionWithoutSelectionAsync(ObservableCollection<StandSettingsNozzleModel> pointSelectedNozzles);
        
        
        void SetLineTargetFlowValue(double? value, int lineIndex);
        void AddLineTargetFlowValue(double? value, int lineIndex);
        
        /// <summary>
        /// Установить направление потока клапанами в линии (если линия позволяет)
        /// </summary>
        /// <param name="reverseDirection">Тип направления</param>
        /// <param name="lineNumber">Номер линии (не индекс)</param>
        /// <returns></returns>
        Task SetFlowDirectionAsync(LineDirectionFlowState reverseDirection, int lineNumber);

        /// <summary>
        /// Установить активную линию (активная линия может быть только одна)
        /// </summary>
        /// <param name="lineNumber">Номер линии (не индекс)</param>
        /// <param name="isActiveLine">Вкл или выкл линию</param>
        void SetActiveLine(int lineNumber, bool isActiveLine);

        /// <summary>
        /// Получить индекс активной линии
        /// </summary>
        /// <returns>Индекс</returns>
        int? GetActiveLine();

        Task<bool> CloseDeviceValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false);
        Task<bool> OpenDeviceValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false);
        
        Task<bool> CloseReverseValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false);
        Task<bool> OpenReverseValveAsync(StandSettingsValveModel standSettingsValveModel, bool withoutWrite = false);
        
        /// <summary>
        /// Установить количество импульсов для БИПЧ
        /// </summary>
        /// <param name="pulseCount">Количество импульсов</param>
        /// <param name="deviceIndex">Индекс устройства (каждому устройству привязан свой БИПЧ)</param>
        /// <returns>Получилось ли отправить запрос</returns>
        Task<bool> SetPulseCountForPulseMeterAsync(int pulseCount, int deviceIndex);
        Task<bool> SetPulseCountForPulseMeterAsync(int pulseCount, int pulseMeterAddress, int pulseMeterChannelNumber);


        /// <summary>
        /// Считать время начала измерения импульсов
        /// </summary>
        /// <param name="deviceIndex">Индекс устройства</param>
        /// <returns>Время</returns>
        Task<float?> ReadStartPulseMeasureTimeAsync(int deviceIndex);
        
        /// <summary>
        /// Считать время конца измерения импульсов
        /// </summary>
        /// <param name="deviceIndex">Индекс устройства</param>
        /// <returns>Время</returns>
        Task<float?> ReadEndPulseMeasureTimeAsync(int deviceIndex);

        /// <summary>
        ///  Считать количество измеренных импульсов импульсов
        /// </summary>
        /// <returns>количество измеренных импульсов</returns>
        Task<int?> ReadPulseCountAsync(int deviceIndex);

        string GetVendorName(int activeLine, int i);
        DeviceNameViewModel GetDeviceInfoType(int activeLine, int deviceIndex);
        
        
        /// <summary>
        /// Считать коэффициенты калибровки БИПЧ
        /// </summary>
        /// <returns></returns>
        Task<List<(float?, float?, float?)>> ReadPulseCoefficientsAsync();

        /// <summary>
        /// Записать коэффициенты калибровка в БИПЧи
        /// </summary>
        /// <param name="coefficientTuples">Кортеж из 3х коэффициентов</param>
        /// <returns></returns>
        Task<bool> WritePulseCoefficientsAsync(List<(float, float, float)> coefficientTuples);
        
        Task<CommonCommandStatus?> ReadCommonCommandStatusPulseCountMeterAsync(int? pulseCountMeterModuleIndex);
        Task<bool> StartPulseCountMeterModuleMeasureAsync(int? pulseCountMeterModuleIndex);
        Task<bool> StopPulseCountModuleMeasureAsync(int? pulseCountMeterModuleIndex);
        Task<float?> ReadPulsePeriodFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex, int? channelNumber);
        Task<float?> ReadPulseDurationFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex, int? channelNumber);
        Task<bool> SendStartPulseCountMeterCommandAsync();
        Task<bool> SetPulseCountMeterModuleChannelSettingsAsync(int? pulseCountMeterModuleIndex, int channelNumber);
        Task<bool> TurnOnPulseCountMeterControlRegister();
        Task<bool> TurnOffPulseCountMeterControlRegister();

        Task<bool> ResetPulseCountMeterAsync();
        Task<float?> ReadPulseCountFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex, int pulseCountMeterModuleChannelNumber);
        Task<float?> ReadMeasureTimeFromPulseCountMeterAsync(int? pulseCountMeterModuleIndex);

        float? GetPressureDifferenceFromMasterDevice(int lineIndex);
        float? GetTemperatureFromMasterDevice(int selectedLineIndex, int indexOfMasterDevice);
        float? GetFlowFromMasterDevice(int selectedLineIndex, int indexOfMasterDevice);

        Task<bool> UseNeedleValveAsync(StandSettingsNeedleValveModel standSettingsValveModel, int selectedNeedleValue);
        float? GetTemperatureFromLine(int lineIndex);
        float? GetPressureDifferenceFromLine(int lineIndex);
        float? GetPressureDifferenceFromDevice(int selectedLineIndex, int deviceIndex);
        float? GetTemperatureFromDevice(int activeLine, int indexOf);

        Task<uint?> ReadFreeRunPulseCount(int pulseMeterAddress, int pulseMeterChannelNumber);
        
        
        /// <summary>
        /// Запуск измерения импульсов на БИПЧе
        /// </summary>
        /// <param name="pulseCount">Импульсы</param>
        /// <param name="pulseMeterAddress">Адрес БИПЧ</param>
        /// <param name="pulseMeterChannelNumber">Канал БИПЧ</param>
        /// <returns></returns>
        Task<bool> StartPulseMeterPeriodMeasureAsync(int pulseCount, int pulseMeterAddress, int pulseMeterChannelNumber);

        /// <summary>
        /// Считать период измерения с БИПЧа
        /// </summary>
        /// <param name="pulseMeterAddress">Адрес БИПЧ</param>
        /// <param name="pulseMeterChannelNumber">Канал БИПЧ</param>
        /// <returns></returns>
        Task<float?> ReadPeriodFromPulseMeterAsync(int pulseMeterAddress, int pulseMeterChannelNumber);

        /// <summary>
        /// Считать текущий статус с БИПЧа
        /// </summary>
        /// <param name="pulseMeterAddress"></param>
        /// <param name="pulseMeterChannelNumber"></param>
        /// <returns></returns>
        Task<PulseMeter2ChannelState> GetPulseMeterStatusAsync(int pulseMeterAddress, int pulseMeterChannelNumber);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pulseWeight"></param>
        /// <param name="pulseTimeList"></param>
        /// <returns></returns>
        Task PulseReadStartAsync(double? pulseWeight, double targetVolume);

        /// <summary>
        /// Запуск задачи ожидания работы БИПЧ
        /// </summary>
        /// <param name="pulseTimeList"></param>
        /// <param name="timeValidation"></param>
        /// <param name="activeLine"></param>
        /// <returns></returns>
        Task PulseReadProcessStartAsync(List<float?> pulseTimeList, double timeValidation, int? activeLine);

        Task<bool> DisableVacuumCreator(int? activeLine);
        Task<bool> EnableVacuumCreator(int? activeLine);
        bool IsDeviceWithTemperatureCorrect(int deviceIndex);
        bool GetTemperatureCorrect(int lineIndex, int i);
        Task<float?> GetPressureDischargerFromLineAsync(int activeLine);
        double? SelectMetrologyCoefficient(float? temperature, float? humidity);
        double GetTargetFlowFromMasterDevice(int lineIndex, int masterDeviceIndex);
        Task<CommonCommandStatus?> GetPulseCountMeterStatusAsync(int? pulseCountMeterModuleIndex,
            int pulseCountMeterModuleChannelNumber);
    }
}
