using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SPU_7.Common.Line;
using SPU_7.Common.Stand;
using SPU_7.Domain.Extensions;
using SPU_7.Models.Stand.Settings.Stand.Extensions;
using SPU_7.ViewModels.DeviceInformationViewModels;

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
        /// Fake Инициализация устройств стенда для отладки GUI
        /// </summary>
        void TestInitialization();
        
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
        /// Установить режим измерения частоты
        /// </summary>
        /// <returns>Результат отправки запроса</returns>
        Task<bool> SetFrequencyMeasureModeAsync();

        /// <summary>
        /// Установить режим измерения периодов
        /// </summary>
        /// <returns>Результат отправки запроса</returns>
        Task<bool> SerPeriodMeasureModeAsync();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="portLogMessages"></param>
        void AddCollectionForPortLogging(ObservableCollection<LogMessage> portLogMessages);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="portLogMessages"></param>
        void AddCollectionForDevicePortLogging(ObservableCollection<LogMessage> portLogMessages);

        /// <summary>
        /// Зарегистрировать наблюдателя за ДД на стенде с СГ
        /// </summary>
        /// <param name="observer">Наблюдатель</param>
        /// <param name="deviceNumber">Номер СГ (от 0)</param>
        void RegisterPressureSensorObserver(IPressureSensorObserver observer, int deviceNumber, int lineNumber);

        /// <summary>
        /// Установить рабочий режим стенда
        /// </summary>
        /// <returns></returns>
        Task<bool> SetStandWorkModeAsync();

        Task<bool> CloseAllNozzleAsync();
        Task<bool> OpenSolenoidValveAsync(StandSettingsSolenoidValveModel solenoidValveModel);
        Task<bool> CloseSolenoidValveAsync(StandSettingsSolenoidValveModel solenoidValveModel);
        Task<bool> EnableVacuumCreator();
        Task<bool> DisableVacuumCreator();
        public void PidEnable();
        public void PidDisable();
        Task<bool> SetFrequencyRegulatorFrequencyAsync(double value);
        Task<bool> EndWorkAsync();

        float? TemperatureTube { get; set; }
        float? PressureAtmosphere { get; set; }
        float? Humidity { get; set; }
        float? Temperature { get; set; }
        float? PressureResiver { get; set; }
        double? TargetFlow { get; set; }

        /// <summary>
        /// Давление перепада, Па
        /// </summary>
        float? PressureDifference { get; set; }
        
        void UpdateDeviceInformation(DeviceAboutViewModel deviceAboutViewModel);
        void RegisterDeviceObserver(IDeviceObserver deviceItemViewModel, int deviceNumber, int lineNumber);
        string GetVendorNumber(int deviceNumber);
        
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

        Task EmergencyPowerOffAsync();
        Task<bool> ResetToZeroPressureDifferenceAsync();
        bool GetDeviceManualEnable(int i);
        Task<bool> SetConsumptionWithoutSelectionAsync(ObservableCollection<StandSettingsNozzleModel> pointSelectedNozzles);

        void SetTargetFlowValue(double? value);
        void AddTargetFlowValue(double? value);
        
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
        Task<bool> SetPulseCountAsync(int pulseCount, int deviceIndex);

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
    }
}
