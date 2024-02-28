using SPU_7.Common.Device;

namespace SPU_7.Domain.Devices
{
    /// <summary>
    /// Интерфейс устройств (общие свойства и методы для всех видов устройств)
    /// </summary>
    public interface IDevice
    {
        /// <summary>
        /// Тип устройства
        /// </summary>
        public DeviceType DeviceType { get; set; }
        
        /// <summary>
        /// Подгруппа устройства (SPI, NBIoT и т.п.)
        /// </summary>
        public DeviceGroupType DeviceGroupType { get; set; }
        
        /// <summary>
        /// Доступно ли усройство для использования
        /// </summary>
        bool IsManualEnabled { get; set; }
        
        /// <summary>
        /// Заводской номер
        /// </summary>
        public string VendorNumberString { get; set; }
        
        /// <summary>
        /// Device name
        /// </summary>
        public string DeviceName { get; set; }

        /// <summary>
        /// Ввод пароля для доступа по протоколу Modbus
        /// </summary>
        /// <returns>Получилось ли ввести пароль</returns>
        Task<bool> SetPasswordAsync();
        
        /// <summary>
        /// Проверка соединения с устройством
        /// </summary>
        /// <returns>True - устройство есть, false - нет связи</returns>
        Task<bool> CheckConnectionWithDeviceAsync();
        
        /// <summary>
        /// Деактивировать устройство
        /// </summary>
        /// <returns>Получилось ли деактивировать</returns>
        Task<bool> DeactivateDeviceAsync();

        /// <summary>
        /// Калибровать устройство
        /// </summary>
        /// <returns>Получилось ли откалибровать</returns>
        Task<bool> CalibrateDeviceAsync();

        /// <summary>
        /// Загрузить прошивку в устройство
        /// </summary>
        /// <param name="firmware">Прошивка из БД</param>
        /// <returns>Получилось ли прошить</returns>
        Task<bool> ProgrammingDeviceAsync(byte[] firmware);

        /// <summary>
        /// Поверка устройства
        /// </summary>
        /// <returns>Получилось ли поверить</returns>
        Task<bool> ValidationDeviceAsync();

        /// <summary>
        /// Синхронизация времени устройства с текущим
        /// </summary>
        /// <returns>Получилось ли синхронизировать время</returns>
        Task<bool> SynchronizeTimeAsync();

        /// <summary>
        /// Проверка клапанов
        /// </summary>
        /// <returns>Удалось ли проверить клапана</returns>
        Task<bool> CheckValveAsync();

        /// <summary>
        /// Проверка связи устройства 
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckConnectionAsync();

        /// <summary>
        /// Проверка платформы
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckPlatformAsync();

        /// <summary>
        /// Установить диапазон (договорные значения)
        /// </summary>
        /// <param name="value">Договорное значение</param>
        /// <returns>Получилось ли установить диапазон</returns>
        Task<bool> SetRangeAsync(float value);

        /// <summary>
        /// Установить чувствительность
        /// </summary>
        /// <param name="value">Значение чувствительности</param>
        /// <param name="comparatorMax">какой-то максимум</param>
        /// <param name="comparatorMin">какой-то минимум</param>
        /// <returns>Получилось ли установить чувствивтельность</returns>
        Task<bool> SetSensitivityAsync(ushort value, ushort comparatorMax, ushort comparatorMin);

        /// <summary>
        /// Сброс ДД на ноль
        /// </summary>
        /// <returns></returns>
        Task<bool> ResetToZeroAsync();

        /// <summary>
        /// Установить количество импульсов для БИПЧ
        /// </summary>
        /// <param name="pulseCount">Количество импульсов</param>
        /// <returns>Получилось ли отправить запрос</returns>
        Task<bool> SetPulseCountAsync(int pulseCount);

        /// <summary>
        /// Считать количество импульсов БИПЧ
        /// </summary>
        /// <param name="pulseCount">Количество импульсов</param>
        /// <returns>Количество импульсов</returns>
        Task<int?> ReadPulseCountAsync();
    }
}
